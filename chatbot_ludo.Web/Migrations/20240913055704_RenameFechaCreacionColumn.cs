namespace chatbot_ludo.Web.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class RenameFechaCreacionColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Fecha_Creacios",
                table: "Consejos",
                newName: "Fecha_Creacion");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Fecha_Creacion",
                table: "Consejos",
                newName: "Fecha_Creacios");
        }
    }
}
