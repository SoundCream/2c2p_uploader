using System.Xml.Serialization;

namespace _2C2P.FileUploader.Models.Dtos
{
    [XmlRoot(ElementName = "PaymentDetails")]
    public class XmlPaymentDetailUploadDto
    {
        [XmlElement(ElementName = "Amount")]
        public string Amount { get; set; }

        [XmlElement(ElementName = "CurrencyCode")]
        public string CurrencyCode { get; set; }
    }
}
