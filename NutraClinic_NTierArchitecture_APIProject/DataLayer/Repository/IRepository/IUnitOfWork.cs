using DataLayer.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IDietitianRepository Dietitian {get; } // get; → dışarıdan okunabilir, set; yok → dışarıdan değiştirilemez
        IUserRepository User { get; }
        ISecretaryRepository Secretary{get;}
        ISpecialtyRepository Specialty { get; }
        IAppointmentRepository Appointment { get; }

        Task SaveAsync();
    }
}
