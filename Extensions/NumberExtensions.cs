namespace LootValueEX.Extensions
{
    internal static class NumberExtensions
    {
        public static string FormatNumber(this int s)
        {
            if (s > 1000)
                return $"{string.Format("{0:0.0}", (double)s / 1000)}k";
            return s.ToString();
        }

        public static string FormatNumber(this double s)
        {
            if (s > 1000)
                return $"{string.Format("{0:0.0}", s / 1000)}k";
            return s.ToString();
        }
    }
}
