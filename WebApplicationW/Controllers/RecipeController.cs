using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationW.Dtos;
using WebApplicationW.Models;
using WebApplicationW.Services.Repositories;
using Ganss.Xss;

namespace WebApplicationW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IHtmlSanitizer _sanitizer;

        public RecipeController(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
            _sanitizer = new HtmlSanitizer();
            _sanitizer.AllowedTags.Add("iframe");
            _sanitizer.AllowedTags.Add("video");
            _sanitizer.AllowedTags.Add("audio");
            _sanitizer.AllowedTags.Add("source");
            _sanitizer.AllowedTags.Add("figure");
            _sanitizer.AllowedTags.Add("figcaption");
            _sanitizer.AllowedAttributes.Add("class");
            _sanitizer.AllowedAttributes.Add("style");
            _sanitizer.AllowedAttributes.Add("src");
            _sanitizer.AllowedAttributes.Add("controls");
            _sanitizer.AllowedAttributes.Add("poster");
            _sanitizer.AllowedAttributes.Add("alt");
            _sanitizer.AllowedAttributes.Add("title");
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
        public async Task<ActionResult<Recipe>> PostRecipe([FromBody] RecipeDto recipeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Невалідна модель.", Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            var recipe = new Recipe
            {
                Title = recipeDto.Title,
                Description = recipeDto.Description,
                Instructions = _sanitizer.Sanitize(recipeDto.Instructions),
                CookingTime = recipeDto.CookingTime,
                DifficultyLevelId = recipeDto.DifficultyLevelId,
                Calories = recipeDto.Calories,
                CreatedDate = recipeDto.CreatedDate,
                Photo = recipeDto.Photo != null ? Convert.FromBase64String(recipeDto.Photo) : null,
                RecipeIngredients = recipeDto.RecipeIngredients.Select(ri => new RecipeIngredient
                {
                    IngredientId = ri.IngredientId,
                    Quantity = ri.Quantity
                }).ToList(),
                RecipeCategories = recipeDto.RecipeCategories.Select(rc => new RecipeCategory
                {
                    CategoryId = rc.CategoryId
                }).ToList(),
                RecipeMealTimes = recipeDto.RecipeMealTimes.Select(rm => new RecipeMealTime
                {
                    MealTimeId = rm.MealTimeId
                }).ToList()
            };

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

            foreach (var rc in recipe.RecipeCategories)
            {
                if (rc.CategoryId <= 0)
                {
                    return BadRequest(new { Message = "Категорія повинна мати валідний ID." });
                }
            }

            foreach (var rm in recipe.RecipeMealTimes)
            {
                if (rm.MealTimeId <= 0)
                {
                    return BadRequest(new { Message = "Час прийому їжі повинен мати валідний ID." });
                }
            }

            var createdRecipe = await _recipeRepository.AddAsync(recipe);
            return CreatedAtAction(nameof(GetRecipe), new { id = createdRecipe.Id }, createdRecipe);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecipe(int id, [FromBody] RecipeDto recipeDto)
        {
            if (id != recipeDto.Id)
            {
                return BadRequest(new { Message = "ID рецепту не співпадає." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Невалідна модель.", Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            var recipe = new Recipe
            {
                Id = id,
                Title = recipeDto.Title,
                Description = recipeDto.Description,
                Instructions = _sanitizer.Sanitize(recipeDto.Instructions),
                CookingTime = recipeDto.CookingTime,
                DifficultyLevelId = recipeDto.DifficultyLevelId,
                Calories = recipeDto.Calories,
                CreatedDate = recipeDto.CreatedDate,
                Photo = recipeDto.Photo != null ? Convert.FromBase64String(recipeDto.Photo) : null,
                RecipeIngredients = recipeDto.RecipeIngredients.Select(ri => new RecipeIngredient
                {
                    IngredientId = ri.IngredientId,
                    Quantity = ri.Quantity
                }).ToList(),
                RecipeCategories = recipeDto.RecipeCategories.Select(rc => new RecipeCategory
                {
                    CategoryId = rc.CategoryId
                }).ToList(),
                RecipeMealTimes = recipeDto.RecipeMealTimes.Select(rm => new RecipeMealTime
                {
                    MealTimeId = rm.MealTimeId
                }).ToList()
            };

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

            foreach (var rc in recipe.RecipeCategories)
            {
                if (rc.CategoryId <= 0)
                {
                    return BadRequest(new { Message = "Категорія повинна мати валідний ID." });
                }
            }

            foreach (var rm in recipe.RecipeMealTimes)
            {
                if (rm.MealTimeId <= 0)
                {
                    return BadRequest(new { Message = "Час прийому їжі повинен мати валідний ID." });
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