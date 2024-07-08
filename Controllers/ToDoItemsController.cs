using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Dto;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoItemsController : ControllerBase
    {
        private readonly ToDoListContext _context;

        public ToDoItemsController(ToDoListContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoItemRead>>> GetToDoItems(bool? isComplete, int? level)
        {
            var result = _context.ToDoItems.AsQueryable();

            /*if (isComplete != null)
            {
                result = result.Where(t => t.IsCompleted == isComplete.Value);
            }

            if (level != null)
            {
                result = result.Where(t => t.Priority != null && t.Priority.Level == level.Value);
            }*/

            if(level != null)
            {
                var priority = await _context.Priorities.FirstOrDefaultAsync(p => p.Level == level);
                if(priority != null)
                {
                    result = priority.ToDoItems.AsQueryable();
                }
            }

            if (isComplete != null)
            {
                result = result.Where(t => t.IsCompleted == isComplete.Value);
            }

            return await result.Select(t => ToDoItemRead.FromToDoItem(t))
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoItemRead>> GetToDoItem(int id)
        {
            var toDoItem = await _context.ToDoItems.FindAsync(id);
            if (toDoItem == null)
            {
                return NotFound();
            }

            return ToDoItemRead.FromToDoItem(toDoItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutToDoItem(int id, ToDoItemWrite toDoItemWrite)
        {
            var toDoItem = await _context.ToDoItems.AsNoTracking().SingleOrDefaultAsync(t => t.Id == id);
            if (toDoItem == null)
            {
                return NotFound();
            }

            var toDoItemModified = await FromToDoItemWrite(toDoItemWrite);
            toDoItemModified.Id = id;
            _context.Update(toDoItemModified);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToDoItemExists(id))
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

        [HttpPost]
        public async Task<ActionResult<ToDoItem>> PostToDoItem(ToDoItemWrite toDoItemWrite)
        {
            var toDoItem = await FromToDoItemWrite(toDoItemWrite);
            _context.ToDoItems.Add(toDoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetToDoItem), new { id = toDoItem.Id }, ToDoItemRead.FromToDoItem(toDoItem));
        }

        [HttpPost("{id}/AssignUser")]
        public async Task<IActionResult> AssignUser(int id, int? userId)
        {
            var toDoItem = await _context.ToDoItems.FindAsync(id);
            var user = userId != null ? await _context.Users.FindAsync(userId) : null;

            if (toDoItem == null)
            {
                return NotFound();
            }

            toDoItem.User = user;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("{id}/AssignPriority")]
        public async Task<IActionResult> AssignPriority(int id, int? level)
        {
            var toDoItem = await _context.ToDoItems.FindAsync(id);
            var priority = level != null ? await _context.Priorities.FindAsync(level) : null;

            if (toDoItem == null)
            {
                return NotFound();
            }

            toDoItem.Priority = priority;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoItem(int id)
        {
            var toDoItem = await _context.ToDoItems.FindAsync(id);
            if (toDoItem == null)
            {
                return NotFound();
            }

            _context.ToDoItems.Remove(toDoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ToDoItemExists(int id)
        {
            return _context.ToDoItems.Any(e => e.Id == id);
        }

        private async Task<ToDoItem> FromToDoItemWrite(ToDoItemWrite toDoItemWrite)
        {
            return new ()
            {
                Title = toDoItemWrite.Title,
                Description = toDoItemWrite.Description,
                IsCompleted = toDoItemWrite.IsCompleted,
                DueDate = toDoItemWrite.DueDate,
                Priority = toDoItemWrite.Level != null ? await _context.Priorities.SingleOrDefaultAsync(p => p.Level == toDoItemWrite.Level.Value) : null,
                User = toDoItemWrite.UserId != null ? await _context.Users.SingleOrDefaultAsync(p => p.Id == toDoItemWrite.UserId.Value) : null
            };
        }
    }
}
