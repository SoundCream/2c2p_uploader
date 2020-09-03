using System.Collections.Generic;
using System.IO;

namespace _2C2P.FileUploader.Interfaces.Managers
{
    public interface IFileUploadManager
    {
        List<T> DeserializeStreamTransactionUploadFile<T>(Stream stream, string fileName) where T : class;
    }
}
