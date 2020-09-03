using System.Collections.Generic;
using System.Threading.Tasks;
using _2C2P.FileUploader.Models.Dtos;
using _2C2P.FileUploader.Models.Entities;

namespace _2C2P.FileUploader.Interfaces.Managers
{
    public interface ITransactionManager
    {
        Task<List<TransactionEntity>> GetTransactions(string currentcy, string statusCode, string from, string to, string dateFormat = "yyyyMMdd");

        Task<int> InsertUploadTransaction(List<TransactionUploadModel> transactionUploadModels);
    }
}
