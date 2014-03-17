
namespace IdentityModelEntities
{
    public class IdentityUserLoginEntity
    {
        public int Id { get; set; }
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public string UserId { get; set; }

    }
}
