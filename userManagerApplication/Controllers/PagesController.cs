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
        private readonly IGenericRepository<TranslationScreen> _repositoryTranslationScr;


        public PagesController(IConfiguration configuration, IGenericRepository<AccessScreen> repositoryAccesScr, IGenericRepository<Screen> repositoryScr, IGenericRepository<TranslationScreen> repositoryTranslationScr)
        {
            _configuration = configuration;
            _repositoryAccesScr = repositoryAccesScr;
            _repositoryScr = repositoryScr;
            _repositoryTranslationScr = repositoryTranslationScr;
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

                //language cookie
                string culture = Request.Cookies["Language"];

                //By default English, if the culture is en-US, it is not necessary to translate
                var screens = new List<ScreenModel>();
                if (culture != "en-US")
                {
                    //Only the menu is translated through db, the rest is through localizer in program.cs
                    var translation = _repositoryTranslationScr.Find(x => x.Translation == culture, "IdScreenNavigation").ToList();

                    screens = userAccess
                        .Select(x => new ScreenModel
                        {
                            Id = (int)x.IdScreen,
                            Title = translation.FirstOrDefault(t => t.IdScreen == x.IdScreen).Value, //name according to value id
                            URL = x.IdScreenNavigation.Url,
                        })
                        .OrderBy(o => o.Id)
                        .ToList();
                }
                else
                {
                    screens = userAccess
                        .Select(x => new ScreenModel
                        {
                            Id = (int)x.IdScreen,
                            Title = x.IdScreenNavigation.Name, //name according to value id
                            URL = x.IdScreenNavigation.Url,
                        })
                        .OrderBy(o => o.Id)
                        .ToList();
                }
                
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

                //language cookie
                string culture = Request.Cookies["Language"];

                //By default English, if the culture is en-US, it is not necessary to translate
                IEnumerable<ScreenUserModel> screens = null;
                if (culture != "en-US")
                {
                    //Only the menu is translated through db, the rest is through localizer in program.cs
                    var translation = _repositoryTranslationScr.Find(x => x.Translation == culture, "IdScreenNavigation").ToList();

                    screens = scr
                        .Select(x => new ScreenUserModel
                        {
                            Id = (int)x.IdScreen,
                            Title = translation.FirstOrDefault(t => t.IdScreen == x.IdScreen).Value, //name according to value id
                            URL = x.Url,
                            UserAccess = userAccess.Any(y => y.IdScreen == x.IdScreen)
                        })
                        .OrderBy(o => o.Id);
                }
                else
                {
                    screens = scr
                        .Select(x => new ScreenUserModel
                        {
                            Id = (int)x.IdScreen,
                            Title = x.Name, //name according to value id
                            URL = x.Url,
                            UserAccess = userAccess.Any(y => y.IdScreen == x.IdScreen)
                        })
                        .OrderBy(o => o.Id);
                        
                }


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
