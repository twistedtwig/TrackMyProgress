using System.Text.RegularExpressions;

namespace GoalManagementLibrary
{
    public class StringHelpers
    {
        public static bool IsStringValidColourHexCode(string colourCode, bool prefixWithHash = false)
        {
            if (string.IsNullOrWhiteSpace(colourCode)) return false;

            if (prefixWithHash)
            {
                if (!colourCode.StartsWith("#"))
                {
                    colourCode = "#" + colourCode;
                }
            }

            return Regex.IsMatch(colourCode, "^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$");
        }
    }
}
