using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AuthorizationAPI.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class InitialAuthDBData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Description", "Title" },
                values: new object[,]
                {
                    { new Guid("0eec148a-43d6-4b32-afb6-1ecf3341be6d"), "The role Doctor gives some administrative rights.", "Doctor" },
                    { new Guid("73b795d3-4917-4219-a1a0-044fcc6606ea"), "The role Administrator gives full admin rights.", "Administrator" },
                    { new Guid("78b25fdf-7199-4066-b677-5bc465bc3d1a"), "The role Patient gives small client rigts.", "Patient" }
                });

            migrationBuilder.InsertData(
                table: "UserStatuses",
                columns: new[] { "Id", "Description", "Title" },
                values: new object[,]
                {
                    { new Guid("6c6feeba-0919-4266-b2d1-9f5b724db31a"), "The Deleted user status means that User Deleted their account.", "Deleted" },
                    { new Guid("a780b7f4-3c8b-4452-a426-e7abc1a46949"), "The Non-Activated user status means that user hasn't been activated yet.", "Non-Activated" },
                    { new Guid("b9f67cf2-60de-48eb-82d0-8a5d6cde1b0f"), "The Activated user status means that user has been already activated.", "Activated" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("0eec148a-43d6-4b32-afb6-1ecf3341be6d"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("73b795d3-4917-4219-a1a0-044fcc6606ea"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("78b25fdf-7199-4066-b677-5bc465bc3d1a"));

            migrationBuilder.DeleteData(
                table: "UserStatuses",
                keyColumn: "Id",
                keyValue: new Guid("6c6feeba-0919-4266-b2d1-9f5b724db31a"));

            migrationBuilder.DeleteData(
                table: "UserStatuses",
                keyColumn: "Id",
                keyValue: new Guid("a780b7f4-3c8b-4452-a426-e7abc1a46949"));

            migrationBuilder.DeleteData(
                table: "UserStatuses",
                keyColumn: "Id",
                keyValue: new Guid("b9f67cf2-60de-48eb-82d0-8a5d6cde1b0f"));
        }
    }
}
