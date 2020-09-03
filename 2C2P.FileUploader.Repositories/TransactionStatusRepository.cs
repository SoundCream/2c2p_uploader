using System.Collections.Generic;
using System.Threading.Tasks;
using _2C2P.FileUploader.Interfaces.Data;
using _2C2P.FileUploader.Interfaces.Repositories;
using _2C2P.FileUploader.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace _2C2P.FileUploader.Repositories
{
    public class TransactionStatusRepository : ITransactionStatusRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransactionStatusRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<TransactionStatusEntity>> GetAllTransactionStatus()
        {
            var result = await _unitOfWork.Get<TransactionStatusEntity>().ToListAsync();
            return result;
        }
    }
}
