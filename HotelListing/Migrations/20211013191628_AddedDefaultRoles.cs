using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelListing.Migrations
{
    public partial class AddedDefaultRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1fe7815c-a693-4c5f-8f2e-4046e27ed8ef", "9bbddde1-50c7-4c62-81a5-3edf68aea491", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1409ff98-dcff-45b9-8d5b-d5b360a66cce", "51cf7362-6ea2-48b8-bec7-5995265a5a83", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1409ff98-dcff-45b9-8d5b-d5b360a66cce");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1fe7815c-a693-4c5f-8f2e-4046e27ed8ef");
        }
    }
}
