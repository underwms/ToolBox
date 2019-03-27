using Microsoft.VisualStudio.TestTools.UnitTesting;
using TreeTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Threading;

namespace GenericTests
{
    [TestClass()]
    public class LINQTests
    {
        [TestInitialize]
        public void TestInitialize()
        {

        }

        [TestMethod]
        public void MultiThread()
        {
            try
            {
                List<Task> taskList = new List<Task>();

                for(int x = 0; x < 100; x++)
                {
                    taskList.Add(Callmelissa(x));
                }

                Task.WaitAll(taskList.ToArray());
            }
            catch (AggregateException ex)
            {                throw;
            }
        }

        private async Task  Callmelissa(int x)
        {
            await Task.Run(() => { ; });
        }

        [TestMethod]
        public void ListExclude()
        {
            //arrange
            var criticalChanges = new List<KeyValuePair<int, int>>()
            {
                new KeyValuePair<int, int>(1, 1),
                new KeyValuePair<int, int>(2, 25),
                new KeyValuePair<int, int>(3, 3)
            };
            var fieldChanges = new List<KeyValuePair<int, int>>()
            {
                new KeyValuePair<int, int>(1, 1),
                new KeyValuePair<int, int>(2, 25),
                new KeyValuePair<int, int>(2, 4),
                new KeyValuePair<int, int>(4, 4),
                new KeyValuePair<int, int>(5, 5),
                new KeyValuePair<int, int>(5, 6),
                new KeyValuePair<int, int>(5, 7)
            };

            //act
            var fieldChangesFiltered = fieldChanges.Except(criticalChanges);
            var critSimplified = criticalChanges.Select(item => item.Key);
            var fieldSimplified = fieldChangesFiltered.Select(item => item.Key);

            var critOnly = critSimplified.Except(fieldSimplified).Distinct().ToList();
            var fieldOnly = fieldSimplified.Except(critSimplified).Distinct().ToList();
            var both = fieldSimplified.Except(critOnly)
                                      .Except(fieldOnly);

            //assert
            Assert.AreEqual(2, critOnly.Count());
            Assert.AreEqual(2, fieldOnly.Count());
            Assert.AreEqual(1, both.Count());
        }

        [TestMethod]
        public void ListExcludeComplexFromSimple()
        {
            // arrange
            var clientInfo = new List<LinkedAccount>() {
                new LinkedAccount() {
                    AccountId = 1,
                    AccountNumber = "123",
                    ACIProfileID = 123,
                    Active = true,
                    ClientId = 1,
                    Disclosure = "test",
                    IsDefault = true,
                    IsLocked = false,
                    TocAccepted = DateTime.Now,
                    Verified = true
                },
                new LinkedAccount() {
                    AccountId = 2,
                    AccountNumber = "456",
                    ACIProfileID = 456,
                    Active = true,
                    ClientId = 2,
                    Disclosure = "test",
                    IsDefault = true,
                    IsLocked = false,
                    TocAccepted = DateTime.Now,
                    Verified = true
                },
                new LinkedAccount() {
                    AccountId = 3,
                    AccountNumber = "789",
                    ACIProfileID = 789,
                    Active = true,
                    ClientId = 3,
                    Disclosure = "test",
                    IsDefault = true,
                    IsLocked = false,
                    TocAccepted = DateTime.Now,
                    Verified = true
                }
            };
            var notExlucdedList = new List<long>() { 1, 3 };

            // act
            var accountExcluded = clientInfo.Where(a => !notExlucdedList.Any(id => id == a.AccountId)).ToList();
            var accountsNotExcluded = clientInfo.Where(a => notExlucdedList.Any(id => id == a.AccountId)).ToList();

            // assert
            Assert.AreEqual(1, accountExcluded.Count());
            Assert.AreEqual(2, accountsNotExcluded.Count());
        }

        [TestMethod]
        public void RandomizeList()
        {
            //act 
            var listUnderTest = new List<QuestionTypes>()
            {
                QuestionTypes.Name,
                QuestionTypes.Address,
                QuestionTypes.DateOfBirth,
                QuestionTypes.Phone,
                QuestionTypes.Zip,
                QuestionTypes.City,
                QuestionTypes.SocialSecurityNumber,
            };

            //arrange
            listUnderTest = RandomizeQuestions(listUnderTest);

            //act
            Assert.IsTrue((listUnderTest.Count == 3));
        }

