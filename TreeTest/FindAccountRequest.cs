namespace TreeTest
{
    using System;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class FindAccountRequest
    {
        public string UserKey { get; set; }

        public string Account { get; set; }
        
        public string Nin { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        private string NinLog
        {
            get
            {
                try
                { return Nin.Substring(Nin.Length - 4); }
                catch (Exception)
                { return string.Empty; }
            }
        }

        public override string ToString()
        {
            var genericJsonObject = JObject.FromObject(this);
            var ninLogProperty = new JProperty("Nin", this.NinLog);

            genericJsonObject.Property("Nin").Remove();
            genericJsonObject.Add(ninLogProperty);

            var json = genericJsonObject.ToString();
            return json;
        }
    }
}
