// using HttpServerLibrary;
// using HttpServerLibrary.Core.Attributes;
// using HttpServerLibrary.Core.HttpResponse;
// using HttpServerLibrary.Models;
// using Microsoft.Data.SqlClient;
// using my_http.Helpers;
// using my_http.Models.Entities;
// using my_http.Models.UpdateObjectsVm;
// using MyORMLibrary;
// using TemlateEngine;
//
// namespace my_http.Endpoints;
//
// public class UpdateObjectsEndpoint : EndpointBase
// {
//     private readonly ResponseHelper _responseHelper = new ResponseHelper();
//     private readonly AppConfig _appConfig = AppConfig.GetInstance();
//     private readonly IHtmlTemplateEngine _htmlTemplateEngine = new HtmlTemplateEngine();
//
//     [Get("admin/movies/update")]
//     public IHttpResponseResult GetUpdateMoviePage(int id)
//     {
//         using var connection = new SqlConnection(_appConfig.ConnectionString);
//         var dbContext = new ORMContext<Movie>(connection);
//         var movie = dbContext.FirstOrDefault(m => m.Id == id);
//         if (movie is null)
//         {
//             return Redirect("/admin");
//         }
//
//         var localPath = "Views/Templates/Pages/UpdatePages/Update-Movie.html";
//         var text = _responseHelper.GetResponseTextCustomPath(localPath);
//         //только ради нормального отображения рейтинга сделала ибо не поняла как через js
//         var model = new UpdateMovieVm(movie);
//         var page = _htmlTemplateEngine.Render(text, model);
//         return Html(page);
//     }
//
//     [Post("admin/movies/update")]
//     public IHttpResponseResult UpdateMovie(Movie movie)
//     {
//         using var connection = new SqlConnection(_appConfig.ConnectionString);
//         var dbContext = new ORMContext<Movie>(connection);
//         dbContext.Update(movie);
//         return Redirect("/admin");
//     }
//     
//     
//     [Get("admin/actors/update")]
//     public IHttpResponseResult GetUpdateActorPage(int id)
//     {
//         using var connection = new SqlConnection(_appConfig.ConnectionString);
//         var dbContext = new ORMContext<Actor>(connection);
//         var actor = dbContext.FirstOrDefault(a => a.Id == id);
//         if (actor is null)
//         {
//             return Redirect("/admin");
//         }
//         var localPath = "Views/Templates/Pages/UpdatePages/Update-Actor.html";
//         var text = _responseHelper.GetResponseTextCustomPath(localPath);
//         var page = _htmlTemplateEngine.Render(text, actor);
//         return Html(page);
//     }
//     
//     [Post("admin/actors/update")]
//     public IHttpResponseResult UpdateMovie(Actor actor)
//     {
//         using var connection = new SqlConnection(_appConfig.ConnectionString);
//         var dbContext = new ORMContext<Actor>(connection);
//         dbContext.Update(actor);
//         return Redirect("/admin");
//     }
//     
//     [Get("admin/movieactors/update")]
//     public IHttpResponseResult GetUpdateMovieActorPage(int id)
//     {
//         using var connection = new SqlConnection(_appConfig.ConnectionString);
//         var dbContext = new ORMContext<MovieActor>(connection);
//         var movieActor = dbContext.FirstOrDefault(a => a.Id == id);
//         if (movieActor is null)
//         {
//             return Redirect("/admin");
//         }
//         var localPath = "Views/Templates/Pages/UpdatePages/Update-MovieActor.html";
//         var text = _responseHelper.GetResponseTextCustomPath(localPath);
//         var page = _htmlTemplateEngine.Render(text, movieActor);
//         return Html(page);
//     }
//     
//     [Post("admin/movieactors/update")]
//     public IHttpResponseResult UpdateMovieActor(MovieActor movieActor)
//     {
//         using var connection = new SqlConnection(_appConfig.ConnectionString);
//         var dbContext = new ORMContext<MovieActor>(connection);
//         dbContext.Update(movieActor);
//         return Redirect("/admin");
//     }
//     
//     [Get("admin/genres/update")]
//     public IHttpResponseResult GetUpdateGenresPage(int id)
//     {
//         using var connection = new SqlConnection(_appConfig.ConnectionString);
//         var dbContext = new ORMContext<Genre>(connection);
//         var genre = dbContext.FirstOrDefault(g => g.Id == id);
//         if (genre is null)
//         {
//             return Redirect("/admin");
//         }
//         var localPath = "Views/Templates/Pages/UpdatePages/Update-Genre.html";
//         var text = _responseHelper.GetResponseTextCustomPath(localPath);
//         var page = _htmlTemplateEngine.Render(text, genre);
//         return Html(page);
//     }
//     
//     [Post("admin/genres/update")]
//     public IHttpResponseResult UpdateGenre(Genre genre)
//     {
//         using var connection = new SqlConnection(_appConfig.ConnectionString);
//         var dbContext = new ORMContext<Genre>(connection);
//         dbContext.Update(genre);
//         return Redirect("/admin");
//     }
//     
//     [Get("admin/users/update")]
//     public IHttpResponseResult GetUpdateUsersPage(int id)
//     {
//         using var connection = new SqlConnection(_appConfig.ConnectionString);
//         var dbContext = new ORMContext<User>(connection);
//         var user = dbContext.FirstOrDefault(g => g.Id == id);
//         if (user is null)
//         {
//             return Redirect("/admin");
//         }
//         var localPath = "Views/Templates/Pages/UpdatePages/Update-User.html";
//         var text = _responseHelper.GetResponseTextCustomPath(localPath);
//         var page = _htmlTemplateEngine.Render(text, user);
//         return Html(page);
//     }
//     
//     [Post("admin/users/update")]
//     public IHttpResponseResult UpdateUser(User user)
//     {
//         using var connection = new SqlConnection(_appConfig.ConnectionString);
//         var dbContext = new ORMContext<User>(connection);
//         dbContext.Update(user);
//         return Redirect("/admin");
//     }
// }


