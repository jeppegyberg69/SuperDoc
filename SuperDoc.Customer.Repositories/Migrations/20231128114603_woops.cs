using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperDoc.Customer.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class woops : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentSignatories_Revision_RevisionId",
                table: "DocumentSignatories");

            migrationBuilder.DropForeignKey(
                name: "FK_Revision_Documents_DocumentId",
                table: "Revision");

            migrationBuilder.DropForeignKey(
                name: "FK_Revision_Users_UserId",
                table: "Revision");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Revision",
                table: "Revision");

            migrationBuilder.RenameTable(
                name: "Revision",
                newName: "Revisions");

            migrationBuilder.RenameIndex(
                name: "IX_Revision_UserId",
                table: "Revisions",
                newName: "IX_Revisions_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Revision_DocumentId",
                table: "Revisions",
                newName: "IX_Revisions_DocumentId");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateSigned",
                table: "DocumentSignatories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Revisions",
                table: "Revisions",
                column: "RevisionId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentSignatories_Revisions_RevisionId",
                table: "DocumentSignatories",
                column: "RevisionId",
                principalTable: "Revisions",
                principalColumn: "RevisionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Revisions_Documents_DocumentId",
                table: "Revisions",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "DocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Revisions_Users_UserId",
                table: "Revisions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentSignatories_Revisions_RevisionId",
                table: "DocumentSignatories");

            migrationBuilder.DropForeignKey(
                name: "FK_Revisions_Documents_DocumentId",
                table: "Revisions");

            migrationBuilder.DropForeignKey(
                name: "FK_Revisions_Users_UserId",
                table: "Revisions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Revisions",
                table: "Revisions");

            migrationBuilder.DropColumn(
                name: "DateSigned",
                table: "DocumentSignatories");

            migrationBuilder.RenameTable(
                name: "Revisions",
                newName: "Revision");

            migrationBuilder.RenameIndex(
                name: "IX_Revisions_UserId",
                table: "Revision",
                newName: "IX_Revision_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Revisions_DocumentId",
                table: "Revision",
                newName: "IX_Revision_DocumentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Revision",
                table: "Revision",
                column: "RevisionId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentSignatories_Revision_RevisionId",
                table: "DocumentSignatories",
                column: "RevisionId",
                principalTable: "Revision",
                principalColumn: "RevisionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Revision_Documents_DocumentId",
                table: "Revision",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "DocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Revision_Users_UserId",
                table: "Revision",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }
    }
}
