namespace System
{
    public static class StringComparisonEx
    {
        public static StringComparison InvariantCultureIgnoreCase =
#if COREFX
            StringComparison.OrdinalIgnoreCase;
#else
        StringComparison.InvariantCultureIgnoreCase;
#endif

        public static StringComparison InvariantCulture =
#if COREFX
            StringComparison.Ordinal;
#else
        StringComparison.InvariantCulture;
#endif
    }
}