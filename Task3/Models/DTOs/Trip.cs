namespace Task3.Models.DTOs;

public class Trip
{
    private int TripID { get; set; }
    private string Name { get; set; }
    private string Description { get; set; }
    private DateTime DateFrom { get; set; }
    private DateTime DateTo { get; set; }
    private int MaxPeople { get; set; }
}