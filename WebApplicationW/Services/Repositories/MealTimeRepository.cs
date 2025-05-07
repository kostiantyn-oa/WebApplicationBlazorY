using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplicationW.Models;
using WebApplicationW.Services;

namespace WebApplicationW.Services.Repositories
{
    public class MealTimeRepository : IMealTimeRepository
    {
        private readonly ApplicationDbContext _context;

        public MealTimeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MealTime>> GetAllAsync()
        {
            return await _context.MealTime.ToListAsync();
        }
    }
}