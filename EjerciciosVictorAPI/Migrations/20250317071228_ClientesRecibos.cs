using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EjerciciosVictorAPI.Migrations
{
    /// <inheritdoc />
    public partial class ClientesRecibos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    DNI = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Tipo = table.Column<string>(type: "text", nullable: false),
                    CuotaMaxima = table.Column<decimal>(type: "numeric", nullable: true),
                    FechaAlta = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.DNI);
                });

            migrationBuilder.CreateTable(
                name: "Recibos",
                columns: table => new
                {
                    NumeroRecibo = table.Column<string>(type: "text", nullable: false),
                    Importe = table.Column<decimal>(type: "numeric", nullable: false),
                    FechaEmision = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClienteDNI = table.Column<string>(type: "character varying(9)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recibos", x => x.NumeroRecibo);
                    table.ForeignKey(
                        name: "FK_Recibos_Clientes_ClienteDNI",
                        column: x => x.ClienteDNI,
                        principalTable: "Clientes",
                        principalColumn: "DNI",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Recibos_ClienteDNI",
                table: "Recibos",
                column: "ClienteDNI");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Recibos");

            migrationBuilder.DropTable(
                name: "Clientes");
        }
    }
}
