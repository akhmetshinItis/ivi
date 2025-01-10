using my_http.Models.Entities;

namespace my_http.Models.FilmPageVms;

public class FilmPageDataVm
{
    public bool IsAuthorized { get; set; }
    public string Username { get; set; }
    public Movie Movie { get; set; }
    public Genre Genre { get; set; }
    public string DescriptionShort { get; set; }
    public string DescriptionLong { get; set; }
    public List<Actor> Actors { get; set; }
    public List<Movie>  SimilarMovies { get; set; }
}