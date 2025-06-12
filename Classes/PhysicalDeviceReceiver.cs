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
                devices.Add(new PhysicalDevice(drive));
            }

            return devices.OrderBy(d => ExtractDriveNumber(d.DevicePath)).ToList();
        }



        #region
        // Helper method to extract the drive number from DevicePath
        private int ExtractDriveNumber(string devicePath)
        {
            const string prefix = @"\\.\PHYSICALDRIVE";
            if (devicePath.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                var numberPart = devicePath.Substring(prefix.Length);
                if (int.TryParse(numberPart, out int number))
                    return number;
            }

            // If parsing fails, return a large number so it sorts last
            return int.MaxValue;
        }
        #endregion


    }
}
