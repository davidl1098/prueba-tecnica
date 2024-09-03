using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CuentaMovimientosService.Migrations
{
    /// <inheritdoc />
    public partial class AddClienteIdToCuenta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClienteId",
                table: "Cuentas",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClienteId",
                table: "Cuentas");
        }
    }
}
