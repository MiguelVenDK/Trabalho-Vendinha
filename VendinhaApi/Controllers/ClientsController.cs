using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VendinhaApi.Data;
using VendinhaApi.DTOs;
using VendinhaApi.Models;

namespace VendinhaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClientsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientResponseDto>>> GetClients(
            [FromQuery] string? name,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _context.Clients
                .Include(c => c.Debts)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(c => c.Name.ToLower().Contains(name.ToLower()));
            }

            var clientsWithDebt = await query
                .Select(c => new
                {
                    Client = c,
                    TotalDebt = c.Debts.Where(d => !d.IsPaid).Sum(d => d.Value)
                })
                .OrderByDescending(x => x.TotalDebt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var response = clientsWithDebt.Select(x => new ClientResponseDto
            {
                Id = x.Client.Id,
                Name = x.Client.Name,
                Cpf = x.Client.Cpf,
                BirthDate = x.Client.BirthDate,
                Age = x.Client.Age,
                Email = x.Client.Email,
                TotalDebt = x.TotalDebt
            });

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClientResponseDto>> GetClient(int id)
        {
            var client = await _context.Clients
                .Include(c => c.Debts)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (client == null)
                return NotFound(new { message = "Cliente não encontrado." });

            var totalDebt = client.Debts.Where(d => !d.IsPaid).Sum(d => d.Value);

            var response = new ClientResponseDto
            {
                Id = client.Id,
                Name = client.Name,
                Cpf = client.Cpf,
                BirthDate = client.BirthDate,
                Age = client.Age,
                Email = client.Email,
                TotalDebt = totalDebt
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ClientResponseDto>> CreateClient([FromBody] CreateClientDto dto)
        {
            if (!IsValidCpf(dto.Cpf))
                return BadRequest(new { message = "CPF inválido." });

            var existingClient = await _context.Clients.AnyAsync(c => c.Cpf == dto.Cpf);
            if (existingClient)
                return Conflict(new { message = "Já existe um cliente cadastrado com este CPF." });

            var client = new Client
            {
                Name = dto.Name,
                Cpf = dto.Cpf,
                BirthDate = dto.BirthDate,
                Email = dto.Email
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            var response = new ClientResponseDto
            {
                Id = client.Id,
                Name = client.Name,
                Cpf = client.Cpf,
                BirthDate = client.BirthDate,
                Age = client.Age,
                Email = client.Email,
                TotalDebt = 0
            };

            return CreatedAtAction(nameof(GetClient), new { id = client.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(int id, [FromBody] UpdateClientDto dto)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
                return NotFound(new { message = "Cliente não encontrado." });

            if (!IsValidCpf(dto.Cpf))
                return BadRequest(new { message = "CPF inválido." });

            var cpfConflict = await _context.Clients.AnyAsync(c => c.Cpf == dto.Cpf && c.Id != id);
            if (cpfConflict)
                return Conflict(new { message = "Já existe outro cliente cadastrado com este CPF." });

            client.Name = dto.Name;
            client.Cpf = dto.Cpf;
            client.BirthDate = dto.BirthDate;
            client.Email = dto.Email;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var client = await _context.Clients.Include(c => c.Debts).FirstOrDefaultAsync(c => c.Id == id);
            if (client == null)
                return NotFound(new { message = "Cliente não encontrado." });

            _context.Debts.RemoveRange(client.Debts);
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IsValidCpf(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf)) return false;
            cpf = new string(cpf.Where(char.IsDigit).ToArray());
            if (cpf.Length != 11) return false;

            if (new string(cpf[0], 11) == cpf) return false;

            int[] multiplier1 = [10, 9, 8, 7, 6, 5, 4, 3, 2];
            int[] multiplier2 = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];

            string tempCpf = cpf.Substring(0, 9);
            int sum = 0;

            for (int i = 0; i < 9; i++)
                sum += int.Parse(tempCpf[i].ToString()) * multiplier1[i];

            int remainder = sum % 11;
            remainder = remainder < 2 ? 0 : 11 - remainder;

            string digit = remainder.ToString();
            tempCpf += digit;
            sum = 0;

            for (int i = 0; i < 10; i++)
                sum += int.Parse(tempCpf[i].ToString()) * multiplier2[i];

            remainder = sum % 11;
            remainder = remainder < 2 ? 0 : 11 - remainder;
            digit += remainder.ToString();

            return cpf.EndsWith(digit);
        }
    }
}