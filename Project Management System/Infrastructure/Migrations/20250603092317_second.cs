using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TutorId",
                table: "ProjectGroups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectGroups_TutorId",
                table: "ProjectGroups",
                column: "TutorId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectGroups_Students_TutorId",
                table: "ProjectGroups",
                column: "TutorId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectGroups_Students_TutorId",
                table: "ProjectGroups");

            migrationBuilder.DropIndex(
                name: "IX_ProjectGroups_TutorId",
                table: "ProjectGroups");

            migrationBuilder.DropColumn(
                name: "TutorId",
                table: "ProjectGroups");
        }
    }
}
