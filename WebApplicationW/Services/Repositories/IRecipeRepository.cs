using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplicationW.Models;

namespace WebApplicationW.Services.Repositories
{
    public interface IRecipeRepository
    {
        Task<IEnumerable<Recipe>> GetAllAsync();
        Task<Recipe> GetByIdAsync(int id);
        Task<Recipe> AddAsync(Recipe recipe);
        Task UpdateAsync(int id, Recipe recipe);
        Task DeleteAsync(int id);
        Task<bool> RecipeExistsAsync(int id);
    }
}