using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2C2P.FileUploader.Interfaces.Data;
using _2C2P.FileUploader.Interfaces.Repositories;
using _2C2P.FileUploader.Managers;
using _2C2P.FileUploader.Mappers;
using _2C2P.FileUploader.Models.Constants;
using _2C2P.FileUploader.Models.CustomExceptions;
using _2C2P.FileUploader.Models.Dtos;
using _2C2P.FileUploader.Models.Entities;
using Moq;
using NUnit.Framework;

namespace _2C2P.FileUploader.UnitTest.Managers
{
    [TestFixture]
    public class TransactionManagerUnitTest : UnitTestBase
    {
        private List<TransactionStatusEntity> _transactionStatusCollection;
        protected override void PrepareTestFixture() 
        {
            _transactionStatusCollection = new List<TransactionStatusEntity>()
            {
                new TransactionStatusEntity(){ Id = 1, IsActive = true, StatusCode = "A", StatusName = "Approved" },
                new TransactionStatusEntity(){ Id = 2, IsActive = true, StatusCode = "R", StatusName = "Rejected" },
                new TransactionStatusEntity(){ Id = 3, IsActive = true, StatusCode = "D", StatusName = "Done" },
            };
        }

        [Test]
        public void TestInsertUploadTransaction_WhenTarnsactionValidCsvSourceAndNotDuplicate_ThenInsertToTransactionRepositoryAndCrateTransactionAndSaveAndCommitEachOneTime()
        {
            // Arrange
            var expectedTransactionDate = new DateTime(2020, 12, 10);
            var transactionStatus = TransactionMapperConstant.CsvSourceStatusMapper.FirstOrDefault();
            var expectedAmount = 1200M;
            var expectedTransactionStatus = _transactionStatusCollection.FirstOrDefault(x => x.StatusCode == transactionStatus.Value);
            var expectedTransactionUploadModel = new TransactionUploadModel()
            {
                Amount = expectedAmount.ToString("#,###.00"),
                CurrencyCode = "THB",
                Source = UploadSourceEnum.Csv,
                Status = transactionStatus.Key,
                TransactionDate = expectedTransactionDate.ToString(TransactionConstant.UploadCsvDateFormat),
                TransactionId = "Invoice00000020"
            };

            var uploadTransactions = new List<TransactionUploadModel>() { expectedTransactionUploadModel };
            AutoMockContainer.Mock<IUnitOfWork>().Setup(x => x.BeginTransactionAsync()).Returns(Task.FromResult(0));
            AutoMockContainer.Mock<IUnitOfWork>().Setup(x => x.SaveChangeAsync()).ReturnsAsync(It.IsAny<int>());
            AutoMockContainer.Mock<IUnitOfWork>().Setup(x => x.CommitTransactionAsync()).Returns(Task.FromResult(0));
            AutoMockContainer.Mock<ITransactionStatusRepository>()
                .Setup(x => x.GetAllTransactionStatus()).ReturnsAsync(_transactionStatusCollection);
            AutoMockContainer.Mock<ITransactionRepository>().Setup(x => x.IsDuplicateTransactionIdAsync(It.IsAny<string>())).ReturnsAsync(false);
            AutoMockContainer.Mock<ITransactionRepository>().Setup(x => x.InsertTransaction(It.IsAny<TransactionEntity>()))
                .Callback<TransactionEntity>((transactionEntity) =>
                {
                    // Assert
                    Assert.AreEqual(expectedTransactionUploadModel.TransactionId, transactionEntity.Id);
                    Assert.AreEqual(expectedTransactionUploadModel.CurrencyCode, transactionEntity.CurrencyCode);
                    Assert.AreEqual(expectedAmount, transactionEntity.Amount);
                    Assert.AreEqual(expectedTransactionStatus.Id, transactionEntity.StatusId);
                    Assert.AreEqual(expectedTransactionDate, transactionEntity.TransactionDate);
                });

            var transactionManager = AutoMockContainer.Create<TransactionManager>();

            // Act
            transactionManager.InsertUploadTransaction(uploadTransactions, It.IsAny<string>()).Wait();

            // Assert
            AutoMockContainer.Mock<ITransactionRepository>()
                .Verify(x => x.InsertTransaction(It.IsAny<TransactionEntity>()), Times.Exactly(uploadTransactions.Count), "Should invoke equal uploadTransactions items.");
            AutoMockContainer.Mock<IUnitOfWork>().Verify(x => x.BeginTransactionAsync(), Times.Once, "Should invoke BeginTransactionAsync Once time.");
            AutoMockContainer.Mock<IUnitOfWork>().Verify(x => x.SaveChangeAsync(), Times.Once, "Should invoke SaveChangeAsync Once time.");
            AutoMockContainer.Mock<IUnitOfWork>().Verify(x => x.CommitTransactionAsync(), Times.Once, "Should invoke CommitTransactionAsync Once time.");
        }


