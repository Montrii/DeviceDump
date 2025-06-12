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

        public Form1()
        {
            InitializeComponent();

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




        // When the 
        private void readBytesFromDeviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(SelectedPhysicalDevice == null) 
            {
                MessageBox.Show("No device selected or device is not readable.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var inputForm = new NumberInputForm("Bytes"))
            {
                var result = inputForm.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(inputForm.UserInput))
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
            else if(SelectedPhysicalDevice != null && SelectedPhysicalDevice.BytesRead > 0)
            {
                string text = richTextBoxHexDump.Text;
                text += "\n";
                text += string.Join(Environment.NewLine, newDumps);

                richTextBoxHexDump.Text = text;
            }
        }

        private void ProcessReadBytesInput(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                MessageBox.Show("Please enter a valid value.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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
    }
}
