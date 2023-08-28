using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController] // no required but more significant
    [Route("api/cities")]
    public class CityController : ControllerBase
    {
        [HttpGet] // HttpGet("api/cities")
        public JsonResult GetCitites()
        {
            return new JsonResult(
                new List<object>
                {
                   new { id = 1, Name = "New York City" },
                   new { id = 2, Name = "Antwerp" }
                });
        }
    }
}
