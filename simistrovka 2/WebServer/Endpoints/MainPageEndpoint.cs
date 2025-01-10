using HttpServerLibrary;
using HttpServerLibrary.Core.Attributes;
using HttpServerLibrary.Core.HttpResponse;
using HttpServerLibrary.Models;
using Microsoft.Data.SqlClient;
using my_http.Helpers;
using my_http.Helpers.AuthHelpers;
using my_http.Models.Entities;
using my_http.Models.MainPageVms;
using MyORMLibrary;
using TemlateEngine;

namespace my_http.Endpoints;

public class MainPageEndpoint : EndpointBase
{
    private readonly ResponseHelper _responseHelper = new ResponseHelper();
    private readonly AppConfig _appConfig = AppConfig.GetInstance();
    private readonly IHtmlTemplateEngine _htmlTemplateEngine = new HtmlTemplateEngine();
    private AuthorizationHelper _authorizationHelper = new AuthorizationHelper();
    
    [Get("index")]
    public IHttpResponseResult Index()
    {
        var localPath = "Views/Templates/Pages/MainPage/index.html";
        var responseText = _responseHelper.GetResponseTextCustomPath(localPath);
        using var connection = new SqlConnection(_appConfig.ConnectionString);
        var dbContext = new ORMContext<Movie>(connection);
        var model = new MainPageDataVm();
        try
        {
            CheckAuthorization(model);
            int genre1 = 2;
            int genre2 = 5;
            int genre3 = 4;

            SetGenres(model, genre1, genre2, genre3);
            var movies1 = dbContext.Where(m => m.GenreId == genre1).ToList();
            var movies2 = dbContext.Where(m => m.GenreId == genre2).ToList();
            var movies3 = dbContext.Where(m => m.GenreId == genre3).ToList();
            model.Movies1 = movies1;
            model.Movies2 = movies2;
            model.Movies3 = movies3;
            var page = _htmlTemplateEngine.Render(responseText, model);
            return Html(page);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return Error(e.Message);
        }
    }

    private void CheckAuthorization(MainPageDataVm model)
    {
        using var connection = new SqlConnection(_appConfig.ConnectionString);
        model.IsAuthorized = _authorizationHelper.IsAuthorized(Context); // Используем метод проверки авторизации
        if (model.IsAuthorized){
        
            var userId = Int32.Parse(SessionStorage.GetUserId(Context.Request.Cookies["session-token"].Value));
            if (userId != 0)
            {
                var dbContextUser = new ORMContext<User>(connection);
                model.Username = dbContextUser.FirstOrDefault(u => u.Id == userId).UserName;
            }
        }
    }

    private void SetGenres(MainPageDataVm data, int genreId1, int genreId2, int genreId3)
    {
        using var connection = new SqlConnection(_appConfig.ConnectionString);
        var dbContext = new ORMContext<Genre>(connection);
        data.Genre1 = dbContext.FirstOrDefault(g => g.Id == genreId1);
        data.Genre2 = dbContext.FirstOrDefault(g => g.Id == genreId2);
        data.Genre3 = dbContext.FirstOrDefault(g => g.Id == genreId3);
    }
}