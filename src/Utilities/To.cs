using OneWay.M3U.Core;
using System;

namespace OneWay.M3U.Utilities
{
    internal static class To
    {
        private static bool? Bool(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return default;
            if (string.Equals(text, M3UAttributes.Predicates.Yes))
                return true;
            if (string.Equals(text, M3UAttributes.Predicates.No))
                return false;

            return Convert.ToBoolean(text);
        }

        public static T? Value<T>(string text) where T : struct
        {
            if (string.IsNullOrWhiteSpace(text))
                return default;

            var t = typeof(T);
            if (t.Equals(typeof(bool)))
                return Bool(text) as T?;

            try { return (T)Convert.ChangeType(text, t); }
            catch { return default; }
        }
    }
}
