using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceDump.Classes
{

    // A class to handle hex dumping of a device.
    public sealed class DeviceHexDumper
    {
        private PhysicalDevice _device;

        private string _outputFilePath;

        public DeviceHexDumper(PhysicalDevice device)
        {
            _device = device ?? throw new ArgumentNullException(nameof(device));

            if (string.IsNullOrWhiteSpace(_device.DevicePath))
                throw new ArgumentException("DevicePath must be a valid non-empty string.");


        }


        // Deletes the output file if it exists.
        public void DeleteHexDump()
        {
            if (!string.IsNullOrEmpty(_outputFilePath) && File.Exists(_outputFilePath))
                File.Delete(_outputFilePath);
        }

        public void CreateHexDumpFile()
        {
            // Generate a safe filename from the device path
            string safeFileName = _device.DevicePath
                .Replace(Path.DirectorySeparatorChar, '_')
                .Replace(Path.AltDirectorySeparatorChar, '_');

            // Default file path in temp directory
            _outputFilePath = Path.Combine(Path.GetTempPath(), $"{safeFileName}_hexdump.txt");
        }


        // Returns a string with the specified amount of bytes read from the device in hex format.
        // Reads from the device and appends hex dump to the file. Returns the file path.
        public string ReadHexFromDevice(bool createNewFile, int offset, int length, int bytesPerLine = 16)
        {
            if (_device?.FileStream == null || !_device.FileStream.CanRead)
                throw new InvalidOperationException("Device stream is not open or not readable.");

            byte[] buffer = new byte[length];

            _device.FileStream.Seek(offset, SeekOrigin.Begin);
            int bytesRead = _device.FileStream.Read(buffer, 0, length);

            if (offset <= 0)
            {
                _device.BytesRead = Convert.ToUInt64(bytesRead);
            }
            else
            {
                _device.BytesRead += Convert.ToUInt64(bytesRead);
            }

            List<string> dump = HexDump(buffer, bytesPerLine);

            if (createNewFile)
            {
                // Call here to delete the existing file if createNewFile is true
                DeleteHexDump();

                CreateHexDumpFile();
                // Overwrite the file with the new dump
                File.WriteAllLines(_outputFilePath, dump);
            }
            else
            {
                // Append to the existing file
                File.AppendAllLines(_outputFilePath, dump);
            }

            return _outputFilePath;
        }




        #region Private Methods

        // From https://www.codeproject.com/Articles/36747/Quick-and-Dirty-HexDump-of-a-Byte-Array
        // Slightly modified.
        private List<string> HexDump(byte[] bytes, int bytesPerLine = 16)
        {
            if (bytes == null) return new List<string> { "<null>" };
            if (bytesPerLine <= 0) throw new ArgumentOutOfRangeException(nameof(bytesPerLine));

            var lines = new List<string>();
            var sb = new StringBuilder();

            long length = bytes.LongLength;

            for (long i = 0; i < length; i += bytesPerLine)
            {
                sb.Clear();

                sb.Append(DetermineCurrentHexAddress(i)).Append("  ");

                // Hex section
                for (int j = 0; j < bytesPerLine; j++)
                {
                    if (i + j < length)
                        sb.AppendFormat("{0:X2} ", bytes[i + j]);
                    else
                        sb.Append("   ");

                    if ((j + 1) % 8 == 0)
                        sb.Append(" ");
                }

                sb
                    .Append(" | ")
                    .Append(FormatAscii(bytes, i, bytesPerLine))
                    .Append(" |");

                lines.Add(sb.ToString());
            }

            return lines;
        }


        private string DetermineCurrentHexAddress(long offset)
        {
            // Format the address as a hex string, ensuring it is padded to 16 characters
            return _device.BytesRead > 0 ? (_device.BytesRead + (ulong)offset).ToString("X16") : offset.ToString("X16");
        }

        private static string FormatAscii(byte[] bytes, long offset, int count)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                if (offset + i < bytes.Length)
                {
                    byte b = bytes[offset + i];
                    sb.Append((b >= 32 && b <= 126) ? (char)b : '·');
                }
                else
                {
                    sb.Append(' ');
                }
            }
            return sb.ToString();
        }

        #endregion
    }
}
