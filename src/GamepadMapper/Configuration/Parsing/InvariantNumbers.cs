using System.Globalization;

namespace GamepadMapper.Configuration.Parsing
{
    internal static class InvariantInt
    {
        private const NumberStyles InvariantNumberStyles =
            NumberStyles.AllowLeadingWhite
            | NumberStyles.AllowTrailingWhite
            | NumberStyles.AllowLeadingSign;

        public static bool TryParse(string s, out int result)
        {
            return int.TryParse(s, InvariantNumberStyles, CultureInfo.InvariantCulture, out result);
        }
    }

    internal static class InvariantDouble
    {
        private const NumberStyles InvariantNumberStyles =
            NumberStyles.AllowLeadingWhite
            | NumberStyles.AllowTrailingWhite
            | NumberStyles.AllowLeadingSign
            | NumberStyles.AllowDecimalPoint;

        public static bool TryParse(string s, out double result)
        {
            return double.TryParse(s, InvariantNumberStyles, CultureInfo.InvariantCulture, out result);
        }
    }
}
