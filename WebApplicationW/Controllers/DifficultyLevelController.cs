using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApplicationW.Models;
using WebApplicationW.Services.Repositories;

namespace WebApplicationW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DifficultyLevelController : ControllerBase
    {
        private readonly IDifficultyLevelRepository _difficultyLevelRepository;

        public DifficultyLevelController(IDifficultyLevelRepository difficultyLevelRepository)
        {
            _difficultyLevelRepository = difficultyLevelRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DifficultyLevel>>> GetDifficultyLevels()
        {
            var difficultyLevels = await _difficultyLevelRepository.GetAllAsync();
            return Ok(difficultyLevels);
        }
    }
}