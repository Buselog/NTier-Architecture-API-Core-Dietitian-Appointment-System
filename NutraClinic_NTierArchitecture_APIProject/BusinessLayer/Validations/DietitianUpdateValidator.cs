using FluentValidation;
using ModelLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Validations
{
    public class DietitianUpdateValidator : AbstractValidator<DietitianUpdateDto>
    {
        public DietitianUpdateValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).MinimumLength(6).When(x => !string.IsNullOrEmpty(x.Password));
            RuleFor(x => x.Biography).MaximumLength(1000);

        }
    }
}
