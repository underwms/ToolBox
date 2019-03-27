using Newtonsoft.Json;
using System;
using System.IO;
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

        public static string ToJson<T>(T obj, bool format = false) =>
            format
                ? ToFormattedString(JsonConvert.SerializeObject(obj))
                : JsonConvert.SerializeObject(obj);

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

        public static void SerializeJsonIntoStream(object value, Stream stream)
        {
            using (var sw = new StreamWriter(stream, new UTF8Encoding(false), 1024, true))
            using (var jtw = new JsonTextWriter(sw) { Formatting = Formatting.None })
            {
                var js = new JsonSerializer();
                js.Serialize(jtw, value);

                jtw.Flush();
            }
        }

        public static T DeserializeJsonFromStream<T>(Stream stream)
        {
            if (stream == null || stream.CanRead == false)
            { return default(T); }
            
            using (var sr = new StreamReader(stream))
            using (var jtr = new JsonTextReader(sr))
            {
                var js = new JsonSerializer();
                var retVal = js.Deserialize<T>(jtr);

                return retVal;
            }
        }
    }
}
