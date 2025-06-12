using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace DeviceDump.Classes
{
    public sealed class PhysicalDeviceReceiver
    {
        public List<PhysicalDevice> GetPhysicalDrives()
        {
            var devices = new List<PhysicalDevice>();

            using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
            using var results = searcher.Get();

            foreach (ManagementObject drive in results)
            {
                try
                {
                    devices.Add(new PhysicalDevice(drive));
                }
                catch
                {
                    // Skip invalid/partially populated drives
                }
            }

            return devices;
        }
    }
}
