using my_http.Models.Entities;

namespace my_http.Models.MainPageVms;

public class MainPageDataVm
{
    public bool IsAuthorized { get; set; }

    public string Username { get; set; } = "";
    public List<Movie> Movies1 { get; set; } = new();
    public List<Movie> Movies2 { get; set; } = new();
    public List<Movie> Movies3 { get; set; } = new();
    public Genre Genre1 { get; set; }
    public Genre Genre2 { get; set; }
    public Genre Genre3 { get; set; }
}