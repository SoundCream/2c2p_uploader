using System;

namespace _2C2P.FileUploader.Helper.Attributes
{
    public class CsvPatternAttribute : Attribute
    {
        public string CsvRegxPattern { get; set; }

        public string Ignore { get; set; }

        public CsvPatternAttribute()
        {
        }
    }
}
