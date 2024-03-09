using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using userManagerAplication.Models.Data;
using userManagerApplication.Models;
using userManagerApplication.Repository.Interfaces;

namespace userManagerApplication.Controllers
{
    //[Authorize(Policy = IdentityData.AdminUserPolicyName)]
    [Authorize(Policy = "AdminPolicy")]

    public class RolesController : Controller
    {
        private readonly IGenericRepository<UsersRole> _repository;

        public RolesController(IGenericRepository<UsersRole> repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetRole(int id) 
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var role = _repository.Get(id);
                response.Data = role;
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

        [HttpGet]
        public IActionResult GetAllRoles()
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var roles = _repository.GetAll();
                response.Data = roles;
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

        [HttpPost]
        public IActionResult AddRole([FromBody] UsersRole model)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                _repository.Add(model);
                response.Message = "Role created successfully";
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
        public IActionResult UpdateRole([FromBody] UsersRole model) 
        {
            ResponseModel response = new ResponseModel();
            try
            {
                _repository.Update(model);
                response.Message = "Role updated successfully";
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
