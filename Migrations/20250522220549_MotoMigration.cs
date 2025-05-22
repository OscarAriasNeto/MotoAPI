using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotoAPI.Migrations
{
    /// <inheritdoc />
    public partial class MotoMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MOTOS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    MODELO = table.Column<string>(type: "VARCHAR2(100)", maxLength: 100, nullable: false),
                    ANO_FABRICACAO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    PLACA = table.Column<string>(type: "VARCHAR2(7)", maxLength: 7, nullable: false),
                    ESTADO = table.Column<string>(type: "VARCHAR2(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOTOS", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MOTOS_PLACA",
                table: "MOTOS",
                column: "PLACA",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MOTOS");
        }
    }
}
