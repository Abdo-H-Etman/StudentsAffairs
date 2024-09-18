using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorAppServer.Migrations
{
    /// <inheritdoc />
    public partial class editTeacherAge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Age",
                table: "Teachers",
                type: "int",
                maxLength: 2,
                nullable: false,
                defaultValue: 27,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldMaxLength: 2,
                oldDefaultValue: (byte)27);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "Age",
                table: "Teachers",
                type: "tinyint",
                maxLength: 2,
                nullable: false,
                defaultValue: (byte)27,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 2,
                oldDefaultValue: 27);
        }
    }
}
