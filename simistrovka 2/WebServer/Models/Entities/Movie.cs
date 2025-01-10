namespace my_http.Models.Entities;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string VerticalImageUrl { get; set; }
    public string HorizontalImageUrl { get; set; }
    public bool IsSubscriptionBased { get; set; }
    public int GenreId { get; set; }
    public decimal Rating { get; set; }
    public int ReleaseYear { get; set; }
    public string Duration { get; set; }
}