using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Globalization;
using userManagerAplication.Models.Data;
using userManagerApplication.Auxiliary;
using userManagerApplication.Models;
using userManagerApplication.Repository.Interfaces;

namespace userManagerApplication.Controllers
{
    //[Authorize(Policy = IdentityData.AdminUserPolicyName)]
    [Authorize(Policy = "AdminPolicy")]

    public class AdminController : Controller
    {
        private readonly IUsersRepository<User> _repository;
        private readonly IConfiguration _configuration;

        public AdminController(IUsersRepository<User> repository, IConfiguration configuration) 
        {
            _configuration = configuration;
            _repository = repository;
        }


        public IActionResult Index()
        {

            //var token = HttpContext.Session.GetString("Token"); //Se descarta sesión por cookie
            string token = HttpContext.Request.Cookies["Token"];

            //if (token == null)
            //    return RedirectToAction("Index", "Access");

            //var tokenHelper = new TokenHelper(_configuration);
            //if (tokenHelper.ValidateTokenAccess(token)) 
            //{
            List<UserModel> users = _repository.GetAllUserAndRoles();
            ViewBag.Users = users;

            //HttpRequest request = HttpContext.Request;

            //if (request == null)
            //{
            //    throw new ArgumentNullException("request");
            //}

            //var t = request.Headers["X-Requested-With"] == "XMLHttpRequest";

            return View(users);


            //if (t)
            //{
            //    return PartialView("./Views/Admin/Index.cshtml", users);
            //}
            //else            
            //    return View(users);
            //}
            //else
            //{
            //    return RedirectToAction("Index", "Access");
            //}

        }


        public IActionResult PartialIndex(string action = "Index") 
        {
            action = "Index";

            List<UserModel> users = _repository.GetAllUserAndRoles();
            ViewBag.Users = users;

            string culture = Request.Cookies["Language"];
            if (culture != null)
                CultureInfo.CurrentUICulture = new CultureInfo(culture);

            var response = new JObject
            {
                ["data"] = this.RenderPartialViewToString(action, null),                
            };
            

            return Content(response.ToString());

        }


        [Authorize(Policy = "AdminPolicy")]
        [HttpGet]
        public IActionResult GetUserData(int id)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var user = _repository.Get(id);
                if (user != null)
                {
                    var encryption = new Helper.Encryption();
                    var userPassDecrypt = encryption.Decrypt(user.Password);
                    user.Password = userPassDecrypt;

                    response.Data = user;                    
                    response.Success = true;                    
                }
                else
                {
                    response.Error = "User not found";
                    response.Message = "An error occurred";
                    response.Success = false;
                }
                
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
        public IActionResult UpdateUser([FromBody]UserModel user)
        {
            ResponseModel response = new ResponseModel();
            UserModel userUpdated = null;
            try
            {
                if (user != null)
                {
                    var encryption = new Helper.Encryption();
                    var userPassEncrypt = encryption.Encrypt(user.Password);

                    var userOldData = _repository.Get(user.IdUser);
                    userOldData.Status = user.StatusName == "Active" ? true : false;
                    userOldData.Email = user.Email;
                    userOldData.IdRole = Convert.ToInt16(user.IdRole);
                    userOldData.Phone = user.Phone;
                    userOldData.Name = user.Name;
                    userOldData.LastName = user.LastName;
                    userOldData.Password = userPassEncrypt;

                    _repository.Update(userOldData);
                    _repository.Save();

                    //get user with updated data
                    userUpdated = _repository.GetUserAndRol(user.IdUser);

                    response.Data = userUpdated;
                    response.Message = "Data updated correctly";
                    response.Success = true;
                }
                
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
        public IActionResult InactiveUser([FromBody] InactiveUserModel model)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                //do not deactivate admin account
                var user = _repository.Find(x => x.Email == "admin@correo.com", null).FirstOrDefault();
                if (user.IdUser != model.IdUser)
                {
                    _repository.DeactivateUser(model.IdUser, model.Status);
                    _repository.Save();
                    response.Success = true;
                }
                else
                {
                    response.Error = "This account should not be deactivated";
                    response.Message = "You do not have permission";
                    response.Success = false;
                }


                
            }
            catch (Exception ex)
            {
                response.Error = ex.Message;
                response.Message = "An error occurred";
                response.Success = false;
            }

            return Json(response);
        }

        [HttpPost]
        public IActionResult AddUser([FromBody] UserModel model)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                if (model != null)
                {
                    var encryption = new Helper.Encryption();
                    var userPassEncrypt = encryption.Encrypt(model.Password);

                    var user = new User 
                    { 
                        DateAdmision = DateTime.Now,
                        Email = model.Email,
                        InactiveDate = null,
                        IdRole = model.IdRole,
                        Name = model.Name,
                        LastName = model.LastName,
                        Password = userPassEncrypt,
                        Phone = model.Phone,
                        Status = true                        
                    };
                    _repository.Add(user);
                    _repository.Save();

                    response.Data = user;
                    response.Message = "User created successfully";
                    response.Success = true;
                }
                else
                {
                    response.Error = "An error occurred when creating the user. Empty parameter";
                    response.Message = "An error occurred";
                    response.Success = false;
                }

               
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
