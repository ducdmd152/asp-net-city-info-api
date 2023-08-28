using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController] // no required but more significant
    [Route("api/cities")]
    public class CityController : ControllerBase
    {
        [HttpGet] // HttpGet("api/cities")
        public JsonResult GetCities()
        {
            return new JsonResult(
                CityDataStore.Instance.Cities);
        }

        [HttpGet("{id}")]
        public ActionResult<CityDTO> GetCity(int id)
        {
            var result = CityDataStore.Instance.Cities.SingleOrDefault(c => c.Id == id);
            
            return result != null ? Ok(result) : NotFound();
        }
    }
}
