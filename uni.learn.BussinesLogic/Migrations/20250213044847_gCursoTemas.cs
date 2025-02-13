using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace uni.learn.BussinesLogic.Migrations
{
    /// <inheritdoc />
    public partial class gCursoTemas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CursoTema_Curso_CursosId",
                table: "CursoTema");

            migrationBuilder.DropForeignKey(
                name: "FK_CursoTema_Tema_TemasId",
                table: "CursoTema");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CursoTema",
                table: "CursoTema");

            migrationBuilder.RenameTable(
                name: "CursoTema",
                newName: "CursoTemas");

            migrationBuilder.RenameIndex(
                name: "IX_CursoTema_TemasId",
                table: "CursoTemas",
                newName: "IX_CursoTemas_TemasId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CursoTemas",
                table: "CursoTemas",
                columns: new[] { "CursosId", "TemasId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CursoTemas_Curso_CursosId",
                table: "CursoTemas",
                column: "CursosId",
                principalTable: "Curso",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CursoTemas_Tema_TemasId",
                table: "CursoTemas",
                column: "TemasId",
                principalTable: "Tema",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CursoTemas_Curso_CursosId",
                table: "CursoTemas");

            migrationBuilder.DropForeignKey(
                name: "FK_CursoTemas_Tema_TemasId",
                table: "CursoTemas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CursoTemas",
                table: "CursoTemas");

            migrationBuilder.RenameTable(
                name: "CursoTemas",
                newName: "CursoTema");

            migrationBuilder.RenameIndex(
                name: "IX_CursoTemas_TemasId",
                table: "CursoTema",
                newName: "IX_CursoTema_TemasId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CursoTema",
                table: "CursoTema",
                columns: new[] { "CursosId", "TemasId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CursoTema_Curso_CursosId",
                table: "CursoTema",
                column: "CursosId",
                principalTable: "Curso",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CursoTema_Tema_TemasId",
                table: "CursoTema",
                column: "TemasId",
                principalTable: "Tema",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
