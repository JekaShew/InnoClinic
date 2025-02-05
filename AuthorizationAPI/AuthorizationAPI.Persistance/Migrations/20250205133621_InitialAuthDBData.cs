using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthorizationAPI.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class InitialAuthDBData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "UserStatuses",
                columns: new[] { "Id", "Description", "Title" },
                values: new object[] { new Guid("7b31946c-6d14-44dc-9f93-3a4c06db902e"), "The Banned user status means that User was Banned by Administrator for some action.", "Banned" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserStatuses",
                keyColumn: "Id",
                keyValue: new Guid("7b31946c-6d14-44dc-9f93-3a4c06db902e"));
        }
    }
}
