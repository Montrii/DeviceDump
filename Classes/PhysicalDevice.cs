﻿using System;
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
        public ulong SizeInBytes { get; }
        public string SerialNumber { get; }
        public string MediaType { get; }
        public string Caption { get; }
        public string FirmwareRevision { get; }
        public string PNPDeviceID { get; }


        // Reference to the opened file stream for the device.
        private Stream? _fileStream;

        public Stream? FileStream { get => _fileStream; set => _fileStream = value; }


        // Number of bytes read from the device.
        private ulong _bytesRead;

        public ulong BytesRead
        {
            get => _bytesRead;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "Bytes read cannot be negative.");
                _bytesRead = value;
            }
        }


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

            SizeInBytes = (ulong)(drive["Size"] ?? 0);

            Name = $"({DevicePath}) {Model}";
        }



        // Opens the PhysicalDevice.
        public void OpenPhysicalDevice()
        {
            if(string.IsNullOrEmpty(DevicePath))
            {
                throw new InvalidOperationException("Device path is not set.");
            }

            FileStream = new FileStream(DevicePath, FileMode.Open, FileAccess.Read);
        }


        // Closes the opened PhysicalDevice stream if it is open.
        public void ClosePhysicalDevice()
        {
            if (FileStream != null)
            {
                FileStream.Dispose();
                FileStream = null;
            }
        }



        #region Static Methods
        public static string FormatBytes(ulong bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB", "PB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }
        #endregion


        public override string ToString() => Name;
    }
}
