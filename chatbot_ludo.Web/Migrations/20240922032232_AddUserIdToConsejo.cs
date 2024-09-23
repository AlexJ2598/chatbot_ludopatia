using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace chatbot_ludo.Web.Migrations
{
    public partial class AddUserIdToConsejo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consejos_AspNetUsers_UserId",
                table: "Consejos");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Consejos",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Consejos_AspNetUsers_UserId",
                table: "Consejos",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consejos_AspNetUsers_UserId",
                table: "Consejos");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Consejos",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Consejos_AspNetUsers_UserId",
                table: "Consejos",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
