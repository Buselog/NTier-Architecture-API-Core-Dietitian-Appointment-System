using ModelLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.IServices
{
    public interface IDietitianService
    {
        Task<IEnumerable<Dietitian>> GetAllAsync();
        Task<Dietitian> GetByIdAsync(int id);
        Task AddAsync(Dietitian dietitian);
        Task UpdateAsync(Dietitian dietitian);
        Task DeleteAsync(int id);
    }
}
