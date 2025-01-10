using my_http.Models.Entities;

namespace my_http.Models.AdminPanelVms;

public class AdminPanelDataVm
{
    public List<Movie> Movies { get; set; } = new();
    public List<MovieActor> MovieActors { get; set; } = new();
    public List<User> Users { get; set; } = new();
    public List<Actor> Actors { get; set; } = new();
    public List<Genre> Genres { get; set; } = new();
}