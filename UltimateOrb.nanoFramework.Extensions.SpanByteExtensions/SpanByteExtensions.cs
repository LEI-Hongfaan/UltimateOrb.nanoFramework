using System;
using System.Diagnostics;

namespace UltimateOrb.nanoFramework.Extensions {

    public static partial class SpanByteExtensions {

        public static UInt32 ReadUInt32(this SpanByte buffer, int offset) {
            if (offset < 0 || offset + sizeof(UInt32) > buffer.Length) {
                throw new ArgumentOutOfRangeException(nameof(offset), "Offset is out of range.");
            }

            // Manually read the bytes and construct the 32-bit unsigned integer
            UInt32 result = (UInt32)buffer[offset]
                | ((UInt32)buffer[offset + 1] << 8)
                | ((UInt32)buffer[offset + 2] << 16)
                | ((UInt32)buffer[offset + 3] << 24);

            return result;
        }

        public static int Read(this SpanByte buffer, int offset, out UInt32 value) {
            value = (UInt32)(buffer[offset]
                    | (buffer[offset + 1] << 8)
                    | (buffer[offset + 2] << 16)
                    | (buffer[offset + 3] << 24));
            return 4; // Return the number of bytes read
        }

        public static int Read(this SpanByte buffer, int offset, out UInt64 value) {
            value = (UInt64)(buffer[offset]
                    | (buffer[offset + 1] << 8)
                    | (buffer[offset + 2] << 16)
                    | (buffer[offset + 3] << 24)
                    | (buffer[offset + 4] << 32)
                    | (buffer[offset + 5] << 40)
                    | (buffer[offset + 6] << 48)
                    | (buffer[offset + 7] << 56));
            return 8; // Return the number of bytes read
        }

        public static int Read(this SpanByte buffer, int offset, out Guid value) {
            value = new Guid(buffer.Slice(offset, 16).ToArray());
            return 16; // Return the number of bytes read
        }

        public static int Read(this SpanByte buffer, int offset, int byteCount, out string value) {
            Debug.Assert((buffer.Length & 1) != 0);

            // Determine the actual length of the string by finding the first trailing '\0'
            int charLength = byteCount / 2;
            for (int i = 0; i < charLength; i++) {
                if (buffer[offset + i * 2] == 0 && buffer[offset + i * 2 + 1] == 0) {
                    charLength = i;
                    break;
                }
            }

            // Read the string as a char array
            char[] nameChars = new char[charLength];
            for (int i = 0; i < charLength; i++) {
                nameChars[i] = (char)(buffer[offset + i * 2] | (buffer[offset + i * 2 + 1] << 8));
            }
            value = new string(nameChars);

            return byteCount / 2 * 2; // Return the number of bytes read
        }
    }
}