using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorAppServer.Migrations
{
    /// <inheritdoc />
    public partial class editUniqueConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Applicants_Name",
                table: "Applicants");

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_Email",
                table: "Applicants",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Applicants_Email",
                table: "Applicants");

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_Name",
                table: "Applicants",
                column: "Name",
                unique: true);
        }
    }
}
