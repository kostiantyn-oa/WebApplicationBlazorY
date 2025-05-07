using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApplicationW.Models;
using WebApplicationW.Services.Repositories;

namespace WebApplicationW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealTimeController : ControllerBase
    {
        private readonly IMealTimeRepository _mealTimeRepository;

        public MealTimeController(IMealTimeRepository mealTimeRepository)
        {
            _mealTimeRepository = mealTimeRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MealTime>>> GetMealTimes()
        {
            var mealTimes = await _mealTimeRepository.GetAllAsync();
            return Ok(mealTimes);
        }
    }
}