using ABCPharmacy.CoreAPI.Models.DTOs.SalesRecord;
using ABCPharmacy.CoreAPI.Models.SalesRecord;
using AutoMapper;
namespace ABCPharmacy.CoreAPI.MapperProfiles.SalesRecords
{
    public class SalesRecordProfile : Profile
    {
        public SalesRecordProfile()
        {
            CreateMap<SalesRecordDTO, SalesRecord>().ReverseMap();
            CreateMap<CreateUpdateSalesRecordDTO, SalesRecord>().ReverseMap();
        }
    }
}
