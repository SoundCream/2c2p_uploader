using System;
using System.Collections.Generic;

namespace _2C2P.FileUploader.Models.CustomExceptions
{
    public class ValidationErrorsException : Exception
    {
        public List<string> ErrorMessages { get; set; }

        public ValidationErrorsException(string message, List<string> errorMessages = default(List<string>)) : base(message)
        {
            ErrorMessages = errorMessages;
        }

    }
}
