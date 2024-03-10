using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Globalization;
using userManagerAplication.Models.Data;
using userManagerApplication.Auxiliary;
using userManagerApplication.Indentity;
using userManagerApplication.Models;
using userManagerApplication.Repository.Interfaces;

namespace userManagerApplication.Controllers
{
    //[Authorize(Policy = IdentityData.UserPolicyName)]
    [Authorize(Policy = "UserPolicy")]

    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger;
        private readonly IGenericRepository<UsersRole> _repository;
        private readonly IStringLocalizer<HomeController> _localizer; //localizacion / globalizacion
        

        public HomeController(IConfiguration configuration, ILogger<HomeController> logger, IGenericRepository<UsersRole> repository, IStringLocalizer<HomeController> localizer)
        {
            _logger = logger;
            _repository = repository;
            _configuration = configuration;
            _localizer = localizer;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            string token = HttpContext.Request.Cookies["Token"];
            
            var tokenHelper = new TokenHelper(_configuration);
            if (tokenHelper.ValidateTokenAccess(token))
            {

                //HttpRequest request = HttpContext.Request;
                //if (request == null)
                //{
                //    throw new ArgumentNullException("request");
                //}

                //var t = request.Headers["X-Requested-With"] == "XMLHttpRequest";
                //if (t)
                //{
                //    return PartialView("./Views/Home/Index.cshtml");
                //}
                //else
                return View();
                //return PartialView("./Views/Home/Index.cshtml");
            }
            else
            {
                return RedirectToAction("Index", "Access");
            }
        }

        [AllowAnonymous]
        public IActionResult PartialIndex(string action = "Index")
        {
            action = "Index";

            string culture = Request.Cookies["Language"];
            if (culture != null)
                CultureInfo.CurrentUICulture = new CultureInfo(culture);

            var response = new JObject
            {
                ["data"] = this.RenderPartialViewToString(action, null)
            };


            return Content(response.ToString());

        }

        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)));

            return LocalRedirect(returnUrl);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}