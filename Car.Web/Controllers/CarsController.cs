using Car.Core.ServiceInterface;
using Microsoft.AspNetCore.Mvc;

namespace Car.Web.Controllers
{
    public class CarsController : Controller
    {
        private readonly ICarServices _carServices;

        public CarsController(ICarServices carServices)
        {
            _carServices = carServices;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _carServices.GetAll();
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var car = await _carServices.Get(id);
            if (car == null)
                return NotFound();

            return View(car);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Car.Core.Domain.Car car, IFormFile? imageFile)
        {
            ModelState.Remove("Id");

            if (!ModelState.IsValid)
                return View(car);

            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot/images/cars");

                Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
                var fullPath = Path.Combine(uploadsFolder, fileName);

                using var stream = new FileStream(fullPath, FileMode.Create);
                await imageFile.CopyToAsync(stream);

                car.ImagePath = "/images/cars/" + fileName;
            }

            car.CreatedAt = DateTime.UtcNow;
            car.ModifiedAt = DateTime.UtcNow;

            await _carServices.Create(car);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var car = await _carServices.Get(id);
            if (car == null)
                return NotFound();

            return View(car);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(
            Car.Core.Domain.Car car,
            IFormFile? imageFile)
        {
           
            ModelState.Remove("Id");

            if (!ModelState.IsValid)
                return View(car);

            var existingCar = await _carServices.Get(car.Id);
            if (existingCar == null)
                return NotFound();

            if (imageFile != null && imageFile.Length > 0)
            {
                if (!string.IsNullOrEmpty(existingCar.ImagePath))
                {
                    var oldPath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        existingCar.ImagePath.TrimStart('/'));

                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                var uploadsFolder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot/images/cars");

                Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
                var newPath = Path.Combine(uploadsFolder, fileName);

                using var stream = new FileStream(newPath, FileMode.Create);
                await imageFile.CopyToAsync(stream);

                existingCar.ImagePath = "/images/cars/" + fileName;
            }

            existingCar.Make = car.Make;
            existingCar.Model = car.Model;
            existingCar.Year = car.Year;
            existingCar.Price = car.Price;
            existingCar.IsUsed = car.IsUsed;
            existingCar.ModifiedAt = DateTime.UtcNow;

            await _carServices.Update(existingCar);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveImage(Guid id)
        {
            var car = await _carServices.Get(id);
            if (car == null)
                return NotFound();

            if (!string.IsNullOrEmpty(car.ImagePath))
            {
                var fullPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    car.ImagePath.TrimStart('/'));

                if (System.IO.File.Exists(fullPath))
                    System.IO.File.Delete(fullPath);

                car.ImagePath = null;
                car.ModifiedAt = DateTime.UtcNow;

                await _carServices.Update(car);
            }

            return RedirectToAction(nameof(Edit), new { id });
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var car = await _carServices.Get(id);

            if (car != null && !string.IsNullOrEmpty(car.ImagePath))
            {
                var fullPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    car.ImagePath.TrimStart('/'));

                if (System.IO.File.Exists(fullPath))
                    System.IO.File.Delete(fullPath);
            }

            await _carServices.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var car = await _carServices.Get(id);
            if (car == null)
                return NotFound();

            return View(car);
        }
    }
}