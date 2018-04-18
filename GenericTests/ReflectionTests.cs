using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericTests
{
    [TestClass]
    public class ReflectionTests
    {
        [TestMethod]
        public void FindAttributesThatAreRequiredButAreBlank()
        {
            // arange
            var classUnderTest = new ProfileCreateRequest()
            {
                Email = "asdf",
                //ProfileUsername = "asdf",
                //ProfileUsername = string.Empty result == ""
                // ProfileUsername not set result == ProfileUsername
                BillingAccountNumber = 1234

            };

            // act
            var result = ReflectionAttributeCheck(classUnderTest);

            //assert
            //Assert.AreEqual("BillingAccountNumber", result);
            Assert.AreEqual("ProfileUsername", result);
        }

        private string ReflectionAttributeCheck(ProfileCreateRequest request)
        {
            var retVal = string.Empty;

            try
            {
                var properties = typeof(ProfileCreateRequest).GetProperties();

                foreach (var property in properties)
                {
                    var attributes = property.GetCustomAttributes(true);
                    foreach (var attribute in attributes)
                    {
                        if (attribute.GetType() == typeof(RequiredAttribute))
                        {
                            var value = property.GetValue(request);
                            if (ReferenceEquals(null, value))
                            { retVal = property.Name; break; }
                        }
                    }
                }
            }
            catch (Exception ex)
            { throw; }

            return retVal;
        }
    }
}
