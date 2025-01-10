using my_http.Models.Entities;

namespace my_http.Models.UpdateObjectsVm;

public class UpdateMovieVm
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string VerticalImageUrl { get; set; }
    public string HorizontalImageUrl { get; set; }
    public bool IsSubscriptionBased { get; set; }
    public int GenreId { get; set; }
    public string Rating { get; set; }
    public int ReleaseYear { get; set; }
    public string Duration { get; set; }

    public UpdateMovieVm(Movie movie)
    {
        Id = movie.Id;
        Title = movie.Title;
        Description = movie.Description;
        VerticalImageUrl = movie.VerticalImageUrl;
        HorizontalImageUrl = movie.HorizontalImageUrl;
        IsSubscriptionBased = movie.IsSubscriptionBased;
        GenreId = movie.GenreId;
        Rating = movie.Rating.ToString().Replace(',', '.');
        ReleaseYear = movie.ReleaseYear;
        Duration = movie.Duration;
    }
}