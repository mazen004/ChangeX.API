using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChangeX.DAL.Migrations
{
    /// <inheritdoc />
    public partial class edituserentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Users_UserID",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Clients",
                newName: "DefaultContactID");

            migrationBuilder.RenameIndex(
                name: "IX_Clients_UserID",
                table: "Clients",
                newName: "IX_Clients_DefaultContactID");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Users_DefaultContactID",
                table: "Clients",
                column: "DefaultContactID",
                principalTable: "Users",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Users_DefaultContactID",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "DefaultContactID",
                table: "Clients",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Clients_DefaultContactID",
                table: "Clients",
                newName: "IX_Clients_UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Users_UserID",
                table: "Clients",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID");
        }
    }
}
