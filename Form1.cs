using DeviceDump.Classes;
using DeviceDump.UI;
using System.Management;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DeviceDump
{
    public partial class Form1 : Form
    {
        // Physical Device Receiver to get the devices.
        private readonly PhysicalDeviceReceiver physicalDeviceReceiver = new PhysicalDeviceReceiver();

        // Clicked Device.
        private PhysicalDevice SelectedPhysicalDevice { get; set; }


        private ToolTip byteToolTip;

        #region Constructor
        public Form1()
        {
            InitializeComponent();
            byteToolTip = new ToolTip();
        }
        #endregion


        #region Click Events

        // When "Jump to Address on Device" was clicked.
        private void jumpToAddressOnDeviceToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (SelectedPhysicalDevice == null)
            {
                MessageBox.Show("No device selected or device is not readable.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Only allow input of full bytes.
            using (var inputForm = new HexInputForm("Enter Address", "Only hex addresses are allowed.", value =>
            {
                string trimmed = value.Trim();

                // Validate hex format (e.g., "0x1A3" or "1A3")
                if (System.Text.RegularExpressions.Regex.IsMatch(trimmed, @"^(0x)?[0-9A-Fa-f]+$"))
                {
                    try
                    {
                        // Try parsing it just to ensure it's a valid number
                        Convert.ToInt32(trimmed, 16);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }

                return false;
            }))
            {
                var result = inputForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    JumpToAddressOnDeviceToolStripMenuItem(SelectedPhysicalDevice, inputForm.UserInput);
                }
                else
                {
                    MessageBox.Show("Input was cancelled or empty.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    inputForm.ShowDialog();
                }
            }

        }


        // Event handler that refreshes the menu items just before it opens
        private void SelectDeviceToolStripMenuItem_DropDownOpening(object? sender, EventArgs e)
        {
            selectDeviceToolStripMenuItem.DropDownItems.Clear();

            List<PhysicalDevice> drives = physicalDeviceReceiver.GetPhysicalDrives();

            foreach (var device in drives)
            {
                var item = new ToolStripMenuItem(device.Name)
                {
                    Tag = device,
                    Checked = device.DevicePath == SelectedPhysicalDevice?.DevicePath,
                    CheckOnClick = true
                };

                item.Click += PhysicalDeviceOption_Clicked;
                selectDeviceToolStripMenuItem.DropDownItems.Add(item);
            }
        }


        // Triggers when the "Select Device" menu item is clicked
        private void selectDeviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectDeviceToolStripMenuItem.DropDownItems.Clear();

            List<PhysicalDevice> drives = physicalDeviceReceiver.GetPhysicalDrives(); // updated method

            foreach (var device in drives)
            {
                var item = new ToolStripMenuItem(device.Name)
                {
                    Tag = device, // ✅ store the whole PhysicalDevice object
                    Checked = device.DevicePath == SelectedPhysicalDevice?.DevicePath,
                    CheckOnClick = true
                };

                item.Click += PhysicalDeviceOption_Clicked;
                selectDeviceToolStripMenuItem.DropDownItems.Add(item);
            }
        }


        // Function to handle UI updating and device open/close
        public void OpenClosePhysicalDevice(bool open, PhysicalDevice device)
        {

            if (open)
            {
                try
                {
                    device.OpenPhysicalDevice();
                    labelDeviceUsed.ForeColor = Color.Green;
                    labelDeviceUsed.Font = new Font(labelDeviceUsed.Font, FontStyle.Bold);
                    labelDeviceUsed.Text = "Device is open (accessed).";
                }
                catch (Exception ex)
                {
                    labelDeviceUsed.ForeColor = Color.Red;
                    labelDeviceUsed.Font = new Font(labelDeviceUsed.Font, FontStyle.Bold);
                    labelDeviceUsed.Text = "Failed to open device.";
                    MessageBox.Show($"Error opening device: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                try
                {
                    device.ClosePhysicalDevice();
                    labelDeviceUsed.ForeColor = Color.Red;
                    labelDeviceUsed.Font = new Font(labelDeviceUsed.Font, FontStyle.Bold);
                    labelDeviceUsed.Text = "Device is closed.";
                }
                catch (Exception ex)
                {
                    labelDeviceUsed.ForeColor = Color.Red;
                    labelDeviceUsed.Font = new Font(labelDeviceUsed.Font, FontStyle.Bold);
                    labelDeviceUsed.Text = "Failed to close device.";
                    MessageBox.Show($"Error closing device: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        // Yes - this is ChatGPT code.
        private void richTextBoxHexDump_MouseMove(object sender, MouseEventArgs e)
        {
            if (SelectedPhysicalDevice == null)
                return;

            if (richTextBoxHexDump.SelectionLength == 0)
            {
                byteToolTip.SetToolTip(richTextBoxHexDump, "");
                return;
            }

            int selectionStartIndex = richTextBoxHexDump.SelectionStart;
            int selectionEndIndex = selectionStartIndex + richTextBoxHexDump.SelectionLength - 1;

            int startLine = richTextBoxHexDump.GetLineFromCharIndex(selectionStartIndex);
            int endLine = richTextBoxHexDump.GetLineFromCharIndex(selectionEndIndex);

            if (startLine < 0 || endLine >= richTextBoxHexDump.Lines.Length)
                return;

            // Check if selection is within address or ASCII area for both start and end
            if (IsInNonHexRegion(selectionStartIndex) || IsInNonHexRegion(selectionEndIndex))
            {
                byteToolTip.SetToolTip(richTextBoxHexDump, "");
                return;
            }

            // Compute byte address
            int startByteAddr = GetByteAddressFromCharIndex(selectionStartIndex);
            int endByteAddr = GetByteAddressFromCharIndex(selectionEndIndex);
            if (startByteAddr == -1 || endByteAddr == -1)
                return;

            // Count actual hex byte pairs in selection range (ignoring spaces)
            int length = CountHexBytesInSelection(selectionStartIndex, selectionEndIndex);
            if (length <= 0)
                return;

            if (length <= 0)
                return;

            byte[] selectedBytes = GetSelectedBytesFromHexDump(startByteAddr, length);
            if (selectedBytes == null || selectedBytes.Length != length)
                return;

            string tooltip = (startByteAddr == endByteAddr)
                ? $"Address: 0x{startByteAddr:X16}\n"
                : $"Range: 0x{startByteAddr:X16} - 0x{endByteAddr:X16}\n";

            switch (length)
            {
                case 1:
                    tooltip += $"Bool: {Convert.ToBoolean(selectedBytes[0])}\n";
                    tooltip += $"Char: {(char)selectedBytes[0]}\n";
                    break;
                case 2:
                    tooltip += $"Short: {BitConverter.ToInt16(selectedBytes, 0)}\n";
                    tooltip += $"String: {System.Text.Encoding.ASCII.GetString(selectedBytes)}\n";
                    break;
                case 4:
                    tooltip += $"Int: {BitConverter.ToInt32(selectedBytes, 0)}\n";
                    tooltip += $"String: {System.Text.Encoding.ASCII.GetString(selectedBytes)}\n";
                    break;
                case 8:
                    tooltip += $"Long: {BitConverter.ToInt64(selectedBytes, 0)}\n";
                    tooltip += $"String: {System.Text.Encoding.ASCII.GetString(selectedBytes)}\n";
                    break;
                default:
                    tooltip += $"String: {System.Text.Encoding.ASCII.GetString(selectedBytes)}\n";
                    break;
            }

            byteToolTip.SetToolTip(richTextBoxHexDump, tooltip.TrimEnd());
        }
        #endregion

        #region Private Methods
        private int CountHexBytesInSelection(int selectionStartIndex, int selectionEndIndex)
        {
            int count = 0;
            for (int i = selectionStartIndex; i <= selectionEndIndex; i++)
            {
                char c = richTextBoxHexDump.Text[i];
                // Only count hex digits — two consecutive hex digits form a byte
                if (IsHexChar(c))
                {
                    int j = i + 1;
                    if (j <= selectionEndIndex && IsHexChar(richTextBoxHexDump.Text[j]))
                    {
                        count++;
                        i = j; // Skip next char since we've counted a full byte
                    }
                }
            }
            return count;
        }

        private bool IsHexChar(char c)
        {
            return (c >= '0' && c <= '9') ||
                   (c >= 'A' && c <= 'F') ||
                   (c >= 'a' && c <= 'f');
        }



        // Helper: Checks if character index is in address or ASCII region
        private bool IsInNonHexRegion(int charIndex)
        {
            int line = richTextBoxHexDump.GetLineFromCharIndex(charIndex);
            if (line < 0 || line >= richTextBoxHexDump.Lines.Length)
                return true;

            int lineStart = richTextBoxHexDump.GetFirstCharIndexFromLine(line);
            int column = charIndex - lineStart;

            // Address region: column 0–7, separator at 8
            // Hex region: starts at 9 and spans 16*3 = 48 chars (e.g., "FF FF FF...")
            // ASCII region: typically starts at column 58+
            return (column < 9 || column >= 58);
        }

        // Helper: Gets byte index from character index (returns -1 if not in byte column)
        private int GetByteAddressFromCharIndex(int charIndex)
        {
            int line = richTextBoxHexDump.GetLineFromCharIndex(charIndex);
            if (line < 0 || line >= richTextBoxHexDump.Lines.Length)
                return -1;

            int lineStart = richTextBoxHexDump.GetFirstCharIndexFromLine(line);
            int column = charIndex - lineStart;

            string lineText = richTextBoxHexDump.Lines[line];
            if (lineText.Length < 16) // Now expect at least 16 chars for address
                return -1;

            // Parse first 16 characters as hex address
            if (!int.TryParse(lineText.Substring(0, 16), System.Globalization.NumberStyles.HexNumber, null, out int baseAddr))
                return -1;

            // Hex dump starts at column 17 (index 16), each byte = 2 hex + 1 space => 3 columns
            if (column < 17 || column > 64) // 16 address + space, then 16 bytes * 3 = 48 chars
                return -1;

            int byteIndex = (column - 17) / 3;
            if (byteIndex < 0 || byteIndex > 15)
                return -1;

            return baseAddr + byteIndex;
        }


        // Helper method to extract byte array from hex dump based on address
        private byte[] GetSelectedBytesFromHexDump(int startAddress, int length)
        {
            byte[] result = new byte[length];
            int resultIndex = 0;

            foreach (var line in richTextBoxHexDump.Lines)
            {
                if (line.Length < 9)
                    continue;

                if (!long.TryParse(line.Substring(0, 16), System.Globalization.NumberStyles.HexNumber, null, out long lineAddress))
                    continue;


                if (startAddress >= lineAddress + 16 || startAddress + length <= lineAddress)
                    continue; // Skip lines not within range

                string[] parts = line.Substring(17).Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < parts.Length && i < 16; i++)
                {
                    long currentAddress = lineAddress + (long)i;
                    if (currentAddress >= startAddress && currentAddress < startAddress + length)
                    {
                        if (byte.TryParse(parts[i], System.Globalization.NumberStyles.HexNumber, null, out byte b))
                        {
                            result[resultIndex++] = b;
                            if (resultIndex == length)
                                return result;
                        }
                    }
                }
            }

            return resultIndex == length ? result : null;
        }

        private void JumpToAddressOnDeviceToolStripMenuItem(PhysicalDevice device, string addressHex)
        {
            // Normalize and validate input
            string normalizedInput = addressHex.Trim().ToLower().Replace("0x", "");
            if (!int.TryParse(normalizedInput, System.Globalization.NumberStyles.HexNumber, null, out int targetAddress))
            {
                MessageBox.Show($"Invalid hex address: {addressHex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            const int bytesPerLine = 16;
            int lineStartAddress = targetAddress & ~(bytesPerLine - 1);
            int byteOffsetInLine = targetAddress - lineStartAddress;

            // Split the hex dump into lines
            List<string> lines = richTextBoxHexDump.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToList();
            int currentCharIndex = 0;

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    currentCharIndex += line.Length + 1;
                    continue;
                }

                string[] parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 2)
                {
                    currentCharIndex += line.Length + 1;
                    continue;
                }

                if (int.TryParse(parts[0], System.Globalization.NumberStyles.HexNumber, null, out int lineAddress))
                {
                    if (lineAddress == lineStartAddress)
                    {
                        if (byteOffsetInLine == 0)
                        {
                            // If address is exactly at line start, highlight whole line
                            richTextBoxHexDump.Select(currentCharIndex, line.Length);
                            richTextBoxHexDump.ScrollToCaret();
                            MessageBox.Show($"Found full line for address '{addressHex}'.", "Address Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        else
                        {
                            // Highlight just the target byte (2-digit hex)
                            int byteCharIndexInLine = 0;
                            int bytesFound = 0;

                            for (int i = 1; i < parts.Length && bytesFound < byteOffsetInLine; i++)
                            {
                                byteCharIndexInLine += parts[i].Length + 1;
                                bytesFound++;
                            }

                            if (byteOffsetInLine + 1 < parts.Length)
                            {
                                int byteTextLength = parts[byteOffsetInLine + 1].Length;
                                int startIndex = currentCharIndex + parts[0].Length + 1 + byteCharIndexInLine;

                                richTextBoxHexDump.Select(startIndex, byteTextLength);
                                richTextBoxHexDump.ScrollToCaret();
                                MessageBox.Show($"Found byte at address '{addressHex}'.", "Byte Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                    }
                }

                currentCharIndex += line.Length + 1; // Account for newline
            }

            MessageBox.Show($"Address {addressHex} not found in hex dump.", "Address Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }



        // When a specific device is clicked.
        private void PhysicalDeviceOption_Clicked(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem item in selectDeviceToolStripMenuItem.DropDownItems)
            {
                item.Checked = false;
                PhysicalDevice currentDevice = item.Tag as PhysicalDevice;
                currentDevice?.ClosePhysicalDevice();
            }


            if (sender is ToolStripMenuItem clickedItem &&
                clickedItem.Tag is PhysicalDevice device)
            {
                clickedItem.Checked = true;

                DeviceHexDumper dumper = new DeviceHexDumper(device);
                try
                {
                    // Open the device.
                    OpenClosePhysicalDevice(true, device);

                    AddDumpsToHexDump(dumper.ReadHexFromDevice(0, 512));

                    // Update frontend.
                    UpdateSelectedDevice(device);

                    // Immediately close the device after reading -> so it can be used again.
                    OpenClosePhysicalDevice(false, SelectedPhysicalDevice);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error reading device: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // When the "Read Bytes from Device" Item is clicked.
        private void readBytesFromDeviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedPhysicalDevice == null)
            {
                MessageBox.Show("No device selected or device is not readable.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Only allow input of full bytes.
            using (var inputForm = new NumberInputForm("Enter Bytes", "Only full amount of bytes are allowed.", value =>
            {
                return value % 8 == 0;
            }))
            {
                var result = inputForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    ProcessReadBytesInput(inputForm.UserInput);
                }
                else
                {
                    MessageBox.Show("Input was cancelled or empty.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    inputForm.ShowDialog();
                }
            }
        }

        private void UpdateSelectedDevice(PhysicalDevice device)
        {
            SelectedPhysicalDevice = device;
            labelSelectedPhysicalDevice.Text = $"{device.Name}";
            labelSelectedPhysicalDevice.Font = new Font(labelSelectedPhysicalDevice.Font, FontStyle.Bold);

            labelSizeInBytes.Text = $"{device.SizeInBytes:N0} bytes ({PhysicalDevice.FormatBytes(device.SizeInBytes)})";
            labelSizeInBytes.Font = new Font(labelSizeInBytes.Font, FontStyle.Bold);

            labelBytesRead.Text = $"{device.BytesRead:N0} bytes ({PhysicalDevice.FormatBytes(device.BytesRead)})";
            labelBytesRead.Font = new Font(labelBytesRead.Font, FontStyle.Bold);
        }



        // Decides if the hex dump should be appended or replaced based on the number of bytes read.
        private void AddDumpsToHexDump(List<string> newDumps)
        {
            if (SelectedPhysicalDevice == null || SelectedPhysicalDevice.BytesRead <= 0)
            {
                richTextBoxHexDump.Text = string.Join(Environment.NewLine, newDumps);
            }
            else if (SelectedPhysicalDevice != null && SelectedPhysicalDevice.BytesRead > 0)
            {
                string text = richTextBoxHexDump.Text;
                text += "\n";
                text += string.Join(Environment.NewLine, newDumps);

                richTextBoxHexDump.Text = text;
            }
        }

        private void ProcessReadBytesInput(ulong value)
        {

            try
            {
                // get the input.
                ulong bytes = Convert.ToUInt64(value);

                // Open the device.
                OpenClosePhysicalDevice(true, SelectedPhysicalDevice);

                // Create a new DeviceHexDumper instance.
                DeviceHexDumper dumper = new DeviceHexDumper(SelectedPhysicalDevice);

                // Read the specified number of bytes from the device.
                AddDumpsToHexDump(dumper.ReadHexFromDevice(SelectedPhysicalDevice.BytesRead > 0 ? (int)SelectedPhysicalDevice.BytesRead : 0, (int)bytes));

                UpdateSelectedDevice(SelectedPhysicalDevice);

                OpenClosePhysicalDevice(false, SelectedPhysicalDevice);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        }
        #endregion


        


    }
}
