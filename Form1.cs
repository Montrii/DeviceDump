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

            // No selection? Clear tooltip and exit
            if (richTextBoxHexDump.SelectionLength == 0)
            {
                byteToolTip.SetToolTip(richTextBoxHexDump, "");
                return;
            }

            int index = richTextBoxHexDump.GetCharIndexFromPosition(e.Location);
            int line = richTextBoxHexDump.GetLineFromCharIndex(index);
            int firstCharIndex = richTextBoxHexDump.GetFirstCharIndexFromLine(line);
            int column = index - firstCharIndex;

            // Safety check
            if (line < 0 || line >= richTextBoxHexDump.Lines.Length)
                return;

            string lineText = richTextBoxHexDump.Lines[line];
            if (lineText.Length < 8)
                return;

            // Parse line address
            string addressPrefix = lineText.Substring(0, 8);
            if (!int.TryParse(addressPrefix, System.Globalization.NumberStyles.HexNumber, null, out int baseAddress))
                return;

            int byteIndex = (column - 9) / 3;
            if (byteIndex < 0 || byteIndex > 15)
                return;

            int currentAddress = baseAddress + byteIndex;

            int selectionStartIndex = richTextBoxHexDump.SelectionStart;
            int selectionEndIndex = selectionStartIndex + richTextBoxHexDump.SelectionLength - 1;

            if (index >= selectionStartIndex && index <= selectionEndIndex)
            {
                // Get line/column of selection start
                int startLine = richTextBoxHexDump.GetLineFromCharIndex(selectionStartIndex);
                int startLineChar = richTextBoxHexDump.GetFirstCharIndexFromLine(startLine);
                int startCol = selectionStartIndex - startLineChar;

                if (startLine < 0 || startLine >= richTextBoxHexDump.Lines.Length)
                    return;

                string startLineText = richTextBoxHexDump.Lines[startLine];
                if (startLineText.Length < 8)
                    return;

                string startAddrPrefix = startLineText.Substring(0, 8);
                if (!int.TryParse(startAddrPrefix, System.Globalization.NumberStyles.HexNumber, null, out int startBaseAddress))
                    return;

                int startByteIndex = (startCol - 9) / 3;
                if (startByteIndex < 0 || startByteIndex > 15)
                    return;

                int startAddress = startBaseAddress + startByteIndex;

                // Get line/column of selection end
                int endLine = richTextBoxHexDump.GetLineFromCharIndex(selectionEndIndex);
                int endLineChar = richTextBoxHexDump.GetFirstCharIndexFromLine(endLine);
                int endCol = selectionEndIndex - endLineChar;

                if (endLine < 0 || endLine >= richTextBoxHexDump.Lines.Length)
                    return;

                string endLineText = richTextBoxHexDump.Lines[endLine];
                if (endLineText.Length < 8)
                    return;

                string endAddrPrefix = endLineText.Substring(0, 8);
                if (!int.TryParse(endAddrPrefix, System.Globalization.NumberStyles.HexNumber, null, out int endBaseAddress))
                    return;

                int endByteIndex = (endCol - 9) / 3;
                if (endByteIndex < 0 || endByteIndex > 15)
                    return;

                int endAddress = endBaseAddress + endByteIndex;

                // Tooltip text: range or single
                if (startAddress == endAddress)
                {
                    byteToolTip.SetToolTip(richTextBoxHexDump, $"Address: 0x{startAddress:X8}");
                }
                else
                {
                    byteToolTip.SetToolTip(richTextBoxHexDump, $"Range: 0x{startAddress:X8} - 0x{endAddress:X8}");
                }
            }
            else
            {
                byteToolTip.SetToolTip(richTextBoxHexDump, "");
            }
        }

        #endregion

        #region Private Methods
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
