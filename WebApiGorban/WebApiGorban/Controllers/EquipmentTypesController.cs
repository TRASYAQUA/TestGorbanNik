using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiGorban.Models;

namespace WebApiGorban.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentTypesController : ControllerBase
    {
        private readonly EquipmentWarehouseContext _context;

        public EquipmentTypesController(EquipmentWarehouseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquipmentType>>> GetEquipmentTypes()
        {
            return await _context.EquipmentTypes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EquipmentType>> GetEquipmentType(Guid id)
        {
            var equipmentType = await _context.EquipmentTypes.FindAsync(id);

            if (equipmentType == null)
            {
                return NotFound();
            }

            return equipmentType;
        }

        [HttpPost]
        public async Task<ActionResult<EquipmentType>> PostEquipmentType(EquipmentType equipmentType)
        {
            equipmentType.Id = Guid.NewGuid();
            equipmentType.CreatedAt = DateTime.UtcNow;
            equipmentType.UpdatedAt = DateTime.UtcNow;

            _context.EquipmentTypes.Add(equipmentType);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEquipmentType), new { id = equipmentType.Id }, equipmentType);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEquipmentType(Guid id, EquipmentType equipmentType)
        {
            if (id != equipmentType.Id)
            {
                return BadRequest();
            }

            equipmentType.UpdatedAt = DateTime.UtcNow;

            _context.Entry(equipmentType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EquipmentTypeExists(id))
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEquipmentType(Guid id)
        {
            var equipmentType = await _context.EquipmentTypes.FindAsync(id);
            if (equipmentType == null)
            {
                return NotFound();
            }

            _context.EquipmentTypes.Remove(equipmentType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EquipmentTypeExists(Guid id)
        {
            return _context.EquipmentTypes.Any(e => e.Id == id);
        }
    }
}