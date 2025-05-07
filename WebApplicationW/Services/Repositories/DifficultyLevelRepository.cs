using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplicationW.Models;
using WebApplicationW.Services;

namespace WebApplicationW.Services.Repositories
{
    public class DifficultyLevelRepository : IDifficultyLevelRepository
    {
        private readonly ApplicationDbContext _context;

        public DifficultyLevelRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DifficultyLevel>> GetAllAsync()
        {
            return await _context.DifficultyLevel.ToListAsync();
        }
    }
}