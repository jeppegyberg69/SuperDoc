using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperDoc.Customer.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PhoneCode = table.Column<int>(type: "int", nullable: true),
                    PhoneNumber = table.Column<long>(type: "bigint", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    IsUserSignedUp = table.Column<bool>(type: "bit", nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Cases",
                columns: table => new
                {
                    CaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseNumber = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResponsibleUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cases", x => x.CaseId);
                    table.ForeignKey(
                        name: "FK_Cases_Users_ResponsibleUserId",
                        column: x => x.ResponsibleUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "CaseUsers",
                columns: table => new
                {
                    CaseManagersUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CasesCaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseUsers", x => new { x.CaseManagersUserId, x.CasesCaseId });
                    table.ForeignKey(
                        name: "FK_CaseUsers_Cases_CasesCaseId",
                        column: x => x.CasesCaseId,
                        principalTable: "Cases",
                        principalColumn: "CaseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaseUsers_Users_CaseManagersUserId",
                        column: x => x.CaseManagersUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    DocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.DocumentId);
                    table.ForeignKey(
                        name: "FK_Documents_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "CaseId");
                });

            migrationBuilder.CreateTable(
                name: "DocumentExternalUsers",
                columns: table => new
                {
                    DocumentsDocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExternalUsersUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentExternalUsers", x => new { x.DocumentsDocumentId, x.ExternalUsersUserId });
                    table.ForeignKey(
                        name: "FK_DocumentExternalUsers_Documents_DocumentsDocumentId",
                        column: x => x.DocumentsDocumentId,
                        principalTable: "Documents",
                        principalColumn: "DocumentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentExternalUsers_Users_ExternalUsersUserId",
                        column: x => x.ExternalUsersUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Revision",
                columns: table => new
                {
                    RevisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DocumentHash = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
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

            migrationBuilder.CreateTable(
                name: "DocumentSignatories",
                columns: table => new
                {
                    SignatureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RevisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PublicKey = table.Column<string>(type: "nvarchar(392)", maxLength: 392, nullable: false),
                    Signature = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentSignatories", x => x.SignatureId);
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
                name: "IX_Cases_ResponsibleUserId",
                table: "Cases",
                column: "ResponsibleUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseUsers_CasesCaseId",
                table: "CaseUsers",
                column: "CasesCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentExternalUsers_ExternalUsersUserId",
                table: "DocumentExternalUsers",
                column: "ExternalUsersUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_CaseId",
                table: "Documents",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentSignatories_RevisionId",
                table: "DocumentSignatories",
                column: "RevisionId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentSignatories_UserId",
                table: "DocumentSignatories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Revision_DocumentId",
                table: "Revision",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Revision_UserId",
                table: "Revision",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmailAddress",
                table: "Users",
                column: "EmailAddress",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaseUsers");

            migrationBuilder.DropTable(
                name: "DocumentExternalUsers");

            migrationBuilder.DropTable(
                name: "DocumentSignatories");

            migrationBuilder.DropTable(
                name: "Revision");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "Cases");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
