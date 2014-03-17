namespace IdentityModels
{
    public class IdentityUserLogin
    {
        public int Id { get; set; }
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
    }
}