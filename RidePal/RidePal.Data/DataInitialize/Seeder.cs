using Microsoft.EntityFrameworkCore;
using RidePal.Data.Models;
using System.Collections.Generic;

namespace RidePal.Data.DataInitialize
{
    public static class Seeder
    {
        public static void Seed(this ModelBuilder db)
        {
            var roles = new List<Role>()
            {
                new Role
                {
                    Id = 1,
                    Name = "Admin"
                },
                new Role
                {
                    Id = 2,
                    Name = "User"
                }
            };

            db.Entity<Role>().HasData(roles);
        }
    }
}
