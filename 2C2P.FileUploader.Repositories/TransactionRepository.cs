using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2C2P.FileUploader.Interfaces.Data;
using _2C2P.FileUploader.Interfaces.Repositories;
using _2C2P.FileUploader.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace _2C2P.FileUploader.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public TransactionRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void InsertTransaction(TransactionEntity transactionEntity)
        {
            _unitOfWork.Insert(transactionEntity);
        }

        public async Task<bool> IsDuplicateTransactionIdAsync(string transactionId)
        {
            var result = await _unitOfWork.Get<TransactionEntity>().AnyAsync(x => x.Id == transactionId);
            return result;
        }

        public async Task<List<TransactionEntity>> GetTransactionBySearchCriteria(string currentcy, string statusCode, DateTime? fromDate, DateTime? toDate)
        {
            var result = await _unitOfWork.Get<TransactionEntity>()
                .Include(x => x.Status)
                .Where(x => 
                    (string.IsNullOrEmpty(currentcy) || x.CurrencyCode == currentcy) && 
                    (string.IsNullOrEmpty(statusCode) || x.Status.StatusCode == statusCode) &&
                    (!fromDate.HasValue || x.TransactionDate >= fromDate) &&
                    (!toDate.HasValue || x.TransactionDate <= toDate)).ToListAsync();

            return result;
        }
    }
}