using System;
using FoodDelivery.Entities;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDelivery.Migrations;

[DbContext(typeof(FoodContext))]
[Migration("20260930120000_AddRestaurantApplicationReview")]
public partial class AddRestaurantApplicationReview : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "ApplicationStatus",
            table: "Restaurants",
            type: "integer",
            nullable: false,
            defaultValue: (int)RestaurantApplicationStatus.Approved);

        migrationBuilder.AddColumn<string>(
            name: "RejectionReason",
            table: "Restaurants",
            type: "text",
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "ReviewedAt",
            table: "Restaurants",
            type: "timestamp with time zone",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "ReviewedByAdminId",
            table: "Restaurants",
            type: "uuid",
            nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(name: "ApplicationStatus", table: "Restaurants");
        migrationBuilder.DropColumn(name: "RejectionReason", table: "Restaurants");
        migrationBuilder.DropColumn(name: "ReviewedAt", table: "Restaurants");
        migrationBuilder.DropColumn(name: "ReviewedByAdminId", table: "Restaurants");
    }
}