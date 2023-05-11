using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExaminationProject.Migrations
{
    /// <inheritdoc />
    public partial class examresult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamResults_Exams_ExamId",
                table: "ExamResults");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamResults_Users_UserId",
                table: "ExamResults");

            migrationBuilder.DropIndex(
                name: "IX_ExamResults_ExamId",
                table: "ExamResults");

            migrationBuilder.DropIndex(
                name: "IX_ExamResults_UserId",
                table: "ExamResults");

            migrationBuilder.RenameColumn(
                name: "ExamId",
                table: "ExamResults",
                newName: "TotalQuestions");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "ExamResults",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "CorrectAnswers",
                table: "ExamResults",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTaken",
                table: "ExamResults",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ExamCategoryId",
                table: "ExamResults",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "ExamResults",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExamResults_ExamCategoryId",
                table: "ExamResults",
                column: "ExamCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamResults_UserId1",
                table: "ExamResults",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamResults_ExamCategories_ExamCategoryId",
                table: "ExamResults",
                column: "ExamCategoryId",
                principalTable: "ExamCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamResults_Users_UserId1",
                table: "ExamResults",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamResults_ExamCategories_ExamCategoryId",
                table: "ExamResults");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamResults_Users_UserId1",
                table: "ExamResults");

            migrationBuilder.DropIndex(
                name: "IX_ExamResults_ExamCategoryId",
                table: "ExamResults");

            migrationBuilder.DropIndex(
                name: "IX_ExamResults_UserId1",
                table: "ExamResults");

            migrationBuilder.DropColumn(
                name: "CorrectAnswers",
                table: "ExamResults");

            migrationBuilder.DropColumn(
                name: "DateTaken",
                table: "ExamResults");

            migrationBuilder.DropColumn(
                name: "ExamCategoryId",
                table: "ExamResults");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "ExamResults");

            migrationBuilder.RenameColumn(
                name: "TotalQuestions",
                table: "ExamResults",
                newName: "ExamId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ExamResults",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_ExamResults_ExamId",
                table: "ExamResults",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamResults_UserId",
                table: "ExamResults",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamResults_Exams_ExamId",
                table: "ExamResults",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamResults_Users_UserId",
                table: "ExamResults",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
