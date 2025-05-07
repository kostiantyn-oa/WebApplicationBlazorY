using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationW.Models;

namespace WebApplicationW.Services.Repositories
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly ApplicationDbContext _context;

        public RecipeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Recipe>> GetAllAsync()
        {
            return await _context.Recipe
                .Include(r => r.DifficultyLevel)
                .Include(r => r.RecipeIngredients).ThenInclude(ri => ri.Ingredient)
                .Include(r => r.RecipeCategories).ThenInclude(rc => rc.Category)
                .Include(r => r.RecipeMealTimes).ThenInclude(rm => rm.MealTime)
                .ToListAsync();
        }

        public async Task<Recipe> GetByIdAsync(int id)
        {
            return await _context.Recipe
                .Include(r => r.DifficultyLevel)
                .Include(r => r.RecipeIngredients).ThenInclude(ri => ri.Ingredient)
                .Include(r => r.RecipeCategories).ThenInclude(rc => rc.Category)
                .Include(r => r.RecipeMealTimes).ThenInclude(rm => rm.MealTime)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Recipe> AddAsync(Recipe recipe)
        {
            recipe.DifficultyLevel = null;
            if (recipe.RecipeIngredients != null)
            {
                foreach (var ri in recipe.RecipeIngredients)
                {
                    ri.RecipeId = 0;
                    ri.Recipe = null;
                    ri.Ingredient = null;
                }
            }
            if (recipe.RecipeCategories != null)
            {
                foreach (var rc in recipe.RecipeCategories)
                {
                    rc.RecipeId = 0;
                    rc.Recipe = null;
                    rc.Category = null;
                }
            }
            if (recipe.RecipeMealTimes != null)
            {
                foreach (var rm in recipe.RecipeMealTimes)
                {
                    rm.RecipeId = 0;
                    rm.Recipe = null;
                    rm.MealTime = null;
                }
            }

            recipe.CreatedDate = DateTime.UtcNow;
            _context.Recipe.Add(recipe);
            await _context.SaveChangesAsync();
            return recipe;
        }

        public async Task UpdateAsync(int id, Recipe recipe)
        {
            var existingRecipe = await _context.Recipe
                .Include(r => r.RecipeIngredients)
                .Include(r => r.RecipeCategories)
                .Include(r => r.RecipeMealTimes)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (existingRecipe == null)
            {
                throw new KeyNotFoundException($"Recipe with ID {id} not found.");
            }

            recipe.DifficultyLevel = null;
            if (recipe.RecipeIngredients != null)
            {
                foreach (var ri in recipe.RecipeIngredients)
                {
                    ri.RecipeId = id;
                    ri.Recipe = null;
                    ri.Ingredient = null;
                }
            }
            if (recipe.RecipeCategories != null)
            {
                foreach (var rc in recipe.RecipeCategories)
                {
                    rc.RecipeId = id;
                    rc.Recipe = null;
                    rc.Category = null;
                }
            }
            if (recipe.RecipeMealTimes != null)
            {
                foreach (var rm in recipe.RecipeMealTimes)
                {
                    rm.RecipeId = id;
                    rm.Recipe = null;
                    rm.MealTime = null;
                }
            }

            _context.Entry(existingRecipe).CurrentValues.SetValues(recipe);

            _context.RecipeIngredient.RemoveRange(existingRecipe.RecipeIngredients);
            if (recipe.RecipeIngredients != null)
            {
                foreach (var ri in recipe.RecipeIngredients)
                {
                    _context.RecipeIngredient.Add(new RecipeIngredient
                    {
                        RecipeId = id,
                        IngredientId = ri.IngredientId,
                        Quantity = ri.Quantity
                    });
                }
            }

            _context.RecipeCategory.RemoveRange(existingRecipe.RecipeCategories);
            if (recipe.RecipeCategories != null)
            {
                foreach (var rc in recipe.RecipeCategories)
                {
                    _context.RecipeCategory.Add(new RecipeCategory
                    {
                        RecipeId = id,
                        CategoryId = rc.CategoryId
                    });
                }
            }

            _context.RecipeMealTime.RemoveRange(existingRecipe.RecipeMealTimes);
            if (recipe.RecipeMealTimes != null)
            {
                foreach (var rm in recipe.RecipeMealTimes)
                {
                    _context.RecipeMealTime.Add(new RecipeMealTime
                    {
                        RecipeId = id,
                        MealTimeId = rm.MealTimeId
                    });
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var recipe = await _context.Recipe.FindAsync(id);
            if (recipe == null)
            {
                throw new KeyNotFoundException($"Recipe with ID {id} not found.");
            }

            _context.Recipe.Remove(recipe);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> RecipeExistsAsync(int id)
        {
            return await _context.Recipe.AnyAsync(r => r.Id == id);
        }
    }
}