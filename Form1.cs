using DeviceDump.Classes;
using System.Management;
using System.Runtime.InteropServices;
using System.Windows.Forms;

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

                UpdateSelectedDevice(device);
                device.OpenPhysicalDevice();

                DeviceHexDumper dumper = new DeviceHexDumper(device);
                try
                {
                    richTextBoxHexDump.Text = string.Join(Environment.NewLine, dumper.ReadHexFromDevice(0, 512));

                    // Immediately close the device after reading -> so it can be used again.
                    device.ClosePhysicalDevice();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error reading device: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        }
    }
}
