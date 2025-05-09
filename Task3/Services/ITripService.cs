using Task3.Models.DTOs;

namespace Task3.Services;

public interface ITripService
{
    Task<List<TripDTO>> GetTrips();
}