using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericTests
{
    [TestClass]
    public class BooleanTest
    {
        [TestMethod]
        public void OrTest()
        {
            // arrange
            var lasname = "";
            var ninLast4 = "1234";
            var dob = "01/01";

            // act
            var actual = (
                string.IsNullOrWhiteSpace(lasname) || 
                (string.IsNullOrWhiteSpace(ninLast4) && string.IsNullOrWhiteSpace(dob))
            );

            // assert
            Assert.IsFalse(actual);
        }
    }
}
