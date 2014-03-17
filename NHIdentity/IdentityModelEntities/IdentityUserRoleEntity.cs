using System;

namespace IdentityModelEntities
{
    public class IdentityUserRoleEntity
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public Guid RoleId { get; set; }
    }
}