        [Test]
        public void TestInsertUploadTransaction_WhenTarnsactionValidXmlSourceAndNotDuplicate_ThenInsertToTransactionRepositoryAndCrateTransactionAndSaveAndCommitEachOneTime()
        {
            // Arrange
            var expectedTransactionDate = new DateTime(2020, 12, 10);
            var transactionStatus = TransactionMapperConstant.XmlSourceStatusMapper.FirstOrDefault();
            var expectedAmount = 1200M;
            var expectedTransactionStatus = _transactionStatusCollection.FirstOrDefault(x => x.StatusCode == transactionStatus.Value);
            var expectedTransactionUploadModel = new TransactionUploadModel()
            {
                Amount = expectedAmount.ToString(),
                CurrencyCode = "THB",
                Source = UploadSourceEnum.Xml,
                Status = transactionStatus.Key,
                TransactionDate = expectedTransactionDate.ToString(TransactionConstant.UploadXmlDateFormat),
                TransactionId = "Invoice00000020"
            };

            var uploadTransactions = new List<TransactionUploadModel>() { expectedTransactionUploadModel };
            AutoMockContainer.Mock<IUnitOfWork>().Setup(x => x.BeginTransactionAsync()).Returns(Task.FromResult(0));
            AutoMockContainer.Mock<IUnitOfWork>().Setup(x => x.SaveChangeAsync()).ReturnsAsync(It.IsAny<int>());
            AutoMockContainer.Mock<IUnitOfWork>().Setup(x => x.CommitTransactionAsync()).Returns(Task.FromResult(0));
            AutoMockContainer.Mock<ITransactionStatusRepository>()
                .Setup(x => x.GetAllTransactionStatus()).ReturnsAsync(_transactionStatusCollection);
            AutoMockContainer.Mock<ITransactionRepository>().Setup(x => x.IsDuplicateTransactionIdAsync(It.IsAny<string>())).ReturnsAsync(false);
            AutoMockContainer.Mock<ITransactionRepository>().Setup(x => x.InsertTransaction(It.IsAny<TransactionEntity>()))
                .Callback<TransactionEntity>((transactionEntity) => 
                {
                    // Assert
                    Assert.AreEqual(expectedTransactionUploadModel.TransactionId, transactionEntity.Id);
                    Assert.AreEqual(expectedTransactionUploadModel.CurrencyCode, transactionEntity.CurrencyCode);
                    Assert.AreEqual(expectedAmount, transactionEntity.Amount);
                    Assert.AreEqual(expectedTransactionStatus.Id, transactionEntity.StatusId);
                    Assert.AreEqual(expectedTransactionDate, transactionEntity.TransactionDate);
                });

            var transactionManager = AutoMockContainer.Create<TransactionManager>();

            // Act
            transactionManager.InsertUploadTransaction(uploadTransactions, It.IsAny<string>()).Wait();

            // Assert
            AutoMockContainer.Mock<ITransactionRepository>()
                .Verify(x => x.InsertTransaction(It.IsAny<TransactionEntity>()), Times.Exactly(uploadTransactions.Count), "Should invoke equal uploadTransactions items.");
            AutoMockContainer.Mock<IUnitOfWork>().Verify(x => x.BeginTransactionAsync(), Times.Once, "Should invoke BeginTransactionAsync Once time.");
            AutoMockContainer.Mock<IUnitOfWork>().Verify(x => x.SaveChangeAsync(), Times.Once, "Should invoke SaveChangeAsync Once time.");
            AutoMockContainer.Mock<IUnitOfWork>().Verify(x => x.CommitTransactionAsync(), Times.Once, "Should invoke CommitTransactionAsync Once time.");
        }

