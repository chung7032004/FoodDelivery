using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FoodDelivery.Migrations
{
    /// <inheritdoc />
    public partial class AddpermissionShipper : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("008769f2-b432-ec12-a2a3-abf5f664c802"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1885), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1872) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("00d42e12-16d8-1161-d9c3-8198d016b61d"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1040), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1027) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("01fbb69d-2fe0-cfe2-60cc-9ddefdb59221"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(658), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(645) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("0798cb38-d699-bcc8-774f-87ac16dcb56d"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(862), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(824) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("08965e65-eaba-3846-f3ca-5de5a9894e08"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1833), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1819) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("18637eb6-dd2d-4916-90e3-d190753fe651"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1154), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1141) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("1972a37c-ad35-0c73-ec42-8f1e18925e33"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(2028), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(2014) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("19bddbc6-bc3d-33b5-9a93-ac104667687f"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(751), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(737) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("19fc086c-165d-67c5-668f-56661a36164d"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(767), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(753) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("1b25fc03-7ad1-c69c-dae0-25daab39a24b"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1901), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1888) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("1cf712ea-bf75-5da7-0315-f6abf938c90c"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(637), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(623) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("1f20e50d-312e-e12d-bbe3-9b176f4055dc"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1653), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1640) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("21bd6ce0-657d-9610-2fc5-34caf572fbe8"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1486), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1472) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("2821a053-365e-d274-2f88-c10912fd142b"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(968), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(955) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("28fc09cf-1d97-d3cc-66b9-e7a6210a3ced"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(999), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(986) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("2e9b21fa-0456-1c67-b02f-77c8ffbdd1bc"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1932), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1920) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("2f7e4df2-8507-4ef5-625a-2f7959334de6"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(568), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(535) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("35f5f086-45fa-c7c6-8a5e-ea8f9c97cea8"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(514), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(498) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("3e1115c6-b4da-0b86-6140-c77889a5dec1"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1815), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1802) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("40486d09-d640-93b6-125b-742b3fb9c30a"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1172), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1159) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("408a4c2e-b781-797e-1ac0-462ac106ba27"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(2063), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(2050) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("416c3b00-b88a-f2d6-0f98-edd2530f4cfd"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(885), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(869) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("428a7672-6e0e-5486-0493-dd54a9d9ecb0"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1606), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1593) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("454b45b2-c9b0-ba27-b071-88c7ce4f9821"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1574), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1559) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("462495bb-1b19-10a6-a40f-a2f15ed27874"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(593), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(577) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("496fc662-2331-b5ab-2dba-803c897dd134"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1759), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1746) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("4a5e57f9-7371-ff34-a7c0-e6cf47f912c8"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(430), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(104) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("4c77f9fc-e86f-78b8-c88b-a2df097f6bd3"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1536), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1490) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("5273f960-e88a-b69b-c87c-aa16332b9d5d"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(2010), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1973) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("531335c1-be81-604d-927f-38492c2a3bdb"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(931), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(919) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("54d379ae-ad74-040a-c946-f56cd58ef72a"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1622), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1609) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("55674d3f-5e0b-2f4e-0123-849bc32efd83"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(717), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(704) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("62bf81bb-b05b-3dab-ab8e-46f87cf16bbf"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1674), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1660) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("641ccf8e-48a4-63f9-ea51-46a8f84e8275"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(485), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(468) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("676d33fc-eacf-fcc4-00a5-09c3521386f3"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1024), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1006) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("67c04dad-c1c4-c165-628f-62f4f39794d1"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(734), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(721) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("6bc3f860-b68c-8568-6564-78b6a17735fa"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1432), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1418) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("6f6232a5-e70c-eb97-a1e8-cfb0190dfc80"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1087), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1059) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("745be01f-d2c8-8cca-6684-f322171229ff"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(952), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(938) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("79a651a3-d1ae-03dd-9d16-6378fa5a8da3"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(680), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(666) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("7c06b883-0b51-f2eb-74b0-435447264e2f"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(804), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(790) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("83bc6004-6954-1c64-f355-b54fa1e8f7cc"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1056), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1043) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("87a96f18-d58d-24eb-e677-ca0abfb99bec"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1138), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1125) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("89c37a1c-4545-d211-ab83-1785370f3c10"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1556), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1542) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("9153963b-fc31-2e9b-2a99-d243b6081845"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1851), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1838) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("91bb8b66-0967-83ce-7056-629707d49a86"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(699), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(684) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("924c4e8c-1963-ec6b-7602-4a7700971a14"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1951), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1939) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("98a8f386-db29-0ea2-98df-a340c0787c08"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(916), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(903) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("a1bb2e7a-c3c4-31c0-8a79-931946540a5e"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1870), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1857) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("a2da381b-d201-15a4-1949-e2e85417c86a"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(820), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(807) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("ba2e8da4-a36d-4bc8-25fc-ae51d41fea7d"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1122), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1094) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("c4bdb36c-4205-1966-10fc-6b5701a69c42"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1721), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1709) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("c501c51b-6061-b652-d47f-8a593e35e5bb"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1637), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1624) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("c5355ae4-569e-3834-47be-60ae6c73b9c5"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1798), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1762) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("ccc21865-8cbd-f136-7f56-5c9551c5c120"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(787), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(770) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d2865fa6-328c-da22-aad5-12cd86505b1b"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(983), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(971) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d8d648d0-c0f5-523a-3dda-d95613982d96"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(2047), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(2033) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("db454c1f-a726-589d-5585-7ee8c79c3434"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1469), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1456) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e35c101b-a88c-8804-7ccb-6f0c2a5cf1e1"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1738), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1724) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e6b7aa38-22c2-348d-5827-efeb05b4b45e"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1448), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1434) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e8031eea-0eef-f077-9aa5-c44e18fef35f"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1590), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1577) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e827ae98-f118-1f4e-e890-4cb036aba6b8"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1691), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1678) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("ec601469-ca78-db72-cd02-248fd76a3558"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(901), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(888) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("ee00f29c-4500-640d-cb1d-8909ae281747"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1968), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1954) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f1d8a62c-3192-969d-5d75-025c2928d679"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(532), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(517) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f8edc86c-8f35-bc18-75a5-17cfec2310f0"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1706), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1694) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("fea911ea-a325-1e7e-8feb-866ad4a9a5c4"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1917), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1903) });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("0cceae31-6ec1-0e3e-78a0-ec9bcd77a9da"), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1320), "Quyền cho Shippers - ViewDeliveryHistory", "Permissions.Shippers.ViewDeliveryHistory", new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1307) },
                    { new Guid("115f4508-cbcf-cc1d-2e59-c81f77ca169c"), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1240), "Quyền cho Shippers - MarkDeliverySuccess", "Permissions.Shippers.MarkDeliverySuccess", new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1227) },
                    { new Guid("12532d80-90fb-fd9c-8f04-3154d7a76cce"), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1393), "Quyền cho Shippers - ViewEarnings", "Permissions.Shippers.ViewEarnings", new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1379) },
                    { new Guid("22756935-8c09-b76e-cf2c-bd564546549c"), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1192), "Quyền cho Shippers - ViewAssignedOrders", "Permissions.Shippers.ViewAssignedOrders", new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1177) },
                    { new Guid("2dbab2bf-4d1c-26ca-84f3-217d416c9d53"), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1357), "Quyền cho Shippers - ViewProposals", "Permissions.Shippers.ViewProposals", new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1344) },
                    { new Guid("3b3a1dbb-a477-6322-4580-4ff269a0591d"), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1409), "Quyền cho Shippers - UpdateAvailability", "Permissions.Shippers.UpdateAvailability", new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1396) },
                    { new Guid("3cd72fe7-9b93-b471-2a4b-97591815b548"), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1224), "Quyền cho Shippers - ConfirmPickup", "Permissions.Shippers.ConfirmPickup", new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1211) },
                    { new Guid("57eefa92-847d-a119-fcd5-6864e4810afe"), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1303), "Quyền cho Shippers - ViewDashboardStats", "Permissions.Shippers.ViewDashboardStats", new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1288) },
                    { new Guid("5ba9233d-0528-5199-202e-b611e789f3fe"), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1376), "Quyền cho Shippers - CancelAssignedOrder", "Permissions.Shippers.CancelAssignedOrder", new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1362) },
                    { new Guid("e68d08c0-aca1-23d5-67b8-1e9ec7d68f4e"), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1340), "Quyền cho Shippers - ProposeOrderChange", "Permissions.Shippers.ProposeOrderChange", new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1323) },
                    { new Guid("e81365d2-eb79-4f90-dbfc-0e796802259a"), new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1208), "Quyền cho Shippers - ViewOrderDetails", "Permissions.Shippers.ViewOrderDetails", new DateTime(2026, 6, 16, 7, 57, 20, 972, DateTimeKind.Utc).AddTicks(1195) }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { new Guid("d8d648d0-c0f5-523a-3dda-d95613982d96"), new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("0cceae31-6ec1-0e3e-78a0-ec9bcd77a9da"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("115f4508-cbcf-cc1d-2e59-c81f77ca169c"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("12532d80-90fb-fd9c-8f04-3154d7a76cce"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("22756935-8c09-b76e-cf2c-bd564546549c"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("2dbab2bf-4d1c-26ca-84f3-217d416c9d53"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("3b3a1dbb-a477-6322-4580-4ff269a0591d"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("3cd72fe7-9b93-b471-2a4b-97591815b548"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("57eefa92-847d-a119-fcd5-6864e4810afe"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("5ba9233d-0528-5199-202e-b611e789f3fe"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("e68d08c0-aca1-23d5-67b8-1e9ec7d68f4e"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("e81365d2-eb79-4f90-dbfc-0e796802259a"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("0cceae31-6ec1-0e3e-78a0-ec9bcd77a9da"), new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("115f4508-cbcf-cc1d-2e59-c81f77ca169c"), new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("12532d80-90fb-fd9c-8f04-3154d7a76cce"), new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("22756935-8c09-b76e-cf2c-bd564546549c"), new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("2dbab2bf-4d1c-26ca-84f3-217d416c9d53"), new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("3b3a1dbb-a477-6322-4580-4ff269a0591d"), new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("3cd72fe7-9b93-b471-2a4b-97591815b548"), new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("57eefa92-847d-a119-fcd5-6864e4810afe"), new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("5ba9233d-0528-5199-202e-b611e789f3fe"), new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("e68d08c0-aca1-23d5-67b8-1e9ec7d68f4e"), new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("e81365d2-eb79-4f90-dbfc-0e796802259a"), new Guid("44444444-4444-4444-4444-444444444444") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("0cceae31-6ec1-0e3e-78a0-ec9bcd77a9da"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("115f4508-cbcf-cc1d-2e59-c81f77ca169c"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("12532d80-90fb-fd9c-8f04-3154d7a76cce"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("22756935-8c09-b76e-cf2c-bd564546549c"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("2dbab2bf-4d1c-26ca-84f3-217d416c9d53"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("3b3a1dbb-a477-6322-4580-4ff269a0591d"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("3cd72fe7-9b93-b471-2a4b-97591815b548"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("57eefa92-847d-a119-fcd5-6864e4810afe"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("5ba9233d-0528-5199-202e-b611e789f3fe"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("e68d08c0-aca1-23d5-67b8-1e9ec7d68f4e"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("e81365d2-eb79-4f90-dbfc-0e796802259a"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("0cceae31-6ec1-0e3e-78a0-ec9bcd77a9da"), new Guid("44444444-4444-4444-4444-444444444444") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("115f4508-cbcf-cc1d-2e59-c81f77ca169c"), new Guid("44444444-4444-4444-4444-444444444444") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("12532d80-90fb-fd9c-8f04-3154d7a76cce"), new Guid("44444444-4444-4444-4444-444444444444") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("22756935-8c09-b76e-cf2c-bd564546549c"), new Guid("44444444-4444-4444-4444-444444444444") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("2dbab2bf-4d1c-26ca-84f3-217d416c9d53"), new Guid("44444444-4444-4444-4444-444444444444") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("3b3a1dbb-a477-6322-4580-4ff269a0591d"), new Guid("44444444-4444-4444-4444-444444444444") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("3cd72fe7-9b93-b471-2a4b-97591815b548"), new Guid("44444444-4444-4444-4444-444444444444") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("57eefa92-847d-a119-fcd5-6864e4810afe"), new Guid("44444444-4444-4444-4444-444444444444") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("5ba9233d-0528-5199-202e-b611e789f3fe"), new Guid("44444444-4444-4444-4444-444444444444") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("d8d648d0-c0f5-523a-3dda-d95613982d96"), new Guid("44444444-4444-4444-4444-444444444444") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("e68d08c0-aca1-23d5-67b8-1e9ec7d68f4e"), new Guid("44444444-4444-4444-4444-444444444444") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("e81365d2-eb79-4f90-dbfc-0e796802259a"), new Guid("44444444-4444-4444-4444-444444444444") });

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("0cceae31-6ec1-0e3e-78a0-ec9bcd77a9da"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("115f4508-cbcf-cc1d-2e59-c81f77ca169c"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("12532d80-90fb-fd9c-8f04-3154d7a76cce"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("22756935-8c09-b76e-cf2c-bd564546549c"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("2dbab2bf-4d1c-26ca-84f3-217d416c9d53"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("3b3a1dbb-a477-6322-4580-4ff269a0591d"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("3cd72fe7-9b93-b471-2a4b-97591815b548"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("57eefa92-847d-a119-fcd5-6864e4810afe"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("5ba9233d-0528-5199-202e-b611e789f3fe"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e68d08c0-aca1-23d5-67b8-1e9ec7d68f4e"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e81365d2-eb79-4f90-dbfc-0e796802259a"));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("008769f2-b432-ec12-a2a3-abf5f664c802"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2786), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2773) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("00d42e12-16d8-1161-d9c3-8198d016b61d"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2148), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2134) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("01fbb69d-2fe0-cfe2-60cc-9ddefdb59221"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(1747), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(1733) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("0798cb38-d699-bcc8-774f-87ac16dcb56d"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(1968), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(1951) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("08965e65-eaba-3846-f3ca-5de5a9894e08"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2734), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2722) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("18637eb6-dd2d-4916-90e3-d190753fe651"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2278), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2265) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("1972a37c-ad35-0c73-ec42-8f1e18925e33"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2948), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2906) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("19bddbc6-bc3d-33b5-9a93-ac104667687f"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(1845), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(1831) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("19fc086c-165d-67c5-668f-56661a36164d"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(1861), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(1847) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("1b25fc03-7ad1-c69c-dae0-25daab39a24b"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2807), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2791) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("1cf712ea-bf75-5da7-0315-f6abf938c90c"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(1725), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(1711) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("1f20e50d-312e-e12d-bbe3-9b176f4055dc"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2544), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2530) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("21bd6ce0-657d-9610-2fc5-34caf572fbe8"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2380), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2367) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("2821a053-365e-d274-2f88-c10912fd142b"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2076), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2062) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("28fc09cf-1d97-d3cc-66b9-e7a6210a3ced"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2109), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2095) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("2e9b21fa-0456-1c67-b02f-77c8ffbdd1bc"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2849), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2827) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("2f7e4df2-8507-4ef5-625a-2f7959334de6"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(1652), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(1620) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("35f5f086-45fa-c7c6-8a5e-ea8f9c97cea8"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(1599), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(1583) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("3e1115c6-b4da-0b86-6140-c77889a5dec1"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2719), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2705) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("40486d09-d640-93b6-125b-742b3fb9c30a"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2297), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2283) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("408a4c2e-b781-797e-1ac0-462ac106ba27"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2995), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2980) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("416c3b00-b88a-f2d6-0f98-edd2530f4cfd"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(1990), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(1975) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("428a7672-6e0e-5486-0493-dd54a9d9ecb0"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2493), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2480) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("454b45b2-c9b0-ba27-b071-88c7ce4f9821"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2459), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2446) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("462495bb-1b19-10a6-a40f-a2f15ed27874"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(1707), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(1662) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("496fc662-2331-b5ab-2dba-803c897dd134"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2686), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2671) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("4a5e57f9-7371-ff34-a7c0-e6cf47f912c8"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(1513), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(1174) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("4c77f9fc-e86f-78b8-c88b-a2df097f6bd3"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2425), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2409) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("5273f960-e88a-b69b-c87c-aa16332b9d5d"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2903), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2891) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("531335c1-be81-604d-927f-38492c2a3bdb"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2039), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2025) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("54d379ae-ad74-040a-c946-f56cd58ef72a"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2509), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2496) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("55674d3f-5e0b-2f4e-0123-849bc32efd83"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(1811), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(1797) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("62bf81bb-b05b-3dab-ab8e-46f87cf16bbf"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2565), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2551) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("641ccf8e-48a4-63f9-ea51-46a8f84e8275"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(1572), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(1553) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("676d33fc-eacf-fcc4-00a5-09c3521386f3"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2131), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2116) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("67c04dad-c1c4-c165-628f-62f4f39794d1"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(1829), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(1814) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("6bc3f860-b68c-8568-6564-78b6a17735fa"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2322), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2308) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("6f6232a5-e70c-eb97-a1e8-cfb0190dfc80"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2226), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2212) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("745be01f-d2c8-8cca-6684-f322171229ff"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2059), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2045) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("79a651a3-d1ae-03dd-9d16-6378fa5a8da3"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(1772), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(1758) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("7c06b883-0b51-f2eb-74b0-435447264e2f"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(1896), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(1881) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("83bc6004-6954-1c64-f355-b54fa1e8f7cc"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2209), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2151) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("87a96f18-d58d-24eb-e677-ca0abfb99bec"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2262), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2248) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("89c37a1c-4545-d211-ab83-1785370f3c10"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2442), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2428) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("9153963b-fc31-2e9b-2a99-d243b6081845"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2750), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2737) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("91bb8b66-0967-83ce-7056-629707d49a86"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(1791), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(1777) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("924c4e8c-1963-ec6b-7602-4a7700971a14"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2869), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2855) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("98a8f386-db29-0ea2-98df-a340c0787c08"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2023), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2009) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("a1bb2e7a-c3c4-31c0-8a79-931946540a5e"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2770), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2757) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("a2da381b-d201-15a4-1949-e2e85417c86a"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(1912), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(1899) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("ba2e8da4-a36d-4bc8-25fc-ae51d41fea7d"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2246), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2232) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("c4bdb36c-4205-1966-10fc-6b5701a69c42"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2614), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2601) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("c501c51b-6061-b652-d47f-8a593e35e5bb"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2527), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2513) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("c5355ae4-569e-3834-47be-60ae6c73b9c5"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2702), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2689) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("ccc21865-8cbd-f136-7f56-5c9551c5c120"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(1878), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(1865) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d2865fa6-328c-da22-aad5-12cd86505b1b"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2092), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2079) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d8d648d0-c0f5-523a-3dda-d95613982d96"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2975), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2958) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("db454c1f-a726-589d-5585-7ee8c79c3434"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2364), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2350) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e35c101b-a88c-8804-7ccb-6f0c2a5cf1e1"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2632), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2619) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e6b7aa38-22c2-348d-5827-efeb05b4b45e"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2342), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2326) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e8031eea-0eef-f077-9aa5-c44e18fef35f"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2476), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2462) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e827ae98-f118-1f4e-e890-4cb036aba6b8"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2581), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2568) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("ec601469-ca78-db72-cd02-248fd76a3558"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2006), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(1993) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("ee00f29c-4500-640d-cb1d-8909ae281747"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2887), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2872) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f1d8a62c-3192-969d-5d75-025c2928d679"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(1617), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(1603) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f8edc86c-8f35-bc18-75a5-17cfec2310f0"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2597), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2584) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("fea911ea-a325-1e7e-8feb-866ad4a9a5c4"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2824), new DateTime(2026, 6, 5, 9, 25, 3, 935, DateTimeKind.Utc).AddTicks(2810) });
        }
    }
}
