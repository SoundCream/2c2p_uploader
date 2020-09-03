using _2C2P.FileUploader.Models.Constants;

namespace _2C2P.FileUploader.Models.Dtos
{
    public class TransactionUploadModel
    {
        public string TransactionId { get; set; }

        public string Amount { get; set; }

        public string CurrencyCode { get; set; }

        public string TransactionDate { get; set; }

        public string Status { get; set; }

        public UploadSourceEnum Source { get; set; }
    }
}
