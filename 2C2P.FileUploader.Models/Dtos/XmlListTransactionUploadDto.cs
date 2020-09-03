using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace _2C2P.FileUploader.Models.Dtos
{
    [XmlRoot(ElementName = "Transactions")]
    public class XmlListTransactionUploadDto
    {
        [XmlElement("Transaction")]
        public List<XmlTransactionUploadDto> Transactions { get; set; }
    }
}
