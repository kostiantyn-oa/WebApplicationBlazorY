using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplicationW.Models;
using WebApplicationW.Services.Repositories;

namespace WebApplicationW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollectionCategoryController : ControllerBase
    {
        private readonly ICollectionCategoryRepository _collectionCategoryRepository;

        public CollectionCategoryController(ICollectionCategoryRepository collectionCategoryRepository)
        {
            _collectionCategoryRepository = collectionCategoryRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CollectionCategory>>> GetCollectionCategories()
        {
            var collectionCategories = await _collectionCategoryRepository.GetAllAsync();
            return Ok(collectionCategories);
        }
    }
}