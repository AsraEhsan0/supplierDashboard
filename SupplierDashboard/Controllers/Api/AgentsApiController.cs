using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupplierDashboard.Data;
using SupplierDashboard.Models;
using SupplierDashboard.Models.Entities;

namespace SupplierDashboard.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AgentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Agent>>> GetAgents()
        {
            return await _context.Agents.Include(a => a.Agency).ToListAsync();
        }

        // GET: api/agents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Agent>> GetAgent(string id)
        {
            var agent = await _context.Agents
                .Include(a => a.Agency)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (agent == null)
            {
                return NotFound();
            }

            return agent;
        }

        [HttpPost]
        public async Task<ActionResult<Agent>> PostAgent(Agent agent)
        {
            _context.Agents.Add(agent);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAgent), new { id = agent.Id }, agent);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAgent(string id, Agent agent)
        {
            if (id != agent.Id)
            {
                return BadRequest();
            }

            _context.Entry(agent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgentExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAgent(int id)
        {
            var agent = await _context.Agents.FindAsync(id);
            if (agent == null)
            {
                return NotFound();
            }

            _context.Agents.Remove(agent);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AgentExists(string id)
        {
            return _context.Agents.Any(e => e.Id == id);
        }
    }
}