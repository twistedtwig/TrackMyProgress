
namespace IdentityModelEntities
{
    public class IdentityUserClaimEntity
    {
        public int Id { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public string UserId { get; set; }
    }
}
