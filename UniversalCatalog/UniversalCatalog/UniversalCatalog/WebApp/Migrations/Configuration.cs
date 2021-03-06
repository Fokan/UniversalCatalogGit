namespace WebApp.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models;

    internal sealed class Configuration : DbMigrationsConfiguration<WebApp.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WebApp.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            string[] Roles = { "admin", "moderator", "user" };
            IdentityResult roleResult;
            foreach (var role in Roles)
            {
                if (!RoleManager.RoleExists(role))
                {
                    roleResult = RoleManager.Create(new IdentityRole(role));
                }
            }

            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            UserManager.AddToRole("6971465c-d6bd-4f17-bc4b-26a724099750", "admin");

        }
    }
}
