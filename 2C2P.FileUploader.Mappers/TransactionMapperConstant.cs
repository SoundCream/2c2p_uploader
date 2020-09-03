using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace _2C2P.FileUploader.Mappers
{
    public struct TransactionMapperConstant
    {
        public static ReadOnlyDictionary<string, string> CsvSourceStatusMapper = new ReadOnlyDictionary<string, string>(new Dictionary<string, string> 
        { 
            { "Approved", "A" }, 
            { "Failed", "R" },
            { "Finished", "D" }
        });

        public static ReadOnlyDictionary<string, string> XmlSourceStatusMapper = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>
        {
            { "Approved", "A" },
            { "Rejected", "R" },
            { "Done", "D" }
        });
    }
}
