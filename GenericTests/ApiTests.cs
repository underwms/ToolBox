using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToolBox;

namespace GenericTests
{
    [TestClass]
    public class ApiTests
    {
        [TestMethod]
        public async Task GetTest()
        {
            // arrange
            var url = @"https://localhost:44340/api/values/";
            var apiHelper = new ApiHelper();

            // act
            var response = await apiHelper.GetAsync<string>(url);

            // assert
            Assert.IsTrue(response.Any());
        }
    }
}
