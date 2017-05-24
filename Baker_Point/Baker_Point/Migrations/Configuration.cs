namespace Baker_Point.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Security;
    using WebMatrix.WebData;

    internal sealed class Configuration : DbMigrationsConfiguration<Baker_Point.Models.BPDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Baker_Point.Models.BPDbContext context)
        {
            SeedMembership();
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }

        private void SeedMembership()
        {
            WebSecurity.InitializeDatabaseConnection("DefaultConnection","UserProfile","UserId","UserName",autoCreateTables:true);

            var roles = (SimpleRoleProvider)Roles.Provider;
            var membership = (SimpleMembershipProvider)Membership.Provider;

            if (!roles.RoleExists("Admin"))
            {
                roles.CreateRole("Admin");
            }

            if (membership.GetUser("Administrator", false) == null)
            {
                membership.CreateUserAndAccount("Administrator","baker123");
            }

            if (!roles.GetRolesForUser("Administrator").Contains("Admin"))
            {
                roles.AddUsersToRoles(new[] {"Administrator"},new[] {"admin"});
            }

            if (membership.GetUser("Druid", false) == null)
            {
                membership.CreateUserAndAccount("Druid", "123");
            }

            if (!roles.GetRolesForUser("Druid").Contains("Admin"))
            {
                roles.AddUsersToRoles(new[] { "Druid" }, new[] { "admin" });
            }

            if (membership.GetUser("Jason", false) == null)
            {
                membership.CreateUserAndAccount("Jason", "123");
            }

            if (!roles.GetRolesForUser("Jason").Contains("Admin"))
            {
                roles.AddUsersToRoles(new[] { "Jason" }, new[] { "admin" });
            }
        }
    }
}
