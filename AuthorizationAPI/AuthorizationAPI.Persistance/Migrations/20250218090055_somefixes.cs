using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthorizationAPI.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class somefixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserStatuses_Title",
                table: "UserStatuses",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Title",
                table: "Roles",
                column: "Title",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserStatuses_Title",
                table: "UserStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Roles_Title",
                table: "Roles");
        }
    }
}
