using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _2C2P.FileUploader.Models.Entities;

namespace _2C2P.FileUploader.Interfaces.Repositories
{
    public interface ITransactionRepository
    {
        void InsertTransaction(TransactionEntity transactionEntity);

        Task<bool> IsDuplicateTransactionIdAsync(string transactionId);

        Task<List<TransactionEntity>> GetTransactionBySearchCriteria(string currentcy, string statusCode, DateTime? fromDate, DateTime? toDate);
    }
}
