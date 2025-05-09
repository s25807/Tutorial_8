using Task3.Models.DTOs;

namespace Task3.Services;

public interface IClientService
{
    public Task<List<ClientTripDTO>> GetClientTrips(int clientID);
    public Task<bool> CreateClient(ClientDTO client);
    
}