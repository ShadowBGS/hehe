using asp2.Entities;
using asp2.Models;
using Microsoft.EntityFrameworkCore;
namespace asp2.DbContexts
{
    public class CityInfoContext : DbContext 
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<PointOfInterest> PointsOfInterest { get; set; }
        public CityInfoContext(DbContextOptions<CityInfoContext> options) : base(options) 
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>().HasData(

                new City("Yaba")
                {
                    Id = 1,
                    Description = "The Heart of Mainland"
                },
                new City("Surulere")
                {
                    Id = 2,
                    Description = "Wizkid's Home"
                },
                new City("Ikeja")
                {
                    Id = 3,
                    Description = "The business center of Lagos"
                }

                );

            modelBuilder.Entity<PointOfInterest>().HasData(

                 new PointOfInterest("Unilag")
                 {
                     Id = 1,
                     CityId = 1,
                     Description = "Main University in Lagos"
                 }, new PointOfInterest ("Under-Bridge")
                 {
                     Id = 2,
                     CityId = 1,
                     Description = "A staple"
                 },
                 new PointOfInterest("Masha")
                 {
                     Id = 3,
                     CityId = 2,
                     Description = "The place with the roundabout"
                 }, new PointOfInterest("Itire")
                 {
                     Id = 4,
                     CityId = 2,
                     Description = "known for its bus-stop"
                 },
                 new PointOfInterest("Idk Road")
                 {
                     Id = 5,
                     CityId = 3,
                     Description = "Has many factories"
                 }, new PointOfInterest(" Another Road")
                 {
                     Id = 6,
                     CityId = 3,
                     Description = "Full of Churches"
                 }

                );
            base.OnModelCreating(modelBuilder); 
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite("connectionstring");
        //    base.OnConfiguring(optionsBuilder);
        //}
    }

}

    

