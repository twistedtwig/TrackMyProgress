using System;

namespace IdentityModels
{
    public class IdentityUserRole
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public Guid RoleId { get; set; }
        public IdentityRole Role { get; set; }
        public IdentityUser User { get; set; }
    }
}