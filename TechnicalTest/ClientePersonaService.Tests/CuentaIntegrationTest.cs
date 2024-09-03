using CuentaMovimientosService.Data;
using CuentaMovimientosService.Models;
using CuentaMovimientosService.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Tests
{
    [TestFixture]
    public class CuentaIntegrationTest
    {
        private ApplicationDbContext _context;
        private CuentaRepository _cuentaRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=db;Trusted_Connection=True;MultipleActiveResultSets=true")
            .Options;

            _context = new ApplicationDbContext(options);
            _cuentaRepository = new CuentaRepository(_context);
        }

        [Test]
        public async Task AddCuenta_ShouldAddCuentaToDatabase()
        {
            // Arrange
            var cuenta = new Cuenta
            {
                NumeroCuenta = "123456",
                TipoCuenta = "Ahorro",
                SaldoInicial = 1000m,
                Estado = true,
                ClienteId = 1  // Asegúrate de que este ID de cliente exista en tu base de datos o contexto
            };

            // Act
            await _cuentaRepository.AddCuentaAsync(cuenta);
            var result = await _cuentaRepository.GetCuentaByIdAsync(cuenta.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(cuenta.NumeroCuenta, result.NumeroCuenta);
            Assert.AreEqual(cuenta.SaldoInicial, result.SaldoInicial);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
