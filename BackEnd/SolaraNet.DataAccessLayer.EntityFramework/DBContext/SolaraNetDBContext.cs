using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SolaraNet.DataAccessLayer.Entities;

namespace SolaraNet.DataAccessLayer.EntityFramework.DBContext
{
    public class SolaraNetDBContext: IdentityDbContext<DBUser>
    {
        public DbSet<DBAdvertisment> Advertisments { get; set; }
        public DbSet<DBCategory> Categories { get; set; }
        public DbSet<DBComment> Comments { get; set; }

        public SolaraNetDBContext(DbContextOptions<SolaraNetDBContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            SeedIdentity(modelBuilder);
            SeedCategories(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        private void SeedCategories(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBCategory>()
                .HasData(new DBCategory
                {
                    Id = 1,
                    CategoryName = "Транспорт",
                }, new DBCategory
                {
                    Id = 2,
                    CategoryName = "Вещи"
                }, new DBCategory
                {
                    Id = 3,
                    CategoryName = "Недвижимость"
                }, new DBCategory
                {
                    Id = 4,
                    CategoryName = "Электроника"
                }, new DBCategory
                {
                    Id = 5,
                    CategoryName = "Автомобили",
                    ParentId = 1
                }, new DBCategory
                {
                    Id = 6,
                    CategoryName = "Мотоциклы",
                    ParentId = 1
                }, new DBCategory
                {
                    Id = 7,
                    CategoryName = "Спецтехника",
                    ParentId = 1
                }, new DBCategory
                {
                    Id = 8,
                    CategoryName = "Водный транспорт",
                    ParentId = 1
                }, new DBCategory
                {
                    Id = 9,
                    CategoryName = "Одежда, обувь",
                    ParentId = 2
                }, new DBCategory
                {
                    Id = 10,
                    CategoryName = "Аксессуары",
                    ParentId = 2
                }, new DBCategory
                {
                    Id = 11,
                    CategoryName = "Для детей",
                    ParentId = 2
                }, new DBCategory
                {
                    Id = 12,
                    CategoryName = "Прочее",
                    ParentId = 2
                }, new DBCategory
                {
                    Id = 17,
                    CategoryName = "Телефоны",
                    ParentId = 4
                }, new DBCategory
                {
                    Id = 18,
                    CategoryName = "Ноутбуки",
                    ParentId = 4
                }, new DBCategory
                {
                    Id = 19,
                    CategoryName = "Комплектующие",
                    ParentId = 4
                }, new DBCategory
                {
                    Id = 20,
                    CategoryName = "Электро-аксессуары",
                    ParentId = 4
                }, new DBCategory
                {
                    Id = 13,
                    CategoryName = "Квартиры",
                    ParentId = 3
                }, new DBCategory
                {
                    Id = 14,
                    CategoryName = "Дома",
                    ParentId = 3
                }, new DBCategory
                {
                    Id = 15,
                    CategoryName = "Комнаты",
                    ParentId = 3
                }, new DBCategory
                {
                    Id = 16,
                    CategoryName = "Гаражи",
                    ParentId = 3
                });
        }

        private void SeedIdentity(ModelBuilder modelBuilder)
        {
            var ADMIN_ROLE_ID = "d3300ca5-846f-4e6b-ac5f-1d3933115e67";
            var ADMIN_ID = "98b651ae-c9aa-4731-9996-57352d525f7e";
            var USER_ROLE_ID = "185230d2-58d8-4e29-aefd-a257fb82a150";

            modelBuilder.Entity<IdentityRole>(x =>
            {
                x.HasData(new IdentityRole
                {
                    Id = ADMIN_ROLE_ID,
                    Name = RoleConstants.AdminRole,
                    NormalizedName = "ADMIN"
                }, new IdentityRole
                {
                    Id = USER_ROLE_ID,
                    Name = RoleConstants.UserRole,
                    NormalizedName = "USER"
                });
            });

            var passwordHasher = new PasswordHasher<DBUser>();
            var adminUser = new DBUser
            {
                Id = ADMIN_ID,
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "zubenko@gmail.com",
                NormalizedEmail = "ZUBENKO@GMAIL.COM",
                PhoneNumber = "79781067697"
            };
            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "admin123");
            adminUser.EmailConfirmed = true;
            modelBuilder.Entity<DBUser>(x =>
            {
                x.HasData(adminUser);
            });

            modelBuilder.Entity<IdentityUserRole<string>>(x =>
            {
                x.HasData(new IdentityUserRole<string>
                {
                    RoleId = ADMIN_ROLE_ID,
                    UserId = ADMIN_ID
                });
            });
        }
    }
}