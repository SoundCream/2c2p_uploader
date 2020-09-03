using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using _2C2P.FileUploader.Helper;
using _2C2P.FileUploader.Interfaces.Data;
using _2C2P.FileUploader.Interfaces.Managers;
using _2C2P.FileUploader.Interfaces.Repositories;
using _2C2P.FileUploader.Mappers;
using _2C2P.FileUploader.Models.Constants;
using _2C2P.FileUploader.Models.CustomExceptions;
using _2C2P.FileUploader.Models.Dtos;
using _2C2P.FileUploader.Models.Entities;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace _2C2P.FileUploader.Managers
{
    public class TransactionManager : ITransactionManager
    {
        private static string[] _statusCodeCollection = new string[] { "A", "R", "D" };

        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionStatusRepository _transactionStatusRepository;

        public TransactionManager(
            ILogger<TransactionManager> logger,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ITransactionRepository transactionRepository, 
            ITransactionStatusRepository transactionStatusRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _transactionRepository = transactionRepository;
            _transactionStatusRepository = transactionStatusRepository;
        }

        public async Task<int> InsertUploadTransaction(List<TransactionUploadModel> transactionUploadModels)
        {
            try
            {
                if (!transactionUploadModels.Any())
                {
                    throw new ValidationErrorsException($"Don't have any transaction record");
                }

                var errorMessages = new List<string>();
                var transactionStatus = await _transactionStatusRepository.GetAllTransactionStatus();
                await _unitOfWork.BeginTransactionAsync();
                var recordNumber = 1;
                foreach (var transactionUploadModel in transactionUploadModels)
                {
                    var validateErrors = await ValidateTransactionUploadModel(recordNumber, transactionUploadModel);
                    errorMessages.AddRange(validateErrors);
                    var transactionEntity = _mapper.Map<TransactionEntity>(transactionUploadModel);
                    transactionEntity.StatusId = transactionStatus.FirstOrDefault(x => x.StatusCode == GetTransactionStatusCode(transactionUploadModel)).Id;
                    _transactionRepository.InsertTransaction(transactionEntity);
                    recordNumber++;
                }

                if (errorMessages.Any())
                {
                    _logger.LogWarning("Invalid validate transaction uploaded.");
                    throw new ValidationErrorsException($"Invalid validate transaction upload", errorMessages);
                }

                var result = await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();
                return result;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw ex;
            }
        }

        public async Task<List<TransactionEntity>> GetTransactions(string currentcy, string statusCode, string from, string to, string dateFormat = "yyyyMMdd")
        {
            var fromDate = default(DateTime?);
            var toDate = default(DateTime?);

            if (!string.IsNullOrWhiteSpace(currentcy) && !IsIsoCurrency(currentcy))
            {
                throw new ValidationErrorsException($"Invalid Currentcy ({currentcy})");
            }

            if (!string.IsNullOrWhiteSpace(statusCode) && !IsValidStatusCode(statusCode))
            {
                throw new ValidationErrorsException($"Invalid StatusCode ({statusCode})");
            }

            if (!string.IsNullOrWhiteSpace(from))
            {
                fromDate = GlobalHelper.GetDateTimeFromString(from, dateFormat);
            }

            if (!string.IsNullOrWhiteSpace(to))
            {
                toDate = GlobalHelper.GetDateTimeFromString(to, dateFormat).AddDays(1).AddSeconds(-1);
            }

            var result = await _transactionRepository.GetTransactionBySearchCriteria(currentcy, statusCode, fromDate, toDate);
            return result;
        }

        private bool IsValidStatusCode(string statusCode)
        {
            return _statusCodeCollection.Any(x => string.Equals(x, statusCode, StringComparison.InvariantCultureIgnoreCase));
        }

        private bool IsIsoCurrency(string currency)
        {
            var regions = CultureInfo.GetCultures(CultureTypes.SpecificCultures).Select(x => new RegionInfo(x.LCID));
            var currencySymbol = regions.Select(x => x.ISOCurrencySymbol).Distinct();
            var isIsoCurrency = currencySymbol.Any(x => string.Equals(x, currency, StringComparison.InvariantCultureIgnoreCase));
            return isIsoCurrency;
        }

        private string GetTransactionStatusCode(TransactionUploadModel transactionUploadModel)
        {
            var result = string.Empty;
            if (transactionUploadModel.Source == UploadSourceEnum.Csv)
            {
                var transactionStatus = TransactionMapperConstant.CsvSourceStatusMapper.FirstOrDefault(x => string.Equals(x.Key, transactionUploadModel.Status, StringComparison.InvariantCultureIgnoreCase));
                result = transactionStatus.Value;
            }
            else if (transactionUploadModel.Source == UploadSourceEnum.Xml)
            {
                var transactionStatus = TransactionMapperConstant.XmlSourceStatusMapper.FirstOrDefault(x => string.Equals(x.Key, transactionUploadModel.Status, StringComparison.InvariantCultureIgnoreCase));
                result = transactionStatus.Value;
            }

            return result;
        }

        private async Task<List<string>> ValidateTransactionUploadModel(int recordNumber, TransactionUploadModel transactionUploadModel)
        {
            var errorMessages = new List<string>();

            if (string.IsNullOrWhiteSpace(transactionUploadModel.TransactionId))
            {
                errorMessages.Add($"Record({recordNumber}) TransactionId is required.");
            }

            var isDuplicatedTransactionId = await _transactionRepository.IsDuplicateTransactionIdAsync(transactionUploadModel.TransactionId);
            if (isDuplicatedTransactionId)
            {
                errorMessages.Add($"Record({recordNumber}) TransactionId has already existed.");
            }

            if (transactionUploadModel.TransactionId != null && transactionUploadModel.TransactionId.Length > 50)
            {
                errorMessages.Add($"Record({recordNumber}) TransactionId length over 50.");
            }

            if (string.IsNullOrWhiteSpace(transactionUploadModel.Amount))
            {
                errorMessages.Add($"Record({recordNumber}) Amount is required.");
            }

            if (string.IsNullOrWhiteSpace(transactionUploadModel.CurrencyCode))
            {
                errorMessages.Add($"Record({recordNumber}) CurrencyCode is required.");
            }

            if (!IsIsoCurrency(transactionUploadModel.CurrencyCode))
            {
                errorMessages.Add($"Record({recordNumber}) CurrencyCode should ISO4217 format.");
            }

            if (string.IsNullOrWhiteSpace(transactionUploadModel.Status))
            {
                errorMessages.Add($"Record({recordNumber}) Status is required.");
            }

            if (transactionUploadModel.Source == UploadSourceEnum.Csv)
            {
                if (!GlobalHelper.IsDecimalNumber(transactionUploadModel.Amount))
                {
                    errorMessages.Add($"Record({recordNumber}) Amount should be only decimal number.");
                }

                if (!GlobalHelper.IsCorrectDateTimeFormat(transactionUploadModel.TransactionDate, TransactionConstant.UploadCsvDateFormat))
                {
                    errorMessages.Add($"Record({recordNumber}) Invalid format date.");
                }

                if (!TransactionMapperConstant.CsvSourceStatusMapper.Any(x =>
                    string.Equals(x.Key, transactionUploadModel.Status, StringComparison.InvariantCultureIgnoreCase)))
                {
                    errorMessages.Add($"Record({recordNumber}) Invalid status.");
                }
            }
            else if (transactionUploadModel.Source == UploadSourceEnum.Xml)
            {
                if (!GlobalHelper.IsDecimal(transactionUploadModel.Amount))
                {
                    errorMessages.Add($"Record({recordNumber}) Amount should be only decimal.");
                }

                if (!GlobalHelper.IsCorrectDateTimeFormat(transactionUploadModel.TransactionDate, TransactionConstant.UploadXmlDateFormat))
                {
                    errorMessages.Add($"Record({recordNumber}) Invalid format date.");
                }

                if (!TransactionMapperConstant.XmlSourceStatusMapper.Any(x =>
                    string.Equals(x.Key, transactionUploadModel.Status, StringComparison.InvariantCultureIgnoreCase)))
                {
                    errorMessages.Add($"Record({recordNumber}) Invalid status.");
                }
            }

            return errorMessages;
        }
    }
}
