using Microsoft.EntityFrameworkCore.Migrations;

namespace Ordering.API.Migrations
{
    public partial class initmig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductName = table.Column<string>(type: "TEXT", nullable: true),
                    UnitPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    Units = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");
        }
    }
}
