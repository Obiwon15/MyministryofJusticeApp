using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ministryofjusticeDomain.Entities;

namespace ministryofjusticeDomain.IdentityEntities.EntityConfigurations
{
    public class AttorneyGeneralConfiguration : EntityTypeConfiguration<AttorneyGeneral>
    {
        public AttorneyGeneralConfiguration()
        {
            HasKey(d => d.Id);

            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}