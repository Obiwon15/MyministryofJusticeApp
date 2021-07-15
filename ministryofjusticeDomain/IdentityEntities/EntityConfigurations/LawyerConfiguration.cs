using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ministryofjusticeDomain.Entities;

namespace ministryofjusticeDomain.IdentityEntities.EntityConfigurations
{
    public class LawyerConfiguration : EntityTypeConfiguration<Lawyer>
    {
        public LawyerConfiguration()
        {
            HasKey(d => d.Id);

            Property(d => d.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
