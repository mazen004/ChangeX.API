using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ChangeX.DAL.Migrations
{
    /// <inheritdoc />
    public partial class SeedCRStatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableStatuses",
                table: "CRStatues");

            migrationBuilder.AddColumn<string>(
                name: "AvailableStatusIDs",
                table: "CRStatues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "CRStatues",
                columns: new[] { "ID", "AccessedBy", "AvailableStatusIDs", "CurrentStatus" },
                values: new object[,]
                {
                    { new Guid("1c8e4b7a-3d9f-4e2c-b6a8-5f3d9e1c7a42"), "Admin", "6A4D2F9E-8C3B-4A7D-9E1F-4B8A6D2C5F93,A5E9C3B7-2D4F-4A8E-9C1B-6F3D7E2A9B58", "Analysis" },
                    { new Guid("2e7c9a4d-5f3b-4c1e-8d6a-7b9f2c4e1a85"), "Admin", "6F4B2E8D-1A9C-4D7F-B3E6-8C2A5F9D4B17", "Accepted (CR)" },
                    { new Guid("3f2a9e7d-8b41-4c6a-9d2e-1a7f5c8b3e90"), "Admin", "2E7C9A4D-5F3B-4C1E-8D6A-7B9F2C4E1A85,8A3E6C1F-4B9D-4E2A-9F7C-2D5B8E4A1C96,7C1D4E2F-9A6B-4F3D-8E7C-2B9A5D1F6C43", "Pending Vendor FeedBack" },
                    { new Guid("4b9e7c2a-6d3f-4a8e-9c2b-1e7a4d8c6f39"), "Admin", "1C8E4B7A-3D9F-4E2C-B6A8-5F3D9E1C7A42", "Rework Required" },
                    { new Guid("5c2e8a4d-9f7b-4e1c-a3d6-8b4f2c9e7a15"), "Admin", "C1F7A4E9-8B2D-4E6C-A3F1-7C9E2A5D8B64", "Delivered" },
                    { new Guid("6a4d2f9e-8c3b-4a7d-9e1f-4b8a6d2c5f93"), "Admin", "F3B9E2D4-7A6C-4D8E-B2F1-9C5A3E7D4B26", "Design" },
                    { new Guid("6f4b2e8d-1a9c-4d7f-b3e6-8c2a5f9d4b17"), "Admin", "3F2A9E7D-8B41-4C6A-9D2E-1A7F5C8B3E90", "Estimation Created" },
                    { new Guid("7c1d4e2f-9a6b-4f3d-8e7c-2b9a5d1f6c43"), "Client", "3F2A9E7D-8B41-4C6A-9D2E-1A7F5C8B3E90", "Pending Client Clarification" },
                    { new Guid("8a3e6c1f-4b9d-4e2a-9f7c-2d5b8e4a1c96"), "Admin", null, "Rejected" },
                    { new Guid("8d4f2c6e-3a9b-4e7d-9c1f-5a2d9b6c3e47"), "Client", "B7E3A9C4-2F8D-4B6E-9A1C-6D4F2E8A7C53,8A3E6C1F-4B9D-4E2A-9F7C-2D5B8E4A1C96,4B9E7C2A-6D3F-4A8E-9C2B-1E7A4D8C6F39", "Pending Customer Approval" },
                    { new Guid("9d3f6a2e-4c8b-4f1d-a7e9-2b6c4d8a3f71"), "Admin", "1C8E4B7A-3D9F-4E2C-B6A8-5F3D9E1C7A42", "Accepted (Estimation)" },
                    { new Guid("a5e9c3b7-2d4f-4a8e-9c1b-6f3d7e2a9b58"), "Client", "9D3F6A2E-4C8B-4F1D-A7E9-2B6C4D8A3F71,8A3E6C1F-4B9D-4E2A-9F7C-2D5B8E4A1C96", "Pending Client Approval" },
                    { new Guid("b7e3a9c4-2f8d-4b6e-9a1c-6d4f2e8a7c53"), "Admin", "D9A4C2F7-6E3B-4D8A-B7C1-2F9E5A3D8C64", "Accepted (Test)" },
                    { new Guid("c1f7a4e9-8b2d-4e6c-a3f1-7c9e2a5d8b64"), "Admin", null, "Completed" },
                    { new Guid("d9a4c2f7-6e3b-4d8a-b7c1-2f9e5a3d8c64"), "Admin", "5C2E8A4D-9F7B-4E1C-A3D6-8B4F2C9E7A15", "Deployed" },
                    { new Guid("e2b7a4c9-6f1d-4e3a-8b9c-3d5a7f2e1c64"), "Admin", "8D4F2C6E-3A9B-4E7D-9C1F-5A2D9B6C3E47", "Testing" },
                    { new Guid("f3b9e2d4-7a6c-4d8e-b2f1-9c5a3e7d4b26"), "Admin", "E2B7A4C9-6F1D-4E3A-8B9C-3D5A7F2E1C64", "Development" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CRStatues",
                keyColumn: "ID",
                keyValue: new Guid("1c8e4b7a-3d9f-4e2c-b6a8-5f3d9e1c7a42"));

            migrationBuilder.DeleteData(
                table: "CRStatues",
                keyColumn: "ID",
                keyValue: new Guid("2e7c9a4d-5f3b-4c1e-8d6a-7b9f2c4e1a85"));

            migrationBuilder.DeleteData(
                table: "CRStatues",
                keyColumn: "ID",
                keyValue: new Guid("3f2a9e7d-8b41-4c6a-9d2e-1a7f5c8b3e90"));

            migrationBuilder.DeleteData(
                table: "CRStatues",
                keyColumn: "ID",
                keyValue: new Guid("4b9e7c2a-6d3f-4a8e-9c2b-1e7a4d8c6f39"));

            migrationBuilder.DeleteData(
                table: "CRStatues",
                keyColumn: "ID",
                keyValue: new Guid("5c2e8a4d-9f7b-4e1c-a3d6-8b4f2c9e7a15"));

            migrationBuilder.DeleteData(
                table: "CRStatues",
                keyColumn: "ID",
                keyValue: new Guid("6a4d2f9e-8c3b-4a7d-9e1f-4b8a6d2c5f93"));

            migrationBuilder.DeleteData(
                table: "CRStatues",
                keyColumn: "ID",
                keyValue: new Guid("6f4b2e8d-1a9c-4d7f-b3e6-8c2a5f9d4b17"));

            migrationBuilder.DeleteData(
                table: "CRStatues",
                keyColumn: "ID",
                keyValue: new Guid("7c1d4e2f-9a6b-4f3d-8e7c-2b9a5d1f6c43"));

            migrationBuilder.DeleteData(
                table: "CRStatues",
                keyColumn: "ID",
                keyValue: new Guid("8a3e6c1f-4b9d-4e2a-9f7c-2d5b8e4a1c96"));

            migrationBuilder.DeleteData(
                table: "CRStatues",
                keyColumn: "ID",
                keyValue: new Guid("8d4f2c6e-3a9b-4e7d-9c1f-5a2d9b6c3e47"));

            migrationBuilder.DeleteData(
                table: "CRStatues",
                keyColumn: "ID",
                keyValue: new Guid("9d3f6a2e-4c8b-4f1d-a7e9-2b6c4d8a3f71"));

            migrationBuilder.DeleteData(
                table: "CRStatues",
                keyColumn: "ID",
                keyValue: new Guid("a5e9c3b7-2d4f-4a8e-9c1b-6f3d7e2a9b58"));

            migrationBuilder.DeleteData(
                table: "CRStatues",
                keyColumn: "ID",
                keyValue: new Guid("b7e3a9c4-2f8d-4b6e-9a1c-6d4f2e8a7c53"));

            migrationBuilder.DeleteData(
                table: "CRStatues",
                keyColumn: "ID",
                keyValue: new Guid("c1f7a4e9-8b2d-4e6c-a3f1-7c9e2a5d8b64"));

            migrationBuilder.DeleteData(
                table: "CRStatues",
                keyColumn: "ID",
                keyValue: new Guid("d9a4c2f7-6e3b-4d8a-b7c1-2f9e5a3d8c64"));

            migrationBuilder.DeleteData(
                table: "CRStatues",
                keyColumn: "ID",
                keyValue: new Guid("e2b7a4c9-6f1d-4e3a-8b9c-3d5a7f2e1c64"));

            migrationBuilder.DeleteData(
                table: "CRStatues",
                keyColumn: "ID",
                keyValue: new Guid("f3b9e2d4-7a6c-4d8e-b2f1-9c5a3e7d4b26"));

            migrationBuilder.DropColumn(
                name: "AvailableStatusIDs",
                table: "CRStatues");

            migrationBuilder.AddColumn<string>(
                name: "AvailableStatuses",
                table: "CRStatues",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
