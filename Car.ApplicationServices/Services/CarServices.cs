using Car.Core.Domain;
using Car.Core.ServiceInterface;
using Car.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Car.ApplicationServices.Services
{
    public class CarServices : ICarServices
    {
        private readonly CarDbContext _context;

        public CarServices(CarDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Car.Core.Domain.Car>> GetAll()
        {
            return await _context.Cars.ToListAsync();
        }

        public async Task<Car.Core.Domain.Car> Get(Guid id)
        {
            return await _context.Cars.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Car.Core.Domain.Car> Create(Car.Core.Domain.Car car)
        {
            car.Id = Guid.NewGuid();
            car.CreatedAt = DateTime.Now;
            car.ModifiedAt = DateTime.Now;

            await _context.Cars.AddAsync(car);
            await _context.SaveChangesAsync();

            return car;
        }

        public async Task<Car.Core.Domain.Car> Update(Car.Core.Domain.Car car)
        {
            car.ModifiedAt = DateTime.Now;

            _context.Cars.Update(car);
            await _context.SaveChangesAsync();

            return car;
        }

        public async Task<Car.Core.Domain.Car> Delete(Guid id)
        {
            var car = await _context.Cars.FirstOrDefaultAsync(x => x.Id == id);
            if (car == null) return null;

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();

            return car;
        }
    }
}