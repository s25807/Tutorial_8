using Microsoft.AspNetCore.Mvc;
using Task3.Services;

namespace Task3.Controllers
{
    [Route("api/trips")]
    [ApiController]
    public class TripController : ControllerBase
    {
        private readonly ITripService _tripService;

        public TripController(ITripService tripService)
        {
            _tripService = tripService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTrips()
        {
            var trips = await _tripService.GetTrips();
            return Ok(trips);
        }
        
    }
}