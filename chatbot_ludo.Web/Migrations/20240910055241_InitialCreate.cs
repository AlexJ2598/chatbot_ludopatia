using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace chatbot_ludo.Web.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Consejos",
                columns: table => new
                {
                    ID_Consejo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Texto_Consejo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Categoria = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Grado_Recomendacion = table.Column<int>(type: "int", nullable: false),
                    Fecha_Creacios = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consejos", x => x.ID_Consejo);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Consejos");
        }
    }
}
