using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using ministryofjusticeDomain.Entities;
using ministryofjusticeDomain.IdentityEntities;
using ministryofjusticeDomain.IdentityEntities.EntityConfigurations;

namespace ministryofjusticeWebUi.Data
{

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Department> Departments { get; set; }
        public DbSet<Lawyer> Lawyers { get; set; }
        public DbSet<DepartmentHead> DepartmentHeads { get; set; }
        public DbSet<AttorneyGeneral> AttorneyGenerals { get; set; }
        public DbSet<Case> Cases { get; set; }
        public DbSet<File> Files { get; set; }
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new DepartmentConfiguration());
            modelBuilder.Configurations.Add(new LawyerConfiguration());
            modelBuilder.Configurations.Add(new AttorneyGeneralConfiguration());
            base.OnModelCreating(modelBuilder);
        }

    }
}