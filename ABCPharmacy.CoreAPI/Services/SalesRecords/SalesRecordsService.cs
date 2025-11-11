using ABCPharmacy.CoreAPI.Models.DTOs;
using ABCPharmacy.CoreAPI.Models.DTOs.Medicine;
using ABCPharmacy.CoreAPI.Models.DTOs.SalesRecord;
using ABCPharmacy.CoreAPI.Models.Medicines;
using ABCPharmacy.CoreAPI.Models.SalesRecord;
using ABCPharmacy.CoreAPI.Services.Response;
using AutoMapper;
using FluentValidation;
using System.Text.Json;

namespace ABCPharmacy.CoreAPI.Services.SalesRecords
{
    public class SalesRecordsService : ISalesRecordsService
    {
        private readonly IMapper _mapper;
        private readonly IValidator<CreateUpdateSalesRecordDTO> _validator;
        private readonly ILogger<SalesRecordsService> _logger;
        private readonly IResponseService _response;
        private const string JsonFilePath = "Data/salesrecords.json";
        private const string MedicinesJsonFilePath = "Data/medicines.json";

        public SalesRecordsService(IMapper mapper, IValidator<CreateUpdateSalesRecordDTO> validator, IResponseService response, ILogger<SalesRecordsService> logger)
        {
            _mapper = mapper;
            _validator = validator;
            _response = response;
            _logger = logger;
        }

        public async Task<ResponseDTO> AddAsync(CreateUpdateSalesRecordDTO entity)
        {
            var validate = await _validator.ValidateAsync(entity);
            if (!validate.IsValid)
            {
                _logger.LogWarning("Validation failed for sales record {@Entity}: {Errors}",
                entity, string.Join(", ", validate.Errors.Select(e => e.ErrorMessage)));
                return _response.Fail(string.Join(", ", validate.Errors.Select(e => e.ErrorMessage)));
            }
            SalesRecord salesRecord = _mapper.Map<SalesRecord>(entity);
            //Ensure Json file exists
            await EnsureJsonFileExistsAsync();

            //Read existing records
            string medicinesjson = await File.ReadAllTextAsync(MedicinesJsonFilePath);
            string salesjson = await File.ReadAllTextAsync(JsonFilePath);
            List<Medicine> medicines = JsonSerializer.Deserialize<List<Medicine>>(medicinesjson) ?? new();
            List<SalesRecord> salesRecords = JsonSerializer.Deserialize<List<SalesRecord>>(salesjson) ?? new();

            //ensure the medicine exists before creating
            var medicineExists = medicines.Any(m => m.Id == entity.MedicineId);
            if (!medicineExists)
            {
                _logger.LogWarning("AddAsync failed: Medicine ID {MedicineId} not found", entity.MedicineId);
                return _response.Fail($"Medicine with ID {entity.MedicineId} not found. Cannot create sales record.");
            }

            //Assign Next Id
            salesRecord.Id = salesRecords.Count > 0 ? salesRecords.Max(m => m.Id) + 1 : 1;
            //Add new record
            salesRecords.Add(salesRecord);

            //Save back to file
            var options = new JsonSerializerOptions { WriteIndented = true };
            string updatedJson = JsonSerializer.Serialize(salesRecords, options);

            await File.WriteAllTextAsync(JsonFilePath, updatedJson);
            _logger.LogInformation("Sales record added successfully with ID {Id}", salesRecord.Id);
            return _response.Success(salesRecord, "Record Successfully Added");
        }

        public async Task<ResponseDTO> DeleteAsync(int id)
        {
            //Ensure Json file exists
            await EnsureJsonFileExistsAsync();
            string json = await File.ReadAllTextAsync(JsonFilePath);
            List<SalesRecord> salesRecords = JsonSerializer.Deserialize<List<SalesRecord>>(json) ?? new();
            var salesRecord = salesRecords.FirstOrDefault(s => s.Id == id);
            if (salesRecord == null)
            {
                _logger.LogWarning("Sales record with ID {Id} not found for deletion", id);
                return _response.Fail($"Sales Record with ID {id} not found");
            }
            salesRecords.Remove(salesRecord);

            //Save back to file
            var options = new JsonSerializerOptions { WriteIndented = true };
            string updatedJson = JsonSerializer.Serialize(salesRecords, options);

            await File.WriteAllTextAsync(JsonFilePath, updatedJson);
            _logger.LogInformation("Sales record with ID {Id} deleted successfully", id);
            return _response.Success(salesRecord, $"Record successfully deleted");
        }

