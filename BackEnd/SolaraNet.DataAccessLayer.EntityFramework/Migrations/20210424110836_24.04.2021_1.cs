using Microsoft.EntityFrameworkCore.Migrations;

namespace SolaraNet.DataAccessLayer.EntityFramework.Migrations
{
    public partial class _24042021_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "185230d2-58d8-4e29-aefd-a257fb82a150",
                column: "ConcurrencyStamp",
                value: "5aa9b122-e56c-4a83-867d-b2f082a0be26");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d3300ca5-846f-4e6b-ac5f-1d3933115e67",
                column: "ConcurrencyStamp",
                value: "c57973a4-4db2-48a0-9a74-41774435a669");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "98b651ae-c9aa-4731-9996-57352d525f7e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4e1273a6-1edb-4813-9833-cc5534690dd0", "AQAAAAEAACcQAAAAEGuPxe7rewuh5UQAnPnIV5wnuYvpVHloFr8Ni0HosqSvwFvm4T1VfqGHCJLVibS9IA==", "6c457cc0-e4d3-4072-9511-f4d8fcd54c34" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
