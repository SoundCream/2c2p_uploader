using System;
using _2C2P.FileUploader.Helper;
using _2C2P.FileUploader.Models.Constants;
using _2C2P.FileUploader.Models.Dtos;
using _2C2P.FileUploader.Models.Entities;
using AutoMapper;

namespace _2C2P.FileUploader.Mappers.MapperProfile
{
    public class TransactionEntiryMapperProfile : Profile
    {
        public TransactionEntiryMapperProfile()
        {
            CreateMap<TransactionUploadModel, TransactionEntity>()
                .ForMember(x => x.Id, source => source.MapFrom(des => des.TransactionId))
                .ForMember(x => x.Amount, source => source.MapFrom(des => MapAmount(des)))
                .ForMember(x => x.CurrencyCode, source => source.MapFrom(des => des.CurrencyCode))
                .ForMember(x => x.TransactionDate, source => source.MapFrom(des => MapDate(des)))
                .ForMember(x => x.CreatedDate, source => source.MapFrom(des => DateTime.Now))
                .ForMember(x => x.Status, source => source.Ignore());

            CreateMap<TransactionEntity, TransactionDto>()
                .ForMember(x => x.Id, source => source.MapFrom(des => des.Id))
                .ForMember(x => x.Payment, source => source.MapFrom(des => $"{des.Amount.ToString("0.00")} {des.CurrencyCode}"))
                .ForMember(x => x.Status, source => source.MapFrom(des => des.Status.StatusCode));
        }

        private decimal MapAmount(TransactionUploadModel transactionUploadModel)
        {
            var value = default(decimal);
            if (transactionUploadModel.Source == UploadSourceEnum.Csv)
            {
                value = GlobalHelper.ParseDecimalNumber(transactionUploadModel.Amount);
            }
            else if (transactionUploadModel.Source == UploadSourceEnum.Xml)
            {
                value = decimal.Parse(transactionUploadModel.Amount);
            }

            return value;
        }

        private DateTime MapDate(TransactionUploadModel transactionUploadModel)
        {
            var dateFormat = string.Empty;
            if (transactionUploadModel.Source == UploadSourceEnum.Csv)
            {
                dateFormat = TransactionConstant.UploadCsvDateFormat;
            }
            else if (transactionUploadModel.Source == UploadSourceEnum.Xml)
            {
                dateFormat = TransactionConstant.UploadXmlDateFormat;
            }

            return GlobalHelper.GetDateTimeFromString(transactionUploadModel.TransactionDate, dateFormat);
        }
    }
}
