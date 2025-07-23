using ModelLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.IServices
{
    public interface ISpecialtyService
    {
        Task<IEnumerable<Specialty>> GetAllAsync();
        Task<Specialty> GetByIdAsync(int id);
        Task AddAsync(Specialty specialty);         
        Task UpdateAsync(Specialty specialty);      
        Task DeleteAsync(int id);
    }
}
