using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplicationW.Models;

namespace WebApplicationW.Services.Repositories
{
    public class CollectionRepository : ICollectionRepository
    {
        private readonly ApplicationDbContext _context;

        public CollectionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Collection>> GetAllAsync()
        {
            return await _context.Collection
                .Include(c => c.CollectionCategory)
                .Include(c => c.CollectionRecipes).ThenInclude(cr => cr.Recipe)
                .ToListAsync();
        }

        public async Task<Collection> GetByIdAsync(int id)
        {
            return await _context.Collection
                .Include(c => c.CollectionCategory)
                .Include(c => c.CollectionRecipes).ThenInclude(cr => cr.Recipe)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Collection> AddAsync(Collection collection)
        {
            collection.CollectionCategory = null;
            if (collection.CollectionRecipes != null)
            {
                foreach (var cr in collection.CollectionRecipes)
                {
                    cr.CollectionId = 0;
                    cr.Collection = null;
                    cr.Recipe = null;
                }
            }

            _context.Collection.Add(collection);
            await _context.SaveChangesAsync();
            return collection;
        }

        public async Task UpdateAsync(int id, Collection collection)
        {
            var existingCollection = await _context.Collection
                .Include(c => c.CollectionRecipes)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (existingCollection == null)
            {
                throw new KeyNotFoundException($"Collection with ID {id} not found.");
            }

            collection.CollectionCategory = null;
            if (collection.CollectionRecipes != null)
            {
                foreach (var cr in collection.CollectionRecipes)
                {
                    cr.CollectionId = id;
                    cr.Collection = null;
                    cr.Recipe = null;
                }
            }

            _context.Entry(existingCollection).CurrentValues.SetValues(collection);

            _context.CollectionRecipe.RemoveRange(existingCollection.CollectionRecipes);
            if (collection.CollectionRecipes != null)
            {
                foreach (var cr in collection.CollectionRecipes)
                {
                    _context.CollectionRecipe.Add(new CollectionRecipe
                    {
                        CollectionId = id,
                        RecipeId = cr.RecipeId
                    });
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var collection = await _context.Collection.FindAsync(id);
            if (collection == null)
            {
                throw new KeyNotFoundException($"Collection with ID {id} not found.");
            }

            _context.Collection.Remove(collection);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CollectionExistsAsync(int id)
        {
            return await _context.Collection.AnyAsync(c => c.Id == id);
        }
    }
}