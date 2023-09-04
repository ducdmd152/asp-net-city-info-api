using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController] // no required but more significant
    [Authorize]
    [Route("api/cities")]
    public class CityController : ControllerBase
    {
        private readonly CityDataStore _cityDataStore;
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        public CityController(CityDataStore cityDataStore, 
            ICityInfoRepository cityInfoRepository,
            IMapper mapper)
        {
            _cityDataStore = cityDataStore ?? throw new ArgumentNullException(nameof(cityDataStore));
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet] // HttpGet("api/cities")
        public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDTO>>> GetCities()
        {
            var cityEntities = await _cityInfoRepository.GetCitiesAsync();

            return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDTO>>(cityEntities));
            //var results = new List<CityWithoutPointsOfInterestDTO>();

            //foreach (var cityEntity in cityEntities)
            //{
            //    results.Add(new CityWithoutPointsOfInterestDTO {
            //        Id = cityEntity.Id,
            //        Description = cityEntity.Description,
            //        Name = cityEntity.Name,
            //    });
            //}

            //return Ok(results);
            //return new JsonResult(
            //    _cityDataStore.Cities);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCity(int id, bool includePointOfInterest = false)
        {
            //var result = _cityDataStore.Cities.SingleOrDefault(c => c.Id == id);

            //return result != null ? Ok(result) : NotFound();

            var cityEntity = await _cityInfoRepository.GetCityAsync(id, includePointOfInterest);
            if (cityEntity == null)
            {
                return NotFound();
            }

            if (includePointOfInterest)
                return Ok(_mapper.Map<CityDTO>(cityEntity));
            else
                return Ok(_mapper.Map<CityWithoutPointsOfInterestDTO>(cityEntity));
        }
    }
}
