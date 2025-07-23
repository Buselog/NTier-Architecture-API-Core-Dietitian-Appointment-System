using BusinessLayer.IServices;
using DataLayer.Repository.IRepository;
using ModelLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class SpecialtyService : ISpecialtyService
    {
        private readonly IUnitOfWork _uow;

        public SpecialtyService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<Specialty>> GetAllAsync()
        {
            return await _uow.Specialty.GetAllAsync();
        }

        public async Task<Specialty> GetByIdAsync(int id)
        {
            return await _uow.Specialty.GetByIdAsync(id);
        }

        public async Task AddAsync(Specialty specialty)
        {
            await _uow.Specialty.AddAsync(specialty);
            await _uow.SaveAsync();
        }

        public async Task UpdateAsync(Specialty specialty)
        {
            _uow.Specialty.Update(specialty);
            await _uow.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _uow.Specialty.GetByIdAsync(id);
            if (entity != null)
            {
                _uow.Specialty.Remove(entity);
                await _uow.SaveAsync();
            }
        }
    }
}
