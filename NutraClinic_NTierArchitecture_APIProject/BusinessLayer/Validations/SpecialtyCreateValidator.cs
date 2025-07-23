using FluentValidation;
using ModelLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Validations
{
    public class SpecialtyCreateValidator: AbstractValidator<SpecialtyCreateDto>
    {
        public SpecialtyCreateValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Uzmanlık adı boş olamaz.")
                .MaximumLength(100).WithMessage("Uzmanlık adı 100 karakterden fazla olamaz.");
        }
    }
}
