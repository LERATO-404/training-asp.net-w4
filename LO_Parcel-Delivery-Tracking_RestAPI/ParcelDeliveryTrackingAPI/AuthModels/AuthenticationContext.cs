using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace ParcelDeliveryTrackingAPI.AuthModels
{
    public class AuthenticationContext : IdentityDbContext
    {
        public AuthenticationContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);

            var roleId_1 = Guid.NewGuid().ToString();
            var userId_1 = Guid.NewGuid().ToString();

            var roleId_2 = Guid.NewGuid().ToString();
            var userId_2 = Guid.NewGuid().ToString();

            var roleId_3 = Guid.NewGuid().ToString();
            var userId_3 = Guid.NewGuid().ToString();

            var roleId_4 = Guid.NewGuid().ToString();
            var userId_4 = Guid.NewGuid().ToString();


            #region "Seed Data"
            builder.Entity<IdentityRole>().HasData(
                new { Id = roleId_1, Name = "Administrator", NormalizedName = "ADMINISTRATOR" },
                new { Id = roleId_2, Name = "Manager", NormalizedName = "MANAGER" },
                new { Id = roleId_3, Name = "Driver", NormalizedName = "DRIVER" },
                new { Id = roleId_4, Name = "User", NormalizedName = "USER" }
             );


            //create Administrator user
            var AdminUser = new ApplicationUser
            {
                Id = userId_1,
                Email = "Admin_1@pdt.gmail.com",
                EmailConfirmed = true,
                FirstName = "AdminF",
                LastName = "AdminL",
                UserName = "AdminOne",
                NormalizedUserName = "ADMINONE",

            };

            //set user password
            PasswordHasher<ApplicationUser> adminph = new PasswordHasher<ApplicationUser>();
            AdminUser.PasswordHash = adminph.HashPassword(AdminUser, "Admin123");

            //seed user
            builder.Entity<ApplicationUser>().HasData(AdminUser);


            //set user role to admin
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = roleId_1,
                UserId = userId_1
            });

            //create Manager user
            var ManagerUser = new ApplicationUser
            {
                Id = userId_2,
                Email = "Manager_1@pdt.gmail.com",
                EmailConfirmed = true,
                FirstName = "ManF",
                LastName = "ManL",
                UserName = "ManagerOne",
                NormalizedUserName = "MANAGERONE"
            };

            //set user password
            PasswordHasher<ApplicationUser> managerph = new PasswordHasher<ApplicationUser>();
            ManagerUser.PasswordHash = managerph.HashPassword(ManagerUser, "Manager123");

            //seed user
            builder.Entity<ApplicationUser>().HasData(ManagerUser);

            //set user role to admin
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = roleId_2,
                UserId = userId_2
            });

            //create Driver user
            var DriverUser = new ApplicationUser
            {
                Id = userId_3,
                Email = "Driver@pdt.gmail.com",
                EmailConfirmed = true,
                FirstName = "DriverF",
                LastName = "DriverL",
                UserName = "DriverOne",
                NormalizedUserName = "DRIVEONE"
            };

            //set user password
            PasswordHasher<ApplicationUser> driverph = new PasswordHasher<ApplicationUser>();
            DriverUser.PasswordHash = driverph.HashPassword(DriverUser, "Driver123");

            //seed user
            builder.Entity<ApplicationUser>().HasData(DriverUser);

            //set user role to admin
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = roleId_3,
                UserId = userId_3
            });

            //create Driver user
            var User = new ApplicationUser
            {
                Id = userId_4,
                Email = "User@pdt.gmail.com",
                EmailConfirmed = true,
                FirstName = "UserF",
                LastName = "UserL",
                UserName = "UserOne",
                NormalizedUserName = "USERONE"
            };

            //set user password
            PasswordHasher<ApplicationUser> userph = new PasswordHasher<ApplicationUser>();
            DriverUser.PasswordHash = userph.HashPassword(User, "User123");

            //seed user
            builder.Entity<ApplicationUser>().HasData(User);

            //set user role to admin
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = roleId_4,
                UserId = userId_4
            });
            #endregion

        }
    }

}
