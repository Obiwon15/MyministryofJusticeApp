using ministryofjusticeDomain.IdentityEntities;

namespace ministryofjusticeDomain.Interfaces.Repository
{
    public interface IProfileRepo
    {
        void UpdateProfile(ApplicationUser user);
    }
}
