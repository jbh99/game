using System.Collections.Generic;
using System.Text;

namespace AIWars.Localization
{
    // Tiny flat JSON parser ({"key":"value", ...}). Avoids extra package dependencies.
    public static class JsonHelper
    {
        public static Dictionary<string, string> ParseFlatJson(string json)
        {
            var dict = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(json)) return dict;
            int i = 0; int n = json.Length;
            while (i < n)
            {
                while (i < n && json[i] != '"') i++;
                if (i >= n) break;
                string key = ReadString(json, ref i);
                while (i < n && json[i] != ':') i++;
                i++;
                while (i < n && char.IsWhiteSpace(json[i])) i++;
                if (i < n && json[i] == '"')
                {
                    string val = ReadString(json, ref i);
                    dict[key] = val;
                }
            }
            return dict;
        }

        static string ReadString(string s, ref int i)
        {
            // i is at opening quote
            i++;
            var sb = new StringBuilder();
            while (i < s.Length && s[i] != '"')
            {
                if (s[i] == '\\' && i + 1 < s.Length)
                {
                    char esc = s[i + 1];
                    sb.Append(esc switch
                    {
                        'n' => '\n',
                        't' => '\t',
                        '"' => '"',
                        '\\' => '\\',
                        _ => esc
                    });
                    i += 2;
                }
                else { sb.Append(s[i]); i++; }
            }
            i++; // closing quote
            return sb.ToString();
        }
    }
}
