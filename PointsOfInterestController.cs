using System.Security.Cryptography.Xml;
using System.Xml.Linq;
using asp2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.JsonPatch;
using asp2.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;


namespace asp2.Controllers
{
    [Route("api/v{version:apiVersion}/cities/{cityId}/pointsofinterest")]
    [Authorize]
    [ApiVersion(2)]
    ////To make it so one people from a particular city can use the point of interest controller
    //[Authorize(Policy ="MustBeFromSurulere")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    { 

        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly IMapper _mapper;
        private readonly ICityInfoRepository _cityInfoRepository;
 
        public PointsOfInterestController(ILogger<PointsOfInterestController> logger,
          IMailService mailService, ICityInfoRepository cityInfoRepository,IMapper mapper)
        {

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository)); ;
            
        }
        [HttpGet]
        public  async Task<ActionResult<IEnumerable<PointsOfInterestDto>>> GetPointsOfInterest(int cityId)
        
        {
            // To make it so people from only people with matching city names can access there point of interest using the id
            //var cityName = User.Claims.FirstOrDefault(c => c.Type =="city")?.Value;
            //if (!await _cityInfoRepository.CityNameMatchesCityId(cityName, cityId))
            //{
            //    return Forbid();    
            //}

            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation($"City with id {cityId} wasn't found when accessing points of interest.");
       
                return NotFound();
            }
            var pointsofInterestForCity = await _cityInfoRepository.GetPointsOfInterestForCityAsync(cityId); 

             
            //try
            //{
                
            //    var city = _citiesDataStore.Cities.LastOrDefault(c => c.Id == cityId);
            //    if (city == null)
            //    {
            //        _logger.LogInformation($"City with id {cityId} wasn't found when accessing points of interest.");
            //        return NotFound();
            //    }
               
            //    return Ok(city.PointsOfInterest);
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogCritical($"Exception while getting points of interest for city with id{cityId}.", ex);
            //    return StatusCode(500, "A problem occurred while handling this request");
            //}
            return Ok(_mapper.Map<IEnumerable<PointsOfInterestDto>>(pointsofInterestForCity));
        }
        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
        public  async Task<ActionResult<PointsOfInterestDto>> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound(); 
            }
            var pointOfInterest = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
            {
                if (pointOfInterest == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<PointsOfInterestDto>(pointOfInterest));
            }
            //var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            //if (city == null)
            //{
            //    return NotFound();
            //}

            ////find point of interest
            //var pointOfInterest = city.PointsOfInterest.FirstOrDefault(c => c.Id == pointOfInterestId);
            //if (pointOfInterest == null)
            //{
            //    return NotFound();
            //}
            //return Ok(pointOfInterest);

        }
        [HttpPost]
        public async  Task<ActionResult<PointsOfInterestDto>> CreatePointOfInterest(int cityId, PointOfInterestForCreationDto pointOfInterest)
        {
            //   if (!ModelState.IsValid) { 

            //       return BadRequest();

            //}
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }
            var finalPointOfInterest = _mapper.Map<Entities.PointOfInterest>(pointOfInterest);
            await _cityInfoRepository.AddPointOfInterestForCityAsync(cityId,finalPointOfInterest);
            await _cityInfoRepository.SaveChangesAsync();
            var createdPointOfInterestToReturn = _mapper.Map<Models.PointsOfInterestDto>(finalPointOfInterest);

            return CreatedAtRoute("GetPointOfInterest",
                new
                {
                    cityId = cityId,
                    pointOfInterestId = createdPointOfInterestToReturn.Id
                },
                createdPointOfInterestToReturn);

        }
        [HttpPut("{pointOfInterestId}")]
        public async Task<ActionResult> UpdatePointOfInterest(int cityId, int pointOfInterestId,
        PointOfInterestForUpdateDto pointOfInterest)
        {
            
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();

            }


            // find point of interest
            var pointOfInterestEntity =await _cityInfoRepository
                .GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            _mapper.Map(pointOfInterest,pointOfInterestEntity);

            await _cityInfoRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{pointOfInterestId}")]

        public async Task< ActionResult> PartiallyUpdatePointOfInterest(
        int cityId, int pointOfInterestId,
        JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();

            }
            var pointOfInterestEntity = await _cityInfoRepository
                .GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }
            var point0fInterestToPatch = _mapper.Map<PointOfInterestForUpdateDto>(pointOfInterestEntity);
                
            patchDocument.ApplyTo(point0fInterestToPatch, ModelState);
            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);
            }

            if (!TryValidateModel(point0fInterestToPatch))
            {
                return BadRequest(ModelState);
            }
            _mapper.Map(point0fInterestToPatch, pointOfInterestEntity);

            await _cityInfoRepository .SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{pointOfInterestId}")]

        public async Task<ActionResult> DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();

            }
            var pointOfInterestEntity = await _cityInfoRepository
                .GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            _cityInfoRepository.DeletePointOfInterest(pointOfInterestEntity);
            await _cityInfoRepository.SaveChangesAsync();
            _mailService.Send("Point of interest deleted.", $"Point of interest {pointOfInterestEntity.Name} with id{pointOfInterestEntity.Id} was deleted");
            return NoContent();
        }

    }
}