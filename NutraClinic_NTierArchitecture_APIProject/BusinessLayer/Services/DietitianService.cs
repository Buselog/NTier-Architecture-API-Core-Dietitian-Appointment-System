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
    public class DietitianService : IDietitianService
    {
        private readonly IUnitOfWork _uow;

        public DietitianService(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<IEnumerable<Dietitian>> GetAllAsync()
        {
            return await _uow.Dietitian.GetAllAsync();
        }

        public async Task<Dietitian> GetByIdAsync(int id)
        {
            return await _uow.Dietitian.GetByIdAsync(id);
        }
        public async Task AddAsync(Dietitian dietitian)
        {
            await _uow.Dietitian.AddAsync(dietitian);
            await _uow.SaveAsync();
        }

        public async Task UpdateAsync(Dietitian dietitian)
        {
            _uow.Dietitian.Update(dietitian);
           await _uow.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var dietitian = await _uow.Dietitian.GetByIdAsync(id);
            if (dietitian != null)
            {
                _uow.Dietitian.Remove(dietitian);
                await _uow.SaveAsync();
            }
        }


    }
}
