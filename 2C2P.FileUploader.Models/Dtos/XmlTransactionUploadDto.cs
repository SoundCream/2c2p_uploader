using System.Collections.Generic;
using System.Xml.Serialization;

namespace _2C2P.FileUploader.Models.Dtos
{
    [XmlRoot(ElementName = "Transaction")]
    public class XmlTransactionUploadDto
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlElement("TransactionDate")]
        public string TransactionDate { get; set; }

        [XmlElement("PaymentDetails")]
        public XmlPaymentDetailUploadDto PaymentDetails { get; set; }

        [XmlElement(ElementName = "Status")]
        public string Status { get; set; }
    }
}
