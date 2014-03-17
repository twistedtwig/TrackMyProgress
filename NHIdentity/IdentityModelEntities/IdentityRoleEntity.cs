
using System;

namespace IdentityModelEntities
{
    public class IdentityRoleEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public IdentityRoleEntity() : this("")
        {
        }

        public IdentityRoleEntity(string roleName)
        {
            Name = roleName;
        }
    }
}
