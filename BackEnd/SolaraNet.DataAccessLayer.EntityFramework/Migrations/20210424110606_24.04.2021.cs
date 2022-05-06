using Microsoft.EntityFrameworkCore.Migrations;

namespace SolaraNet.DataAccessLayer.EntityFramework.Migrations
{
    public partial class _24042021 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeleteReason",
                table: "Advertisments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "185230d2-58d8-4e29-aefd-a257fb82a150",
                column: "ConcurrencyStamp",
                value: "8be86482-3990-4250-8f64-108789a75b81");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d3300ca5-846f-4e6b-ac5f-1d3933115e67",
                column: "ConcurrencyStamp",
                value: "4ada3502-c503-4f57-8068-305ea2454d8d");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "98b651ae-c9aa-4731-9996-57352d525f7e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a941b3c1-bfb5-4538-97b7-abed0e4267ab", "AQAAAAEAACcQAAAAEJILuXuYb/o37X7gTLJloDAgZyKs8Dw5cBsOYQMAN8bBwAcwKi32jvkcjfytda15MA==", "5d6d54bf-2a7c-4a97-9866-5f4811f17e58" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeleteReason",
                table: "Advertisments");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "185230d2-58d8-4e29-aefd-a257fb82a150",
                column: "ConcurrencyStamp",
                value: "18ddf7af-41cd-4cc0-bf6f-ede8bb106f3a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d3300ca5-846f-4e6b-ac5f-1d3933115e67",
                column: "ConcurrencyStamp",
                value: "a7696093-ddb4-4f5d-83eb-2924c02dd1a5");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "98b651ae-c9aa-4731-9996-57352d525f7e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b9484475-5741-4925-89dd-fdb1ca52cb64", "AQAAAAEAACcQAAAAEJgHCsmGBfUNoc3h9ouYAs1GlHTaIaoEw3CwYFNHGr4aQehqAfsukcefdZEMEyUMCQ==", "c83e0715-5489-4190-92ea-3d3a3ea0b7a2" });
        }
    }
}
