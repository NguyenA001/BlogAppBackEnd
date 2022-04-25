using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogAppBackEnd.Models;
using BlogAppBackEnd.Models.DTO;
using BlogAppBackEnd.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlogAppBackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _data;
        public UserController(UserService dataFromService)
        {
            _data = dataFromService;
        }

        [HttpGet("userbyusername/{username}")]
        public UserIdDTO GetUserByUsername(string username)
        {
            return _data.GetUserDTOByUsername(username);
        }

        //add a user
        [HttpPost("AddUsers")]
        public bool AddUser(CreateAccountDTO UserToAdd)
        {
            return _data.AddUser(UserToAdd);
        }

        //Get users
        [HttpGet("GetAllUsers")]
        //our data is in a list format but its actually a collection
        public IEnumerable<UserModel> GetAllUsers()
        {
            return _data.GetAllUsers();
        }

        //log in
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginDTO User)
        {
            return _data.Login(User);
        }
        //update user account

        [HttpPost("UpdateUser")]

        public bool UpdateUser(UserModel userToUpdate)
        {
            return _data.UpdateUser(userToUpdate);
        }

        [HttpPost("UpdateUser/{id}/{username}")]

        public bool UpdateUser(int id, string username)
        {
            return _data.UpdateUsername(id,username);
        }

        //delete user account

        [HttpPost("DeleteUser/{userToDelete}")]

        public bool DeleteUser(string userToDelete)
        {
            return _data.DeleteUser(userToDelete);
        }
    }
}