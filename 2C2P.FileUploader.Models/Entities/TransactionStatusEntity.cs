using System;
using System.Collections.Generic;
using System.Text;

namespace _2C2P.FileUploader.Models.Entities
{
    public class TransactionStatusEntity
    {
        public int Id { get; set; }

        public string StatusName { get; set; }

        public string StatusCode { get; set; }

        public bool IsActive { get; set; }
    }
}
