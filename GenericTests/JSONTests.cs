using Microsoft.VisualStudio.TestTools.UnitTesting;
using TreeTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace GenericTests
{
    [TestClass()]
    public class JSONTests
    {
        [TestMethod()]
        public void ToStringTest()
        {
            var testNin = "123456789";
            var last4 = testNin.Substring(testNin.Length - 4);
            var classUnderTest = new FindAccountRequest()
            {
                Account = "123",
                FirstName = "test",
                LastName = "test",
                UserKey = "test",
                Nin = "123456789",
                BirthDate = new DateTime()
            };

            var json = classUnderTest.ToString();

            Assert.IsTrue(json.Contains(last4));
        }

        private string Thingy(FindAccountRequest obj)
        {
            JObject jo = JObject.FromObject(obj);
            jo["Nin"].Parent.Remove();

            var json = jo.ToString();
            return json;
        }
    }
}