namespace CuentaMovimientosService.Models
{
    public class Cuenta
    {
        public int Id { get; set; }  // Clave primaria
        public string NumeroCuenta { get; set; }
        public string TipoCuenta { get; set; }
        public decimal SaldoInicial { get; set; }
        public bool Estado { get; set; }
    }
}
