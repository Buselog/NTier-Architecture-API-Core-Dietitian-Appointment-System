using DataLayer.Context;
using DataLayer.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using ModelLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public class DietitianRepository : GenericRepository<Dietitian>, IDietitianRepository
    {
        private readonly AppDbContext _context;

        public DietitianRepository(AppDbContext context): base(context)
        {
            _context = context;

        }
        public async Task<IEnumerable<Dietitian>> GetBySpecialtyAsync(int specialtyId)
        {
            return await _context.Dietitians.Where(p => p.SpecialtyId == specialtyId).ToListAsync();
        }
    }
}
