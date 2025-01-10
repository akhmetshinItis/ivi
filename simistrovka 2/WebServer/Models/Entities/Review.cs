namespace my_http.Models.Entities;

public class Review
{
    public int? Id { get; set; }
    public int MovieId { get; set; }
    public string UserName { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
    public DateTime CreatedAt { get; set; }
}