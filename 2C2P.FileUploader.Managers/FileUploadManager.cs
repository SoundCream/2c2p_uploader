﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using _2C2P.FileUploader.Helper.Serializers;
using _2C2P.FileUploader.Interfaces.Managers;
using _2C2P.FileUploader.Models.ConfigurationOptions;
using _2C2P.FileUploader.Models.CustomExceptions;
using _2C2P.FileUploader.Models.Dtos;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace _2C2P.FileUploader.Managers
{
    /// <summary>
    /// FileUploadManager
    /// </summary>
    public class FileUploadManager : IFileUploadManager
    {
        private const string CsvExtension = ".csv";
        private const string XmlExtension = ".xml";

        private readonly AppConfiguration _appConfiguration;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public FileUploadManager(IOptions<AppConfiguration> appConfiguration, ILogger<FileUploadManager> logger, IMapper mapper)
        {
            _mapper = mapper;
            _logger = logger;
            _appConfiguration = appConfiguration.Value;
        }

        /// <summary>
        /// Deserialize UploadFile Stream Transaction.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public List<T> DeserializeStreamTransactionUploadFile<T>(Stream stream, string fileName) where T : class
        {
            try
            {
                _logger.LogTrace($"Begin DeserializeStream file {fileName}");
                var result = new List<T>();
                ValidateFileUploadedSize(stream.Length);
                var fileExtension = GetFileExtensionAllowed(fileName);
                if (string.Equals(CsvExtension, fileExtension, StringComparison.InvariantCultureIgnoreCase))
                {
                    var data = CsvStreamDeserialize<CsvTransactionUploadDto>(stream);
                    result = _mapper.Map<List<T>>(data);
                }
                else if (string.Equals(XmlExtension, fileExtension, StringComparison.InvariantCultureIgnoreCase))
                {
                    var data = XmlStreamDeserialize<XmlListTransactionUploadDto>(stream);
                    result = _mapper.Map<List<T>>(data.Transactions);
                }
                else
                {
                    throw new FileUploadErrorException($"File {fileExtension} not supported.");
                }

                _logger.LogTrace($"Complete DeserializeStream file {fileName}");
                return result;
            }
            catch (FileUploadErrorException fuex)
            {
                throw fuex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<T> CsvStreamDeserialize<T>(Stream stream) where T : class, new()
        {
            try
            {
                var result = new List<T>();
                using (var reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        var row = reader.ReadLine();
                        var item = CsvSerializerHelper.Deserialize<T>(row);
                        result.Add(item);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new FileUploadErrorException("Incorrect file format.");
            }
        }

        private T XmlStreamDeserialize<T>(Stream stream) where T : class
        {
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                var result = (T)serializer.Deserialize(stream);
                return result;
            }
            catch (Exception ex)
            {
                throw new FileUploadErrorException("Incorrect file format.");
            }
        }

        public void ValidateFileUploadedSize(long fileSize)
        {
            if (fileSize > _appConfiguration.MaximunFilesize)
            {
                throw new FileUploadErrorException($"File size cannot more than {fileSize} byte.");
            }

            if (fileSize < 1)
            {
                throw new FileUploadErrorException("Don't have any file uploaded.");
            }
        }

        public string GetFileExtensionAllowed(string fileName)
        {
            var fi = new FileInfo(fileName);
            if (!_appConfiguration.AllowFileExtionsions.Any(x => string.Equals(x, fi.Extension, StringComparison.InvariantCultureIgnoreCase)))
            {
                var extensionAllowed = string.Join(",", _appConfiguration.AllowFileExtionsions);
                throw new FileUploadErrorException($"Uploadfile allowed extensions: ({extensionAllowed}) only.");
            }

            return fi.Extension;
        }
    }
}
