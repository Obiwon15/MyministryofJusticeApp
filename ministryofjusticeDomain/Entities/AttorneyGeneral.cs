using ministryofjusticeDomain.IdentityEntities;


namespace ministryofjusticeDomain.Entities
{
    public class AttorneyGeneral
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
