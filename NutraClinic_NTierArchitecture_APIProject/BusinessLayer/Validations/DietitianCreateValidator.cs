using FluentValidation;
using ModelLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Validations
{
    public class DietitianCreateValidator: AbstractValidator<DietitianCreateDto>
    {
        public DietitianCreateValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Diyetisyen adı boş olamaz.");

            RuleFor(x => x.Email)
                .NotEmpty().EmailAddress().WithMessage("Geçerli bir e-posta girin.");

            RuleFor(x => x.Password)
                .NotEmpty().MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalı.");

            RuleFor(x => x.SpecialtyId)
                .GreaterThan(0).WithMessage("Geçerli bir uzmanlık ID girin.");
        }
    }
}
