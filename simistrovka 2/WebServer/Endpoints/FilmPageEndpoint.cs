using HttpServerLibrary;
using HttpServerLibrary.Core.Attributes;
using HttpServerLibrary.Core.HttpResponse;
using HttpServerLibrary.Models;
using Microsoft.Data.SqlClient;
using my_http.Helpers;
using my_http.Helpers.AuthHelpers;
using my_http.Models.Entities;
using my_http.Models.FilmPageVms;
using MyORMLibrary;
using TemlateEngine;

namespace my_http.Endpoints;

public class FilmPageEndpoint : EndpointBase
{
    private readonly ResponseHelper _responseHelper = new ResponseHelper();
    private readonly AppConfig _appConfig = AppConfig.GetInstance();
    private readonly IHtmlTemplateEngine _htmlTemplateEngine = new HtmlTemplateEngine();
    private AuthorizationHelper _authorizationHelper = new AuthorizationHelper();
    
    [Get("film")]
    public IHttpResponseResult Index(int id)
    {
        try
        {
            if (id == 0)
            {
                return Redirect("index");
            }

            var localPath = "Views/Templates/Pages/FilmPage/film-page.html";
            var responseText = _responseHelper.GetResponseTextCustomPath(localPath);
            var model = SetModel(id);
            CheckAuthorization(model);
            var page = _htmlTemplateEngine.Render(responseText, model);
            return Html(page);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return Error(e.Message);
        }
    }

    private FilmPageDataVm SetModel(int filmId)
    {
        using var connection = new SqlConnection(_appConfig.ConnectionString);
        var dbContextMovie = new ORMContext<Movie>(connection);
        var movie = dbContextMovie.FirstOrDefault(m => m.Id == filmId);
        var genreId = movie.GenreId;
        var similarMovies = dbContextMovie.Where(m => m.GenreId == genreId).ToList();
        var dbContextGenre = new ORMContext<Genre>(connection);
        var genre = dbContextGenre.FirstOrDefault(g => g.Id == genreId);
        var dbContextMovieActor = new ORMContext<MovieActor>(connection);
        var actorsIds = dbContextMovieActor.Where(ma => ma.MovieId == filmId);
        var dbContextActors = new ORMContext<Actor>(connection);
        var actors = new List<Actor>();
        foreach (var movieActor in actorsIds)
        {
            actors.Add(dbContextActors.GetById(movieActor.ActorId));
        }

        var descriptionParts = SplitDescription(movie.Description);

        var model = new FilmPageDataVm
        {
            Actors = actors,
            Movie = movie,
            DescriptionShort = descriptionParts.Item1,
            DescriptionLong = descriptionParts.Item2,
            Genre = genre,
            SimilarMovies = similarMovies
        };
        return model;
    }

    private void CheckAuthorization(FilmPageDataVm model)
    {
        using var connection = new SqlConnection(_appConfig.ConnectionString);
        model.IsAuthorized = _authorizationHelper.IsAuthorized(Context); // Используем метод проверки авторизации
        if (model.IsAuthorized)
        {

            var userId = Int32.Parse(SessionStorage.GetUserId(Context.Request.Cookies["session-token"].Value));
            if (userId != 0)
            {
                var dbContextUser = new ORMContext<User>(connection);
                model.Username = dbContextUser.FirstOrDefault(u => u.Id == userId).UserName;
            }
        }

    }

    private (string, string) SplitDescription(string description)
    {
        int totalLength = description.Length;
        int oneThirdLength = totalLength / 3;

        string firstPart = description.Substring(0, oneThirdLength); // Первая треть
        string remainingParts = description.Substring(oneThirdLength);
        return (firstPart, remainingParts);
    }
}