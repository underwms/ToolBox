using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ExpressionTreeRuleEngine;
using System.Linq.Expressions;
using System.Linq;

namespace GenericTests
{
    [TestClass]
    public class RuleEngineTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            //arrange
            var rules = new List<Rule>
            {
                 // Create some rules using LINQ.ExpressionTypes for the comparison operators
                 new Rule ( "Year", ExpressionType.GreaterThan, 2012),
                 new Rule ( "Make", ExpressionType.Equal, "El Diablo"),
                 new Rule ( "Model", ExpressionType.Equal, "Torch" )
            };

            var carsToTest = new List<Car>() {
                new Car() { // will fail
                    Year = 2011,
                    Make = "El Diablo",
                    Model = "Torche"
                },
                new Car() { // will pass
                    Year = 2015,
                    Make = "El Diablo",
                    Model = "Torch"
                },
                new Car() { // will fail
                    Year = 1990,
                    Make = "Chevy",
                    Model = "Cavalier"
                }
            };

            var carsThatPassTheRules = new List<Car>();
            var carsThatFailTheRules = new List<Car>();

            //act
            var compiledRules = PrecompiledRules.CompileRuleForList(new List<ICar>(), rules);
            carsToTest.ForEach(car => {
                var passedRules = compiledRules.TakeWhile(rule => rule(car));
                if (passedRules.Count() == compiledRules.Count())
                { carsThatPassTheRules.Add(car); }
                else
                { carsThatFailTheRules.Add(car); }
            });

            //assert
            Assert.IsTrue(carsThatPassTheRules.Count() == 1);
            Assert.IsTrue(carsThatFailTheRules.Count() == 2);
        }

        [TestMethod]
        public void CustomRulesWithOutCome()
        {
            //arrange
            var setlementRules = new List<SettlementRule>() {
                new SettlementRule() {
                    Id = 1,
                    Description = "PlaceTypeClassId = 20 and PlaceTypeId not in (990,710,1038,1037)",
                    Rules = new List<Rule> {
                        new Rule ( "PlaceTypeClassId", ExpressionType.Equal, 20),
                        new Rule ( "PlaceTypeId", ExpressionType.NotEqual, 990),
                        new Rule ( "PlaceTypeId", ExpressionType.NotEqual, 710),
                        new Rule ( "PlaceTypeId", ExpressionType.NotEqual, 1038),
                        new Rule ( "PlaceTypeId", ExpressionType.NotEqual, 1037),
                    },
                    Result = string.Empty
                },
                new SettlementRule() {
                    Id = 2,
                    Description = "Status in (SLD, PIF, CRC, WRI)",
                    Rules = new List<Rule> {
                        new Rule ( "Status", ExpressionType.Equal, "SLD"),
                        new Rule ( "Status", ExpressionType.Equal, "PIF"),
                        new Rule ( "Status", ExpressionType.Equal, "CRC"),
                        new Rule ( "Status", ExpressionType.Equal, "WRI")
                    },
                    Result = string.Empty
                },
                new SettlementRule() {
                    Id = 3,
                    Description = "PlaceTypeId in (40,460,850,950,1001)",
                    Rules = new List<Rule> {
                        new Rule ( "PlaceTypeId", ExpressionType.Equal, 40),
                        new Rule ( "PlaceTypeId", ExpressionType.Equal, 460),
                        new Rule ( "PlaceTypeId", ExpressionType.Equal, 850),
                        new Rule ( "PlaceTypeId", ExpressionType.Equal, 950),
                        new Rule ( "PlaceTypeId", ExpressionType.Equal, 1001)
                    },
                    Result = string.Empty
                },
                new SettlementRule() {
                    Id = 3,
                    Description = "PlaceTypeId = 110 AND JudgmentDate is not null",
                    Rules = new List<Rule> {
                        new Rule ( "PlaceTypeId", ExpressionType.Equal, 110),
                        new Rule ("JudgmentDateIsNull", ExpressionType.Equal, false)
                    },
                    Result = "50% of Current Balance, [Amount]"
                }
                ,
                new SettlementRule() {
                    Id = 3,
                    Description = "PlaceTypeId = 110 AND JudgmentDate is null",
                    Rules = new List<Rule> {
                        new Rule ( "PlaceTypeId", ExpressionType.Equal, 110),
                        new Rule ( "JudgmentDateIsNull", ExpressionType.Equal, true)
                    },
                    Result = "25% of Current Balance, [Amount]"
                }
            };

            var accountUnderTest = new RuleTestAccount()
            {
                PlaceTypeId = 110,
                JudgmentDate = new DateTime()
            };
            var listUnderTest = new List<IAccount>() { accountUnderTest };

            var results = new List<string>();

            //act
            foreach (var settlementRule in setlementRules)
            {
                var compiledRules = PrecompiledRules.CompileRuleForSingleObject(new RuleTestAccount(), settlementRule.Rules);
                var passedRules = compiledRules.TakeWhile(rule => rule(accountUnderTest));

                if (passedRules.Count() == compiledRules.Count())
                { results.Add(settlementRule.Result); break; }
            }
            //foreach (var settlementRule in setlementRules)
            //{
            //    var compiledRules = PrecompiledRules.CompileRuleForList(new List<IAccount>(), settlementRule.Rules);
            //    listUnderTest.ForEach(account =>
            //    {
            //        var passedRules = compiledRules.TakeWhile(rule => rule(account));
            //        if (passedRules.Count() == compiledRules.Count())
            //        { results.Add(settlementRule.Result); }
            //    });

            //    if (results.Count() > 0) { break; }
            //}

            //assert
            Assert.AreEqual(1, results.Count());
        }
    }

    public interface IAccount
    {
        long Id { get; set; }

        int PlaceTypeClassId { get; set; }

        int PlaceTypeId { get; set; }

        string Status { get; set; }

        int ServicerId { get; set; }

        DateTime? JudgmentDate { get; set; }

        DateTime? SatuteDate { get; set; }

        bool JudgmentDateIsNull { get; }

        bool SatuteDateIsNull { get; }
    }

    public class RuleTestAccount : IAccount
    {
        public long Id { get; set; }

        public int PlaceTypeClassId { get; set; }

        public int PlaceTypeId { get; set; }

        public string Status { get; set; }

        public int ServicerId { get; set; }

        public DateTime? JudgmentDate { get; set; }

        public DateTime? SatuteDate { get; set; }

        public bool JudgmentDateIsNull => !JudgmentDate.HasValue;

        public bool SatuteDateIsNull => !SatuteDate.HasValue;
    }

    public class SettlementRule
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public List<Rule> Rules { get; set; } = new List<Rule>();

        public string Result { get; set; }
    }
}
