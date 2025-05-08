using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplicationW.Models;

namespace WebApplicationW.Services.Repositories
{
    public class CollectionCategoryRepository : ICollectionCategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CollectionCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CollectionCategory>> GetAllAsync()
        {
            return await _context.CollectionCategory.ToListAsync();
        }
    }
}