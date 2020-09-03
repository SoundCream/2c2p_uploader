using System;

namespace _2C2P.FileUploader.Helper.Attributes
{
    public class CsvIndexAttribute : Attribute
    {
        public int Index { get; set; }

        public CsvIndexAttribute(int index)
        {
            Index = index;
        }
    }
}
