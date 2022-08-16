using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarStore_API.Data;
using CarStore_API.Model;

namespace CarStore_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CarsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Cars
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Car>>> GetCars()
        {
            if (_context.Cars == null)
            {
                return NotFound();
            }
            return Ok(await _context.Cars.ToListAsync());
        }

        // GET: api/Cars/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Car>> GetCar(int id)
        {
            if (_context.Cars == null)
            {
                return NotFound();
            }
            var car = await _context.Cars.FindAsync(id);

            if (car == null)
            {
                return NotFound();
            }

            return Ok(car);
        }

        [HttpGet("filter")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Car>>> GetCarsByFilter([FromQuery] string? condition = null, [FromQuery] string? bodytype = null, [FromQuery] string? fuel = null)
        {
            if (_context.Cars is null)
            {
                return NotFound();
            }
            var cars = _context.Cars.AsQueryable();

            if (!string.IsNullOrEmpty(condition))
            {
                cars = cars.Where(c => c.Condition.ToLower().Contains(condition.ToLower()));
            }
            if (!string.IsNullOrEmpty(bodytype))
            {
                cars = cars.Where(c => c.BodyType.ToLower().Contains(bodytype.ToLower()));
            }
            if (!string.IsNullOrEmpty(fuel))
            {
                cars = cars.Where(c => c.Fuel.ToLower().Contains(fuel.ToLower()));
            }

            if (!await cars.AnyAsync())
            {
                return NotFound();
            }

            return Ok(await cars.AsNoTracking().ToListAsync());
        }

        // PUT: api/Cars/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCar(int id, Car car)
        {
            if (id != car.Id)
            {
                return BadRequest();
            }

           // car.CreatedDate = _context.Cars.FirstOrDefault(c => c.Id == id).CreatedDate;

            //_context.Entry(car.CreatedDate).State = EntityState.Unchanged;
            //car.UpdateDate = DateTime.Now;
            _context.Entry(car).State = EntityState.Modified;
            _context.Entry(car).Property(nameof(Car.CreatedDate)).IsModified = false;
            _context.Entry(car).Property(nameof(Car.UpdateDate)).CurrentValue = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Cars
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Car>> CreateCar(Car car)
        {
            if (_context.Cars == null)
            {
                return Problem("Entity set 'AppDbContext.Cars'  is null.");
            }

            car.CreatedDate = car.UpdateDate = DateTime.Now;

            _context.Cars.Add(car);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCar", new { id = car.Id }, car);
        }

        // DELETE: api/Cars/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCar(int id)
        {
            if (_context.Cars == null)
            {
                return NotFound();
            }
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CarExists(int id)
        {
            return (_context.Cars?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