        [TestMethod]
        public void RemoveItemsFromOtherFucntions()
        {
            //act 
            var listUnderTest = new List<QuestionTypes>()
            {
                QuestionTypes.Name,
                QuestionTypes.Address,
                QuestionTypes.DateOfBirth,
                QuestionTypes.Phone,
                QuestionTypes.Zip,
                QuestionTypes.City,
                QuestionTypes.SocialSecurityNumber,
            };

            //arrange
            RemoveItem(listUnderTest, QuestionTypes.Name);

            //act
            Assert.IsFalse(listUnderTest.Contains(QuestionTypes.Name));
        }

        [TestMethod]
        public void ExcludeComplexListBySimpleList()
        {
            //arrange
            var classUnderTest = new ComplextObject()
            {
                SomethingElse = "Test",
                BaseObjects = new List<BaseObject>() {
                    new BaseObject() { Id = 1, SomeString = "1" },
                    new BaseObject() { Id = 2, SomeString = "2" },
                    new BaseObject() { Id = 3, SomeString = "3" }
                }
            };
            var filterList = new List<long>() { 1, 3 };

            //act
            //classUnderTest.BaseObjects = (from index in filterList
            //                              join baseObject in classUnderTest.BaseObjects
            //                                on index equals baseObject.Id
            //                              select baseObject).ToList();

            classUnderTest.BaseObjects = classUnderTest.BaseObjects.Where(bo => filterList.Any(i => i == bo.Id))
                                                                   .ToList();

            //assert
            Assert.AreEqual(2, classUnderTest.BaseObjects.Count);
            Assert.AreEqual(1, classUnderTest.BaseObjects[0].Id);
            Assert.AreEqual(3, classUnderTest.BaseObjects[1].Id);
        }

        [TestMethod]
        public void ListToDictionary()
        {
            // arrange
            var classUnderTest = new ComplextObject()
            {
                SomethingElse = "Test",
                BaseObjects = new List<BaseObject>() {
                    new BaseObject() { Id = 1, SomeString = "1" },
                    new BaseObject() { Id = 2, SomeString = "2" },
                    new BaseObject() { Id = 3, SomeString = "3" }
                }
            };

            // act
            var newDictionary = classUnderTest.BaseObjects.ToDictionary(x => x.Id,
                                                                        y => y.SomeString);

            // asert
            Assert.IsTrue(newDictionary.Keys.Count == 3);
        }

        [TestMethod]
        public void FileBatchesBySize()
        {
            //NOTES: THESE ARE ROUNDED FOR EASY MATH
            //1KB == 1000 bytes
            //1MB == 1000 KB == 1,000,000 bytes
            //1GB == 1000 MB == 1,000,000 KB == 1,000,000,000 bytes

            //arrange
            const long maxSizeBytes = 500_000_000; //500 MB
            var batches = new List<IEnumerable<FileInfo>>();

            var di = new DirectoryInfo(@"C:\Windows\System32");
            var files = di.EnumerateFiles();

            var totalSize = files.Select(f => f.Length).Sum();
            var expectedBatches = Math.Ceiling((float)totalSize / maxSizeBytes);

            //act 
            while (files.Any())
            {
                long currentSize = 0;

                //get all files that fit into a chunk
                var batch = files.TakeWhile(f => (currentSize += f.Length) < maxSizeBytes)
                                 .ToList();

                //remove all files in current chunk from main dictionary
                files = files.Where(f => !batch.Any(b => b.FullName == f.FullName));

                batches.Add(batch);
            }

            //asert
            Assert.AreEqual(expectedBatches, batches.Count);
        }

        [TestMethod]
        public void FileBatchesByCharacterAndSize()
        {
            //arrange
            const long maxSizeBytes = 500_000_000; //500 MB
            var batches = new List<IEnumerable<FileInfo>>();

            var di = new DirectoryInfo(@"C:\Windows\System32");
            var files = di.EnumerateFiles();

            var totalSize = files.Select(f => f.Length).Sum();
            var expectedBatches = Math.Ceiling((float)totalSize / maxSizeBytes);

            //act 
            var uniqueStartingCharacters = files.Select(f => f.Name[0])
                                                .Distinct();
            var characterSizeDictionary = uniqueStartingCharacters.ToDictionary(k => k,
                                                                                v => files.Where(f => f.Name[0] == v)
                                                                                          .Select(f => f.Length)
                                                                                          .Sum());

            while (characterSizeDictionary.Any())
            {
                long currentSize = 0;

                //get all character's that will fit into a chunk
                var charactersInCurrentGroup = characterSizeDictionary.TakeWhile(kvp => (currentSize += kvp.Value) < maxSizeBytes)
                                                                      .ToList()
                                                                      .Select(x => x.Key);

                //get all character's files in current chunk
                var batch = files.Where(f => charactersInCurrentGroup.Any(c => f.Name[0] == c));

                //remove all characters in current chunk from main dictionary
                characterSizeDictionary = characterSizeDictionary.Where(kvp => !charactersInCurrentGroup.Any(c => kvp.Key == c))
                                                                 .ToDictionary(k => k.Key,
                                                                               v => v.Value);

                batches.Add(batch);
            }

            //assert
            Assert.AreEqual(expectedBatches, batches.Count);
        }

