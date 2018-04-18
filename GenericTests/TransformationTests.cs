using Microsoft.VisualStudio.TestTools.UnitTesting;
using TreeTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericTests
{
    [TestClass()]
    public class TransformationTests
    {
        public List<QA> _initialClass = new List<QA>() { };

        [TestInitialize]
        public void TestInitialize()
        {
            _initialClass = new List<QA>()
            {
                new QA()
                {
                    QuestionTypeID = 1,
                    QuestionType = "Last Name",
                    QuestionID = 9,
                    QuestionDescription = "What is maximum airspeed velocity of an unladen swallow?",
                    AnswerID = 50,
                    AnswerDescription = "SANCHEZ"
                },
                new QA()
                {
                    QuestionTypeID = 1,
                    QuestionType = "Last Name",
                    QuestionID = 9,
                    QuestionDescription = "What is maximum airspeed velocity of an unladen swallow?",
                    AnswerID = 66,
                    AnswerDescription = "Yelich"
                },
                new QA()
                {
                    QuestionTypeID = 1,
                    QuestionType = "Last Name",
                    QuestionID = 9,
                    QuestionDescription = "What is maximum airspeed velocity of an unladen swallow?",
                    AnswerID = 83,
                    AnswerDescription = "Dales"
                },
                new QA()
                {
                    QuestionTypeID = 3,
                    QuestionType = "Date of Birth",
                    QuestionID = 3,
                    QuestionDescription = "When is your birth date?",
                    AnswerID = 261,
                    AnswerDescription = "09/01/1953"
                },
                new QA()
                {
                    QuestionTypeID = 3,
                    QuestionType = "Date of Birth",
                    QuestionID = 3,
                    QuestionDescription = "When is your birth date?",
                    AnswerID = 219,
                    AnswerDescription = "09/19/1975"
                },
                new QA()
                {
                    QuestionTypeID = 3,
                    QuestionType = "Date of Birth",
                    QuestionID = 3,
                    QuestionDescription = "When is your birth date?",
                    AnswerID = 251,
                    AnswerDescription = "12/21/1986"
                },
                new QA()
                {
                    QuestionTypeID = 5,
                    QuestionType = "State",
                    QuestionID = 5,
                    QuestionDescription = "What state do you live in?",
                    AnswerID = 411,
                    AnswerDescription = "GA"
                },
                new QA()
                {
                    QuestionTypeID = 5,
                    QuestionType = "State",
                    QuestionID = 5,
                    QuestionDescription = "What state do you live in?",
                    AnswerID = 401,
                    AnswerDescription = "AK"
                },
                new QA()
                {
                    QuestionTypeID = 5,
                    QuestionType = "State",
                    QuestionID = 5,
                    QuestionDescription = "What state do you live in?",
                    AnswerID = 404,
                    AnswerDescription = "AZ"
                },
            };
        }

        [TestMethod]
        public void ListExclude()
        {
            //act
            ;

            //arrange
            var results = Transform(_initialClass);

            //assert
            Assert.IsTrue(results.Count == 3);
        }

        private List<Question> Transform(List<QA> dbResults)
        {
            var retVal = new List<Question>();

            try
            {
                var questionTypes = dbResults.Select(x => x.QuestionTypeID)
                                             .Distinct();

                foreach (var type in questionTypes)
                {
                    var question = dbResults.Where(x => x.QuestionTypeID == type)
                                            .Select(x => new Question()
                                            {
                                                QuestionTypeId = x.QuestionTypeID,
                                                QuestionId = x.QuestionID,
                                                QuestionDescription = x.QuestionDescription,
                                            })
                                            .Distinct()
                                            .FirstOrDefault();

                    var answers = dbResults.Where(x => x.QuestionTypeID == type)
                                           .Select(x => new Answer()
                                           {
                                               QuestionTypeId = x.QuestionTypeID,
                                               AnswerId = x.AnswerID,
                                               AnswerDescription = x.AnswerDescription,
                                           })
                                           .Distinct()
                                           .ToList();

                    question.Answers = answers;

                    retVal.Add(question);
                }

                return retVal;
            }
            catch (Exception)
            { throw; }
        }
    }
}

