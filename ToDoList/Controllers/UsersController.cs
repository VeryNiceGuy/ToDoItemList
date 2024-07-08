using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Models;
using ToDoList.Dto;

namespace ToDoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ToDoListContext _context;

        public UsersController(ToDoListContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserRead>>> GetUsers()
        {
            return await _context.Users
                .Select(u => UserRead.FromUser(u))
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserRead>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return UserRead.FromUser(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, string userName)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
            if(user == null)
            {
                return NotFound();
            }
            user.Name = userName;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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
        public async Task<ActionResult<UserRead>> PostUser(string userName)
        {
            var user = new User { Name = userName };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, UserRead.FromUser(user));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
