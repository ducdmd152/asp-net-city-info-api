using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController] // no required but more significant
    [Route("api/cities")]
    public class CityController : ControllerBase
    {
        private readonly CityDataStore _cityDataStore;

        public CityController(CityDataStore cityDataStore)
        {
            _cityDataStore = cityDataStore ?? throw new ArgumentNullException(nameof(cityDataStore));
        }

        [HttpGet] // HttpGet("api/cities")
        public JsonResult GetCities()
        {
            return new JsonResult(
                _cityDataStore.Cities);
        }

        [HttpGet("{id}")]
        public ActionResult<CityDTO> GetCity(int id)
        {
            var result = _cityDataStore.Cities.SingleOrDefault(c => c.Id == id);
            
            return result != null ? Ok(result) : NotFound();
        }
    }
}
