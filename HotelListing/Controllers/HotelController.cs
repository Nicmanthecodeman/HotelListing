using AutoMapper;
using HotelListing.Data;
using HotelListing.IRepository;
using HotelListing.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HotelController> _logger;
        private readonly IMapper _mapper;

        public HotelController(
            IUnitOfWork unitOfWork,
            ILogger<HotelController> logger,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            IList<Hotel> hotels = await _unitOfWork.Hotels
                    .GetAll();

            IList<HotelDTO> results = _mapper.Map<IList<HotelDTO>>(hotels);

            return Ok(results);
        }

        [HttpGet("{id:int}", Name = "GetHotel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int id)
        {
            Hotel hotel = await _unitOfWork.Hotels
                    .Get(hotel => hotel.Id == id,
                        include: hotel => hotel.Include(x => x.Country));

            if (hotel == null)
            {
                return NotFound();
            }

            HotelDTO result = _mapper.Map<HotelDTO>(hotel);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateHotel(
            [FromBody] CreateHotelDTO hotelDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(CreateHotel)}");
                return BadRequest(ModelState);
            }

            Hotel hotel = _mapper.Map<Hotel>(hotelDTO);

            await _unitOfWork.Hotels
                .Insert(hotel);

            await _unitOfWork.Save();

            return CreatedAtRoute("GetHotel", new { id = hotel.Id }, hotel);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateHotel(
            int id,
            [FromBody] UpdateHotelDTO hotelDTO)
        {
            if (!ModelState.IsValid
                || id < 1)
            {
                _logger.LogError($"Invalid PUT attempt in {nameof(UpdateHotel)}");
                return BadRequest(ModelState);
            }

            Hotel hotel = await _unitOfWork.Hotels
                    .Get(hotel => hotel.Id == id);

            if (hotel == null)
            {
                _logger.LogError($"Hotel not found in {nameof(UpdateHotel)}");
                return NotFound();
            }

            _mapper.Map(hotelDTO, hotel);

            _unitOfWork.Hotels
                .Update(hotel);

            await _unitOfWork.Save();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteHotel)}");
                return ValidationProblem();
            }

            Hotel hotel = await _unitOfWork.Hotels
                    .Get(hotel => hotel.Id == id);

            if (hotel == null)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteHotel)}");
                return NotFound();
            }

            await _unitOfWork.Hotels
                .Delete(id);

            await _unitOfWork.Save();

            return NoContent();
        }
    }
}
