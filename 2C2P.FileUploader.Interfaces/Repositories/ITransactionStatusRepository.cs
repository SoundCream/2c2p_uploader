using System.Collections.Generic;
using System.Threading.Tasks;
using _2C2P.FileUploader.Models.Entities;

namespace _2C2P.FileUploader.Interfaces.Repositories
{
    public interface ITransactionStatusRepository
    {
        Task<List<TransactionStatusEntity>> GetAllTransactionStatus();
    }
}
