using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using ModelLayer;

namespace BusinessLayer.Validations
{
    public class AppointmentCreateValidator : AbstractValidator<AppointmentCreateDto>
    {
        public AppointmentCreateValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0);
            RuleFor(x => x.DietitianId).GreaterThan(0);
            RuleFor(x => x.AppointmentDate)
                .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Geçmiş tarih seçilemez.");
            RuleFor(x => x.StartTime).NotEmpty();
            RuleFor(x => x.EndTime)
                .GreaterThan(x => x.StartTime).WithMessage("Bitiş saati başlangıçtan büyük olmalı.");
        }
    }
}
