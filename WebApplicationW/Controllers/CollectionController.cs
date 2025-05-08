using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplicationW.Models;
using WebApplicationW.Services.Repositories;
using Ganss.Xss;

namespace WebApplicationW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollectionController : ControllerBase
    {
        private readonly ICollectionRepository _collectionRepository;
        private readonly IHtmlSanitizer _sanitizer;

        public CollectionController(ICollectionRepository collectionRepository)
        {
            _collectionRepository = collectionRepository;
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
        public async Task<ActionResult<IEnumerable<Collection>>> GetCollections()
        {
            var collections = await _collectionRepository.GetAllAsync();
            return Ok(collections);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Collection>> GetCollection(int id)
        {
            var collection = await _collectionRepository.GetByIdAsync(id);
            if (collection == null)
            {
                return NotFound();
            }
            return Ok(collection);
        }

        [HttpPost]
        public async Task<ActionResult<Collection>> PostCollection([FromBody] CollectionDto collectionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Невалідна модель.", Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            if (string.IsNullOrWhiteSpace(collectionDto.Title))
            {
                return BadRequest(new { Message = "Назва колекції є обов'язковою." });
            }

            if (string.IsNullOrWhiteSpace(collectionDto.Description))
            {
                return BadRequest(new { Message = "Опис є обов'язковим." });
            }

            if (collectionDto.CollectionCategoryId <= 0)
            {
                return BadRequest(new { Message = $"Невалідний CollectionCategoryId: {collectionDto.CollectionCategoryId}." });
            }

            byte[]? photoBytes = null;
            if (!string.IsNullOrEmpty(collectionDto.Photo))
            {
                try
                {
                    photoBytes = Convert.FromBase64String(collectionDto.Photo);
                }
                catch (FormatException)
                {
                    return BadRequest(new { Message = "Невалідний формат фото (очікується Base64)." });
                }
            }

            var collection = new Collection
            {
                Title = collectionDto.Title,
                Description = _sanitizer.Sanitize(collectionDto.Description),
                CollectionCategoryId = collectionDto.CollectionCategoryId,
                Photo = photoBytes, // Дозволяємо null
                CollectionRecipes = collectionDto.CollectionRecipes?.Select(cr => new CollectionRecipe
                {
                    RecipeId = cr.RecipeId
                }).ToList() ?? new List<CollectionRecipe>()
            };

            var createdCollection = await _collectionRepository.AddAsync(collection);
            return CreatedAtAction(nameof(GetCollection), new { id = createdCollection.Id }, createdCollection);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCollection(int id, [FromBody] CollectionDto collectionDto)
        {
            if (id != collectionDto.Id)
            {
                return BadRequest(new { Message = "ID колекції не співпадає." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Невалідна модель.", Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            if (string.IsNullOrWhiteSpace(collectionDto.Title))
            {
                return BadRequest(new { Message = "Назва колекції є обов'язковою." });
            }

            if (string.IsNullOrWhiteSpace(collectionDto.Description))
            {
                return BadRequest(new { Message = "Опис є обов'язковим." });
            }

            if (collectionDto.CollectionCategoryId <= 0)
            {
                return BadRequest(new { Message = $"Невалідний CollectionCategoryId: {collectionDto.CollectionCategoryId}." });
            }

            byte[]? photoBytes = null;
            if (!string.IsNullOrEmpty(collectionDto.Photo))
            {
                try
                {
                    photoBytes = Convert.FromBase64String(collectionDto.Photo);
                }
                catch (FormatException)
                {
                    return BadRequest(new { Message = "Невалідний формат фото (очікується Base64)." });
                }
            }

            var collection = new Collection
            {
                Id = id,
                Title = collectionDto.Title,
                Description = _sanitizer.Sanitize(collectionDto.Description),
                CollectionCategoryId = collectionDto.CollectionCategoryId,
                Photo = photoBytes, // Дозволяємо null
                CollectionRecipes = collectionDto.CollectionRecipes?.Select(cr => new CollectionRecipe
                {
                    RecipeId = cr.RecipeId
                }).ToList() ?? new List<CollectionRecipe>()
            };

            try
            {
                await _collectionRepository.UpdateAsync(id, collection);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCollection(int id)
        {
            try
            {
                await _collectionRepository.DeleteAsync(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}