using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace uni.learn.BussinesLogic.Migrations
{
    /// <inheritdoc />
    public partial class Temas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Tema",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Tema_Nombre",
                table: "Tema",
                column: "Nombre",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tema_Nombre",
                table: "Tema");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Tema",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
