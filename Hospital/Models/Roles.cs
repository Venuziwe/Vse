using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Hospital.Models
{
    public class AppDbInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            
            var role1 = new IdentityRole { Name = "reception" };
            var role2 = new IdentityRole { Name = "user" };

            
            roleManager.Create(role1);
            roleManager.Create(role2);

            
            var reception = new ApplicationUser { Email = "reception@mail.ru", UserName = "reception@mail.ru", PersonBirthDay=DateTime.Now };
            string password = "reception";
            var result = userManager.Create(reception, password);

            // если создание пользователя прошло успешно
            if (result.Succeeded)
            {
                // добавляем для пользователя роль
                userManager.AddToRole(reception.Id, role1.Name);
                userManager.AddToRole(reception.Id, role2.Name);
            }

            base.Seed(context);
        }
    }
}