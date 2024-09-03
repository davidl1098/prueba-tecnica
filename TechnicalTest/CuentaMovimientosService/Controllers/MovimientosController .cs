using CuentaMovimientosService.Models;
using CuentaMovimientosService.Repositories.Implementations;
using CuentaMovimientosService.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CuentaMovimientosService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovimientosController : ControllerBase
    {
        private readonly IMovimientoRepository _movimientoRepository;
        private readonly ICuentaRepository _cuentaRepository;

        public MovimientosController(IMovimientoRepository movimientoRepository, ICuentaRepository cuentaRepository)
        {
            _movimientoRepository = movimientoRepository;
            _cuentaRepository = cuentaRepository;
        }

        // GET: api/Movimientos
        [HttpGet]
        public async Task<IActionResult> GetMovimientos()
        {
            var movimientos = await _movimientoRepository.GetAllMovimientosAsync();
            return Ok(movimientos);
        }

        // GET: api/Movimientos/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovimiento(int id)
        {
            var movimiento = await _movimientoRepository.GetMovimientoByIdAsync(id);
            if (movimiento == null)
            {
                return NotFound();
            }
            return Ok(movimiento);
        }

        // POST: api/Movimientos
        [HttpPost]
        public async Task<IActionResult> PostMovimiento(Movimiento movimiento)
        {
            // Obtener la cuenta asociada
            var cuenta = await _cuentaRepository.GetCuentaByIdAsync(movimiento.CuentaId);
            if (cuenta == null)
            {
                return NotFound("Cuenta no encontrada.");
            }

            // Verificar si el movimiento es un retiro y si hay saldo suficiente
            if (movimiento.Valor < 0 && cuenta.SaldoInicial + movimiento.Valor < 0)
            {
                return BadRequest("Saldo no disponible");
            }

            // Actualizar el saldo de la cuenta
            cuenta.SaldoInicial += movimiento.Valor;

            // Registrar el movimiento
            await _movimientoRepository.AddMovimientoAsync(movimiento);

            // Actualizar la cuenta en la base de datos
            await _cuentaRepository.UpdateCuentaAsync(cuenta);

            return CreatedAtAction(nameof(GetMovimiento), new { id = movimiento.Id }, movimiento);
        }


        // PUT: api/Movimientos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovimiento(int id, Movimiento movimiento)
        {
            if (id != movimiento.Id)
            {
                return BadRequest();
            }

            await _movimientoRepository.UpdateMovimientoAsync(movimiento);

            return NoContent();
        }

        // DELETE: api/Movimientos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovimiento(int id)
        {
            var movimiento = await _movimientoRepository.GetMovimientoByIdAsync(id);
            if (movimiento == null)
            {
                return NotFound();
            }

            await _movimientoRepository.DeleteMovimientoAsync(id);

            return NoContent();
        }
    }
}
