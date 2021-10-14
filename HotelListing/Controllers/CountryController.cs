using AutoMapper;
using HotelListing.Core.DTOs;
using HotelListing.Core.IRepository;
using HotelListing.Core.Models;
using HotelListing.Data;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CountryController> _logger;
        private readonly IMapper _mapper;

        public CountryController(
            IUnitOfWork unitOfWork,
            ILogger<CountryController> logger,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 60)]
        [HttpCacheValidation(MustRevalidate = false)]
        public async Task<IActionResult> Get(
            [FromQuery] RequestParams requestParams)
        {
            IPagedList<Country> countries = await _unitOfWork.Countries
                    .GetAll(requestParams, null);

            IList<CountryDTO> results = _mapper.Map<IList<CountryDTO>>(countries);

            return Ok(results);
        }

        [HttpGet("{id:int}", Name = "GetCountry")]
        public async Task<IActionResult> Get(int id)
        {
            Country country = await _unitOfWork.Countries
                    .Get(country => country.Id == id,
                        include: country => country.Include(x => x.Hotels));

            if (country == null)
            {
                return NotFound();
            }

            CountryDTO result = _mapper.Map<CountryDTO>(country);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCountry(
            [FromBody] CreateCountryDTO countryDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Error Creating Country in the method {nameof(CreateCountry)}. Please Try Again.");
                return BadRequest(ModelState);
            }

            Country country = _mapper.Map<Country>(countryDTO);

            await _unitOfWork.Countries
                .Insert(country);

            await _unitOfWork.Save();

            return CreatedAtRoute("GetCountry", new { id = country.Id }, country);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCountry(
            int id,
            [FromBody] UpdateCountryDTO countryDTO)
        {
            if (!ModelState.IsValid
                || id < 1)
            {
                _logger.LogError($"Invalid PUT attempt in {nameof(UpdateCountry)}");
                return BadRequest(ModelState);
            }

            Country country = await _unitOfWork.Countries
                    .Get(country => country.Id == id);

            if (country == null)
            {
                _logger.LogError($"Country not found in {nameof(UpdateCountry)}");
                return NotFound();
            }

            _mapper.Map(countryDTO, country);

            _unitOfWork.Countries
                .Update(country);

            await _unitOfWork.Save();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteCountry)}");
                return ValidationProblem();
            }

            Country country = await _unitOfWork.Countries
                    .Get(country => country.Id == id);

            if (country == null)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteCountry)}");
                return NotFound();
            }

            await _unitOfWork.Countries
                .Delete(id);

            await _unitOfWork.Save();

            return NoContent();
        }
    }
}
