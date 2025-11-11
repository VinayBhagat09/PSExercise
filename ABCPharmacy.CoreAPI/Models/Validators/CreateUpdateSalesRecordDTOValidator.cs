using ABCPharmacy.CoreAPI.Models.DTOs.SalesRecord;
using FluentValidation;

namespace ABCPharmacy.CoreAPI.Models.Validators
{
    public class CreateUpdateSalesRecordDTOValidator : AbstractValidator<CreateUpdateSalesRecordDTO>
    {
        public CreateUpdateSalesRecordDTOValidator()
        {
            RuleFor(x => x.MedicineId)
                .NotEmpty()
                .WithMessage("Medicine Id is Required");

            RuleFor(x => x.QuantitySold)
                .GreaterThan(0)
                .WithMessage("Quantity sold field must be greator than 0");
        }
    }
}
