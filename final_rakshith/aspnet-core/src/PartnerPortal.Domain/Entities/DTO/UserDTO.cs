using System;
using System.Collections.Generic;

namespace PartnerPortal.Domain.Entities.DTO
{
    public class UserDTO
    {
        public UserDTO(string? id,string? userName, string? email, string? phoneNumber, List<string> roles)
        {
            Id = id;
            UserName = userName;
			Email = email;
            PhoneNumber = phoneNumber;
            Roles = roles;
        }
       public string Id { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<string> Roles { get; set; }
    }

}