        public async Task<ResponseDTO> GetAllAsync()
        {
            await EnsureJsonFileExistsAsync();
            string json = await File.ReadAllTextAsync(JsonFilePath);
            List<SalesRecord> salesRecords = JsonSerializer.Deserialize<List<SalesRecord>>(json) ?? new();
            _logger.LogInformation("Fetched {Count} sales records", salesRecords.Count);
            var salesRecordsDto = _mapper.Map<List<SalesRecordDTO>>(salesRecords);
            return _response.Success(salesRecordsDto, $"Record fetched Sucessfully");
        }

        public async Task<ResponseDTO> GetAsync(int id)
        {
            await EnsureJsonFileExistsAsync();
            string json = await File.ReadAllTextAsync(JsonFilePath);
            List<SalesRecord> salesRecords = JsonSerializer.Deserialize<List<SalesRecord>>(json) ?? new();
            var existingRecord = salesRecords.FirstOrDefault(s => s.Id == id);
            if (existingRecord == null)
            {
                _logger.LogWarning("Sales record with ID {Id} not found", id);
                return _response.Fail($"Records with ID {id} not found");
            }
            var salesRecordsDto = _mapper.Map<SalesRecordDTO>(existingRecord);
            _logger.LogInformation("Fetched sales record with ID {Id}", id);
            return _response.Success(salesRecordsDto, $"Record with {id} fetched Sucessfully");
        }

        public async Task<ResponseDTO> UpdateAsync(int id, CreateUpdateSalesRecordDTO entity)
        {
            await EnsureJsonFileExistsAsync();
            string medicinesjson = await File.ReadAllTextAsync(MedicinesJsonFilePath);
            string salesjson = await File.ReadAllTextAsync(JsonFilePath);
            List<Medicine> medicines = JsonSerializer.Deserialize<List<Medicine>>(medicinesjson) ?? new();
            List<SalesRecord> salesRecords = JsonSerializer.Deserialize<List<SalesRecord>>(salesjson) ?? new();

            //find existing sales record
            var existingSalesRecords = salesRecords.FirstOrDefault(s => s.Id == id);
            if (existingSalesRecords == null)
            {
                _logger.LogWarning("Sales record with ID {Id} not found for update", id);
                return _response.Fail($"Sales Record with ID {id} not found");
            }
               

            //ensure the medicine exists before updating
            var medicineExists = medicines.Any(m => m.Id == entity.MedicineId);
            if (!medicineExists)
            {
                _logger.LogWarning("Update failed: Medicine ID {MedicineId} not found", entity.MedicineId);
                return _response.Fail($"Medicine with ID {entity.MedicineId} not found. Cannot update sales record.");
            }


            //Map new values
            _mapper.Map(entity, existingSalesRecords);

            //Save back to file
            var options = new JsonSerializerOptions { WriteIndented = true };
            string updatedJson = JsonSerializer.Serialize(salesRecords, options);

            await File.WriteAllTextAsync(JsonFilePath, updatedJson);
            _logger.LogInformation("Sales record with ID {Id} updated successfully", id);
            return _response.Success(existingSalesRecords, $"Record Successully Updated");
        }
        private async Task EnsureJsonFileExistsAsync()
        {
            string directory = Path.GetDirectoryName(JsonFilePath)!;
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            if (!File.Exists(JsonFilePath))
                await File.WriteAllTextAsync(JsonFilePath, "[]");

            // Ensure Medicines JSON exists
            string medicinesDirectory = Path.GetDirectoryName(MedicinesJsonFilePath)!;
            if (!Directory.Exists(medicinesDirectory))
                Directory.CreateDirectory(medicinesDirectory);

            if (!File.Exists(MedicinesJsonFilePath))
            {
                await File.WriteAllTextAsync(MedicinesJsonFilePath, "[]");
            }

        }
    }
}
