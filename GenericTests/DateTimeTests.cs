using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericTests
{
    [TestClass]
    public class DateTimeTests
    {
        [TestMethod]
        public void HourCheck()
        {
            // arange
            var midnight = 0;
            var noon = 11;
            var elvenpm = 23;

            //act
            // var midnightMod = midnight % 0;  // this is a divide by zero excpetion as expected
            var noonMod = noon % 11;
            var elevenpmMod = elvenpm % 23;

            //assert
            //Assert.AreEqual(0, midnightMod);
            Assert.AreEqual(0, noonMod);
            Assert.AreEqual(0, elevenpmMod);
        }

        [TestMethod]
        public void ShortDateOutput()
        {
            var someday = new DateTime(2017, 4, 9).ToShortDateString();
            var somedayFormatted = new DateTime(2017, 4, 9).ToString("MM/dd/yyy");
            var expected = "04/09/2017";

            Assert.AreNotEqual(expected, someday);
            Assert.AreEqual(expected, somedayFormatted);
        }

        [TestMethod]
        public void ConvertToUTC()
        { 
            var now = DateTime.UtcNow;
            var northAmericanTimeZones = new List<string>(){
                "Atlantic Standard Time",
                "Eastern Standard Time",
                "US Eastern Standard Time",
                "Central Standard Time",
                "Canada Central Standard Time",
                "US Mountain Standard Time",
                "Mountain Standard Time",
                "Pacific Standard Time",
                "Alaskan Standard Time",
                "Hawaiian Standard Time"
            };
            var actual = new List<string>();

            northAmericanTimeZones.ForEach(timeZone => { 
                var tz = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
                var converted = TimeZoneInfo.ConvertTime(now, tz);
                actual.Add($"The date and time are {converted} {(tz.IsDaylightSavingTime(converted) ? tz.DaylightName : tz.StandardName)}");
            });

            Assert.IsTrue(actual.Any());


        }

        [TestMethod]
        public void FirstDayOfNextMonth()
        {
            var currentDate = new DateTime(2019, 03, 01); //System.DateTime.Now;
            var nextMonth = currentDate.AddMonths(1);
            var firstDayOfNextMonth = new DateTime(nextMonth.Year, nextMonth.Month, 1);

            Assert.IsNotNull(firstDayOfNextMonth);
        }

        [TestMethod]
        public void AddToMaxSubtractFromMin()
        {
            var max = DateTime.MaxValue;
            var min = DateTime.MinValue;

            //var add = max.AddDays(1);
            //var sub = min.AddDays(-1);

            //Assert.IsTrue(add == max);
            //Assert.IsTrue(sub == min);
        }

        [TestMethod]
        public void ShortDateTimeStringToObject()
        {
            var shortString = "2019-03-30";
            var dateObject = DateTime.Parse(shortString);

            Assert.IsTrue(dateObject.Hour == 0);
        }

    }
}
