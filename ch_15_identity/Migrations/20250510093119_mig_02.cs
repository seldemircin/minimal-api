using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ch_15_identity.Migrations
{
    /// <inheritdoc />
    public partial class mig_02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0baf3430-ffe2-4a67-af6d-2f52607a85d7", null, "Admin", "ADMIN" },
                    { "87cafb5d-6a2f-40ff-b392-57ae9bc0b070", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0baf3430-ffe2-4a67-af6d-2f52607a85d7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "87cafb5d-6a2f-40ff-b392-57ae9bc0b070");
        }
    }
}
