using CuentaMovimientosService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CuentaMovimientosService.Repositories.Interfaces
{
    public interface ICuentaRepository
    {
        Task<Cuenta> GetCuentaByIdAsync(int id);
        Task<IEnumerable<Cuenta>> GetAllCuentasAsync();
        Task AddCuentaAsync(Cuenta cuenta);
        Task UpdateCuentaAsync(Cuenta cuenta);
        Task DeleteCuentaAsync(int id);
        Task<List<Cuenta>> GetCuentasConMovimientosPorClienteAsync(int clienteId, DateTime fechaInicio, DateTime fechaFin);

    }
}
