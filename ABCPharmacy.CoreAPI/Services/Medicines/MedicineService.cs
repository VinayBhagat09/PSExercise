using ABCPharmacy.CoreAPI.Models.DTOs;
using ABCPharmacy.CoreAPI.Models.DTOs.Medicine;
using ABCPharmacy.CoreAPI.Models.Medicines;
using ABCPharmacy.CoreAPI.Services.Response;
using AutoMapper;
using FluentValidation;
using System.Text.Json;
namespace ABCPharmacy.CoreAPI.Services.Medicines
{
    public class MedicineService : IMedicineService
    {
        private readonly IMapper _mapper;
        private readonly IValidator<CreateUpdateMedicineDTO> _validator;
        private readonly IResponseService _response;
        private readonly ILogger<MedicineService> _logger;
        private const string JsonFilePath = "Data/medicines.json";
        public MedicineService(IMapper mapper, IValidator<CreateUpdateMedicineDTO> validator, IResponseService response, ILogger<MedicineService> logger)
        {
            _mapper = mapper;
            _validator = validator;
            _response = response;
            _logger = logger;
        }
        public async Task<ResponseDTO> AddAsync(CreateUpdateMedicineDTO entity)
        {
            var validate = await _validator.ValidateAsync(entity);
            if (!validate.IsValid)
            {
                _logger.LogWarning("Validation failed for entity {@Entity}. Errors: {Errors}",
                    entity, string.Join(", ", validate.Errors.Select(e => e.ErrorMessage)));
                return _response.Fail(string.Join(", ", validate.Errors.Select(e => e.ErrorMessage)));
            }
            Medicine medicine = _mapper.Map<Medicine>(entity);
            //Ensure Json file exists
            await EnsureJsonFileExistsAsync();

            //Read existing Medicines
            string json = await File.ReadAllTextAsync(JsonFilePath);
            List<Medicine> medicines = JsonSerializer.Deserialize<List<Medicine>>(json) ?? new ();

            //Assign Next Id
            medicine.Id = medicines.Count > 0 ? medicines.Max(m => m.Id) + 1 : 1;
            //Add new medicine
            medicines.Add(medicine);

            //Save back to file
            var options = new JsonSerializerOptions { WriteIndented = true };
            string updatedJson = JsonSerializer.Serialize(medicines, options);

            await File.WriteAllTextAsync(JsonFilePath, updatedJson);
            _logger.LogInformation("Medicine added successfully with ID {Id}", medicine.Id);
            return _response.Success(medicine, "Record Successfully Added");
        }

        public async Task<ResponseDTO> DeleteAsync(int id)
        {
            //Ensure Json file exists
            await EnsureJsonFileExistsAsync();
            string json = await File.ReadAllTextAsync(JsonFilePath);
            List<Medicine> medicines = JsonSerializer.Deserialize<List<Medicine>>(json) ?? new();
            var medicine = medicines.FirstOrDefault(m => m.Id == id);
            if(medicine == null)
            {
                _logger.LogWarning("Medicine with ID {Id} not found for deletion", id);
                return _response.Fail($"Medicine with ID {id} not found");
            }
            medicines.Remove(medicine);

            //Save back to file
            var options = new JsonSerializerOptions { WriteIndented = true };
            string updatedJson = JsonSerializer.Serialize(medicines, options);

            await File.WriteAllTextAsync(JsonFilePath, updatedJson);
            _logger.LogInformation("Medicine with ID {Id} deleted successfully", id);
            return _response.Success(medicine, $"Record successfully deleted ");

        }
        public async Task<ResponseDTO> GetAllAsync()
        {
            await EnsureJsonFileExistsAsync();
            string json = await File.ReadAllTextAsync(JsonFilePath);
            List<Medicine> medicines = JsonSerializer.Deserialize<List<Medicine>>(json) ?? new();
            var medicineDto = _mapper.Map<List<MedicineDTO>>(medicines);
            return _response.Success(medicineDto, $"Record fetched Sucessfully");

        }
        public async Task<ResponseDTO> GetAsync(int id)
        {
            await EnsureJsonFileExistsAsync();
            string json = await File.ReadAllTextAsync(JsonFilePath);
            List<Medicine> medicines = JsonSerializer.Deserialize<List<Medicine>>(json) ?? new();
            var existingMedicine =  medicines.FirstOrDefault(m => m.Id == id);
            if(existingMedicine == null)
            {
                _logger.LogWarning("Medicine with ID {Id} not found", id);
                return _response.Fail($"Medicine with ID {id} not found");
            }
            var medicineDto = _mapper.Map<MedicineDTO>(existingMedicine);

            return _response.Success(medicineDto, $"Record with {id} fetched Sucessfully");

        }

        public async Task<ResponseDTO> UpdateAsync(int id, CreateUpdateMedicineDTO entity)
        {
            await EnsureJsonFileExistsAsync();
            string json = await File.ReadAllTextAsync(JsonFilePath);

            List<Medicine> medicines = JsonSerializer.Deserialize<List<Medicine>>(json) ?? new();
            var existingMedicine = medicines.FirstOrDefault(m => m.Id == id);
            if(existingMedicine == null)
                return _response.Fail($"Medicine with ID {id} not found");
            //Map new values
            _mapper.Map(entity, existingMedicine);

            //Save back to file
            var options = new JsonSerializerOptions { WriteIndented = true };
            string updatedJson = JsonSerializer.Serialize(medicines, options);

            await File.WriteAllTextAsync(JsonFilePath, updatedJson);
            _logger.LogInformation("Medicine Updated successfully with ID {Id}", existingMedicine.Id);
            return _response.Success(existingMedicine, $"Record Successully Updated");

        }
        private async Task EnsureJsonFileExistsAsync()
        {
            string directory = Path.GetDirectoryName(JsonFilePath)!;
            if(!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            if (!File.Exists(JsonFilePath))
            {
                await File.WriteAllTextAsync(JsonFilePath, "[]");
            }

        }
    }
}
