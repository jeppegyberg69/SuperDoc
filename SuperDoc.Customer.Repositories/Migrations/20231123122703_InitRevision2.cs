using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperDoc.Customer.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class InitRevision2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileHash",
                table: "Revision",
                newName: "DocumentHash");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DocumentHash",
                table: "Revision",
                newName: "FileHash");
        }
    }
}
