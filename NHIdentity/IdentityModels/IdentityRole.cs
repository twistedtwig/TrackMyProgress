using System;
using Microsoft.AspNet.Identity;

namespace IdentityModels
{
    public class IdentityRole : IRole
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public IdentityRole() : this("")
        {
        }

        public IdentityRole(string roleName)
        {
            Id = Guid.NewGuid().ToString();
            Name = roleName;
        }
    }
}