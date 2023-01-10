using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartnerPortal.Application.Partners.Commands;
using PartnerPortal.Application.Partners.Queries;
using PartnerPortal.Domain.Entities;
using PartnerPortal.Infrastructure.Identity;
using PartnerPortal.Infrastructure.Persistence;
using YamlDotNet.Core;


namespace PartnerPortal.WebApi.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class PartnersController : ApiControllerBase
    {
        private readonly ApplicationDbContext _fullStackDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        //private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public Guid spocuserid;

        public PartnersController(ApplicationDbContext fullStackDbContext, UserManager<ApplicationUser> userManager,
         RoleManager<IdentityRole> roleManager)
        {
            _fullStackDbContext = fullStackDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [HttpGet]
        public async Task<ActionResult> GetAllPartners()
        {
            var partners = await _fullStackDbContext.Partners.ToListAsync();
            var country = await _fullStackDbContext.Countries.ToListAsync();
            //var country=
            //    await _fullStackDbContext.Countries.ToListAsync();
            //Console.WriteLine(country);

            var Partner = from p in partners
                          join c in country on p.CountryID equals c.CountryID
                          select new PartnerBriefDto()
                          {
                              PartnerID = p.PartnerID,
                              Country = c.CountryName,
                             
                              PartnerName = p.PartnerName,
                              Location = p.Location,
                              RegisteredDate =p.RegisteredDate,
                              
                          };



            return Ok(Partner);
        }
        [HttpGet("GetAllCountries")]
        public Country[] Get()
        {
            var listofCountry = _fullStackDbContext.Countries.ToList();
            return listofCountry.ToArray();
        }
        [HttpGet("GetAllSkills")]
        public Skill[] GetSkills()
        {
            var listofSkills = _fullStackDbContext.Skills.ToList();
            return listofSkills.ToArray();
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<ActionResult> GetPartnerDetails([FromRoute] Guid id)
        {
            var partner = await _fullStackDbContext.Partners.FirstOrDefaultAsync(x => x.PartnerID == id);
            if (partner == null)
            {
                return NotFound();
            }
            return Ok(partner);
            //var partners = await _fullStackDbContext.Partners.ToListAsync();
            //var country = await _fullStackDbContext.Countries.ToListAsync();
            //var partnerSkill = await _fullStackDbContext.PartnerSkills.ToListAsync();
            //var skill = await _fullStackDbContext.Skills.ToListAsync();
            //var country=
            //    await _fullStackDbContext.Countries.ToListAsync();
            //Console.WriteLine(country);

            //var Partner = from p in partners
            //              join c in country on p.CountryID equals c.CountryID
            //              join ps in partnerSkill on p.PartnerID equals ps.PartnerID
            //              join s in skill on ps.SkillID equals s.SkillID
            //              where (p.PartnerID == id)
            //              select new PartnerBriefDto()
            //              {
            //                  PartnerID = p.PartnerID,
            //                  Country = c.CountryName,
            //                  PartnerStatus = p.PartnerStatus,
            //                  PartnerName = p.PartnerName,
            //                  Location = p.Location,
            //                  RegisteredDate = p.RegisteredDate,
            //                  Address1 = p.Address1,
            //                  BillingAddress = p.BillingAddress1,
            //                  SkillName = s.SkillName

            //              };



            //return Ok(Partner);

        }
        //[HttpGet]
        //public async Task<ActionResult<PaginatedList<PartnerBriefDto>>> GetTodoItemsWithPagination([FromQuery] GetPartnersWithPaginationQuery query)
        //{
        //    return await Mediator.Send(query);
        //}
        //[HttpGet("{id:Guid}")]
        //public async Task<IActionResult> GetOnePartner(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var details = await _fullStackDbContext.Partners


        //        .AsNoTracking()
        //        .FirstOrDefaultAsync(m => m.PartnerID == id);

        //    if (details == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(details);
        //}
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] Users registerUser, string role )
        {
            //Check User Exist
            var userExist = await _userManager.FindByEmailAsync(registerUser.Email);
            if (userExist != null)
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                new Domain.Entities.Response { Status = "Error", Message = "User already exists!" });
            }
            //Add the User in the database
            ApplicationUser user = new()
            {
                Id= Guid.NewGuid().ToString(),
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
                    new Domain.Entities.Response { Status = "Error", Message = "User Failed to Create" });
                }
                //Add role to the user....

                await _userManager.AddToRoleAsync(user, role);

                ////Add Token to Verify the email....
                //var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                // spocuserid= user.Id;
               
                return StatusCode(StatusCodes.Status200OK,
                new Domain.Entities.Response { Status = "Success", Message = $"User created {user.Id}  SuccessFully" });

                //return CreatedAtAction("GetUserId", new { id = registerUser.Id }, registerUser);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                new Domain.Entities.Response { Status = "Error", Message = "This Role Does not Exist." });
            }
        }
      
        [HttpPost("AddPartner")]
        public async Task<ActionResult<int>> Create(CreatePartnerCommand command)
        {
          
            return await Mediator.Send(command);
        }
        //[HttpPost]
        //public async Task<ActionResult<Partner>> PostPaymentDetail(Partner partnerDetail)
        //{
        //    _fullStackDbContext.Partners.Add(partnerDetail);
        //    await _fullStackDbContext.SaveChangesAsync();

        //    return CreatedAtAction("GetPartnerDetail", new { id = partnerDetail.PartnerID }, partnerDetail);
        //}
        [HttpGet("{email}")]
        public async Task<string> GetUserNameAsync(string email)
        {
            var user = await _userManager.Users.FirstAsync(u => u.Email == email);

            return user.Id;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, UpdatePartnerCommand command)
        {
            if (id != command.PartnerID)
            {
                return BadRequest();
            }
            await Mediator.Send(command);
            return NoContent();
        }
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeletePartner([FromRoute] Guid id)
        {
            var partner = await _fullStackDbContext.Partners.FindAsync(id);
            var pskill= _fullStackDbContext.PartnerSkills.FirstOrDefault(e => e.PartnerID == id);
            if (partner == null)
            {
                return NotFound();
            }
            _fullStackDbContext.PartnerSkills.Remove(pskill);
            _fullStackDbContext.Partners.Remove(partner);
            await _fullStackDbContext.SaveChangesAsync();
            return Ok();
        }
       
    }
}
