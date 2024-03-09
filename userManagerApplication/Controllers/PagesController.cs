using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Globalization;
using System.Linq;
using userManagerAplication.Models.Data;
using userManagerApplication.Auxiliary;
using userManagerApplication.Indentity;
using userManagerApplication.Models;
using userManagerApplication.Repository.Interfaces;

namespace userManagerApplication.Controllers
{
    public class PagesController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IGenericRepository<AccessScreen> _repositoryAccesScr;
        private readonly IGenericRepository<Screen> _repositoryScr;
        //private readonly IStringLocalizerFactory _localizer; //localizacion / globalizacion
        private readonly IStringLocalizer<HomeController> _localizer; //localizacion / globalizacion

        public PagesController(IConfiguration configuration, IGenericRepository<AccessScreen> repositoryAccesScr, IGenericRepository<Screen> repositoryScr, IStringLocalizer<HomeController> localizer /*IStringLocalizerFactory localizer*/)
        {
            _configuration = configuration;
            _repositoryAccesScr = repositoryAccesScr;
            _repositoryScr = repositoryScr;
            _localizer = localizer;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            string token = HttpContext.Request.Cookies["Token"];

            var tokenHelper = new TokenHelper(_configuration);
            if (tokenHelper.ValidateTokenAccess(token))
            {
                return View();
                
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

        //Get pages that the user has access
        [HttpGet]
        public IActionResult GetAllUserPages()
        {
            var response = new ResponseModel();

            try
            {
                int idUser = Convert.ToInt16(HttpContext.Request.Cookies["IdUser"]);                
                var userAccess = _repositoryAccesScr.Find(x => x.IdUser == idUser, "IdScreenNavigation").ToList();

                var r = _localizer.GetString("Home");

                //language cookie
                string culture = Request.Cookies["Language"];
                CultureInfo.CurrentUICulture = new CultureInfo(culture);
                //var localizerPages = _localizer.Create(typeof(PagesController));

                var t = _localizer.GetString("Home");

                var screens = userAccess
                    .Select(x => new ScreenModel
                    {
                        Id = (int)x.IdScreen,
                        Title = _localizer.GetString(x.IdScreenNavigation.Name), //name according to location
                        URL = x.IdScreenNavigation.Url,
                    })
                    .OrderBy(o => o.Id)
                    .ToList();

                

                ////add close sesion
                //var screenClose = new ScreenModel
                //{
                //    Id = -1,
                //    Title = localizerPages["SignOff"],
                //    URL = "/Access",
                //};
                //screens.Add(screenClose);

                response.Data = screens;
                response.Success = true;


            }
            catch (Exception ex)
            {
                response.Error = ex.Message;
                response.Message = "An error occurred";
                response.Success = false;
            }

            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            return Json(response);
        }

        //Get all pages and get pages that the user has access
        [HttpGet]
        public IActionResult GetAllUserAndAccessPages(int idUser)
        {
            var response = new ResponseModel();

            try
            {
                var scr = _repositoryScr.GetAll();
                var userAccess = _repositoryAccesScr.Find(x => x.IdUser == idUser, "IdScreenNavigation").ToList();
                var screens = scr.Select(x => new ScreenUserModel
                {
                    Id = (int)x.IdScreen,
                    Title = x.Name,
                    URL = x.Url,
                    UserAccess = userAccess.Any(y => y.IdScreen == x.IdScreen)
                });

                response.Data = screens;
                response.Success = true;


            }
            catch (Exception ex)
            {
                response.Error = ex.Message;
                response.Message = "An error occurred";
                response.Success = false;
            }
            return Json(response);
        }

        //Get all pages
        [HttpGet]
        public IActionResult GetAllPages()
        {
            var response = new ResponseModel();

            try
            {
                int idUser = Convert.ToInt16(HttpContext.Request.Cookies["IdUser"]);
                var userAccess = _repositoryScr.GetAll();
                var screens = userAccess.Select(x => new ScreenModel
                {
                    Id = (int)x.IdScreen,
                    Title = x.Name
                });

                response.Data = screens;
                response.Success = true;


            }
            catch (Exception ex)
            {
                response.Error = ex.Message;
                response.Message = "An error occurred";
                response.Success = false;
            }
            return Json(response);
        }

        [HttpPut]
        public IActionResult UpdateUserPages([FromBody]List<UserAccessPageModel> model)
        {
            var response = new ResponseModel();
            try
            {
                var idUser = model.Select(x => x.IdUser).FirstOrDefault();
                var userAccess = _repositoryAccesScr.Find(x => x.IdUser == idUser, "IdScreenNavigation").ToList();

                //remove all access
                _repositoryAccesScr.DeleteList(userAccess);

                //add new access, if notScreens is true, there are no screens to add to the user 
                var notScreens = model.Any(x => x.IdScreen == -1);
                if (!notScreens)
                {
                    var lstAccess = model.Select(x => new AccessScreen
                    {
                        IdScreen = x.IdScreen,
                        IdUser = x.IdUser,
                    });
                    _repositoryAccesScr.AddList(lstAccess);
                }
               

                
                response.Message = "User access successfully updated";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Error = ex.Message;
                response.Message = "An error occurred";
                response.Success = false;
            }
            return Json(response);
        }


    }
}
