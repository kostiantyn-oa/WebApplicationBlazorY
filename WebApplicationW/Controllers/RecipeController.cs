using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApplicationW.Models;
using WebApplicationW.Services.Repositories;

namespace WebApplicationW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeRepository _recipeRepository;

        public RecipeController(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipes()
        {
            var recipes = await _recipeRepository.GetAllAsync();
            return Ok(recipes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Recipe>> GetRecipe(int id)
        {
            var recipe = await _recipeRepository.GetByIdAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }
            return Ok(recipe);
        }

        [HttpPost]
        public async Task<ActionResult<Recipe>> PostRecipe(Recipe recipe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Невалідна модель.", Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            if (string.IsNullOrWhiteSpace(recipe.Title))
            {
                return BadRequest(new { Message = "Назва рецепту є обов'язковою." });
            }

            if (string.IsNullOrWhiteSpace(recipe.Instructions))
            {
                return BadRequest(new { Message = "Інструкції є обов'язковими." });
            }

            if (recipe.CookingTime <= 0)
            {
                return BadRequest(new { Message = "Час приготування має бути більше 0." });
            }

            if (!recipe.DifficultyLevelId.HasValue || recipe.DifficultyLevelId <= 0)
            {
                return BadRequest(new { Message = $"Невалідний DifficultyLevelId: {recipe.DifficultyLevelId ?? 0}." });
            }

            // Валідація інгредієнтів
            if (recipe.RecipeIngredients == null || !recipe.RecipeIngredients.Any())
            {
                return BadRequest(new { Message = "Потрібен принаймні один інгредієнт." });
            }

            foreach (var ri in recipe.RecipeIngredients)
            {
                if (ri.IngredientId <= 0)
                {
                    return BadRequest(new { Message = "Інгредієнт повинен мати валідний ID." });
                }
                if (string.IsNullOrWhiteSpace(ri.Quantity))
                {
                    return BadRequest(new { Message = $"Кількість для інгредієнта {ri.IngredientId} є обов'язковою." });
                }
            }

            // Валідація категорій
            if (recipe.RecipeCategories != null)
            {
                foreach (var rc in recipe.RecipeCategories)
                {
                    if (rc.CategoryId <= 0)
                    {
                        return BadRequest(new { Message = "Категорія повинна мати валідний ID." });
                    }
                }
            }

            // Валідація часу прийому їжі
            if (recipe.RecipeMealTimes != null)
            {
                foreach (var rm in recipe.RecipeMealTimes)
                {
                    if (rm.MealTimeId <= 0)
                    {
                        return BadRequest(new { Message = "Час прийому їжі повинен мати валідний ID." });
                    }
                }
            }

            var createdRecipe = await _recipeRepository.AddAsync(recipe);
            return CreatedAtAction(nameof(GetRecipe), new { id = createdRecipe.Id }, createdRecipe);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecipe(int id, Recipe recipe)
        {
            if (id != recipe.Id)
            {
                return BadRequest(new { Message = "ID рецепту не співпадає." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Невалідна модель.", Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            if (string.IsNullOrWhiteSpace(recipe.Title))
            {
                return BadRequest(new { Message = "Назва рецепту є обов'язковою." });
            }

            if (string.IsNullOrWhiteSpace(recipe.Instructions))
            {
                return BadRequest(new { Message = "Інструкції є обов'язковими." });
            }

            if (recipe.CookingTime <= 0)
            {
                return BadRequest(new { Message = "Час приготування має бути більше 0." });
            }

            if (!recipe.DifficultyLevelId.HasValue || recipe.DifficultyLevelId <= 0)
            {
                return BadRequest(new { Message = $"Невалідний DifficultyLevelId: {recipe.DifficultyLevelId ?? 0}." });
            }

            // Валідація інгредієнтів
            if (recipe.RecipeIngredients == null || !recipe.RecipeIngredients.Any())
            {
                return BadRequest(new { Message = "Потрібен принаймні один інгредієнт." });
            }

            foreach (var ri in recipe.RecipeIngredients)
            {
                if (ri.IngredientId <= 0)
                {
                    return BadRequest(new { Message = "Інгредієнт повинен мати валідний ID." });
                }
                if (string.IsNullOrWhiteSpace(ri.Quantity))
                {
                    return BadRequest(new { Message = $"Кількість для інгредієнта {ri.IngredientId} є обов'язковою." });
                }
            }

            // Валідація категорій
            if (recipe.RecipeCategories != null)
            {
                foreach (var rc in recipe.RecipeCategories)
                {
                    if (rc.CategoryId <= 0)
                    {
                        return BadRequest(new { Message = "Категорія повинна мати валідний ID." });
                    }
                }
            }

            // Валідація часу прийому їжі
            if (recipe.RecipeMealTimes != null)
            {
                foreach (var rm in recipe.RecipeMealTimes)
                {
                    if (rm.MealTimeId <= 0)
                    {
                        return BadRequest(new { Message = "Час прийому їжі повинен мати валідний ID." });
                    }
                }
            }

            try
            {
                await _recipeRepository.UpdateAsync(id, recipe);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            try
            {
                await _recipeRepository.DeleteAsync(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}