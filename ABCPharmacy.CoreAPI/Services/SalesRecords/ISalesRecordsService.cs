using ABCPharmacy.CoreAPI.Models.DTOs.SalesRecord;
using ABCPharmacy.CoreAPI.Models.SalesRecord;

namespace ABCPharmacy.CoreAPI.Services.SalesRecords
{
    public interface ISalesRecordsService : IBaseService<SalesRecord,CreateUpdateSalesRecordDTO>
    {
    }
}
