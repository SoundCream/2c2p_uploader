using System;
using System.Collections.Generic;
using System.Text;

namespace _2C2P.FileUploader.Models.ConfigurationOptions
{
    public class AppConfiguration
    {
        public const string ConfigurataionName = "FileUploaderAppSetting";

        public string DateFormat { get; set; }

        public int MaximunFilesize { get; set; }

        public string[] AllowFileExtionsions { get; set; }
    }
}
