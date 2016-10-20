namespace System
{
    public static class StringComparerEx
    {
        public static StringComparer InvariantCultureIgnoreCase =
#if COREFX
            StringComparer.OrdinalIgnoreCase;
#else
        StringComparer.InvariantCultureIgnoreCase;
#endif

        public static StringComparer InvariantCulture =
#if COREFX
            StringComparer.Ordinal;
#else
        StringComparer.InvariantCulture;
#endif
    }
}