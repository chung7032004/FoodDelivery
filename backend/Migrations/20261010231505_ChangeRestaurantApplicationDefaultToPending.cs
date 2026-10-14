using FoodDelivery.Entities;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDelivery.Migrations;

[DbContext(typeof(FoodContext))]
[Migration("20261010231505_ChangeRestaurantApplicationDefaultToPending")]
public partial class ChangeRestaurantApplicationDefaultToPending : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<int>(
            name: "ApplicationStatus",
            table: "Restaurants",
            type: "integer",
            nullable: false,
            defaultValue: (int)RestaurantApplicationStatus.Pending,
            oldClrType: typeof(int),
            oldType: "integer",
            oldDefaultValue: (int)RestaurantApplicationStatus.Approved);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<int>(
            name: "ApplicationStatus",
            table: "Restaurants",
            type: "integer",
            nullable: false,
            defaultValue: (int)RestaurantApplicationStatus.Approved,
            oldClrType: typeof(int),
            oldType: "integer",
            oldDefaultValue: (int)RestaurantApplicationStatus.Pending);
    }
}
