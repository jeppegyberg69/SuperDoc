using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperDoc.Customer.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class InitSignatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocumentSignatories",
                columns: table => new
                {
                    SignatureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RevisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PublicKey = table.Column<string>(type: "nvarchar(392)", maxLength: 392, nullable: false),
                    Signature = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentSignatories", x => x.SignatureId);
                    table.ForeignKey(
                        name: "FK_DocumentSignatories_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "DocumentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentSignatories_Revision_RevisionId",
                        column: x => x.RevisionId,
                        principalTable: "Revision",
                        principalColumn: "RevisionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentSignatories_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentSignatories_DocumentId",
                table: "DocumentSignatories",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentSignatories_RevisionId",
                table: "DocumentSignatories",
                column: "RevisionId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentSignatories_UserId",
                table: "DocumentSignatories",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentSignatories");
        }
    }
}
