using DataLayer.Context;
using DataLayer.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IDietitianRepository Dietitian => new DietitianRepository(_context);

        public IUserRepository User => new UserRepository(_context);

        public ISecretaryRepository Secretary => new SecretaryRepository(_context);

        public ISpecialtyRepository Specialty => new SpecialtyRepository(_context);

        public IAppointmentRepository Appointment => new AppointmentRepository(_context);

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
