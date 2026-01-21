using Car.ApplicationServices.Services;
using Car.Core.Domain;
using Car.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;
using Car.Core.ServiceInterface;

namespace Car.CarTest
{
    public class CarServiceTests
    {
        // Method for creating a test database context (virtual database)
        private CarDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<CarDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new CarDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        // 1. Verifies that a car is created

        [Fact]
        public async Task Create_ShouldAddCar_WhenInputIsValid()
        {
            // Arrange
            var context = GetDbContext();
            var service = new CarServices(context);
            var newCar = new Core.Domain.Car
            {
                Make = "TestBMW",
                Model = "X5",
                Year = 2022,
                Price = 50000,
                IsUsed = false
            };

            // Act
            var createdCar = await service.Create(newCar);

            // Assert
            Assert.NotNull(createdCar);
            Assert.NotEqual(Guid.Empty, createdCar.Id);
            // The ID should be generated automatically
            Assert.Equal("TestBMW", createdCar.Make);
        }

        // 2. Verifies that a car can be retrieved by ID

        [Fact]
        public async Task Get_ShouldReturnCar_WhenIdExists()
        {
            // Arrange
            var context = GetDbContext();
            var service = new CarServices(context);

            // First, we add a car to the database manually
            var carId = Guid.NewGuid();
            var existingCar = new Core.Domain.Car
            {
                Id = carId,
                Make = "Audi",
                Model = "A6",
                Year = 2020,
                Price = 30000,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };
            context.Cars.Add(existingCar);
            context.SaveChanges();

            // Act
            var result = await service.Get(carId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Audi", result.Make);
            Assert.Equal(carId, result.Id);
        }

        // 3. Verifies that the data is updated

        [Fact]
        public async Task Update_ShouldModifyCar_WhenCarExists()
        {
            // Arrange
            var context = GetDbContext();
            var service = new CarServices(context);

            var carId = Guid.NewGuid();
            var originalCar = new Core.Domain.Car
            {
                Id = carId,
                Make = "Toyota",
                Model = "Corolla",
                Year = 2010,
                Price = 5000,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };
            context.Cars.Add(originalCar);
            context.SaveChanges();

            // Modify the data
            originalCar.Price = 6000;
            originalCar.IsUsed = true;

            // Act
            var updatedCar = await service.Update(originalCar);

            // Assert
            Assert.Equal(6000, updatedCar.Price);
            Assert.True(updatedCar.IsUsed);
            // Check that the database has been updated as well
            var carInDb = await context.Cars.FindAsync(carId);
            Assert.Equal(6000, carInDb.Price);
        }

        // 4. Verifies that the car is deleted

        [Fact]
        public async Task Delete_ShouldRemoveCar_WhenIdExists()
        {
            // Arrange
            var context = GetDbContext();
            var service = new CarServices(context);

            var carId = Guid.NewGuid();
            var carToDelete = new Core.Domain.Car
            {
                Id = carId,
                Make = "Lada",
                Model = "Sedan",
                Year = 1990,
                Price = 500,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };
            context.Cars.Add(carToDelete);
            context.SaveChanges();

            // Act
            var deletedCar = await service.Delete(carId);

            // Assert
            Assert.NotNull(deletedCar); // The method returns the deleted object

            // Check that it no longer exists in the database
            var carInDb = await context.Cars.FindAsync(carId);
            Assert.Null(carInDb);
        }
    }
}