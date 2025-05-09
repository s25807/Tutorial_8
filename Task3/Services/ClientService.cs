using Microsoft.Data.SqlClient;
using Task3.Models.DTOs;

namespace Task3.Services;

public class ClientService : IClientService
{
    private readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=APBD;Integrated Security=True";

    public async Task<List<ClientTripDTO>> GetClientTrips(int clientId)
    {
        var trips = new List<ClientTripDTO>();

        string query = @"
        SELECT t.TripID, t.Name, t.Description, t.DateFrom, t.DateTo, t.MaxPeople,
               ct.RegisteredAt, ct.PaymentDone
        FROM Client c
        LEFT JOIN Client_Trip ct ON c.IdClient = ct.IdClient
        LEFT JOIN Trip t ON ct.IdTrip = t.IdTrip
        WHERE c.IdClient = @ClientId";

        using (var conn = new SqlConnection(_connectionString))
        using (var cmd = new SqlCommand(query, conn))
        {
            cmd.Parameters.AddWithValue("@ClientId", clientId);
            await conn.OpenAsync();

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                if (!reader.HasRows)
                {
                    return trips;
                }

                while (await reader.ReadAsync())
                {
                    if (reader.IsDBNull(reader.GetOrdinal("TripID")))
                        continue;

                    var trip = new ClientTripDTO
                    {
                        TripID = reader.GetInt32(reader.GetOrdinal("TripID")),
                        Name = reader.GetString(reader.GetOrdinal("Name")),
                        Description = reader.GetString(reader.GetOrdinal("Description")),
                        DateFrom = reader.GetDateTime(reader.GetOrdinal("DateFrom")),
                        DateTo = reader.GetDateTime(reader.GetOrdinal("DateTo")),
                        MaxPeople = reader.GetInt32(reader.GetOrdinal("MaxPeople")),
                        RegisteredAt = reader.GetDateTime(reader.GetOrdinal("RegisteredAt")),
                        PaymentDone = reader.GetBoolean(reader.GetOrdinal("PaymentDone"))
                    };

                    trips.Add(trip);
                }
            }
        }

        return trips;
    }

    public async Task<bool> CreateClient(ClientDTO client)
    {
        using (var conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();

            if (string.IsNullOrWhiteSpace(client.FirstName) ||
                string.IsNullOrWhiteSpace(client.LastName) ||
                string.IsNullOrWhiteSpace(client.Email) ||
                string.IsNullOrWhiteSpace(client.Telephone) ||
                string.IsNullOrWhiteSpace(client.Pesel))
            {
                throw new ArgumentException("All fields are required.");
            }

            var checkCmd = new SqlCommand("SELECT COUNT(*) FROM Client WHERE Pesel = @Pesel", conn);
            checkCmd.Parameters.AddWithValue("@Pesel", client.Pesel);
            var exists = (int)await checkCmd.ExecuteScalarAsync() > 0;

            if (exists)
            {
                throw new InvalidOperationException("A client with this PESEL already exists.");
            }

            var cmd = new SqlCommand(@"
            INSERT INTO Client (FirstName, LastName, Email, Telephone, Pesel)
            VALUES (@FirstName, @LastName, @Email, @Telephone, @Pesel)", conn);

            cmd.Parameters.AddWithValue("@FirstName", client.FirstName);
            cmd.Parameters.AddWithValue("@LastName", client.LastName);
            cmd.Parameters.AddWithValue("@Email", client.Email);
            cmd.Parameters.AddWithValue("@Telephone", client.Telephone);
            cmd.Parameters.AddWithValue("@Pesel", client.Pesel);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }
    }
}