using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericTests
{
    [TestClass]
    public class NullTests
    {
        [TestMethod]
        public void QuestionDotOperator()
        {
            //arrange
            const string NULLSTRING = "NULL";
            var classUnderTest = new ClassMain();

            //act
            var resultOperatorOnProperty = classUnderTest.NullClass?.NullProperty ?? NULLSTRING;
            //var resultOperatorOnSubClass = classUnderTest?.NullClass.NullProperty ?? NULLSTRING; // this will throw an exception
            var resultOperatorOnBoth = classUnderTest?.NullClass?.NullProperty ?? NULLSTRING; // this is redundant as resultOperatorOnProperty

            //assert
            Assert.AreEqual(NULLSTRING, resultOperatorOnProperty);
            //Assert.AreEqual(NULLSTRING, resultOperatorOnSubClass);
            Assert.AreEqual(NULLSTRING, resultOperatorOnBoth);
        }

        [TestMethod]
        public void IsNullOrWhiteSpace()
        {
            //arrange
            var classUnderTest = new ClassMain();

            //act
            //var actual = string.IsNullOrWhiteSpace(classUnderTest.NullClass.NullProperty); // this will throw an exception
            var actual = string.IsNullOrWhiteSpace(classUnderTest.NullClass?.NullProperty);

            //assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void AnyOnNull()
        {
            //arrange
            List<int> myList = new List<int>(){1,2,3};

            //act
            var any = myList?.Any() ?? false;

            //assert
            Assert.IsFalse(any);
        }

        [TestMethod]
        public void ListIsNullAfterIntializing()
        {
            var actual = new List<ClassSub>();
            actual = FunctionRetrunNull().ToList();

            Assert.IsNull(actual);
        }


        public IEnumerable<ClassSub> FunctionRetrunNull() => null;
    }

    public class ClassMain
    {
        public ClassSub NullClass { get; set; }
    }

    public class ClassSub
    {
        public string NullProperty { get; set; }
    }
}
