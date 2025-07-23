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
    public class SecretaryService : ISecretaryService
    {
        private readonly IUnitOfWork _uow;

        public SecretaryService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<Secretary>> GetAllAsync()
        {
            return await _uow.Secretary.GetAllAsync();
        }

        public async Task<Secretary> GetByIdAsync(int id)
        {
            return await _uow.Secretary.GetByIdAsync(id);
        }

        public async Task AddAsync(Secretary sec)
        {
            await _uow.Secretary.AddAsync(sec);
            await _uow.SaveAsync();
        }

        public async Task UpdateAsync(Secretary sec)
        {
            _uow.Secretary.Update(sec);
            await _uow.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _uow.Secretary.GetByIdAsync(id);
            if (entity != null)
            {
                _uow.Secretary.Remove(entity);
                await _uow.SaveAsync();
            }
        }
    }
}
