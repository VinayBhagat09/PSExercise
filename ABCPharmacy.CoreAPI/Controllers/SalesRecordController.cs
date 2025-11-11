using ABCPharmacy.CoreAPI.Models.DTOs.SalesRecord;
using ABCPharmacy.CoreAPI.Services.SalesRecords;
using Microsoft.AspNetCore.Mvc;

namespace ABCPharmacy.CoreAPI.Controllers
{
    public class SalesRecordController : CustomControllerBase
    {
        public readonly ISalesRecordsService _salesRecordsService;
        public SalesRecordController(ISalesRecordsService salesRecordsService)
        {
            _salesRecordsService = salesRecordsService;
        }
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var result = await _salesRecordsService.GetAllAsync();
            return result.isSucess ? Ok(result) : BadRequest(result);

        }
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var result = await _salesRecordsService.GetAsync(id);
            return result.isSucess ? Ok(result) : BadRequest(result);
        }
        [HttpPost]
        public async Task<ActionResult> AddAsync(CreateUpdateSalesRecordDTO createUpdateSalesRecordDTO)
        {
            var result = await _salesRecordsService.AddAsync(createUpdateSalesRecordDTO);
            return result.isSucess ? Ok(result) : BadRequest(result);
        }
        [HttpPut]
        public async Task<ActionResult> UpdateAsync(int id, CreateUpdateSalesRecordDTO createUpdateSalesRecordDTO)
        {
            var result = await _salesRecordsService.UpdateAsync(id, createUpdateSalesRecordDTO);
            return result.isSucess ? Ok(result) : BadRequest(result);
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var result = await _salesRecordsService.DeleteAsync(id);
            return result.isSucess ? Ok(result) : BadRequest(result);
        }
    }
}
