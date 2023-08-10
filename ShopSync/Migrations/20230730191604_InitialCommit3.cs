using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopSync.Migrations
{
    public partial class InitialCommit3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReceipt",
                table: "Products");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReceipt",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
