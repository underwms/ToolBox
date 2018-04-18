using Microsoft.VisualStudio.TestTools.UnitTesting;
using TreeTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace GenericTests
{
    [TestClass()]
    public class SerializationTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
        }

        [TestMethod]
        public void PositionOfSerializedBaseClass()
        {
            //act
            var calssUnderTest = new AddBillingAccountRequest(Constants._environmentProdKey);

            //arrange
            var xml = BuildACIXml(calssUnderTest);

            //assert
            Assert.IsTrue(xml.Contains("identity"));
        }

        [TestMethod]
        public void DeserializeToBaseClass()
        {
            //act
            var responseXML =
                "<add-billing-account-response xmlns=\"http://www.princetonecom.com/fundingPortal/addbillingaccountrequest\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" +
                    "<message>" +
                        "<message-code>0</message-code>" +
                        "<message-text>STUB: Your request has been processed successfully.</message-text>" +
                    "</message>" +
                    "<fubar>ASDF</fubar>" +
                "</add-billing-account-response>"; ;

            //arrange
            var classUnderTest = ConsumeACIXml<AddBillingAccountResponse>(responseXML);

            //assert
            Assert.IsFalse(ReferenceEquals(classUnderTest, null));
        }

        [TestMethod]
        public void BoolParse()
        {
            // arrange
            const string true1 = "True";
            const string true2 = "true";
            //const string true3 = "1";
            const string false1 = "False";
            const string false2 = "false";
            //const string false3 = "0";

            // act
            var t1Actual = bool.Parse(true1);
            var t2Actual = bool.Parse(true2);
            //var t3Actual = bool.Parse(true3); // throws and exception
            var f1Actual = bool.Parse(false1);
            var f2Actual = bool.Parse(false2);
            //var f3Actual = bool.Parse(false3); // throws and exception

            // assert
            Assert.IsTrue(t1Actual);
            Assert.IsTrue(t2Actual);
            //Assert.IsTrue(t3Actual);
            Assert.IsFalse(f1Actual);
            Assert.IsFalse(f2Actual);
            //Assert.IsFalse(f3Actual);
        }

        private string BuildACIXml<T>(T obj)
            where T : IACIRequestEntity
        {
            try
            {
                var xmlStr = string.Empty;

                var settings = new XmlWriterSettings()
                {
                    Indent = false,
                    OmitXmlDeclaration = false,
                    NewLineChars = string.Empty,
                    NewLineHandling = NewLineHandling.None
                };

                using (var stringWriter = new UTF8StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
                    {
                        var namespaces = new XmlSerializerNamespaces();
                        namespaces.Add(string.Empty, obj.XmlNameSpace);

                        var serializer = new XmlSerializer(obj.GetType());
                        serializer.Serialize(xmlWriter, obj, namespaces);

                        xmlStr = stringWriter.ToString();
                    }
                }

                return xmlStr;
            }
            catch (Exception ex)
            { throw; }
        }

        private T ConsumeACIXml<T>(string xml)
            where T : IACIResponseEntity
        {
            try
            {
                var obj = new object();
                using (var stringReader = new StringReader(xml))
                {
                    using (var xmlReader = new XmlTextReader(stringReader))
                    {
                        var serializer = new XmlSerializer(typeof(T));
                        obj = serializer.Deserialize(xmlReader);
                    }
                }

                return (T)obj;
            }
            catch (Exception)
            { throw; }
        }

        [TestMethod]
        public void RepeatingNodeSerialization()
        {
            // arrange
            var testXML = "<ROOT xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><some-string>SOMESTRING</some-string>" +
                    "<object><Id>1</Id></object><object><Id>2</Id></object>" +
                "</ROOT>";

            var classUnderTest = new TestClass();

            // act
            try
            {
                classUnderTest = ConsumeXml<TestClass>(testXML);
            }
            catch (Exception ex)
            {
                throw;
            }

            // assert
            Assert.AreEqual(2, classUnderTest.MyList.Count());
        }

        public T ConsumeXml<T>(string xml)
        {
            var obj = new object();
            using (var stringReader = new StringReader(xml))
            {
                using (var xmlReader = new XmlTextReader(stringReader))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    obj = serializer.Deserialize(xmlReader);
                }
            }

            return (T)obj;
        }
    }
}

[XmlRoot(ElementName = "ROOT")]
public class TestClass
{
    public TestClass()
    {
        SomeString = string.Empty;
        MyList = new List<BaseObject2>();
    }

    [XmlElement(ElementName = "some-string")]
    public string SomeString { get; set; }
    

    [XmlElement("object")]
    public List<BaseObject2> MyList { get; set; }
}

public class BaseObject2
{
    public long Id { get; set; }
}

public class UTF8StringWriter : StringWriter
{
    public override Encoding Encoding => Encoding.UTF8;
}


