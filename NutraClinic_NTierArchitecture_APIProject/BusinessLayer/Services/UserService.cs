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
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;

        public UserService(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _uow.User.GetAllAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _uow.User.GetByIdAsync(id);
        }
        public async Task AddAsync(User User)
        {
            await _uow.User.AddAsync(User);
            await _uow.SaveAsync();
        }

        public async Task UpdateAsync(User User)
        {
            _uow.User.Update(User);
            await _uow.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var User = await _uow.User.GetByIdAsync(id);
            if (User != null)
            {
                _uow.User.Remove(User);
                await _uow.SaveAsync();
            }
        }

    }
}
