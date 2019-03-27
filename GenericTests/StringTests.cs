using System;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;

namespace GenericTests
{
    [TestClass]
    public class StringTests
    {

        [TestMethod]
        public void Contains()
        {
            //arrange
            var fullName = "provider_C7430_FILE_FIELD";
            var stringUnderTest = "7430_FILE_FIELD";

            //act

            //assert
            Assert.IsTrue(fullName.Contains(stringUnderTest));
        }

        [TestMethod]
        public void URLSplit()
        {
            //arrange
            var testKey = Guid.NewGuid();
            var url = $"https://www.blah.com?ssoKey={testKey}";

            //act
            var urlSplit = url.Split(new string[] { "ssoKey=" }, StringSplitOptions.RemoveEmptyEntries);
            var ssoKey = urlSplit.Length != 2 ? string.Empty : urlSplit[1];

            //assert
            Assert.AreEqual(testKey, ssoKey);
        }

        [TestMethod]
        public void ShortDate()
        {
            //arrange
            var today = DateTime.Today;
            var expected = $"{today.Year}/{today.Month}/{today.Day}";
            var expected2 = String.Format("{0:yyyy/MM/dd}", today);

            //act
            var actual = today.ToShortDateString(); // M/d/yyyy

            //assert
            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public void ListOutput()
        {
            //arrange
            var expected = "1, 2, 3, 4, 5";
            var listUnderTest = new List<int>() { 1, 2, 3, 4, 5 };

            //act
            var actual = string.Join(", ", listUnderTest);

            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RandomString()
        {
            var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var randomString = new string(chars.Select(c => chars[random.Next(chars.Length)]).Take(8).ToArray());

            Assert.IsTrue(!string.IsNullOrWhiteSpace(randomString));
        }

        [TestMethod]
        public void RandomString2()
        {
            var take = 10;
            var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            var data = new byte[1];

            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[take];
                crypto.GetNonZeroBytes(data);
            }

            var result = new StringBuilder(take);
            foreach (byte b in data)
            { result.Append(chars[b % (chars.Length)]); }

            Assert.IsTrue(!string.IsNullOrWhiteSpace(result.ToString()));
        }

        [TestMethod]
        public void TakeLastFour()
        {
            //arrange
            const string ssnIncomplete = "123";
            const string ssnComplete = "123456789";

            //act
            var lastFourIncomplete = ((ssnIncomplete.Length >= 4) ? (ssnIncomplete.Substring(ssnIncomplete.Length - 4)) : (string.Empty));
            var lastFourComplete = ((ssnComplete.Length >= 4) ? (ssnComplete.Substring(ssnComplete.Length - 4)) : (string.Empty));

            //assert
            Assert.AreEqual(string.Empty, lastFourIncomplete);
            Assert.AreEqual("6789", lastFourComplete);
        }

        [TestMethod]
        public void ToUpperWithNumbers()
        {
            //arrange
            var stringUnderTest = "123 Test STREET";
            const string expected = "123 TEST STREET";

            //act
            var actual = stringUnderTest.ToUpper();

            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TrailingZeros()
        {
            //arrange
            var phone1 = "123";
            var phone2 = "5138272509";
            var phone3 = "5138272509 ext 123";
            var phone4 = "1-(513)-827-2509";

            //act
            var newPhone1 = new string(phone1.Where(c => char.IsDigit(c)).ToArray()).PadRight(10, '0').Substring(0, 10);
            var newPhone2 = new string(phone2.Where(c => char.IsDigit(c)).ToArray()).PadRight(10, '0').Substring(0, 10);
            var newPhone3 = new string(phone3.Where(c => char.IsDigit(c)).ToArray()).PadRight(10, '0').Substring(0, 10);
            var newPhone4 = new string(phone4.Where(c => char.IsDigit(c)).ToArray()).PadRight(10, '0').Substring(0, 10);

            var fomrattedPhone = $"({phone2.Substring(0, 3)}) {phone2.Substring(3, 3)}-{phone2.Substring(6, 4)}";

            //assert
            Assert.AreEqual("1230000000", newPhone1);
            Assert.AreEqual("5138272509", newPhone2);
            Assert.AreEqual("5138272509", newPhone3);
            Assert.AreEqual("1513827250", newPhone4);
        }

        [TestMethod]
        public void Equals()
        {
            // arrange
            var ln1 = "dwyer";
            var ln2 = "Dwyer";
            var ln3 = "DWYER";
            var result = false;

            // act
            result = (ln1 == ln2);
            result = (ln1 == ln3);
            result = (ln2 == ln1);
            result = (ln2 == ln3);
            result = (ln3 == ln1);
            result = (ln3 == ln2);

            result = ln1.Equals(ln2);
            result = ln1.Equals(ln3);
            result = ln2.Equals(ln1);
            result = ln2.Equals(ln3);
            result = ln3.Equals(ln1);
            result = ln3.Equals(ln2);
            // ALL FALSE
        }

        [TestMethod]
        public void ComplexSplit()
        {
            //arange
            var agentName = "Huertas, Ivelisse), 2401 (Griffin, Cheryl";
            var expectedFirstName = "Cheryl";
            var expectedLastName = "Griffin";

            //act
            var nameSplit = ComplexSplit(agentName);

            //assert
            Assert.AreEqual(expectedFirstName, nameSplit.ElementAt(0));
            Assert.AreEqual(expectedLastName, nameSplit.ElementAt(1));

            //-------------------------------------------------------------------------------------
            //arange
            agentName = "Davis, Rosalind";
            expectedFirstName = "Rosalind";
            expectedLastName = "Davis";

            //act
            nameSplit = ComplexSplit(agentName);

            //assert
            Assert.AreEqual(expectedFirstName, nameSplit.ElementAt(0));
            Assert.AreEqual(expectedLastName, nameSplit.ElementAt(1));

            //-------------------------------------------------------------------------------------
            agentName = "Munoz Torres, Jeisa";
            expectedFirstName = "Jeisa";
            expectedLastName = "Munoz Torres";

            //act
            nameSplit = ComplexSplit(agentName);

            //assert
            Assert.AreEqual(expectedFirstName, nameSplit.ElementAt(0));
            Assert.AreEqual(expectedLastName, nameSplit.ElementAt(1));

            //-------------------------------------------------------------------------------------
            agentName = "C Noyes BK";
            expectedFirstName = "C Noyes BK";
            expectedLastName = string.Empty;

            //act
            nameSplit = ComplexSplit(agentName);

            //assert
            Assert.AreEqual(expectedFirstName, nameSplit.ElementAt(0));
            //Assert.AreEqual(expectedLastName, nameSplit.ElementAt(1));    //there is no element 1

            //-------------------------------------------------------------------------------------
            agentName = "6466422924, 8547(Noyes, Chelsey)";
            expectedFirstName = "Chelsey";
            expectedLastName = "Noyes";

            //act
            nameSplit = ComplexSplit(agentName);

            //assert
            Assert.AreEqual(expectedFirstName, nameSplit.ElementAt(0));
            Assert.AreEqual(expectedLastName, nameSplit.ElementAt(1));


        }

        [TestMethod]
        public void URLEncoding()
        {
            //arange
            var unescaped = "m+S#u@gmail.com";
            var expected = "m%2bS%23u%40gmail.com"; //interesting that the '.' is not encoded

            //act
            var escaped = HttpUtility.UrlEncode(unescaped, System.Text.Encoding.UTF8);

            //assert
            Assert.AreEqual(expected, escaped);

            //////////////////////////////////////////////////////////////////////////////////

            unescaped = string.Empty;
            escaped = HttpUtility.UrlEncode(unescaped, System.Text.Encoding.UTF8);
            Assert.AreEqual(unescaped, escaped);

            //////////////////////////////////////////////////////////////////////////////////

            unescaped = null;
            escaped = HttpUtility.UrlEncode(unescaped, System.Text.Encoding.UTF8);
            Assert.AreEqual(unescaped, escaped);
        }

        [TestMethod]
        public void GuidForRandomStringGeneration()
        {
            var listOfRandomStrings = new List<string>();

            for (int x = 0; x < 10; x++)
            {
                //var newGuid = Guid.NewGuid().ToString().Replace("-", string.Empty);
                //var guidAsArray = newGuid.ToCharArray().Take(10);

                //listOfRandomStrings.Add(string.Join("", guidAsArray));

                listOfRandomStrings.Add(string.Join("", Guid.NewGuid().ToString().Replace("-", string.Empty).Take(10)));
            }

            Assert.AreEqual(10, listOfRandomStrings.Count);
        }

        [TestMethod]
        public void SubStringOfDifferentLengths()
        {
            var s1 = "12345";
            var s2 = "1234";
            var s3 = "123456";

            var ss1 = s1.Substring(0, 5);
            //var ss2 = s2.Substring(0, 5);
            var ss3 = s3.Substring(0, 5);

            Assert.IsTrue(ss1.Length == 5);
            //Assert.IsTrue(ss2.Length == 5);
            Assert.IsTrue(ss3.Length == 5);
            
        }

        private IEnumerable<string> ComplexSplit(string agentName)
        {
            return agentName.Split(',')
                            .ToList()
                            .Select(x => Regex.Replace(x, "[^A-Za-z ]+", string.Empty).Trim())
                            .Reverse()
                            .Take(2);
        }
    }
}
