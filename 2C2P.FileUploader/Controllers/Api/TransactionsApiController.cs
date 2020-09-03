using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using _2C2P.FileUploader.Interfaces.Managers;
using _2C2P.FileUploader.Models.CustomExceptions;
using _2C2P.FileUploader.Models.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.Swagger.Annotations;

namespace _2C2P.FileUploader.Controllers.Api
{
    [ApiController]
    [Route("api/Transactions")]
    public class TransactionsApiController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly ITransactionManager _transactionManager;
        private readonly IFileUploadManager _fileUploadManager;

        public TransactionsApiController(
            IMapper mapper,
            ILogger<TransactionsApiController> logger,
            ITransactionManager transactionManager,
            IFileUploadManager fileUploadManager)
        {
            _logger = logger;
            _mapper = mapper;
            _transactionManager = transactionManager;
            _fileUploadManager = fileUploadManager;
        }

        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<TransactionDto>))]
        public async Task<IActionResult> GetTransactions(string currentcy, string statusCode, string from, string to)
        {
            try
            {
                var transactions = await _transactionManager.GetTransactions(currentcy, statusCode, from, to);
                var result = _mapper.Map<List<TransactionDto>>(transactions);
                return new JsonResult(result);
            }
            catch (ValidationErrorsException vex)
            {
                _logger.LogError(vex.Message);
                return HttpResponse(HttpStatusCode.BadRequest, vex.ErrorMessages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return HttpResponse(HttpStatusCode.InternalServerError, "InternalServerError");
            }
        }

        [HttpPost]
        public async Task<JsonResult> UploadTransaction(IFormFile formFile)
        {
            try
            {
                using (var stream = formFile.OpenReadStream())
                {
                    var uploadTransactions = _fileUploadManager.DeserializeStreamTransactionUploadFile<TransactionUploadModel>(stream, formFile.FileName);
                    await _transactionManager.InsertUploadTransaction(uploadTransactions, formFile.FileName);
                }

                return HttpResponse(HttpStatusCode.OK, "Success");
            }
            catch (FileUploadErrorException fuex)
            {
                _logger.LogError(fuex.Message);
                return HttpResponse(HttpStatusCode.BadRequest, "Unknown format");
            }
            catch (ValidationErrorsException vex)
            {
                _logger.LogError(vex.Message);
                return HttpResponse(HttpStatusCode.BadRequest, vex.ErrorMessages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return HttpResponse(HttpStatusCode.InternalServerError, "InternalServerError");
            }
        }

        private JsonResult HttpResponse(HttpStatusCode httpStatusCode, object result)
        {
            HttpContext.Response.StatusCode = (int)httpStatusCode;
            var response = new HttpResponseMessage(httpStatusCode);
            return new JsonResult(result);
        }
    }
}
