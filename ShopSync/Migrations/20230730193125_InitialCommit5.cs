using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopSync.Migrations
{
    public partial class InitialCommit5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ShoppingListId",
                table: "ShoppingLists",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShoppingListId",
                table: "ShoppingLists");
        }
    }
}
