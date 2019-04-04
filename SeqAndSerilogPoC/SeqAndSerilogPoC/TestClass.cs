using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;

namespace SeqAndSerilogPoC
{
    public class TestClass
    {
        // Fields ---------------------------------------------------------------------------------
        private readonly ILogger _logger;

        // Constructors ---------------------------------------------------------------------------
        internal TestClass() =>
            _logger = Log.ForContext<TestClass>();
        
        public static TestClass Create() =>
            new TestClass();

        // Public Functions -----------------------------------------------------------------------
        public void ContextAndDestructureTest()
        {
            var me = new Person()
            {
                FirstName = "Mike",
                LastName = "Underwood",
                Age = 35
            };

            _logger.Information("My Age: {Age}", me.Age);
            _logger.Information("Me: {@Person}", me);
        }

        public void CorrelationAndTimerTest()
        {
            using (_logger.BeginTimedOperation("CorrelationAndTimerTest", level: LogEventLevel.Debug))
            {
                var companies = new List<IdClass>();
                for(var i = 0; i < 100; i++)
                {
                    companies.Add(new IdClass() {
                        Id = Guid.NewGuid(),
                        CompanyName = AppHelper.RandomString(),
                        Address = AppHelper.RandomString()
                    });
                }

                foreach(var company in companies)
                {
                    var loopLogger = _logger.ForContext("Id", company.Id);
                    loopLogger.Verbose("Company: {@Company}", company);
                }
            }
        }
    }

    public class IdClass
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
    }

}
