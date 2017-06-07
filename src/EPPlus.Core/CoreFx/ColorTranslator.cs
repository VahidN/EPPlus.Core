#if NETSTANDARD2_0

namespace System.Drawing
{
    /// <summary>
    ///    Translates colors to and from GDI+ <see cref='System.Drawing.Color'/> objects.
    /// </summary>
    public static class ColorTranslator
    {
        /// <summary>
        ///    Translates an Html color representation to
        ///    a GDI+ <see cref='System.Drawing.Color'/>.
        /// </summary>
        public static Color FromHtml(string htmlColor)
        {
            // empty color
            if ((htmlColor == null) || (htmlColor.Length == 0))
                return Color.Empty;

            // #RRGGBB or #RGB
            if ((htmlColor[0] == '#'))
            {
                if (htmlColor.Length == 7)
                {
                    return Color.FromArgb(Convert.ToInt32(htmlColor.Substring(1, 2), 16),
                                       Convert.ToInt32(htmlColor.Substring(3, 2), 16),
                                       Convert.ToInt32(htmlColor.Substring(5, 2), 16));
                }

                if (htmlColor.Length == 4)
                {
                    string r = Char.ToString(htmlColor[1]);
                    string g = Char.ToString(htmlColor[2]);
                    string b = Char.ToString(htmlColor[3]);

                    return Color.FromArgb(Convert.ToInt32(r + r, 16),
                                       Convert.ToInt32(g + g, 16),
                                       Convert.ToInt32(b + b, 16));
                }

                if(htmlColor.Length == 9)
                {
                    var argb = int.Parse(htmlColor.Replace("#", ""), System.Globalization.NumberStyles.HexNumber);
                    return Color.FromArgb((byte)((argb & -16777216) >> 0x18),
                                          (byte)((argb & 0xff0000) >> 0x10),
                                          (byte)((argb & 0xff00) >> 8),
                                          (byte)(argb & 0xff));
                }
            }

            throw new NotSupportedException("Only Hex Colors (#RRGGBB or #RGB) Are Supported.");
        }
    }
}
#endif