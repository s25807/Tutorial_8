using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Task3.Models.DTOs;
using Task3.Services;

namespace Task3.Controllers
{
    [Route("api/clients")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ClientService _clientService;

        public ClientController(ClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet("{clientId}/trips")]
        public async Task<IActionResult> GetClientTrips(int clientId)
        {
            var trips = await _clientService.GetClientTrips(clientId);

            if (trips.Count == 0)
            {
                return NotFound("Client not found or no trips registered.");
            }

            return Ok(trips);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] ClientDTO clientDTO)
        {
            try
            {
                var success = await _clientService.CreateClient(clientDTO);
                if (success)
                    return StatusCode(201, new { message = "Client created successfully." });
                else
                    return StatusCode(500, new { message = "Failed to create client." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", detail = ex.Message });
            }
        }
    }
}