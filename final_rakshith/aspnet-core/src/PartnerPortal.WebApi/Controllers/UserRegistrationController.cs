
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PartnerPortal.Domain.Entities;
using PartnerPortal.Domain.Entities.BindingModel;
using PartnerPortal.Domain.Entities.DataTransferObjects;
using PartnerPortal.Domain.Entities.DTO;
using PartnerPortal.Domain.Enums;
using PartnerPortal.Infrastructure.Identity;
using PartnerPortal.Infrastructure.Persistence;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Duende.IdentityServer.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using PartnerPortal.Domain.Entities.DataTransferObjects;
using Microsoft.AspNetCore.WebUtilities;
using PartnerPortal.WebApi.Services;
using PartnerPortal.WebApi.Settings;
using SendGrid.Helpers.Mail;
using Microsoft.IdentityModel.Tokens;

namespace PartnerPortal.WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserRegistrationController : ApiControllerBase
	{
		private readonly ILogger<UserController> _logger;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IConfiguration _configuration;
		private readonly IMapper _mapper;
		private readonly Services.IMailService _mailService;
		private readonly Settings.MailSettings _mailSettings;
		private readonly MailRequest _mailRequest;
		private readonly ApplicationDbContext _DbContext;
		public UserRegistrationController(ILogger<UserController> logger, IConfiguration configuration, IMapper mapper, Services.IMailService mailService, MailRequest mailRequest, ApplicationDbContext DbContext, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signManager, RoleManager<IdentityRole> roleManager)
		{
			_userManager = userManager;
			_signInManager = signManager;
			_roleManager = roleManager;
			_logger = logger;
			_mailService = mailService;
			_configuration = configuration;
			_mailRequest = mailRequest;
			_DbContext = DbContext;
		}
		//-----------------------------------------------------------------------------------------------------------------
		//User register start
		//To register User with Roles Incoperated with it//
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
				//Check User Exist
				var userExist = await _userManager.FindByEmailAsync(model.Email);
				if (userExist != null)
				{
					return StatusCode(StatusCodes.Status403Forbidden,
					new Response { Status = "Error", Message = "User already exists!" });
				}
				var user = new ApplicationUser() {Id = Guid.NewGuid().ToString(), UserName = model.UserName, Email = model.Email, PhoneNumber = model.PhoneNumber, SecurityStamp = Guid.NewGuid().ToString(), TwoFactorEnabled = true };
				var result = await _userManager.CreateAsync(user);
				//---------------------------------------------------------------------------------------------------------------------------------------
				// email generation  
				var token = await _userManager.GeneratePasswordResetTokenAsync(user);
				var param = new Dictionary<string, string?> {
				{ "token", token},
				{ "email", model.Email}
			};

				var callback = QueryHelpers.AddQueryString(model.ClientURI, param);
				var emailBody = _DbContext.EmailTemplates.FirstOrDefault(x => x.TemplateCode == 3.ToString());
				var emailBody1 = emailBody.MessageTemplate.Replace("{NAME}", user.Email).Replace("{link}", callback);


				_mailRequest.ToEmail = user.Email;
				_mailRequest.Subject = "Reset password";
				_mailRequest.Body = emailBody1;
				_mailRequest.Attachments = null;

				var message = _mailRequest;
				//end email 
				//-----------------------------------------------------------------------------------------------------------------
				if (result.Succeeded)
				{
					var tempUser = await _userManager.FindByEmailAsync(model.Email);
					foreach (var role in model.Roles)
					{
						await _userManager.AddToRoleAsync(tempUser, role);
						await _mailService.SendEmailAsync(message);
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
		//-----------------------------------------------------------------------------------------------------------------
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
		//-----------------------------------------------------------------------------------------------------------------
		//add roles if personal use.

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
		//-----------------------------------------------------------------------------------------------------------------
		//to call all rolename as list so that use it in userlist {D}
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
