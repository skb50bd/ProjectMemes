using System;

namespace XMemes.Models.Utils
{
    public static class StringExtensions
    {
        public static string ToLowerOrEmpty(this string? str) =>
            string.IsNullOrWhiteSpace(str)
                ? string.Empty
                : str.ToLowerInvariant();

        public static bool InsensitiveEquals(this string? lhs, string? rhs) =>
            string.Equals(lhs, rhs, StringComparison.InvariantCultureIgnoreCase);
    }
}