        [TestMethod]
        public void ListOfList()
        {
            //arrange
            const int batchSize = 5_000;
            const int initialSize = 56_000;
            var listUnderTest = new List<BaseObject>();

            for (var x = 0; x < initialSize; x++)
            { listUnderTest.Add(new BaseObject() { Id = x, SomeString = RandomString() }); }

            //act
            var batches = listUnderTest.Select((x, i) => new { Index = i, Value = x })  //i is auto populated by linq
                                       .GroupBy(x => x.Index / batchSize)               //because this is an integer divide the grouping is on a discrete set of whole numbers
                                       .Select(x => x.Select(v => v.Value).ToList())    //getting the original values from group into a list
                                       .ToList();                                       //make a list of lists

            //assert
            var expected = Math.Ceiling((float)initialSize / batchSize);
            Assert.AreEqual(expected, batches.Count);
        }

        [TestMethod]
        public void ComplexDistinct()
        {
            //arrange
            var listUnderTest = new List<Request>() {
                new Request(){
                    RequestID = 123,
                    DocID = 123
                },
                new Request(){
                    RequestID = 456,
                    DocID = 123
                },
                new Request(){
                    RequestID = 789,
                    DocID = 456
                }
            };

            //act
            //var uniqueList = listUnderTest.Select(r => r.DocID)
            //                              .Distinct()
            //                              .ToList();
            var uniqueList = listUnderTest.GroupBy(p => p.DocID)
                                          .Select(r => r.First())
                                          .ToList();

            //assert
            Assert.AreEqual(2, uniqueList.Count);
        }

        [TestMethod]
        public void NullableContains()
        {
            //arrange
            var ListOfNullableLongs = new List<long?>()
            {
                new long?(1),
                null,
                new long?(2)
            };
            var nullableIntToFind = new int?(1);
            var nullableIntNotFound = new int?(3);
            int basicIntToFind = 2;

            //act
            var actual1 = ListOfNullableLongs.Contains(nullableIntToFind);
            var actual2 = ListOfNullableLongs.Contains(null);
            var actual3 = ListOfNullableLongs.Contains(nullableIntNotFound);
            var actual4 = ListOfNullableLongs.Contains(basicIntToFind);

            //assert
            Assert.IsTrue(actual1);
            Assert.IsTrue(actual2);
            Assert.IsFalse(actual3);
            Assert.IsTrue(actual4);
        }

        private List<QuestionTypes> RandomizeQuestions(List<QuestionTypes> listUnderTest)
        {
            var random = new Random(DateTime.Now.Millisecond);
            var randomSortTable = listUnderTest.ToDictionary(x => random.NextDouble(),
                                                             y => y);

            return randomSortTable.OrderBy(kvp => kvp.Key)
                                  .Take(3)
                                  .Select(kvp => kvp.Value)
                                  .ToList();
        }

        private void RemoveItem(List<QuestionTypes> listUnderTest, QuestionTypes itemToRemove)
        {
            listUnderTest.Remove(itemToRemove);
        }

        public string RandomString()
        {
            var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var randomString = new string(chars.Select(c => chars[random.Next(chars.Length)]).Take(8).ToArray());

            return randomString;
        }
    }

    public class BaseObject
    {
        public string SomeString { get; set; }
        public long Id { get; set; }
    }

    public class ComplextObject
    {
        public List<BaseObject> BaseObjects { get; set; }
        public string SomethingElse { get; set; }
    }

    public class LinkedAccount
    {
        public long? AccountId { get; set; }
        public string AccountNumber { get; set; }
        public long? ACIProfileID { get; set; }
        public bool Active { get; set; }
        public long ClientId { get; set; }
        public string Disclosure { get; set; }
        public bool IsDefault { get; set; }
        public bool IsLocked { get; set; }
        public DateTime? TocAccepted { get; set; }
        public bool Verified { get; set; }
    }

    public class Request
    {
        public Int64? RequestID { get; set; }
        public int? RequestStatusID { get; set; }
        public int? BatchID { get; set; }
        public int? ServicerID { get; set; }
        public decimal? AccountID { get; set; }
        public int? DocID { get; set; }
        public int? DocTypeID { get; set; }
        public string DocType { get; set; }
        public string SubDocType { get; set; }
        public int? PlaceBatchRequestID { get; set; }
        public string FileName { get; set; }
        public DateTime? BillStatementDate { get; set; }
    }

}

