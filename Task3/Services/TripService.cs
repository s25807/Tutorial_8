using Microsoft.Data.SqlClient;
using Task3.Models.DTOs;

namespace Task3.Services;

public class TripService : ITripService
{
    private readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=APBD;Integrated Security=True";

    public async Task<List<TripDTO>> GetTrips()
    {
        var trips = new List<TripDTO>();
        
        string query = @"
                SELECT TripID, Name, Description, DateFrom, DateTo, MaxPeople
                FROM Trips";
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(query, conn))
        {
            await conn.OpenAsync();

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var trip = new TripDTO
                    {
                        TripID = reader.GetInt32(reader.GetOrdinal("TripID")),
                        Name = reader.GetString(reader.GetOrdinal("Name")),
                        Description = reader.GetString(reader.GetOrdinal("Description")),
                        DateFrom = reader.GetDateTime(reader.GetOrdinal("DateFrom")),
                        DateTo = reader.GetDateTime(reader.GetOrdinal("DateTo")),
                        MaxPeople = reader.GetInt32(reader.GetOrdinal("MaxPeople"))
                    };

                    trips.Add(trip);
                }
            }
        }
        return trips;
    }
}