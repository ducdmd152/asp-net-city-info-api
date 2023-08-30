using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController] // no required but more significant
    [Route("api/cities")]
    public class CityController : ControllerBase
    {
        private readonly CityDataStore _cityDataStore;
        private readonly ICityInfoRepository _cityInfoRepository;

        public CityController(CityDataStore cityDataStore, ICityInfoRepository cityInfoRepository)
        {
            _cityDataStore = cityDataStore ?? throw new ArgumentNullException(nameof(cityDataStore));
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
        }

        [HttpGet] // HttpGet("api/cities")
        public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDTO>>> GetCities()
        {
            var cityEntities = await _cityInfoRepository.GetCitiesAsync();

            var results = new List<CityWithoutPointsOfInterestDTO>();

            foreach (var cityEntity in cityEntities)
            {
                results.Add(new CityWithoutPointsOfInterestDTO {
                    Id = cityEntity.Id,
                    Description = cityEntity.Description,
                    Name = cityEntity.Name,
                });
            }

            return Ok(results);
            //return new JsonResult(
            //    _cityDataStore.Cities);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CityDTO>> GetCity(int id)
        {
            //var result = _cityDataStore.Cities.SingleOrDefault(c => c.Id == id);

            //return result != null ? Ok(result) : NotFound();

            var cityEntity = await _cityInfoRepository.GetCityAsync(id, true);
            return Ok();
        }
    }
}
