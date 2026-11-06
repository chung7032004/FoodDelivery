using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDelivery.Migrations;

[DbContext(typeof(FoodContext))]
[Migration("20261103050926_AddShipperAddressPermissions")]
public partial class AddShipperAddressPermissions : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            INSERT INTO "RolePermissions" ("PermissionId", "RoleId")
            VALUES
                ('e827ae98-f118-1f4e-e890-4cb036aba6b8', '44444444-4444-4444-4444-444444444444'),
                ('f8edc86c-8f35-bc18-75a5-17cfec2310f0', '44444444-4444-4444-4444-444444444444'),
                ('c4bdb36c-4205-1966-10fc-6b5701a69c42', '44444444-4444-4444-4444-444444444444'),
                ('e35c101b-a88c-8804-7ccb-6f0c2a5cf1e1', '44444444-4444-4444-4444-444444444444')
            ON CONFLICT ("RoleId", "PermissionId") DO NOTHING;
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            DELETE FROM "RolePermissions"
            WHERE "RoleId" = '44444444-4444-4444-4444-444444444444'
              AND "PermissionId" IN (
                  'e827ae98-f118-1f4e-e890-4cb036aba6b8',
                  'f8edc86c-8f35-bc18-75a5-17cfec2310f0',
                  'c4bdb36c-4205-1966-10fc-6b5701a69c42',
                  'e35c101b-a88c-8804-7ccb-6f0c2a5cf1e1'
              );
            """);
    }
}
