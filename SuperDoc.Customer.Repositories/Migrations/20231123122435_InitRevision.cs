using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperDoc.Customer.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class InitRevision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Revision",
                columns: table => new
                {
                    RevisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FileHash = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DateUploaded = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Revision", x => x.RevisionId);
                    table.ForeignKey(
                        name: "FK_Revision_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "DocumentId");
                    table.ForeignKey(
                        name: "FK_Revision_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Revision_DocumentId",
                table: "Revision",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Revision_UserId",
                table: "Revision",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Revision");
        }
    }
}
