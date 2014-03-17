namespace IdentityModels
{
    public class IdentityUserClaim
    {
        public int Id { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public IdentityUser User { get; set; }
    }
}