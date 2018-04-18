using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeTest
{
    public class Account
    {
        public string AccountNumber { get; set; }
        public string SocialSecurityNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Name { get; set; }

        public static bool AreEqual(Account @base, Account input, List<InputEnum> proertiesToValidate)
        {
            //declarations
            var matchFound = false;
            var accountProperties = typeof(Account).GetProperties();

            //iterate over inputed list of properties
            foreach (var propertyToValidate in proertiesToValidate)
            {
                //iterate over account properties
                foreach(var accountProptery in accountProperties)
                {
                    //check if property to validate is an account property
                    if (accountProptery.Name == propertyToValidate.ToString())
                    {
                        //get values via reflection
                        var baseValue = (int)accountProptery.GetValue(@base);
                        var inputValue = accountProptery.GetValue(input);

                        //compare
                        matchFound = baseValue.Equals(inputValue);
                        break;
                    }
                    //check that property is not there
                    else if (propertyToValidate.ToString().Contains("_no") &&
                             accountProptery.Name == propertyToValidate.ToString().Replace("_no", ""))
                    {
                        var inputValue = accountProptery.GetValue(input);

                        // compare
                        matchFound = ReferenceEquals(null, inputValue);
                        break;
                    }
                }

                //if no match was found there is no need to match any other properties
                if (!matchFound)
                { break; }
            }

            return matchFound;
        }
    }
}
