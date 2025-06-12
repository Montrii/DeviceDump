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
        
        public DeviceHexDumper(PhysicalDevice device)
        {
            _device = device;
        }



        // Returns a string with the specified amount of bytes read from the device in hex format.
        public List<string> ReadHexFromDevice(int offset, int length, int bytesPerLine = 16)
        {
            if (_device?.FileStream == null || !_device.FileStream.CanRead)
                throw new InvalidOperationException("Device stream is not open or not readable.");

            byte[] buffer = new byte[length];

            // Seek to the specified offset - Read bytes into the buffer
            _device.FileStream.Seek(offset, SeekOrigin.Begin);
            _device.FileStream.Read(buffer, 0, length);

            return HexDump(buffer, bytesPerLine);
        }


        #region Private Methods

        // From https://www.codeproject.com/Articles/36747/Quick-and-Dirty-HexDump-of-a-Byte-Array
        // Slightly modified.
        private static List<string> HexDump(byte[] bytes, int bytesPerLine = 16)
        {
            if (bytes == null) return new List<string> { "<null>" };
            if (bytesPerLine <= 0) throw new ArgumentOutOfRangeException(nameof(bytesPerLine));

            var lines = new List<string>();
            var sb = new StringBuilder();

            for (int i = 0; i < bytes.Length; i += bytesPerLine)
            {
                sb.Clear();

                // Address
                sb.Append(i.ToString("X8")).Append("  ");

                // Hex section
                for (int j = 0; j < bytesPerLine; j++)
                {
                    if (i + j < bytes.Length)
                        sb.AppendFormat("{0:X2} ", bytes[i + j]);
                    else
                        sb.Append("   ");

                    // Optional spacing after every 8 bytes
                    if ((j + 1) % 8 == 0)
                        sb.Append(" ");
                }

                sb
                    .Append(" | ")
                    // adding ascii
                    .Append(FormatAscii(bytes, i, bytesPerLine))

                    .Append(" |");


                lines.Add(sb.ToString());
            }

            return lines;
        }

        private static string FormatAscii(byte[] bytes, int offset, int count)
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
