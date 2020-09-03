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
    /// <summary>
    /// 
    /// </summary>
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public TransactionRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// InsertTransaction.
        /// </summary>
        /// <param name="transactionEntity"></param>
        public void InsertTransaction(TransactionEntity transactionEntity)
        {
            _unitOfWork.Insert(transactionEntity);
        }

        /// <summary>
        /// Check DuplicateTransaction by Id
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        public async Task<bool> IsDuplicateTransactionIdAsync(string transactionId)
        {
            var result = await _unitOfWork.Get<TransactionEntity>().AnyAsync(x => x.Id == transactionId);
            return result;
        }

        /// <summary>
        /// GetTransactionBySearchCriteria
        /// </summary>
        /// <param name="currentcy"></param>
        /// <param name="statusCode"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
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