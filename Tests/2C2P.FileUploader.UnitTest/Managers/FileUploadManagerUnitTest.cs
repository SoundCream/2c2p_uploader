using System.IO;
using System.Linq;
using System.Text;
using _2C2P.FileUploader.Managers;
using _2C2P.FileUploader.Models.ConfigurationOptions;
using _2C2P.FileUploader.Models.Constants;
using _2C2P.FileUploader.Models.CustomExceptions;
using _2C2P.FileUploader.Models.Dtos;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace _2C2P.FileUploader.UnitTest.Managers
{
    [TestFixture]
    public class FileUploadManagerUnitTest : UnitTestBase
    {
        protected override void PrepareTestFixture()
        {
            var defaultAppConfiguration = new AppConfiguration()
            {
                AllowFileExtionsions = new string[] { ".csv", ".xml" },
                DateFormatForGetTransaction = "yyyyMMdd",
                MaximunFilesize = 1048576
            };

            var optionsMock = AutoMockContainer.Mock<IOptions<AppConfiguration>>();
            optionsMock.SetupGet(o => o.Value).Returns(defaultAppConfiguration);
        }

        [Test]
        public void TestDeserializeStreamTransactionUploadFile_WhenDeserializeStreamCsvCorrectFormat_ThenCanDeserializeToTransactionUploadModel()
        {
            // Arrange
            var csvContent = "\"Invoice0000001\",\"1,000.00\", \"USD\", \"20/02/2019 12:33:16\", \"Approved\"";
            var csvExtension = ".csv";
            var fileUploadManager = AutoMockContainer.Create<FileUploadManager>();

            using (Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent)))
            {
                // Act
                var result = fileUploadManager.DeserializeStreamTransactionUploadFile<TransactionUploadModel>(stream, csvExtension);

                // Assert
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(UploadSourceEnum.Csv, result.First().Source);
                Assert.AreEqual("1,000.00", result.First().Amount);
                Assert.AreEqual("USD", result.First().CurrencyCode);
                Assert.AreEqual("Approved", result.First().Status);
                Assert.AreEqual("20/02/2019 12:33:16", result.First().TransactionDate);
                Assert.AreEqual("Invoice0000001", result.First().TransactionId);
            }
        }

        [Test]
        public void TestDeserializeStreamTransactionUploadFile_WhenDeserializeStreamXmlCorrectFormat_ThenCanDeserializeToTransactionUploadModel()
        {
            // Arrange
            var sbXmlContent = new StringBuilder();
            sbXmlContent.Append("<Transactions>");
            sbXmlContent.Append("<Transaction id=\"Inv00001\">");
            sbXmlContent.Append("<TransactionDate>2019-01-23T13:45:10</TransactionDate>");
            sbXmlContent.Append("<PaymentDetails>");
            sbXmlContent.Append("<Amount>200.00</Amount>");
            sbXmlContent.Append("<CurrencyCode>USD</CurrencyCode>");
            sbXmlContent.Append("</PaymentDetails>");
            sbXmlContent.Append("<Status>Done</Status>");
            sbXmlContent.Append("</Transaction>");
            sbXmlContent.Append("</Transactions>");
            var xmlExtension = ".xml";
            var fileUploadManager = AutoMockContainer.Create<FileUploadManager>();

            using (Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(sbXmlContent.ToString())))
            {
                // Act
                var result = fileUploadManager.DeserializeStreamTransactionUploadFile<TransactionUploadModel>(stream, xmlExtension);

                // Assert
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(UploadSourceEnum.Xml, result.First().Source);
                Assert.AreEqual("200.00", result.First().Amount);
                Assert.AreEqual("USD", result.First().CurrencyCode);
                Assert.AreEqual("Done", result.First().Status);
                Assert.AreEqual("2019-01-23T13:45:10", result.First().TransactionDate);
                Assert.AreEqual("Inv00001", result.First().TransactionId);
            }
        }

        [Test]
        public void TestValidateFileUploadedSize_WhenFileSizeIsOverLimitInConfiguration_ThenThrowFileUploadErrorException()
        {
            // Arrange
            var fileSize = 2097152;
            var fileUploadManager = AutoMockContainer.Create<FileUploadManager>();

            // Assert
            Assert.Throws<FileUploadErrorException>(
                () =>
                {
                    // Act
                    fileUploadManager.ValidateFileUploadedSize(fileSize);
                },
                $"Should Throw FileUploadErrorException when file size over limit.");
        }

        [Test]
        public void TestGetFileExtensionAllowed_WhenFileExtensionNotAllowInConfiguration_ThenThrowFileUploadErrorException()
        {
            // Arrange
            var fileName = "test.txt";
            var fileUploadManager = AutoMockContainer.Create<FileUploadManager>();

            // Assert
            Assert.Throws<FileUploadErrorException>(
                () =>
                {
                    // Act
                    fileUploadManager.GetFileExtensionAllowed(fileName);
                },
                $"Should Throw FileUploadErrorException when not allowed file extension.");
        }
    }
}
