using CuentaMovimientosService.Data;
using CuentaMovimientosService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CuentaMovimientosService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovimientosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MovimientosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movimiento>>> GetMovimientos()
        {
            return await _context.Movimientos.Include(m => m.Cuenta).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Movimiento>> GetMovimiento(int id)
        {
            var movimiento = await _context.Movimientos.Include(m => m.Cuenta)
                                                        .FirstOrDefaultAsync(m => m.Id == id);

            if (movimiento == null)
            {
                return NotFound();
            }

            return movimiento;
        }

        [HttpPost]
        public async Task<ActionResult<Movimiento>> PostMovimiento(Movimiento movimiento)
        {
            var cuenta = await _context.Cuentas.FindAsync(movimiento.CuentaId);

            if (cuenta == null)
            {
                return BadRequest("Cuenta no encontrada");
            }

            if (movimiento.TipoMovimiento.ToLower() == "retiro" && cuenta.SaldoInicial < movimiento.Valor)
            {
                return BadRequest("Saldo no disponible");
            }

            cuenta.SaldoInicial += movimiento.TipoMovimiento.ToLower() == "deposito" ? movimiento.Valor : -movimiento.Valor;
            movimiento.Saldo = cuenta.SaldoInicial;

            _context.Movimientos.Add(movimiento);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMovimiento", new { id = movimiento.Id }, movimiento);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovimiento(int id)
        {
            var movimiento = await _context.Movimientos.FindAsync(id);
            if (movimiento == null)
            {
                return NotFound();
            }

            _context.Movimientos.Remove(movimiento);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
