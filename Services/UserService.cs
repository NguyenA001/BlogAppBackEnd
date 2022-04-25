using System;
using System.Collections.Generic;
using System.Linq; //language integrated query
using System.Threading.Tasks;
using BlogAppBackEnd.Models;
using BlogAppBackEnd.Models.DTO;
using BlogAppBackEnd.Services.Context;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BlogAppBackEnd.Services
{
    public class UserService : ControllerBase
    {
        private readonly DataContext _context;
        public UserService(DataContext DataFromContext)
        {
            _context = DataFromContext;
        }


        public IEnumerable<UserModel> GetAllUsers()
        {
            return _context.UserInfo;
        }

        public bool DoesUserExist(string? username)
        {
            //check the table to see if they username exists (so i know i need to connect to _context for access to tables)
            //single or default
            //if one item matches our condition, that item will returned
            //if no item matches our condition, a null will be returned
            //if multiple item match our condition, an error will occur

            // UserModel foundUser = _context.UserInfo.SingleOrDefault( user => user.Username == username);
            // if(foundUser == null)
            // {
            //     the user does not exist in the table
            // }
            // else
            // {
            //     the user does exist
            // }

            //refer above

            return _context.UserInfo.SingleOrDefault(user => user.Username == username) != null;
        }


        public UserModel GetUserByUsername(string username)
        {
            return _context.UserInfo.SingleOrDefault(user => user.Username == username);
        }
        public UserIdDTO GetUserDTOByUsername(string username)
        {
            var UserInfo = new UserIdDTO();
            var foundUser =  _context.UserInfo.SingleOrDefault(user => user.Username == username);
            UserInfo.UserId = foundUser.Id;
            UserInfo.PublisherName = foundUser.Username;
            return UserInfo;
        }
        public UserModel GetUserByID(int ID)
        {
            return _context.UserInfo.SingleOrDefault(user => user.Id == ID);
        }
        public IActionResult Login(LoginDTO user)
        {
            IActionResult Result = Unauthorized();
            //check to see if the user exist
            if (DoesUserExist(user.Username))
            {
                //true
                var foundUser = GetUserByUsername(user.Username);
                //check to see if the password is correct
                var verifypass = VerifyUserPassword(user.Password, foundUser.Hash, foundUser.Salt);
                if (verifypass)
                {
                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("DayClassSuperDuperSecretKey@209"));
                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                    var tokeOptions = new JwtSecurityToken(
                        issuer: "http://localhost:5000",
                        audience: "http://localhost:5000",
                        claims: new List<Claim>(),
                        expires: DateTime.Now.AddMinutes(30),
                        signingCredentials: signinCredentials
                    );
                    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                    Result = Ok(new { Token = tokenString });
                }
            }
            return Result;
        }

        public bool AddUser(CreateAccountDTO UserToAdd)
        {
            //check if the user already exists
            //if they do not exist, we can then have the account be created
            //else throw a false

            bool result = false;

            if (!DoesUserExist(UserToAdd.Username))
            {
                // the user does exist so we add them to our table
                UserModel newUser = new UserModel();
                var hashedPassword = HashPassword(UserToAdd.Password);
                newUser.Id = UserToAdd.Id; //0
                newUser.Username = UserToAdd.Username; //whatever username you pass in 
                newUser.Salt = hashedPassword.Salt;
                newUser.Hash = hashedPassword.Hash;

                _context.Add(newUser);
                result = _context.SaveChanges() != 0;
            }
            return result;
        }



        public PasswordDTO HashPassword(string? password)
        {
            PasswordDTO newHashedPassword = new PasswordDTO();
            byte[] SaltBytes = new byte[64];
            var provider = new RNGCryptoServiceProvider();
            provider.GetNonZeroBytes(SaltBytes);
            var Salt = Convert.ToBase64String(SaltBytes);
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, SaltBytes, 10000);
            var Hash = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));
            newHashedPassword.Salt = Salt;
            newHashedPassword.Hash = Hash;
            return newHashedPassword;
        }

        public bool VerifyUserPassword(string? Password, string? StoredHash, string? StoredSalt)
        {
            var SaltBytes = Convert.FromBase64String(StoredSalt);
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(Password, SaltBytes, 10000);
            var newHash = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));
            return newHash == StoredHash;
        }

        public bool UpdateUser(UserModel userToUpdate)
        {
            //This one is sednig over the whole object to be updated
            _context.Update<UserModel>(userToUpdate);
            return _context.SaveChanges() !=0; 
        }
        public bool UpdateUsername(int id, string Username)
        {
            //This one is sednig over just the username.
            //Then you have to get the object to then be updated.
            UserModel foundUser = GetUserByID(id);
            bool result = false;
            if(foundUser != null)
            {
                //A user was foundUser
                foundUser.Username = Username;
                _context.Update<UserModel>(foundUser);
               result =  _context.SaveChanges() != 0;
            }
            return result;
        }

        public bool DeleteUser(string Username)
        {
            //This one is sednig over just the username.
            //Then you have to get the object to then be updated.
            UserModel foundUser = GetUserByUsername(Username);
            bool result = false;
            if(foundUser != null)
            {
                //A user was foundUser
                foundUser.Username = Username;
                _context.Remove<UserModel>(foundUser);
               result =  _context.SaveChanges() != 0;
            }
            return result;
        }
    }
}