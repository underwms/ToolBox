using Newtonsoft.Json;
using System;
using System.Text;

namespace ToolBox
{
    public class JsonHelper
    {
        public static bool TryParse<T>(string json, out T obj)
        {
            obj = (T)Activator.CreateInstance(typeof(T));

            try { obj = JsonConvert.DeserializeObject<T>(json); }
            catch { obj = default(T); }

            return !ReferenceEquals(null, obj);
        }

        public static string ToJson<T>(T obj) =>
            JsonConvert.SerializeObject(obj);

        public static string ToFormattedString(string json)
        {
            if (string.IsNullOrEmpty(json))
                return string.Empty;

            var sb = new StringBuilder();
            var quote = false;
            var ignore = false;
            var offset = 0;
            const int indentLength = 3;

            json = json.Replace(Environment.NewLine, "").Replace("\t", "");
            foreach (var ch in json)
            {
                switch (ch)
                {
                    case '"':
                        if (!ignore) quote = !quote;
                        break;
                    case '\'':
                        if (quote) ignore = !ignore;
                        break;
                }

                if (quote)
                { sb.Append(ch); }
                else
                {
                    switch (ch)
                    {
                        case '{':
                        case '[':
                            sb.Append(ch);
                            sb.Append(Environment.NewLine);
                            sb.Append(new string(' ', ++offset * indentLength));
                            break;
                        case '}':
                        case ']':
                            sb.Append(Environment.NewLine);
                            sb.Append(new string(' ', --offset * indentLength));
                            sb.Append(ch);
                            break;
                        case ',':
                            sb.Append(ch);
                            sb.Append(Environment.NewLine);
                            sb.Append(new string(' ', offset * indentLength));
                            break;
                        case ':':
                            sb.Append(ch);
                            sb.Append(' ');
                            break;
                        default:
                            if (ch != ' ') sb.Append(ch);
                            break;
                    }
                }
            }

            return sb.ToString().Trim();
        }
    }
}
