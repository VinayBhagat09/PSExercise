using ABCPharmacy.CoreAPI.Models.DTOs.Medicine;
using FluentValidation;

namespace ABCPharmacy.CoreAPI.Models.Validators
{
    public class CreateUpdateMedicineDTOValidator : AbstractValidator<CreateUpdateMedicineDTO>
    {
        public CreateUpdateMedicineDTOValidator()
        {
            RuleFor(x => x.MedicineName)
                .NotEmpty()
                .WithMessage("Medicine Name is Required")
                .MaximumLength(80);

            RuleFor(x => x.Notes)
                .MaximumLength(500);

            RuleFor(x => x.ExpiryDate)
                .GreaterThan(DateTime.Now)
                .WithMessage("Expiry Date Must be in Future");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity Must be greator than 0");

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Price must be greator than 0");

            RuleFor(x => x.Brand)
                .NotEmpty()
                .WithMessage("Brand Name is Required")
                .MaximumLength(50);

        }
    }
}
