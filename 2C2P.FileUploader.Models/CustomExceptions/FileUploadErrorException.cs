using System;

namespace _2C2P.FileUploader.Models.CustomExceptions
{
    public class FileUploadErrorException : Exception
    {
        public FileUploadErrorException(string message) : base (message)
        {
        }
    }
}
