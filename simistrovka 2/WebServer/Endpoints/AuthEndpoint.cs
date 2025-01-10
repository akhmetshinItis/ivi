using System.Net;
using HttpServerLibrary;
using HttpServerLibrary.Core.Attributes;
using HttpServerLibrary.Core.HttpResponse;
using HttpServerLibrary.Models;
using Microsoft.Data.SqlClient;
using my_http.Helpers;
using my_http.Models.Entities;
using MyORMLibrary;
using TemlateEngine;

namespace my_http.Endpoints;

public class AuthEndpoint : EndpointBase
{
    private readonly ResponseHelper _responseHelper = new ResponseHelper();
    private readonly AppConfig _appConfig = AppConfig.GetInstance();
    private readonly IHtmlTemplateEngine _htmlTemplateEngine = new HtmlTemplateEngine();
    
    [Get("login")]
    public IHttpResponseResult Index()
    {
        try
        {
            var localPath = "sign-in.html";
            var responseText = _responseHelper.GetResponseText(localPath);
            return Html(responseText);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return Error(e.Message);
        }
    }

    [Post("login")]
    public IHttpResponseResult Login(string login, string password)
    {
        try
        {
            using var connection = new SqlConnection(_appConfig.ConnectionString);
            var dbContext = new ORMContext<User>(connection);
            var user = dbContext.FirstOrDefault(u => u.UserName == login);
            if (user is null || user.Password != password)
            {
                return Redirect("login");
            }

            var token = Guid.NewGuid().ToString();
            Cookie nameCookie = new Cookie("session-token", token);
            nameCookie.Path = "/";
            Context.Response.Cookies.Add(nameCookie);
            SessionStorage.SaveSession(token, user.Id.ToString());

            return Redirect("index");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return Error(e.Message);
        }
    }
}