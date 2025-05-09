namespace Task3.Models.DTOs;

public class ClientDTO
{
    public int ID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Telephone { get; set; }
    public string Pesel { get; set; }
}

public class ClientTripDTO
{
    public int TripID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public int MaxPeople { get; set; }

    public DateTime RegisteredAt { get; set; }
    public bool PaymentDone { get; set; }
}