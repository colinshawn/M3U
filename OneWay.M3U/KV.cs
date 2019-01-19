using System;
using Pair = System.Collections.Generic.KeyValuePair<string, string>;

namespace OneWay.M3U
{
    internal static class KV
    {
        public static Pair? Parse(string text, char separator = M3UAttributes.TagSeparator)
        {
            if (string.IsNullOrEmpty(text))
                return default;

            var kv = text.Split(new[] { separator }, 2, StringSplitOptions.RemoveEmptyEntries);
            var k = kv[0];
            var v = kv.Length > 1 ? kv[1] : string.Empty;

            return new Pair(k.Trim(), v.Trim(' ', '"'));
        }
    }
}
