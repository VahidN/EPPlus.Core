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
            Color c = Color.Empty;

            // empty color
            if ((htmlColor == null) || (htmlColor.Length == 0))
                return c;

            // #RRGGBB or #RGB
            if ((htmlColor[0] == '#') &&
                ((htmlColor.Length == 7) || (htmlColor.Length == 4)))
            {
                if (htmlColor.Length == 7)
                {
                    c = Color.FromArgb(Convert.ToInt32(htmlColor.Substring(1, 2), 16),
                                       Convert.ToInt32(htmlColor.Substring(3, 2), 16),
                                       Convert.ToInt32(htmlColor.Substring(5, 2), 16));
                }
                else
                {
                    string r = Char.ToString(htmlColor[1]);
                    string g = Char.ToString(htmlColor[2]);
                    string b = Char.ToString(htmlColor[3]);

                    c = Color.FromArgb(Convert.ToInt32(r + r, 16),
                                       Convert.ToInt32(g + g, 16),
                                       Convert.ToInt32(b + b, 16));
                }

                return c;
            }

            throw new NotSupportedException("Only Hex Colors (#RRGGBB or #RGB) Are Supported.");
        }
    }
}
#endif