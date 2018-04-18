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
    public class TreeNodeTests
    {
        private TreeNode _tree = new TreeNode(InputEnum.Root)
        {
            Children = new List<TreeNode>()
            {
                new TreeNode(InputEnum.AccountNumber)
                {
                    Children = new List<TreeNode>()
                    {
                        new TreeNode(InputEnum.SocialSecurityNumber)
                        {
                            Children = new List<TreeNode>()
                            {
                                new TreeNode(InputEnum.DateOfBirth)
                                {
                                    QuestionsToAsk = new QuestionList()
                                    { Questions = new List<QuestionTypes>() { QuestionTypes.Exact } }
                                }//end account number --> social security --> date of birth branch
                            },
                            QuestionsToAsk = new QuestionList()
                            {
                                Questions = new List<QuestionTypes>()
                                {
                                    QuestionTypes.Name,        
                                    QuestionTypes.Address,
                                    QuestionTypes.DateOfBirth,
                                    QuestionTypes.Phone,
                                    QuestionTypes.Zip,          
                                    QuestionTypes.City          
                                }
                            }
                        },//end account number --> social secutiry brnach
                        new TreeNode(InputEnum.Name)
                        {
                            QuestionsToAsk = new QuestionList()
                            {
                                Questions = new List<QuestionTypes>()
                                {
                                    QuestionTypes.SocialSecurityNumber, 
                                    QuestionTypes.Address,
                                    QuestionTypes.DateOfBirth,  
                                    QuestionTypes.Phone,
                                    QuestionTypes.Zip,                  
                                    QuestionTypes.City                  
                                }
                            }
                        },//end account number --> name branch
                        new TreeNode(InputEnum.DateOfBirth)
                        {
                            QuestionsToAsk = new QuestionList()
                            {
                                Questions = new List<QuestionTypes>()
                                {
                                    QuestionTypes.Name,  
                                    QuestionTypes.SocialSecurityNumber,
                                    QuestionTypes.Address,
                                    QuestionTypes.Phone,
                                    QuestionTypes.Zip,
                                    QuestionTypes.City
                                }
                            }
                        }//end account number --> date of birth branch
                    },
                    QuestionsToAsk = new QuestionList()
                    {
                        Questions = new List<QuestionTypes>()
                        {
                            QuestionTypes.Name,
                            QuestionTypes.SocialSecurityNumber,
                            QuestionTypes.Address,
                            QuestionTypes.DateOfBirth,
                            QuestionTypes.Phone,
                            QuestionTypes.Zip,
                            QuestionTypes.City
                        }
                    }
                },  //end account number search brnach
                new TreeNode(InputEnum.AccountNumber_no)
                {
                    Children = new List<TreeNode>()
                    {
                        new TreeNode(InputEnum.Name)
                        {
                            Children = new List<TreeNode>()
                            {
                                new TreeNode(InputEnum.SocialSecurityNumber)
                                {
                                    Children = new List<TreeNode>()
                                    {
                                        new TreeNode(InputEnum.DateOfBirth)
                                        {
                                            QuestionsToAsk = new QuestionList()
                                            {
                                                Questions = new List<QuestionTypes>()
                                                {
                                                    QuestionTypes.Name,
                                                    QuestionTypes.Address,
                                                    QuestionTypes.DateOfBirth,
                                                    QuestionTypes.Phone,
                                                    QuestionTypes.Zip,
                                                    QuestionTypes.City
                                                }
                                            }
                                        }//end no account number --> name --> social security number --> date of birth branch
                                    },
                                    QuestionsToAsk = new QuestionList()
                                    {
                                        Questions = new List<QuestionTypes>()
                                        {
                                            QuestionTypes.Address,
                                            QuestionTypes.DateOfBirth,
                                            QuestionTypes.Phone,
                                            QuestionTypes.Zip,
                                            QuestionTypes.City
                                        }
                                    }
                                },// end no account number --> name --> social security number brnach
                                new TreeNode(InputEnum.DateOfBirth)
                                {
                                    QuestionsToAsk = new QuestionList()
                                    {
                                        Questions = new List<QuestionTypes>()
                                        {
                                            QuestionTypes.Name,
                                            QuestionTypes.Address,
                                            QuestionTypes.Phone,
                                            QuestionTypes.Zip,
                                            QuestionTypes.City
                                        }
                                    }
                                }//end no acccount numvber --> name --> date of birth branch
                            }
                        },//end no account number --> name branch 
                            //*** NOTE this bnanch does not contain questions as it is not a validating branch***
                        new TreeNode(InputEnum.SocialSecurityNumber)
                        {
                            Children = new List<TreeNode>()
                            {
                                new TreeNode(InputEnum.DateOfBirth)
                                {
                                    QuestionsToAsk = new QuestionList()
                                    {
                                        Questions = new List<QuestionTypes>()
                                        {
                                            QuestionTypes.SocialSecurityNumber,
                                            QuestionTypes.Address,
                                            QuestionTypes.Phone,
                                            QuestionTypes.Zip,
                                            QuestionTypes.City
                                        }
                                    }
                                }//end no account number --> social security number --> date of birth branch
                            },
                            QuestionsToAsk = new QuestionList()
                            { Questions = new List<QuestionTypes>() { QuestionTypes.None } }
                        }//end no account number --> social security number branch
                    }
                } //end no account number search branch
            }//end root
        };//end tree
        private Account _dbResults = new Account();
        private Account _userInput = new Account();

        [TestInitialize]
        public void TestInitialize()
        {
            _dbResults = new Account()
            {
                AccountNumber = "123",
                Name = "test",
                SocialSecurityNumber = "123456789",
                DateOfBirth = new DateTime(2016, 01, 01)
            };
            _userInput = new Account()
            {
                AccountNumber = "123",
                Name = "test",
                SocialSecurityNumber = "123456789",
                DateOfBirth = new DateTime(2016, 01, 01)
            };
        }

        [TestMethod()]
        public void AccountsWillEvaluateAreEqual()
        {
            //arrange
            var path = new List<InputEnum>() { InputEnum.AccountNumber };
            
            //act
            var results = Account.AreEqual(_dbResults, _userInput, path);

            //assert
            Assert.IsTrue(results);
        }

        [TestMethod()]
        public void Branch1_AccountNumber_SocialSecurityNumber_DateOfBirt()
        {
            //arrange
            _userInput = new Account()
            {
                AccountNumber = "123",
                SocialSecurityNumber = "123456789",
                DateOfBirth = new DateTime(2016, 01, 01)
            };

            //act
            var results = FindQuestions();

            //assert
            Assert.IsTrue(results.Questions.Any());
        }

        [TestMethod()]
        public void Branch2_AccountNumber_SocialSecurityNumber()
        {
            //arrange
            _userInput = new Account()
            {
                AccountNumber = "123",
                SocialSecurityNumber = "123456789"
            };

            //act
            var results = FindQuestions();

            //assert
            Assert.IsTrue(results.Questions.Any());
        }

        [TestMethod()]
        public void Branch3_AccountNumber_Name()
        {
            //arrange
            _userInput = new Account()
            {
                AccountNumber = "123",
                Name = "test"
            };

            //act
            var results = FindQuestions();

            //assert
            Assert.IsTrue(results.Questions.Any());
        }

        [TestMethod()]
        public void Branch4_AccountNumber_DateOfBirth()
        {
            //arrange
            _userInput = new Account()
            {
                AccountNumber = "123",
                DateOfBirth = new DateTime(2016, 01, 01)
            };

            //act
            var results = FindQuestions();

            //assert
            Assert.IsTrue(results.Questions.Any());
        }

        [TestMethod()]
        public void Branch5_AccountNumber()
        {
            //arrange
            _userInput = new Account() { AccountNumber = "123" };

            //act
            var results = FindQuestions();

            //assert
            Assert.IsTrue(results.Questions.Any());
        }

        [TestMethod()]
        public void Branch6_NOAccountNumber_Name_SocialSecurityNumber_DateOfBirth()
        {
            //arrange
            _userInput = new Account()
            {
                Name = "test",
                SocialSecurityNumber = "123456789",
                DateOfBirth = new DateTime(2016, 01, 01)
            };

            //act
            var results = FindQuestions();

            //assert
            Assert.IsTrue(results.Questions.Any());
        }

        [TestMethod()]
        public void Branch7_NOAccountNumber_Name_SocialSecurityNumber()
        {
            //arrange
            _userInput = new Account()
            {
                Name = "test",
                SocialSecurityNumber = "123456789"
            };

            //act
            var results = FindQuestions();

            //assert
            Assert.IsTrue(results.Questions.Any());
        }

        [TestMethod()]
        public void Branch8_NOAccountNumber_Name_DateOfBirth()
        {
            //arrange
            _userInput = new Account()
            {
                Name = "test",
                DateOfBirth = new DateTime(2016, 01, 01)
            };

            //act
            var results = FindQuestions();

            //assert
            Assert.IsTrue(results.Questions.Any());
        }

        [TestMethod()]
        public void Branch9_NOAccountNumber_SocialSecurityNumber_DateOfBirth()
        {
            //arrange
            _userInput = new Account()
            {
                SocialSecurityNumber = "123456789",
                DateOfBirth = new DateTime(2016, 01, 01)
            };

            //act
            var results = FindQuestions();

            //assert
            Assert.IsTrue(results.Questions.Any());
        }

        [TestMethod()]
        public void Branch10_NOAccountNumber_SocialSecurityNumber()
        {
            //arrange
            _userInput = new Account() { SocialSecurityNumber = "123456789" };

            //act
            var results = FindQuestions();

            //assert
            Assert.IsTrue(results.Questions.Any());
        }

        private QuestionList FindQuestions()
        {
            return TreeNode.FindQuesions
            (
                _tree,
                _dbResults,
                _userInput,
                new List<InputEnum>() { },
                new QuestionList() { Questions = new List<QuestionTypes>() { } }
            );
        }
    }
}

