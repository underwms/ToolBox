using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

// https://blogs.msdn.microsoft.com/dotnet/2016/08/24/whats-new-in-csharp-7-0/

namespace GenericTests
{
    public class MyClass
    {
        public int RandomNumberBetween1and10 { get; set; } = (new Random()).Next(1, 11);

        public int Length { get; set; }
        public int Width { get; set; }
        public int Area => Length * Width;

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{LastName}, {FirstName}";

        public int? NullableNumber { get; set; }
    }

    [TestClass]
    public class CSharpFeatures
    {
        [TestMethod]
        public void ClassIntiilizationandExpressionBody()
        {
            //arrange
            const int expectedArea = 4;
            const string exptectedName = "Underwood, Mike";
            var classUnderTest = new MyClass()
            {
                Length = 2,
                Width = 2,
                FirstName = "Mike",
                LastName = "Underwood"
            };

            //act
            var actualArea = classUnderTest.Area;
            var actualFullName = classUnderTest.FullName;
            var actualRandomNumber = classUnderTest.RandomNumberBetween1and10;
            classUnderTest.RandomNumberBetween1and10 = expectedArea;

            //assert
            Assert.AreEqual(expectedArea, actualArea);
            Assert.AreEqual(exptectedName, actualFullName);
            Assert.IsTrue(actualRandomNumber >= 1 && actualRandomNumber <= 10);
            Assert.AreEqual(expectedArea, classUnderTest.RandomNumberBetween1and10);
        }

        [TestMethod]
        public void NullExistanceOperator()
        {
            //arrange
            const int expected1 = 5;
            const int expected2 = 10;
            var testClass1 = new MyClass();
            var testClass2 = new MyClass() { NullableNumber = expected2 };

            //act
            var testResult1 = testClass1?.NullableNumber ?? expected1;
            var testResult2 = testClass2?.NullableNumber ?? expected1;
            //int testResult3 = (int)testClass1.NullableNumber;

            //assert
            Assert.AreEqual(expected1, testResult1);
            Assert.AreEqual(expected2, testResult2);
        }

        [TestMethod]
        public void DictionaryInitializer()
        {
            //arrange
            var classUnderTest = new MyClass()
            {
                Length = 2,
                Width = 2,
                FirstName = "Mike",
                LastName = "Underwood"
            };
            var testDictionary = new Dictionary<string, MyClass>()
            {
                ["key1"] = new MyClass(),
                ["key2"] = classUnderTest
            };

            //act
            var actualClass = testDictionary["key2"];

            //assert
            Assert.AreEqual(classUnderTest, actualClass);
        }

        [TestMethod]
        public void CatchFiltering()
        {
            try
            {
                //throw new Exception("THIS IS AN EXECPTION THAT WILL NOT BE FILTERED");
                throw new Exception("THIS IS AN EXECPTION THAT WILL BE FILTERED");
            }
            catch (Exception ex) when (ex.Message.Contains("WILL BE")) //<-- can be anything boolean evaluation
            { ; }
            catch (Exception ex)
            {
                // await LogMe(ex); <-- PRETEND THAT'S REAL
                ;
            }
        }

        [TestMethod]
        public void NameOf()
        {
            //arrange
            const string expected = "classUnderTest";
            var classUnderTest = new MyClass()
            {
                Length = 2,
                Width = 2,
                FirstName = "Mike",
                LastName = "Underwood"
            };

            //act
            var actual = nameof(classUnderTest);

            //assert
            Assert.AreEqual(expected, actual);
        }

        //output variables
        //public void PrintCoordinates(Point p)
        //{
        //    p.GetCoordinates(out int x, out int y);
        //    WriteLine($"({x}, {y})");
        //}

        //pattern matching
        //switch(shape)
        //{
        //    case Circle c:
        //        WriteLine($"circle with radius {c.Radius}");
        //        break;
        //    case Rectangle s when(s.Length == s.Height):
        //        WriteLine($"{s.Length} x {s.Height} square");
        //        break;
        //    case Rectangle r:
        //        WriteLine($"{r.Length} x {r.Height} rectangle");
        //        break;
        //    default:
        //        WriteLine("<unknown shape>");
        //        break;
        //    case null:
        //        throw new ArgumentNullException(nameof(shape));
        //}

        //local functions
        //public int Fibonacci(int x)
        //{
        //    if (x < 0) throw new ArgumentException("Less negativity please!", nameof(x));
        //    return Fib(x).current;

        //    (int current, int previous) Fib(int i)
        //    {
        //        if (i == 0) return (1, 0);
        //        var(p, pp) = Fib(i - 1);
        //        return (p + pp, p);
        //    }
        //}

        //tuples - a way to reutnr more than one value from a method
        //(string, string, string) LookupName(long id) // tuple return type
        //{
        //    ... // retrieve first, middle and last from data storage
        //    return (first, middle, last); // tuple literal
        //}

        //literal improvements
        //var someBinary = 0b1010_1011_1100_1101_1110_1111;
        //var someHex = 0xAB_CD_EF;
        //var _oneMillion = 1_000_000;

        //even more expression bodided members
        //public Person(string name) => names.TryAdd(id, name); // constructors
        //~Person() => names.TryRemove(id, out *);              // destructors
        //public Person(string name) => Name = name ?? throw new ArgumentNullException(name); //throw as an expression
    }
}
