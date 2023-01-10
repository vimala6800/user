
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PartnerPortal.Domain.Entities;
using PartnerPortal.Domain.Entities.BindingModel;
using PartnerPortal.Domain.Entities.DTO;
using PartnerPortal.Domain.Entities.Models;
using PartnerPortal.Domain.Enums;
using PartnerPortal.Infrastructure.Identity;
using PartnerPortal.Infrastructure.Persistence;
using PartnerPortal.WebApi.Services;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PartnerPortal.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AController : ApiControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _DbContext;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        public AController(ILogger<UserController> logger, IConfiguration configuration, IEmailService emailService, ApplicationDbContext DbContext, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signManager;
            _roleManager = roleManager;
            _logger = logger;
            _DbContext = DbContext;
            _emailService = emailService;
            _configuration = configuration;
        }
        //    [HttpPost("RegisterUser")]
        //    public async Task<object> RegisterUser([FromBody] AddUpdateRegisterUserBindingModel model)
        //    {
        //        try
        //        {
        //            if (model.Roles == null)
        //            {
        //                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Roles are missing", null));
        //            }
        //            foreach (var role in model.Roles)
        //            {

        //                if (!await _roleManager.RoleExistsAsync(role))
        //                {

        //                    return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Role does not exist", null));
        //                }
        //            }


        //            var user = new ApplicationUser() { UserName = model.UserName, Email = model.Email, PhoneNumber = model.PhoneNumber, SecurityStamp = Guid.NewGuid().ToString(), TwoFactorEnabled = true };
        //            var result = await _userManager.CreateAsync(user);
        ////            //Add Token to Verify the email....
        ////            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        ////            var param = new Dictionary<string, string?>
        ////{
        ////    {"token", token },
        ////    {"email", user.Email }
        ////};
        ////            var callback = QueryHelpers.AddQueryString(userForRegistration.ClientURI, param);
        ////            var message = new Message(new string[] { user.Email }, "Email Confirmation token", callback, null);
        ////            await _emailSender.SendEmailAsync(message);

        //            if (result.Succeeded)
        //            {
        //                var tempUser = await _userManager.FindByEmailAsync(model.Email);
        //                foreach (var role in model.Roles)
        //                {
        //                    await _userManager.AddToRoleAsync(tempUser, role);
        //                }
        //                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "User has been Registered", null));
        //            }
        //            return await Task.FromResult(new ResponseModel(ResponseCode.Error, "", result.Errors.Select(x => x.Description).ToArray()));
        //        }
        //        catch (Exception ex)
        //        {
        //            return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
        //        }
        //    }
        //[HttpPost("RegisterUser")]
        //public async Task<object> RegisterUser([FromBody] AddUpdateRegisterUserBindingModel model)
        //{
        //    try
        //    {
        //        if (model.Roles == null)
        //        {
        //            return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Roles are missing", null));
        //        }
        //        foreach (var role in model.Roles)
        //        {

        //            if (!await _roleManager.RoleExistsAsync(role))
        //            {

        //                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Role does not exist", null));
        //            }
        //        }


        //        var user = new ApplicationUser() { UserName = model.UserName, Email = model.Email, PhoneNumber = model.PhoneNumber, SecurityStamp = Guid.NewGuid().ToString(), TwoFactorEnabled = true };
        //        var result = await _userManager.CreateAsync(user);
        //        Add Token to Verify the email....var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        //        var confirmationLink = Url.Action(nameof(ConfirmEmail), "Authentication", new { token, email = user.Email }, Request.Scheme);
        //        var message = new Message(new string[] { user.Email! }, "Confirmation email link", confirmationLink!);
        //        _emailService.SendEmail(message);
        //        if (result.Succeeded)
        //        {
        //            var tempUser = await _userManager.FindByEmailAsync(model.Email);
        //            foreach (var role in model.Roles)
        //            {
        //                await _userManager.AddToRoleAsync(tempUser, role);
        //            }
        //            return await Task.FromResult(new ResponseModel(ResponseCode.OK, "User has been Registered", null));
        //        }
        //        return await Task.FromResult(new ResponseModel(ResponseCode.Error, "", result.Errors.Select(x => x.Description).ToArray()));
        //    }
        //    catch (Exception ex)
        //    {
        //        return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
        //    }
        //}

        //[HttpGet("ConfirmEmail")]
        //public async Task<IActionResult> ConfirmEmail(string token, string email)
        //{
        //    var user = await _userManager.FindByEmailAsync(email);
        //    if (user != null)
        //    {
        //        var result = await _userManager.ConfirmEmailAsync(user, token);
        //        if (result.Succeeded)
        //        {
        //            return StatusCode(StatusCodes.Status200OK,
        //              new Response { Status = "Success", Message = "Email Verified Successfully" });
        //        }
        //    }
        //    return StatusCode(StatusCodes.Status500InternalServerError,
        //               new Response { Status = "Error", Message = "This User Doesnot exist!" });
        //}
        ///<summary>
        ///Get All User from database   
        ///</summary>

        [HttpGet("GetAllUser")]
        public async Task<object> GetAllUser()
        {
            try
            {
                List<UserDTO> allUserDTO = new List<UserDTO>();
                var users = _userManager.Users.ToList();
                foreach (var user in users)
                {
                    var roles = (await _userManager.GetRolesAsync(user)).ToList();

                    allUserDTO.Add(new UserDTO(user.Id, user.UserName, user.Email, user.PhoneNumber, roles));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", allUserDTO));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }
        /*[HttpGet("GetUserList")]
		public async Task<object> GetUserList()
		{
			try
			{
				List<UserDTO> allUserDTO = new List<UserDTO>();
				var users = _userManager.Users.ToList();
				foreach (var user in users)
				{
					var role = (await _userManager.GetRolesAsync(user)).ToList();
					if (role.Any(x => x == "User"))
					{
						allUserDTO.Add(new UserDTO(user.UserName, user.Email, user.PhoneNumber, role));
					}
				}
				return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", allUserDTO));
			}
			catch (Exception ex)
			{
				return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
			}
		}*/
        [HttpPost("RegisterUser")]
        public async Task<object> RegisterUser([FromBody] AddUpdateRegisterUserBindingModel model)
        {
            try
            {
                if (model.Roles == null)
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Roles are missing", null));
                }
                foreach (var role in model.Roles)
                {

                    if (!await _roleManager.RoleExistsAsync(role))
                    {

                        return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Role does not exist", null));
                    }
                }

                var user = new ApplicationUser() { UserName = model.UserName, Email = model.Email, PhoneNumber = model.PhoneNumber };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    var tempUser = await _userManager.FindByEmailAsync(model.Email);
                    foreach (var role in model.Roles)
                    {
                        await _userManager.AddToRoleAsync(tempUser, role);
                    }
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "User has been Registered", null));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "", result.Errors.Select(x => x.Description).ToArray()));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        [HttpPost("AddRole")]
        public async Task<object> AddRole([FromBody] AddRoleBindingModel model)
        {
            try
            {
                if (model == null || model.Role == "")
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.Error, "parameters are missing", null));

                }
                if (await _roleManager.RoleExistsAsync(model.Role))
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "Role already exist", null));

                }
                var role = new IdentityRole();
                role.Name = model.Role;
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {

                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "Role added successfully", null));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "something went wrong please try again later", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        [HttpGet("GetRoles")]
        public async Task<object> GetRoles()
        {
            try
            {

                var roles = _roleManager.Roles.Select(x => x.Name).ToList();

                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", roles));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }
        [HttpGet("GetRoleID")]
        public async Task<ActionResult<IEnumerable<IdentityRole>>> GetRoleID()
        {
            if (_DbContext.Roles == null)
            {
                return NotFound();
            }
            return await _DbContext.Roles.ToListAsync();
        }

    }
}
