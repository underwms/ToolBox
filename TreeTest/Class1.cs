//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace DevTest
//{
//    public enum NumberState
//    {
//        Postive,
//        Zero,
//        Negative
//    }

//    public interface IOperations
//    {
//        int Quotient(int dividend, int divisor);
//        int Product(int number1, int number2);
//    }

//    public class BaseClass
//    {
//        public BaseClass()
//        {
//            Number1 = (new Random()).Next(1, 11); //inclusive, exclusive
//            Number2 = (new Random()).Next(0, 10);
//        }

//        public BaseClass(int number1, int number2)
//        {
//            Number1 = number1;
//            Number2 = number2;
//        }

//        public int Number1 { get; }
//        public int Number2 { get; set; }
//    }

//    public class MyClass : BaseClass, IOperations
//    {
//        private const int _age = 100;
//        private string _firstName = "";

//        public MyClass()
//            : base()
//        { }

//        public MyClass(int number1, int number2)
//            : base(number1, number2)
//        { }

//        public int Age { get { return _age; } set { _age = value; } }
//        public string FirstName { get { return _firstName; } set { _firstName = value; } }
//        public string LastName { get; set; }
//        public List<int> FirstTen => new List<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};

//        public int Quotient(int dividend, int divisor)
//        {
//            try
//            { return dividend / divisor }
//            catch (Exception)
//            { throw; }
//        }

//        public int Difference(int number1, int number2)
//        {
//            var difference = number1 - number2;
//            return difference;
//        }
//    }
//}

////What's Wrong
////missing interface implementation of Product method
////no setter on Number1 property in BaseClass
////trying to set to a constant value Age property in MyClass
////missing semicolin in Quotient method

////Please Explain
////how would you catch divide my zero exceptions first
////what is the purpose of : base(...
////what happens if I call the default constructor of MyClass
////what happens if I call the overloaded constructor of MyClass with values of 1 and 2
////explain the differences in the synatx of the FirstName and LastName proerties
////explain var in Difference method

////Write Some Code
////implement Product
////create a Sum method on FirstTen List
////create a method where you return a list of only odd numbers using FirstTen list as your input
////create a method that updates the previous selection to make all odd values even
////user a ternary opperator to ensure a divide by zero exception can't happen in the Quotient method
////modify Differenc method to return proper NumberState enumeration instead of the actual difference 
