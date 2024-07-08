using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Models;
using ToDoList.Dto;

namespace ToDoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrioritiesController : ControllerBase
    {
        private readonly ToDoListContext _context;

        public PrioritiesController(ToDoListContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PriorityRead>>> GetPriorities()
        {
            return await _context.Priorities
                .Select(p => PriorityRead.FromPriority(p))
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<PriorityRead>> PostPriority(int level)
        {
            var existingPriority = await _context.Priorities.FindAsync(level);
            if (existingPriority != null)
            {
                return Ok(PriorityRead.FromPriority(existingPriority));
            }

            var priority = new Priority { Level = level };
            _context.Priorities.Add(priority);
            await _context.SaveChangesAsync();

            return Ok(PriorityRead.FromPriority(priority));
        }

        [HttpDelete("{level}")]
        public async Task<IActionResult> DeletePriority(int level)
        {
            var priority = await _context.Priorities.FindAsync(level);
            if (priority == null)
            {
                return NotFound();
            }

            _context.Priorities.Remove(priority);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PriorityExists(int level)
        {
            return _context.Priorities.Any(e => e.Level == level);
        }
    }
}
