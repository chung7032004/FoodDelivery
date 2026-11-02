using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDelivery.Migrations
{
    [DbContext(typeof(FoodContext))]
    [Migration("20261101160806_IncludeShippingInOrderTotal")]
    public partial class IncludeShippingInOrderTotal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Subtotal",
                table: "Orders",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.Sql("""
                UPDATE "Orders"
                SET "Subtotal" = "TotalAmount",
                    "TotalAmount" = "TotalAmount" + "ShippingFee";
                """);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                UPDATE "Orders"
                SET "TotalAmount" = "TotalAmount" - "ShippingFee";
                """);

            migrationBuilder.DropColumn(
                name: "Subtotal",
                table: "Orders");
        }
    }
}
