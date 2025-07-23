using DataLayer.Context;
using DataLayer.Repository.IRepository;
using ModelLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public class SpecialtyRepository : GenericRepository<Specialty>, ISpecialtyRepository
    {
        private readonly AppDbContext _context;
        public SpecialtyRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
