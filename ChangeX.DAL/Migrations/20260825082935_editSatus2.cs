using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChangeX.DAL.Migrations
{
    /// <inheritdoc />
    public partial class editSatus2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CRStatues",
                keyColumn: "ID",
                keyValue: new Guid("4b9e7c2a-6d3f-4a8e-9c2b-1e7a4d8c6f39"),
                column: "AvailableStatusIDs",
                value: "C1F7A4E9-8B2D-4E6C-A3F1-7C9E2A5D8B64");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CRStatues",
                keyColumn: "ID",
                keyValue: new Guid("4b9e7c2a-6d3f-4a8e-9c2b-1e7a4d8c6f39"),
                column: "AvailableStatusIDs",
                value: "1C8E4B7A-3D9F-4E2C-B6A8-5F3D9E1C7A42");
        }
    }
}
