using _2C2P.FileUploader.Models.Constants;
using _2C2P.FileUploader.Models.Dtos;
using AutoMapper;

namespace _2C2P.FileUploader.Mappers.MapperProfile
{
    public class TransactionUploadMapperProfile : Profile
    {
        public TransactionUploadMapperProfile()
        {
            CreateMap<CsvTransactionUploadDto, TransactionUploadModel>()
                .ForMember(x => x.Source, source => source.MapFrom(des => UploadSourceEnum.Csv))
                .ForMember(x => x.TransactionId, source => source.MapFrom(des => des.TransactionId))
                .ForMember(x => x.Amount, source => source.MapFrom(des => des.Amount))
                .ForMember(x => x.CurrencyCode, source => source.MapFrom(des => des.CurrencyCode))
                .ForMember(x => x.TransactionDate, source => source.MapFrom(des => des.TransactionDate))
                .ForMember(x => x.Status, source => source.MapFrom(des => des.Status));

            CreateMap<XmlTransactionUploadDto, TransactionUploadModel>()
                .ForMember(x => x.Source, source => source.MapFrom(des => UploadSourceEnum.Xml))
                .ForMember(x => x.TransactionId, source => source.MapFrom(des => des.Id))
                .ForMember(x => x.Amount, source => source.MapFrom(des => des.PaymentDetails.Amount))
                .ForMember(x => x.CurrencyCode, source => source.MapFrom(des => des.PaymentDetails.CurrencyCode))
                .ForMember(x => x.TransactionDate, source => source.MapFrom(des => des.TransactionDate))
                .ForMember(x => x.Status, source => source.MapFrom(des => des.Status));
        }
    }
}
