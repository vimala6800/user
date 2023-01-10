using System.ComponentModel.DataAnnotations;

namespace PartnerPortal.Domain.Entities.BindingModel
{
    public class LoginBindingModel
    {
       [Required]
        public string Email { get; set; }
         [Required]
        public string Password { get; set; }
    }
}