using System.Threading.Channels;
using HttpServerLibrary;
using HttpServerLibrary.Core.Attributes;
using HttpServerLibrary.Core.HttpResponse;
using HttpServerLibrary.Models;
using Microsoft.Data.SqlClient;
using my_http.Helpers;
using my_http.Models.AdminPanelVms;
using my_http.Models.Entities;
using my_http.Repositories.EntityRepositories;
using MyORMLibrary;
using TemlateEngine;

namespace my_http.Endpoints;

public class AdminPanelEndpoint : EndpointBase
{
    private readonly ResponseHelper _responseHelper = new ResponseHelper();
    private readonly AppConfig _appConfig = AppConfig.GetInstance();
    private readonly IHtmlTemplateEngine _htmlTemplateEngine = new HtmlTemplateEngine();
    
    [Get("admin")]
    public IHttpResponseResult Index()
    {
        try
        {
            var localPath = "Views/Templates/Pages/AdminPanel/index.html";
            var responseText = _responseHelper.GetResponseTextCustomPath(localPath);
            var model = new AdminPanelDataVm
            {
                Genres = GetData<Genre>(),
                Movies = GetData<Movie>(),
                Users = GetData<User>(),
                Actors = GetData<Actor>(),
                MovieActors = GetData<MovieActor>()
            };
            var page = _htmlTemplateEngine.Render(responseText, model);
            return Html(page);
        }
        catch(Exception e)
        {
            return Json(e.Message);
        }
    }
    
    [Post("admin/movies/add")]
    public IHttpResponseResult AddMovie(Movie movie)
    {
        using var connection = new SqlConnection(_appConfig.ConnectionString);
        var dbContext = new ORMContext<Movie>(connection);
    
        var title = movie.Title;
        var description = movie.Description;
        var genreId = movie.GenreId;
        var releaseYear = movie.ReleaseYear;
        var isSubscriptionBased = movie.IsSubscriptionBased;
        try
        {
            
            var savedMovie = dbContext.FirstOrDefault(m =>
                            m.Title == title && m.Description == description && m.GenreId == genreId &&
                            m.ReleaseYear == releaseYear && m.IsSubscriptionBased == isSubscriptionBased);
            if (savedMovie is not null)
            {
                return Json(false);
            }
            
            dbContext.Create(movie);
            savedMovie = dbContext.FirstOrDefault(m =>
                m.Title == title && m.Description == description && m.GenreId == genreId &&
                m.ReleaseYear == releaseYear && m.IsSubscriptionBased == isSubscriptionBased);
            movie.Id = savedMovie.Id;
            return Json(movie);
        }
        catch(Exception e)
        {
            return Json(new { Message = e.Message });
        }
    }


    [Post("admin/movies/delete")]
    public IHttpResponseResult DeleteMovie(int id)
    {
        using var connection = new SqlConnection(_appConfig.ConnectionString);
        var dbContext = new ORMContext<Movie>(connection);

        try
        {
            dbContext.Delete(id, "\"Movies\"");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return Redirect("/admin");
        }

        return Redirect("/admin");
    }
    
    [Post("admin/actors/add")]
    public IHttpResponseResult AddActor(Actor actor)
    {
        try
        {
            using var connection = new SqlConnection(_appConfig.ConnectionString);
            var dbContext = new ORMContext<Actor>(connection);

            var name = actor.Name;
            var photoUrl = actor.PhotoUrl;
            var biography = actor.Biography;

            var savedActor = dbContext.FirstOrDefault(a =>
                a.Name == name && a.PhotoUrl == photoUrl && a.Biography == biography);
            if (savedActor is not null)
            {
                return Json(false);
            }

            dbContext.Create(actor);
            savedActor = dbContext.FirstOrDefault(a =>
                a.Name == name && a.PhotoUrl == photoUrl && a.Biography == biography);
            actor.Id = savedActor.Id;
            return Json(actor);
        }
        catch(Exception e)
        {
            return Json(new { Message = e.Message });
        }
    }
    
    [Post("admin/movieactors/add")]
    public IHttpResponseResult AddMovieActor(MovieActor movieActor)
    {
        try
        {
            using var connection = new SqlConnection(_appConfig.ConnectionString);
            var dbContext = new ORMContext<MovieActor>(connection);

            var movieId = movieActor.MovieId;
            var actorId = movieActor.ActorId;

            var savedMovieActor = dbContext.FirstOrDefault(ma =>
                ma.MovieId == movieId && ma.ActorId == actorId);
            if (savedMovieActor is not null)
            {
                return Json(false);
            }

            if (!ValidateMovieActorConstraint(movieActor))
            {
                return Json(1);
            }

            dbContext.Create(movieActor);
            savedMovieActor = dbContext.FirstOrDefault(ma =>
                ma.MovieId == movieId && ma.ActorId == actorId);
            movieActor.Id = savedMovieActor.Id;
            return Json(movieActor);
        }
        catch(Exception e)
        {
            return Json(new { Message = e.Message });
        }
    }
    
    [Post("admin/genres/add")]
    public IHttpResponseResult AddGenre(Genre genre)
    {
        try
        {
            using var connection = new SqlConnection(_appConfig.ConnectionString);
            var dbContext = new ORMContext<Genre>(connection);

            var name = genre.Name;

            var savedGenre = dbContext.FirstOrDefault(g => g.Name == name);
            if (savedGenre is not null)
            {
                return Json(false);
            }

            dbContext.Create(genre);
            savedGenre = dbContext.FirstOrDefault(g => g.Name == name);
            genre.Id = savedGenre.Id;
            return Json(genre);
        }
        catch(Exception e)
        {
            return Json(new { Message = e.Message });
        }
    }
    
    [Post("admin/users/add")]
    public IHttpResponseResult AddUser(User user)
    {
        try
        {
            using var connection = new SqlConnection(_appConfig.ConnectionString);
            var dbContext = new ORMContext<User>(connection);

            var userName = user.UserName;
            var password = user.Password;
            var isAdmin = user.IsAdmin;

            var savedUser = dbContext.FirstOrDefault(u =>
                u.UserName == userName && u.Password == password && u.IsAdmin == isAdmin);
            if (savedUser is not null)
            {
                return Json(false);
            }

            dbContext.Create(user);
            savedUser = dbContext.FirstOrDefault(u =>
                u.UserName == userName && u.Password == password && u.IsAdmin == isAdmin);
            user.Id = savedUser.Id;
            return Json(user);
        }
        catch(Exception e)
        {
            return Json(new { Message = e.Message });
        }
    }

    // валидация на сервере
    private bool ValidateMovieActorConstraint(MovieActor movieActor)
    {
        var movieRepository = new MovieRepository();
        var movie = movieRepository.GetById(movieActor.MovieId);
        var actorRepository = new ActorRepository();
        var actor = actorRepository.GetById(movieActor.ActorId);
        return actor is not null && movie is not null;
    }
    private List<T> GetData<T>() where T : class, new()
    {
        using var connection = new SqlConnection(_appConfig.ConnectionString);
        var dbContext = new ORMContext<T>(connection);
        var data = dbContext.GetAll();
        return data;
    }
}