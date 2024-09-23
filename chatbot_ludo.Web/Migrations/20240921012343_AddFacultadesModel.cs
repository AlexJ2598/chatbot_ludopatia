using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace chatbot_ludo.Web.Migrations
{
    public partial class AddFacultadesModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID_Consejo",
                table: "Consejos",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Consejos",
                newName: "ID_Consejo");
        }
    }
}
