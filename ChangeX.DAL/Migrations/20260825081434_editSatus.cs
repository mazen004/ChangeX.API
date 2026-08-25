using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChangeX.DAL.Migrations
{
    /// <inheritdoc />
    public partial class editSatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CRStatues",
                keyColumn: "ID",
                keyValue: new Guid("5c2e8a4d-9f7b-4e1c-a3d6-8b4f2c9e7a15"),
                column: "AvailableStatusIDs",
                value: null);

            migrationBuilder.UpdateData(
                table: "CRStatues",
                keyColumn: "ID",
                keyValue: new Guid("8d4f2c6e-3a9b-4e7d-9c1f-5a2d9b6c3e47"),
                column: "AvailableStatusIDs",
                value: "B7E3A9C4-2F8D-4B6E-9A1C-6D4F2E8A7C53,4B9E7C2A-6D3F-4A8E-9C2B-1E7A4D8C6F39");

            migrationBuilder.UpdateData(
                table: "CRStatues",
                keyColumn: "ID",
                keyValue: new Guid("c1f7a4e9-8b2d-4e6c-a3f1-7c9e2a5d8b64"),
                columns: new[] { "AvailableStatusIDs", "CurrentStatus" },
                values: new object[] { "1c8e4b7a-3d9f-4e2c-b6a8-5f3d9e1c7a42,8d4f2c6e-3a9b-4e7d-9c1f-5a2d9b6c3e47", "Pending Vendor Rework Feedback" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CRStatues",
                keyColumn: "ID",
                keyValue: new Guid("5c2e8a4d-9f7b-4e1c-a3d6-8b4f2c9e7a15"),
                column: "AvailableStatusIDs",
                value: "C1F7A4E9-8B2D-4E6C-A3F1-7C9E2A5D8B64");

            migrationBuilder.UpdateData(
                table: "CRStatues",
                keyColumn: "ID",
                keyValue: new Guid("8d4f2c6e-3a9b-4e7d-9c1f-5a2d9b6c3e47"),
                column: "AvailableStatusIDs",
                value: "B7E3A9C4-2F8D-4B6E-9A1C-6D4F2E8A7C53,8A3E6C1F-4B9D-4E2A-9F7C-2D5B8E4A1C96,4B9E7C2A-6D3F-4A8E-9C2B-1E7A4D8C6F39");

            migrationBuilder.UpdateData(
                table: "CRStatues",
                keyColumn: "ID",
                keyValue: new Guid("c1f7a4e9-8b2d-4e6c-a3f1-7c9e2a5d8b64"),
                columns: new[] { "AvailableStatusIDs", "CurrentStatus" },
                values: new object[] { null, "Completed" });
        }
    }
}
