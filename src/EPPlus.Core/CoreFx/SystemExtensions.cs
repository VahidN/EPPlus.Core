using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace System
{
    public static class SystemExtensions
    {
        public static byte[] GetBuffer(this MemoryStream ms)
        {
#if COREFX
            ArraySegment<byte> buffer;
            if (ms.TryGetBuffer(out buffer))
            {
                return buffer.ToArray();
            }
            throw new InvalidOperationException();
#else
            return ms.GetBuffer();
#endif
        }

#if COREFX
        public static string ToString(this char ch, CultureInfo c)
        {
            return ch.ToString();
        }
#endif
    }
}
