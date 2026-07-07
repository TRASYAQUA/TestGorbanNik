using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiGorban.Models;

namespace WebApiGorban.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentItemsController : ControllerBase
    {
        private readonly EquipmentWarehouseContext _context;

        public EquipmentItemsController(EquipmentWarehouseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquipmentItem>>> GetEquipmentItems()
        {
            return await _context.EquipmentItems
                .Include(e => e.Manufacturer)
                .Include(e => e.EquipmentType)
                .Include(e => e.Country)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EquipmentItem>> GetEquipmentItem(Guid id)
        {
            var equipmentItem = await _context.EquipmentItems
                .Include(e => e.Manufacturer)
                .Include(e => e.EquipmentType)
                .Include(e => e.Country)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (equipmentItem == null)
            {
                return NotFound();
            }

            return equipmentItem;
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<EquipmentItem>>> GetFiltered(
            [FromQuery] Guid? manufacturerId,
            [FromQuery] Guid? equipmentTypeId,
            [FromQuery] Guid? countryId,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] string? search)
        {
            var query = _context.EquipmentItems
                .Include(e => e.Manufacturer)
                .Include(e => e.EquipmentType)
                .Include(e => e.Country)
                .AsQueryable();

            if (manufacturerId.HasValue)
            {
                query = query.Where(e => e.ManufacturerId == manufacturerId.Value);
            }

            if (equipmentTypeId.HasValue)
            {
                query = query.Where(e => e.EquipmentTypeId == equipmentTypeId.Value);
            }

            if (countryId.HasValue)
            {
                query = query.Where(e => e.CountryId == countryId.Value);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(e => e.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(e => e.Price <= maxPrice.Value);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchLower = search.ToLower();
                query = query.Where(e =>
                    e.Model.ToLower().Contains(searchLower) ||
                    e.Manufacturer.Name.ToLower().Contains(searchLower) ||
                    e.EquipmentType.Name.ToLower().Contains(searchLower));
            }

            return await query.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<EquipmentItem>> PostEquipmentItem(EquipmentItem equipmentItem)
        {
            equipmentItem.Id = Guid.NewGuid();
            equipmentItem.CreatedAt = DateTime.UtcNow;
            equipmentItem.UpdatedAt = DateTime.UtcNow;

            _context.EquipmentItems.Add(equipmentItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEquipmentItem), new { id = equipmentItem.Id }, equipmentItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEquipmentItem(Guid id, EquipmentItem equipmentItem)
        {
            if (id != equipmentItem.Id)
            {
                return BadRequest();
            }

            equipmentItem.UpdatedAt = DateTime.UtcNow;

            _context.Entry(equipmentItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EquipmentItemExists(id))
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
        public async Task<IActionResult> DeleteEquipmentItem(Guid id)
        {
            var equipmentItem = await _context.EquipmentItems.FindAsync(id);
            if (equipmentItem == null)
            {
                return NotFound();
            }

            _context.EquipmentItems.Remove(equipmentItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EquipmentItemExists(Guid id)
        {
            return _context.EquipmentItems.Any(e => e.Id == id);
        }
    }
}