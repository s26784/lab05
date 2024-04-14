namespace WebApplication1.Models;

public class Visit
{
    public int IdVisit { get; set; }
    public int IdAnimal { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
}