using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperDoc.Customer.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDoc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentSignatories_Documents_DocumentId",
                table: "DocumentSignatories");

            migrationBuilder.DropIndex(
                name: "IX_DocumentSignatories_DocumentId",
                table: "DocumentSignatories");

            migrationBuilder.DropColumn(
                name: "DocumentId",
                table: "DocumentSignatories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DocumentId",
                table: "DocumentSignatories",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_DocumentSignatories_DocumentId",
                table: "DocumentSignatories",
                column: "DocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentSignatories_Documents_DocumentId",
                table: "DocumentSignatories",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "DocumentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
