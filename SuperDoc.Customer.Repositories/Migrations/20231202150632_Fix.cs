using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperDoc.Customer.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class Fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Signature",
                table: "DocumentSignatories",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "PublicKey",
                table: "DocumentSignatories",
                type: "nvarchar(736)",
                maxLength: 736,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(392)",
                oldMaxLength: 392);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Signature",
                table: "DocumentSignatories",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "PublicKey",
                table: "DocumentSignatories",
                type: "nvarchar(392)",
                maxLength: 392,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(736)",
                oldMaxLength: 736);
        }
    }
}
