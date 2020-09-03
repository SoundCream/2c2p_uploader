using _2C2P.FileUploader.Helper.Attributes;

namespace _2C2P.FileUploader.Models.Dtos
{
    [CsvPattern(CsvRegxPattern = @"\"".*?\""", Ignore = "\"")]
    public class CsvTransactionUploadDto
    {
        [CsvIndex(0)]
        public string TransactionId { get; set; }

        [CsvIndex(1)]
        public string Amount { get; set; }

        [CsvIndex(2)]
        public string CurrencyCode { get; set; }

        [CsvIndex(3)]
        public string TransactionDate { get; set; }

        [CsvIndex(4)]
        public string Status { get; set; }
    }
}
