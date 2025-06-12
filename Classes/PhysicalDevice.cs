using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace DeviceDump.Classes
{

    // Wrapper class for a Physical Device.
    public sealed class PhysicalDevice
    {
        public string Name { get; }
        public string DevicePath { get; }
        public string Model { get; }
        public string Manufacturer { get; }
        public string InterfaceType { get; }
        public ulong Size { get; }
        public string SerialNumber { get; }
        public string MediaType { get; }
        public string Caption { get; }
        public string FirmwareRevision { get; }
        public string PNPDeviceID { get; }

        public PhysicalDevice(ManagementObject drive)
        {
            // Use ?.ToString() ?? "" to avoid nulls
            Model = drive["Model"]?.ToString() ?? "Unknown";
            Manufacturer = drive["Manufacturer"]?.ToString() ?? "Unknown";
            InterfaceType = drive["InterfaceType"]?.ToString() ?? "Unknown";
            SerialNumber = drive["SerialNumber"]?.ToString() ?? "Unknown";
            MediaType = drive["MediaType"]?.ToString() ?? "Unknown";
            Caption = drive["Caption"]?.ToString() ?? "Unknown";
            FirmwareRevision = drive["FirmwareRevision"]?.ToString() ?? "Unknown";
            PNPDeviceID = drive["PNPDeviceID"]?.ToString() ?? "Unknown";

            DevicePath = drive["DeviceID"]?.ToString() ?? string.Empty;

            Size = (ulong)(drive["Size"] ?? 0);

            Name = $"{Model} ({DevicePath})";
        }

        public override string ToString() => Name;
    }
}
