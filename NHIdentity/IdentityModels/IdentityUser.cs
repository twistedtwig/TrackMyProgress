using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;

namespace IdentityModels
{
    public class IdentityUser : IUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public ICollection<IdentityUserRole> Roles { get; set; }
        public ICollection<IdentityUserClaim> Claims { get; set; }
        public ICollection<IdentityUserLogin> Logins { get; set; }

        public IdentityUser()
        {
            Id = Guid.NewGuid().ToString();
            Claims = new List<IdentityUserClaim>();
            Roles =  new List<IdentityUserRole>();
            Logins = new List<IdentityUserLogin>();
        }

        public IdentityUser(string userName) : this()
        {
            UserName = userName;
        }


    }
}