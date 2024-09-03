using CuentaMovimientosService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CuentaMovimientosService.Repositories.Interfaces
{
    public interface IMovimientoRepository
    {
        Task<Movimiento> GetMovimientoByIdAsync(int id);
        Task<IEnumerable<Movimiento>> GetAllMovimientosAsync();
        Task AddMovimientoAsync(Movimiento movimiento);
        Task UpdateMovimientoAsync(Movimiento movimiento);
        Task DeleteMovimientoAsync(int id);
    }
}