        [Test]
        public void TestGetTransactions_WhenSetCorrectCriteria_ThenAllCriteriaShouldSetToRepositoryCorrectly()
        {
            // Arrange
            var expectedCurrentcy = "THB";
            var expectedStatusCode = "A";
            var expectedFromDate = "20201220";
            var expectedToDate = "20201020";
            var dataFormat = "yyyyMMdd";

            AutoMockContainer.Mock<ITransactionRepository>()
                .Setup(x => x.GetTransactionBySearchCriteria(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>()))
                .Callback<string, string, DateTime?, DateTime?>((currentcy, statusCode, fromDate, toDate) => 
                {
                    // Assert
                    Assert.AreEqual(expectedCurrentcy, currentcy);
                    Assert.AreEqual(expectedStatusCode, statusCode);
                    Assert.AreEqual(expectedFromDate, fromDate.Value.ToString(dataFormat));                                                               
                    Assert.AreEqual(expectedToDate, toDate.Value.ToString(dataFormat));
                }).ReturnsAsync(It.IsAny<List<TransactionEntity>>());                                    
                
            var transactionManager = AutoMockContainer.Create<TransactionManager>();

            // Act
            transactionManager.GetTransactions(expectedCurrentcy, expectedStatusCode, expectedFromDate, expectedToDate, dataFormat).Wait();
        }

        [Test]
        public void TestInsertUploadTransaction_WhenSetIncorrectCurrentcyCriteria_ThenThrowValidationErrorsException()
        {
            // Arrange
            var expectedCurrentcy = "THA";
            var expectedStatusCode = "A";
            var expectedFromDate = default(string);
            var expectedToDate = default(string);
            AutoMockContainer.Mock<ITransactionRepository>()
                .Setup(x => x.GetTransactionBySearchCriteria(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>()))
                .ReturnsAsync(It.IsAny<List<TransactionEntity>>());
            var transactionManager = AutoMockContainer.Create<TransactionManager>();

            // Assert
            Assert.ThrowsAsync<ValidationErrorsException>(
                async () =>
                {
                    // Act
                    await transactionManager.GetTransactions(expectedCurrentcy, expectedStatusCode, expectedFromDate, expectedToDate);
                },
                $"Should Throw ValidationErrorsException");
        }

        [Test]
        public void TestInsertUploadTransaction_WhenSetIncorrectStatusCodeCriteria_ThenThrowValidationErrorsException()
        {
            // Arrange
            var expectedCurrentcy = "THB";
            var expectedStatusCode = "0";
            var expectedFromDate = default(string);
            var expectedToDate = default(string);
            AutoMockContainer.Mock<ITransactionRepository>()
                .Setup(x => x.GetTransactionBySearchCriteria(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>()))
                .ReturnsAsync(It.IsAny<List<TransactionEntity>>());
            var transactionManager = AutoMockContainer.Create<TransactionManager>();

            // Assert
            Assert.ThrowsAsync<ValidationErrorsException>(
                async () =>
                {
                    // Act
                    await transactionManager.GetTransactions(expectedCurrentcy, expectedStatusCode, expectedFromDate, expectedToDate);
                },
                $"Should Throw ValidationErrorsException");
        }
    }
}
