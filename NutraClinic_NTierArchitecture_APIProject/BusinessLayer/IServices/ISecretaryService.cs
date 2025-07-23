using ModelLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.IServices
{
    public interface ISecretaryService
    {
        Task<IEnumerable<Secretary>> GetAllAsync();
        Task<Secretary> GetByIdAsync(int id);
        Task AddAsync(Secretary sec);
        Task UpdateAsync(Secretary sec);
        Task DeleteAsync(int id);
    }
}
