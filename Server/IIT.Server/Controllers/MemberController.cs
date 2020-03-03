using CoreEngine.APIHandlers;
using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using IIT.Server.WebServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Student.Infrasructure.DBModel;
using Student.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IIT.Server.Controllers
{
    [Authorize]
    public class MemberController : Controller, IMemberHandler
    {
        private readonly UserService _userService;
        private readonly SignInManager<IdentityDBUser> _signInmanager;
        private readonly UserManager<IdentityDBUser> _userManager;
        private readonly TokenService _tokenService;
        private readonly BatchService _batchService;

        public MemberController(
            UserService userService,
            SignInManager<IdentityDBUser> signInManager,
            UserManager<IdentityDBUser> userManager,
            TokenService tokenService,
            BatchService batchService)
        {
            _userService = userService;
            _signInmanager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
            _batchService = batchService;
        }

        public async Task<ActionResponse> ChangePassword(string currentPassword, string newPassword)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var idUser = await _userService.GetIdentityUserByDBUserId(userId);
            if (idUser == null)
            {
                return new ActionResponse(false, "Invalid User");
            }
            else
            {
                var res = await _userManager.ChangePasswordAsync(idUser, currentPassword, newPassword);
                return new ActionResponse(res.Succeeded, res.Errors.Select(x => x.Description));
            }
        }

        public Task<ActionResponse> DeleteUser(DBUser user)
        {
            return Task.FromResult(new ActionResponse(false, "Not Allowed"));
        }

        public async Task<ActionResponse> ForgetPassword(string rollNo, string phoneNo, string password)
        {
            var res = await _userService.RecoverPasswordPhoneNo(rollNo, phoneNo, password);
            return res;
        }

        public async Task<List<DBUser>> GetCurrentBatchUsers()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var batch = await _userService.GetBatch(userId);
            if (batch == null)
            {
                return null;
            }
            else
            {
                return await _batchService.GetBatchStudents(batch.Id);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<SignInResponse> Login(string username, string password)
        {
            try
            {
                var dbUser = await _userService.GetUserByName(username);
                if (dbUser == null)
                {
                    return new SignInResponse(false)
                    {
                        Message = "Invalid Username"
                    };
                }
                else if (dbUser.Role == AppConstants.Student && !dbUser.PhoneNumberConfirmed)
                {
                    return new SignInResponse(false)
                    {
                        Message = "Please complete the registration"
                    };
                }
                else
                {
                    var res = await _signInmanager.PasswordSignInAsync(username, password, true, false);
                    if (res.Succeeded)
                    {
                        var token = _tokenService.GenerateJwtToken(username, dbUser);
                        return new SignInResponse(true, token);
                    }
                    else
                    {
                        return new SignInResponse(false);
                    }
                }
            }
            catch (Exception ex)
            {
                return new SignInResponse(false)
                {
                    Message = ex.Message
                };
            }
        }

        public void Logout()
        {
            var user = HttpContext.User;
            if (user != null)
            {
                _signInmanager.SignOutAsync();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public string Test()
        {
            return "Routing is ok";
        }

        public async Task<DBUser> TouchLogin()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await _userService.GetUser(userId);
            return res;
        }

        [Authorize(Roles = AppConstants.Admin)]
        public async Task<DBUser> GetUser(string userId)
        {
            var res = await _userService.GetUser(userId);
            return res;
        }

        public Task<ActionResponse> UpdateUser(DBUser user)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return _userService.Update(userId, user);
        }

        public async Task<List<DBUser>> SearchStudents(string key)
        {
            return await _userService.SearchStudent(key);
        }

        [Authorize(Roles = AppConstants.Admin)]
        [HttpPost]
        public async Task<ActionResponse> CreateStudent(int batchId, string roll, string name, string email, string phone)
        {
            return await _userService.AddStudent(batchId, roll, name, email, phone);
        }

        [Authorize(Roles = AppConstants.Admin)]
        public async Task<ActionResponse> MakeCR(string userId, bool isCR)
        {
            return await _userService.MakeCR(userId, isCR);
        }

        [Authorize(Roles = AppConstants.Admin)]
        [HttpPost]
        public async Task<ActionResponse> CreateBatchStudents(int batchId, DBFile dBFile, IFormFile formFile = null)
        {
            if (formFile == null || formFile.Length == 0)
            {
                return new ActionResponse(false, "Invalid File Respoonse");
            }
            else
            {
                var filePath = Path.GetTempFileName();

                try
                {
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    if (formFile.FileName.EndsWith("csv"))
                    {
                        var res = await _userService.UploadCSVStudents(filePath, batchId);
                        return new ActionResponse(true)
                        {
                            Data = res
                        };
                    }
                    else
                    {
                        return new ActionResponse(false);
                    }
                }
                catch (Exception ex)
                {
                    return new ActionResponse(false, ex.Message);
                }
            }
        }

        

        public async Task<List<DBUser>> GetCurrentCr()
        {
            return await _userService.GetCurrentCr();
        }

        [AllowAnonymous]
        public Task<ActionResponse> VerifyPhoneNo(string rollNo, string phoneNo)
        {
            return _userService.VerifyPhoneNo(rollNo, phoneNo);
        }

        [AllowAnonymous]
        public Task<ActionResponse> Register(string rollNo, string phoneNo, string password)
        {
            return _userService.ConfirmRegistration(rollNo, phoneNo, password);
        }
    }
}