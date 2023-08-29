using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace CityInfo.API.Controllers
{
    [Route("api/cities/{cityId}/pointsofinterest")]
    [ApiController]
    public class PointOfInterestController : ControllerBase
    {
        private readonly ILogger<PointOfInterestController> _logger;
        private readonly LocalMailService _mailService;
        public PointOfInterestController(ILogger<PointOfInterestController> logger, LocalMailService mailService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            // HttpContext.RequestServices.GetService()
        }

        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDTO>> Get(int cityId)
        {
            try
            {
                throw new Exception("Exception sample.");

                var city = CityDataStore.Instance.Cities.SingleOrDefault(c => c.Id == cityId);

                if (city == null)
                {
                    _logger.LogInformation($"City with id {cityId} wasn't found.");
                    return NotFound();
                }

                return Ok(city.PointsOfInterest);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(
                    $"Exception while getting points for city with id {cityId}.",
                    ex
                    );
                return StatusCode(500, "A problem happened while handling your request.");
            }
            
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

        [HttpPut("{pointOfInterestId}")]
        public ActionResult<PointOfInterestDTO> UpdatePointOfInterest(
            int cityId,
            int pointOfInterestId,
            PointOfInterestForUpdateDTO pointOfInterest
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

            var point = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);

            if (point == null)
            {
                return NotFound();
            }

            point.Name = pointOfInterest.Name;
            point.Description = pointOfInterest.Description;

            return NoContent();
        }

        [HttpPatch("{pointOfInterestId}")]
        public ActionResult PartiallyUpdatePointOfInterest(
            int cityId,
            int pointOfInterestId,
            JsonPatchDocument<PointOfInterestForUpdateDTO> patchDocument
            )
        {            
            var city = CityDataStore.Instance.Cities.SingleOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var pointFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);
            if (pointFromStore == null)
            {
                return NotFound();
            }

            var pointToPatch =
                new PointOfInterestForUpdateDTO()
                {
                    Name = pointFromStore.Name,
                    Description = pointFromStore.Description,
                };
            patchDocument.ApplyTo(pointToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(pointToPatch))
            {
                return BadRequest(ModelState);
            }

            pointFromStore.Name = pointToPatch.Name;
            pointFromStore.Description = pointToPatch.Description;

            return NoContent();
        }

        [HttpDelete("{pointOfInterestId}")]
        public ActionResult DeletePointOfInterest(
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

            city.PointsOfInterest.Remove(point);

            _mailService.Send(
                "Point of interest deleted",
                $"Point of interest {point.Name} with in {point.Id}."
                );
            return NoContent();
        }
    }
}
