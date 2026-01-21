using Microsoft.EntityFrameworkCore;
using Car.Core.Domain;

namespace Car.Data
{
    public class CarDbContext : DbContext
    {
        public CarDbContext(DbContextOptions<CarDbContext> options)
            : base(options)
        {
        }

        public DbSet<Car.Core.Domain.Car> Cars { get; set; }
    }
}