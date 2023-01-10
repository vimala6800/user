using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartnerPortal.Application.ProjectManagers.Commands;
using PartnerPortal.Application.ProjectManagers.Queries;
using PartnerPortal.Domain.Entities;
using PartnerPortal.Infrastructure.Identity;
using PartnerPortal.Infrastructure.Persistence;
using System.Runtime.InteropServices;
using PartnerPortal.Application.Common.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Reflection.Metadata.Ecma335;

namespace PartnerPortal.WebApi.Controllers
{
    
    public class ProjectManagerController : ApiControllerBase
    {
        private readonly ApplicationDbContext _fullStackDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        

        public ProjectManagerController(ApplicationDbContext fullStackDbContext, UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
        {
            _fullStackDbContext = fullStackDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        [HttpGet]
        public async Task<ActionResult> GetAllEmployees()
        {
            var projectmanagers = await _fullStackDbContext.ProjectManagers.ToListAsync();
            return Ok(projectmanagers);
        }
        /*[HttpGet]
        public async Task<ActionResult<PaginatedList<ProjectManagerBriefDto>>> GetProjectManagersWithPagination([FromQuery] GetProjectManagersWithPaginationQuery query)
        {
            return await Mediator.Send(query);
        }*/
        [HttpPost]
        //forusers
        public async Task<IActionResult> Register([FromBody] Users registerUser, string role)
        {
            //Check User Exist
            var userExist = await _userManager.FindByEmailAsync(registerUser.Email);
            if (userExist != null)
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                new Response { Status = "Error", Message = "User already exists!" });
            }

            //Add the User in the database
            ApplicationUser user = new()
            {
                Email = registerUser.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerUser.Username,
                PhoneNumber = registerUser.PhoneNumber,
                TwoFactorEnabled = true
            };
            if (await _roleManager.RoleExistsAsync(role))
            {
                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response { Status = "Error", Message = "User Failed to Create" });
                }
                //Add role to the user....

                await _userManager.AddToRoleAsync(user, role);

                ////Add Token to Verify the email....
                //var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);




                return StatusCode(StatusCodes.Status200OK,
                 new Domain.Entities.Response { Status = "Success", Message = $"User created {user.Email}  SuccessFully" });

            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Error", Message = "This Role Doesnot Exist." });
            }


        }
        //forusers
        /*[HttpGet("viewPM")]
        public async Task<IActionResult> Create(string lat)
        {
            var view = await lat;
            
             return view;
        }*/


        //for
        /*[HttpPost("AddPM")]
        public async Task<IActionResult> AddEmployee([FromBody] ProjectManager employeeRequest)
        {


            *//*            employeeRequest.PMUserID = await _fullStackDbContext.AspNetUsers.FirstAsync(x => x.UserName == employeeRequest.ProjectManagerName);
            *//*
            var getid = await _userManager.Users.FirstAsync(x => x.Email == employeeRequest.PMEmailID);
            var got = Guid.Parse(getid.Id);
            employeeRequest.PMUserID = got;
            await _fullStackDbContext.ProjectManagers. AddAsync(employeeRequest);
            await _fullStackDbContext.SaveChangesAsync();
            return Ok(employeeRequest);

        }*/
        //for


        [HttpPost("AddPM")]
        public async Task<ActionResult<int>> Create(CreateProjectManagerCommand command)
        {

            return await Mediator.Send(command);
        }






        [HttpGet]
        [Route("{projectManagerID:Guid}")]
        public async Task<IActionResult> GetEmployee([FromRoute] Guid projectManagerID)
        {
            var employee = await _fullStackDbContext.ProjectManagers.FirstOrDefaultAsync(x => x.ProjectManagerID == projectManagerID);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);



        }

        [HttpGet("skill")]

        public Skill[] GetPMSkills()
        {
            var projectmanagerskills = _fullStackDbContext.Skills.ToList();
            return projectmanagerskills.ToArray();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, UpdateProjectManagerCommand command)
        {
            if (id != command.ProjectManagerID)
            {
                return BadRequest();
            }



            await Mediator.Send(command);



            return NoContent();
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await Mediator.Send(new DeleteProjectManagerCommand(id));



            return NoContent();
        }

        

    }
}
