using ModelLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository.IRepository
{

    public interface IDietitianRepository : IGenericRepository<Dietitian>
    {
        Task<IEnumerable<Dietitian>> GetBySpecialtyAsync(int specialtyId);
    }
}
