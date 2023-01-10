using System.Collections.Generic;

namespace PartnerPortal.Domain.Entities.BindingModel
{
	public class AddUpdateRegisterUserBindingModel
	{   public string Id { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
		public List<string> Roles { get; set; }

		public string? ClientURI { get;set;}
	}
}