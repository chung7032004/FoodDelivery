using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDelivery.Migrations;

public partial class AddOrderDispatchMetadata : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<DateTime>(
            name: "DispatchStartedAt",
            table: "OrderDetails",
            type: "timestamp with time zone",
            nullable: true);

        migrationBuilder.AddColumn<int>(
            name: "DispatchStatus",
            table: "OrderDetails",
            type: "integer",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<string>(
            name: "LastShipperReleaseReason",
            table: "OrderDetails",
            type: "text",
            nullable: true);

        migrationBuilder.AddColumn<int>(
            name: "PrepMinutes",
            table: "OrderDetails",
            type: "integer",
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "ShipperAssignedAt",
            table: "OrderDetails",
            type: "timestamp with time zone",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "ShipperCancellationCompensationPending",
            table: "OrderDetails",
            type: "boolean",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<int>(
            name: "ShipperReleaseCount",
            table: "OrderDetails",
            type: "integer",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.Sql("""
            UPDATE "OrderDetails"
            SET "DispatchStatus" = 2
            WHERE "ShipperId" IS NOT NULL;

            UPDATE "OrderDetails"
            SET "DispatchStatus" = 1,
                "DispatchStartedAt" = CURRENT_TIMESTAMP
            WHERE "ShipperId" IS NULL
              AND "Status" IN (3, 4, 5);
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(name: "DispatchStartedAt", table: "OrderDetails");
        migrationBuilder.DropColumn(name: "DispatchStatus", table: "OrderDetails");
        migrationBuilder.DropColumn(name: "LastShipperReleaseReason", table: "OrderDetails");
        migrationBuilder.DropColumn(name: "PrepMinutes", table: "OrderDetails");
        migrationBuilder.DropColumn(name: "ShipperAssignedAt", table: "OrderDetails");
        migrationBuilder.DropColumn(name: "ShipperCancellationCompensationPending", table: "OrderDetails");
        migrationBuilder.DropColumn(name: "ShipperReleaseCount", table: "OrderDetails");
    }
}
