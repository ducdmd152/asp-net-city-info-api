using CityInfo.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities/{cityId}/pointsofinterest")]
    [ApiController]
    public class PointOfInterestController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDTO>> Get(int cityId)
        {
            var city = CityDataStore.Instance.Cities.SingleOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            return Ok(city.PointsOfInterest);
        }

        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
        public ActionResult<PointOfInterestDTO> GetPointOfInterest(
            int cityId,
            int pointOfInterestId)
        {
            var city = CityDataStore.Instance.Cities.SingleOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var point = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);

            if (point == null)
            {
                return NotFound();
            }

            return Ok(point);
        }

        [HttpPost]
        public ActionResult<PointOfInterestDTO> CreatePointOfInterest(
            int cityId,
            PointOfInterestForCreationDTO pointOfInterest
            )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var city = CityDataStore.Instance.Cities.SingleOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var maxPointOfInterestId = CityDataStore.Instance.Cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);


            var finalPointOfInterest = new PointOfInterestDTO()
            {
                Id = ++maxPointOfInterestId,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };

            city.PointsOfInterest.Add(finalPointOfInterest);

            return CreatedAtRoute("GetPointOfInterest", 
                new
                {
                    cityId = cityId,
                    pointOfInterestId = finalPointOfInterest.Id
                },
                finalPointOfInterest
                );
        }
    }
}
