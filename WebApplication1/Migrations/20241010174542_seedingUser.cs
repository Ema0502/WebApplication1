using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class seedingUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Birth", "Email", "FirstName", "LastName", "Password", "Role", "UserName" },
                values: new object[] { new Guid("3fa85f64-5717-4562-b3fc-2c963f55afa6"), new DateTime(1987, 6, 27, 14, 38, 10, 548, DateTimeKind.Local), "messi@word.com", "lio", "messi", "admin123", "admin", "messias" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("3fa85f64-5717-4562-b3fc-2c963f55afa6"));
        }
    }
}
