using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ministryofjusticeDomain.Entities;

namespace ministryofjusticeWebUi.Migrations
{
    using System.Data.Entity.Migrations;
    using ministryofjusticeDomain.IdentityEntities;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            //Seeding of Roles  

            context.Departments.AddOrUpdate(department => department.Id,
                new Department()
                {
                    Id = 1,
                    DepartmentName = "Criminal"
                },
                new Department()
                {
                    Id = 2,
                    DepartmentName = "Civil"
                }
            );

            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            // Seeded roles list
            IList<IdentityRole> roles = new List<IdentityRole>()
            {
                new IdentityRole() {Name = "System Administrator"},
                new IdentityRole() {Name = "Attorney General"},
                new IdentityRole() {Name = "Director of Department"},
                new IdentityRole() {Name = "Lawyer"}
            };

            // Creates new roles in database
            foreach (var role in roles)
            {
                if (!roleManager.RoleExists(role.Name))
                    roleManager.Create(role);
            }

            var systemAdmin = new ApplicationUser()
            {
                UserName = "systemadmin@ministryofjustice.com",
                Email = "systemadmin@ministryofjustice.com",
                EmailConfirmed = true
            };

            //Creates a User and assign it to the role of  System Admin
            var result = manager.Create(systemAdmin, "ZAcwx5@");
            if (result.Succeeded) manager.AddToRole(systemAdmin.Id, "System Administrator");

            var attorney = new ApplicationUser()
            {
                UserName = "attorneygeneral@ministryofjustice.com",
                Email = "attorneygeneral@ministryofjustice.com",
                EmailConfirmed = true
            };

            //Creates a User and assign it to the role of Attorney General

            result = manager.Create(attorney, "ASdf:lkj");
            if (result.Succeeded) manager.AddToRole(attorney.Id, "Attorney General");
        }
    }
}
