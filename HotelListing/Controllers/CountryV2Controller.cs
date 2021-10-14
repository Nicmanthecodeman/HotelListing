using AutoMapper;
using HotelListing.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace HotelListing.Controllers
{
    [ApiVersion("2.0", Deprecated = true)]
    [Route("api/country")]
    [ApiController]
    public class CountryV2Controller : ControllerBase
    {
        private readonly DatabaseContext _db;
        private readonly ILogger<CountryV2Controller> _logger;
        private readonly IMapper _mapper;

        public CountryV2Controller(
            DatabaseContext db,
            ILogger<CountryV2Controller> logger,
            IMapper mapper)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _db.Countries.ToListAsync());
        }
    }
}
