using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VendinhaApi.Data;
using VendinhaApi.DTOs;
using VendinhaApi.Models;

namespace VendinhaApi.Controllers
{
    [ApiController]
    [Route("api/clients/{clientId}/debts")]
    public class DebtsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DebtsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DebtResponseDto>>> GetDebts(int clientId)
        {
            var clientExists = await _context.Clients.AnyAsync(c => c.Id == clientId);
            if (!clientExists)
                return NotFound(new { message = "Cliente não encontrado." });

            var debts = await _context.Debts
                .Where(d => d.ClientId == clientId)
                .OrderByDescending(d => d.CreatedAt)
                .Select(d => new DebtResponseDto
                {
                    Id = d.Id,
                    Value = d.Value,
                    IsPaid = d.IsPaid,
                    CreatedAt = d.CreatedAt,
                    PaidAt = d.PaidAt,
                    ClientId = d.ClientId
                })
                .ToListAsync();

            return Ok(debts);
        }

        [HttpPost]
        public async Task<ActionResult<DebtResponseDto>> CreateDebt(int clientId, [FromBody] CreateDebtDto dto)
        {
            var client = await _context.Clients
                .Include(c => c.Debts)
                .FirstOrDefaultAsync(c => c.Id == clientId);

            if (client == null)
                return NotFound(new { message = "Cliente não encontrado." });

            var hasOpenDebt = client.Debts.Any(d => !d.IsPaid);
            if (hasOpenDebt)
                return BadRequest(new { message = "O cliente já possui uma dívida em aberto. É necessário quitá-la antes de cadastrar uma nova." });

            var debt = new Debt
            {
                Value = dto.Value,
                IsPaid = false,
                CreatedAt = DateTime.UtcNow,
                ClientId = clientId
            };

            _context.Debts.Add(debt);
            await _context.SaveChangesAsync();

            var response = new DebtResponseDto
            {
                Id = debt.Id,
                Value = debt.Value,
                IsPaid = debt.IsPaid,
                CreatedAt = debt.CreatedAt,
                PaidAt = debt.PaidAt,
                ClientId = debt.ClientId
            };

            return CreatedAtAction(nameof(GetDebts), new { clientId = clientId }, response);
        }

        [HttpPatch("{debtId}/pay")]
        public async Task<IActionResult> MarkDebtAsPaid(int clientId, int debtId)
        {
            var debt = await _context.Debts
                .FirstOrDefaultAsync(d => d.Id == debtId && d.ClientId == clientId);

            if (debt == null)
                return NotFound(new { message = "Dívida não encontrada para este cliente." });

            if (debt.IsPaid)
                return BadRequest(new { message = "Esta dívida já está paga." });

            debt.IsPaid = true;
            debt.PaidAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}