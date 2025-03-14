using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurfSwift.Entities;
using SurfSwift.Infra;

namespace SurfSwift.Api.Controllers
{
    [Route("api/actionscript")]
    [ApiController]
    public class ActionScriptController(SurfSwiftDbContext context) : ControllerBase
    {
        private readonly SurfSwiftDbContext _context = context;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var scripts = await _context.ActionScripts.ToListAsync();
            return Ok(scripts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var script = await _context.ActionScripts.FindAsync(id);
            if (script == null)
                return NotFound();

            return Ok(script);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ActionScript script)
        {
            script.DateAdded = DateTime.UtcNow;
            script.DateUpdated = DateTime.UtcNow;

            _context.ActionScripts.Add(script);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = script.Id }, script);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ActionScript script)
        {
            var existing = await _context.ActionScripts.FindAsync(id);
            if (existing == null)
                return NotFound();

            existing.Name = script.Name;
            existing.JsonScript = script.JsonScript;
            existing.DateUpdated = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _context.ActionScripts.FindAsync(id);
            if (existing == null)
                return NotFound();

            _context.ActionScripts.Remove(existing);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
