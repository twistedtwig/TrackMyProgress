using System;
using System.Collections.Generic;

namespace IdentityModelEntities
{
    public class IdentityUserEntity
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public ICollection<IdentityUserRoleEntity> Roles { get; set; }
        public ICollection<IdentityUserClaimEntity> Claims { get; set; }
        public ICollection<IdentityUserLoginEntity> Logins { get; set; }

        public IdentityUserEntity()
        {
            Id = Guid.NewGuid().ToString();
            Claims = new List<IdentityUserClaimEntity>();
            Roles = new List<IdentityUserRoleEntity>();
            Logins = new List<IdentityUserLoginEntity>();
        }

        public IdentityUserEntity(string userName) : this()
        {
            UserName = userName;
        }


    }
}
