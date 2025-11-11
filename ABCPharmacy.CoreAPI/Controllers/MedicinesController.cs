using ABCPharmacy.CoreAPI.Models.DTOs.Medicine;
using ABCPharmacy.CoreAPI.Services.Medicines;
using Microsoft.AspNetCore.Mvc;

namespace ABCPharmacy.CoreAPI.Controllers
{
    public class MedicinesController : CustomControllerBase
    {
        public readonly IMedicineService _medicineService;

        public MedicinesController(IMedicineService medicineService)
        {
            _medicineService = medicineService;
        }
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var result = await _medicineService.GetAllAsync();
            return result.isSucess ? Ok(result) : BadRequest(result);

        }
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var result = await _medicineService.GetAsync(id);
            return result.isSucess ? Ok(result) : BadRequest(result);
        }
        [HttpPost]
        public async Task<ActionResult> AddAsync(CreateUpdateMedicineDTO createUpdateMedicine)
        {
            var result = await _medicineService.AddAsync(createUpdateMedicine);
            return result.isSucess ? Ok(result) : BadRequest(result);
        }
        [HttpPut]
        public async Task<ActionResult>UpdateAsync(int id,CreateUpdateMedicineDTO createUpdateMedicineDTO)
        {
            var result = await _medicineService.UpdateAsync(id,createUpdateMedicineDTO);
            return result.isSucess ? Ok(result) : BadRequest(result);
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var result = await _medicineService.DeleteAsync(id);
            return result.isSucess ? Ok(result) : BadRequest(result);
        }

    }
}
