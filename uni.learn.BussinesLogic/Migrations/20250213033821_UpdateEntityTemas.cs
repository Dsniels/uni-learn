using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace uni.learn.BussinesLogic.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntityTemas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CursoTemas");

            migrationBuilder.AddColumn<int>(
                name: "CursoId",
                table: "Tema",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tema_CursoId",
                table: "Tema",
                column: "CursoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tema_Curso_CursoId",
                table: "Tema",
                column: "CursoId",
                principalTable: "Curso",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tema_Curso_CursoId",
                table: "Tema");

            migrationBuilder.DropIndex(
                name: "IX_Tema_CursoId",
                table: "Tema");

            migrationBuilder.DropColumn(
                name: "CursoId",
                table: "Tema");

            migrationBuilder.CreateTable(
                name: "CursoTemas",
                columns: table => new
                {
                    CursosId = table.Column<int>(type: "int", nullable: false),
                    TemasId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CursoTemas", x => new { x.CursosId, x.TemasId });
                    table.ForeignKey(
                        name: "FK_CursoTemas_Curso_CursosId",
                        column: x => x.CursosId,
                        principalTable: "Curso",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CursoTemas_Tema_TemasId",
                        column: x => x.TemasId,
                        principalTable: "Tema",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CursoTemas_TemasId",
                table: "CursoTemas",
                column: "TemasId");
        }
    }
}
