using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExaminationProject.Migrations
{
    /// <inheritdoc />
    public partial class examresultе : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamResults_Users_UserId1",
                table: "ExamResults");

            migrationBuilder.DropIndex(
                name: "IX_ExamResults_UserId1",
                table: "ExamResults");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "ExamResults");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ExamResults",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_ExamResults_UserId",
                table: "ExamResults",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamResults_Users_UserId",
                table: "ExamResults",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamResults_Users_UserId",
                table: "ExamResults");

            migrationBuilder.DropIndex(
                name: "IX_ExamResults_UserId",
                table: "ExamResults");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "ExamResults",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "ExamResults",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExamResults_UserId1",
                table: "ExamResults",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamResults_Users_UserId1",
                table: "ExamResults",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