using HttpServerLibrary;
using HttpServerLibrary.Core.Attributes;
using HttpServerLibrary.Core.HttpResponse;
using HttpServerLibrary.Models;
using Microsoft.Data.SqlClient;
using my_http.Helpers;
using MyORMLibrary;
using TemlateEngine;
using System;
using my_http.Models.Entities;
using my_http.Models.UpdateObjectsVm;

namespace my_http.Endpoints;

public class UpdateObjectsEndpoint : EndpointBase
{
    private readonly ResponseHelper _responseHelper = new ResponseHelper();
    private readonly AppConfig _appConfig = AppConfig.GetInstance();
    private readonly IHtmlTemplateEngine _htmlTemplateEngine = new HtmlTemplateEngine();

    // Универсальный метод для получения страницы обновления
    private IHttpResponseResult GetUpdatePage<T>(int id, string templatePath) where T : class, new()
    {
        try
        {
            using var connection = new SqlConnection(_appConfig.ConnectionString);
            var dbContext = new ORMContext<T>(connection);
            var entity = dbContext.GetById(id);

            if (entity is null)
            {
                return Redirect("/admin");
            }

            object model;
            if (typeof(T) == typeof(Movie))
            {
                model = new UpdateMovieVm((Movie)(object)entity); // Кастомная модель для Movie
            }
            else
            {
                model = entity; // Для всех остальных сущностей используем саму модель
            }

            var text = _responseHelper.GetResponseTextCustomPath(templatePath);
            var page = _htmlTemplateEngine.Render(text, model);
            return Html(page);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return Error(e.Message);
        }
    }

    // Универсальный метод для обновления сущности
    private IHttpResponseResult UpdateEntity<T>(T entity) where T : class, new()
    {
        try
        {
            using var connection = new SqlConnection(_appConfig.ConnectionString);
            var dbContext = new ORMContext<T>(connection);
            dbContext.Update(entity);
            return Redirect("/admin");
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            return Json(new { Message = e.Message });
        }
    }

    [Get("admin/movies/update")]
    public IHttpResponseResult GetUpdateMoviePage(int id)
        => GetUpdatePage<Movie>(id, "Views/Templates/Pages/UpdatePages/Update-Movie.html");

    [Post("admin/movies/update")]
    public IHttpResponseResult UpdateMovie(Movie movie)
        => UpdateEntity(movie);

    [Get("admin/actors/update")]
    public IHttpResponseResult GetUpdateActorPage(int id)
        => GetUpdatePage<Actor>(id, "Views/Templates/Pages/UpdatePages/Update-Actor.html");

    [Post("admin/actors/update")]
    public IHttpResponseResult UpdateActor(Actor actor)
        => UpdateEntity(actor);

    [Get("admin/movieactors/update")]
    public IHttpResponseResult GetUpdateMovieActorPage(int id)
        => GetUpdatePage<MovieActor>(id, "Views/Templates/Pages/UpdatePages/Update-MovieActor.html");

    [Post("admin/movieactors/update")]
    public IHttpResponseResult UpdateMovieActor(MovieActor movieActor)
        => UpdateEntity(movieActor);

    [Get("admin/genres/update")]
    public IHttpResponseResult GetUpdateGenresPage(int id)
        => GetUpdatePage<Genre>(id, "Views/Templates/Pages/UpdatePages/Update-Genre.html");

    [Post("admin/genres/update")]
    public IHttpResponseResult UpdateGenre(Genre genre)
        => UpdateEntity(genre);

    [Get("admin/users/update")]
    public IHttpResponseResult GetUpdateUsersPage(int id)
        => GetUpdatePage<User>(id, "Views/Templates/Pages/UpdatePages/Update-User.html");

    [Post("admin/users/update")]
    public IHttpResponseResult UpdateUser(User user)
        => UpdateEntity(user);
}
