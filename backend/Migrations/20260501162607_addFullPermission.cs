using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FoodDelivery.Migrations
{
    /// <inheritdoc />
    public partial class addFullPermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("9a3ba0a0-2914-e381-87df-4a00988bf036"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("9a3ba0a0-2914-e381-87df-4a00988bf036"), new Guid("22222222-2222-2222-2222-222222222222") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("9a3ba0a0-2914-e381-87df-4a00988bf036"), new Guid("33333333-3333-3333-3333-333333333333") });

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("9a3ba0a0-2914-e381-87df-4a00988bf036"));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("00d42e12-16d8-1161-d9c3-8198d016b61d"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1873), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1861) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("01fbb69d-2fe0-cfe2-60cc-9ddefdb59221"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1523), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1510) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("08965e65-eaba-3846-f3ca-5de5a9894e08"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2323), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2312) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("18637eb6-dd2d-4916-90e3-d190753fe651"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1946), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1934) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("19bddbc6-bc3d-33b5-9a93-ac104667687f"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1634), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1621) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("19fc086c-165d-67c5-668f-56661a36164d"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1647), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1635) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("1cf712ea-bf75-5da7-0315-f6abf938c90c"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1501), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1487) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("1f20e50d-312e-e12d-bbe3-9b176f4055dc"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2161), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2150) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("21bd6ce0-657d-9610-2fc5-34caf572fbe8"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2050), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2038) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("2821a053-365e-d274-2f88-c10912fd142b"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1811), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1770) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("28fc09cf-1d97-d3cc-66b9-e7a6210a3ced"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1840), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1827) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("2f7e4df2-8507-4ef5-625a-2f7959334de6"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1460), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1431) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("35f5f086-45fa-c7c6-8a5e-ea8f9c97cea8"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1412), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1396) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("3e1115c6-b4da-0b86-6140-c77889a5dec1"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2310), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2297) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("40486d09-d640-93b6-125b-742b3fb9c30a"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1959), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1947) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("428a7672-6e0e-5486-0493-dd54a9d9ecb0"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2119), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2107) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("454b45b2-c9b0-ba27-b071-88c7ce4f9821"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2093), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2081) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("462495bb-1b19-10a6-a40f-a2f15ed27874"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1485), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1472) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("496fc662-2331-b5ab-2dba-803c897dd134"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2282), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2270) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("4a5e57f9-7371-ff34-a7c0-e6cf47f912c8"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1363), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(613) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("4c77f9fc-e86f-78b8-c88b-a2df097f6bd3"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2065), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2051) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("54d379ae-ad74-040a-c946-f56cd58ef72a"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2134), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2122) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("55674d3f-5e0b-2f4e-0123-849bc32efd83"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1606), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1592) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("62bf81bb-b05b-3dab-ab8e-46f87cf16bbf"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2180), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2169) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("641ccf8e-48a4-63f9-ea51-46a8f84e8275"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1384), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1368) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("676d33fc-eacf-fcc4-00a5-09c3521386f3"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1857), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1845) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("67c04dad-c1c4-c165-628f-62f4f39794d1"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1619), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1608) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("6bc3f860-b68c-8568-6564-78b6a17735fa"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1977), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1965) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("6f6232a5-e70c-eb97-a1e8-cfb0190dfc80"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1901), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1888) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("83bc6004-6954-1c64-f355-b54fa1e8f7cc"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1887), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1875) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("87a96f18-d58d-24eb-e677-ca0abfb99bec"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1932), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1921) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("89c37a1c-4545-d211-ab83-1785370f3c10"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2079), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2067) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("9153963b-fc31-2e9b-2a99-d243b6081845"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2336), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2325) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("ba2e8da4-a36d-4bc8-25fc-ae51d41fea7d"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1919), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1907) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("c4bdb36c-4205-1966-10fc-6b5701a69c42"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2249), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2235) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("c501c51b-6061-b652-d47f-8a593e35e5bb"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2148), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2136) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("c5355ae4-569e-3834-47be-60ae6c73b9c5"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2296), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2284) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("ccc21865-8cbd-f136-7f56-5c9551c5c120"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1662), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1649) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d2865fa6-328c-da22-aad5-12cd86505b1b"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1826), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1813) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("db454c1f-a726-589d-5585-7ee8c79c3434"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2036), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2024) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e35c101b-a88c-8804-7ccb-6f0c2a5cf1e1"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2263), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2250) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e6b7aa38-22c2-348d-5827-efeb05b4b45e"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2017), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1981) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e8031eea-0eef-f077-9aa5-c44e18fef35f"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2106), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2095) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e827ae98-f118-1f4e-e890-4cb036aba6b8"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2194), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2182) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f1d8a62c-3192-969d-5d75-025c2928d679"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1429), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1414) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f8edc86c-8f35-bc18-75a5-17cfec2310f0"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2233), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2196) });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("008769f2-b432-ec12-a2a3-abf5f664c802"), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2365), "Quyền cho Review - Edit", "Permissions.Review.Edit", new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2354) },
                    { new Guid("0798cb38-d699-bcc8-774f-87ac16dcb56d"), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1706), "Quyền cho Orders - OutOfStock", "Permissions.Orders.OutOfStock", new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1694) },
                    { new Guid("1b25fc03-7ad1-c69c-dae0-25daab39a24b"), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2380), "Quyền cho Review - View", "Permissions.Review.View", new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2367) },
                    { new Guid("416c3b00-b88a-f2d6-0f98-edd2530f4cfd"), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1722), "Quyền cho Orders - StartPreparing", "Permissions.Orders.StartPreparing", new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1710) },
                    { new Guid("531335c1-be81-604d-927f-38492c2a3bdb"), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1764), "Quyền cho Orders - MarkPaymentComplete", "Permissions.Orders.MarkPaymentComplete", new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1752) },
                    { new Guid("79a651a3-d1ae-03dd-9d16-6378fa5a8da3"), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1536), "Quyền cho Orders - ViewAll", "Permissions.Orders.ViewAll", new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1524) },
                    { new Guid("7c06b883-0b51-f2eb-74b0-435447264e2f"), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1677), "Quyền cho Orders - RespondToProposal", "Permissions.Orders.RespondToProposal", new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1664) },
                    { new Guid("91bb8b66-0967-83ce-7056-629707d49a86"), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1588), "Quyền cho Orders - ViewHistory", "Permissions.Orders.ViewHistory", new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1539) },
                    { new Guid("98a8f386-db29-0ea2-98df-a340c0787c08"), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1750), "Quyền cho Orders - MarkAsDelivered", "Permissions.Orders.MarkAsDelivered", new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1737) },
                    { new Guid("a1bb2e7a-c3c4-31c0-8a79-931946540a5e"), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2352), "Quyền cho Review - Create", "Permissions.Review.Create", new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2341) },
                    { new Guid("a2da381b-d201-15a4-1949-e2e85417c86a"), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1691), "Quyền cho Orders - ConfirmOrder", "Permissions.Orders.ConfirmOrder", new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1679) },
                    { new Guid("ec601469-ca78-db72-cd02-248fd76a3558"), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1736), "Quyền cho Orders - MarkAsReady", "Permissions.Orders.MarkAsReady", new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(1724) },
                    { new Guid("fea911ea-a325-1e7e-8feb-866ad4a9a5c4"), new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2396), "Quyền cho Review - Manage", "Permissions.Review.Manage", new DateTime(2026, 5, 1, 16, 26, 5, 638, DateTimeKind.Utc).AddTicks(2383) }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { new Guid("21bd6ce0-657d-9610-2fc5-34caf572fbe8"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("4c77f9fc-e86f-78b8-c88b-a2df097f6bd3"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("62bf81bb-b05b-3dab-ab8e-46f87cf16bbf"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("21bd6ce0-657d-9610-2fc5-34caf572fbe8"), new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("4c77f9fc-e86f-78b8-c88b-a2df097f6bd3"), new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("67c04dad-c1c4-c165-628f-62f4f39794d1"), new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("db454c1f-a726-589d-5585-7ee8c79c3434"), new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("62bf81bb-b05b-3dab-ab8e-46f87cf16bbf"), new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("db454c1f-a726-589d-5585-7ee8c79c3434"), new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("08965e65-eaba-3846-f3ca-5de5a9894e08"), new Guid("55555555-5555-5555-5555-555555555555") },
                    { new Guid("3e1115c6-b4da-0b86-6140-c77889a5dec1"), new Guid("55555555-5555-5555-5555-555555555555") },
                    { new Guid("496fc662-2331-b5ab-2dba-803c897dd134"), new Guid("55555555-5555-5555-5555-555555555555") },
                    { new Guid("9153963b-fc31-2e9b-2a99-d243b6081845"), new Guid("55555555-5555-5555-5555-555555555555") },
                    { new Guid("c5355ae4-569e-3834-47be-60ae6c73b9c5"), new Guid("55555555-5555-5555-5555-555555555555") },
                    { new Guid("db454c1f-a726-589d-5585-7ee8c79c3434"), new Guid("55555555-5555-5555-5555-555555555555") },
                    { new Guid("008769f2-b432-ec12-a2a3-abf5f664c802"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("0798cb38-d699-bcc8-774f-87ac16dcb56d"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("1b25fc03-7ad1-c69c-dae0-25daab39a24b"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("416c3b00-b88a-f2d6-0f98-edd2530f4cfd"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("531335c1-be81-604d-927f-38492c2a3bdb"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("79a651a3-d1ae-03dd-9d16-6378fa5a8da3"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("7c06b883-0b51-f2eb-74b0-435447264e2f"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("91bb8b66-0967-83ce-7056-629707d49a86"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("98a8f386-db29-0ea2-98df-a340c0787c08"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("a1bb2e7a-c3c4-31c0-8a79-931946540a5e"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("a2da381b-d201-15a4-1949-e2e85417c86a"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("ec601469-ca78-db72-cd02-248fd76a3558"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("fea911ea-a325-1e7e-8feb-866ad4a9a5c4"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("416c3b00-b88a-f2d6-0f98-edd2530f4cfd"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("91bb8b66-0967-83ce-7056-629707d49a86"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("ec601469-ca78-db72-cd02-248fd76a3558"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("fea911ea-a325-1e7e-8feb-866ad4a9a5c4"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("416c3b00-b88a-f2d6-0f98-edd2530f4cfd"), new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("91bb8b66-0967-83ce-7056-629707d49a86"), new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("ec601469-ca78-db72-cd02-248fd76a3558"), new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("91bb8b66-0967-83ce-7056-629707d49a86"), new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("98a8f386-db29-0ea2-98df-a340c0787c08"), new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("008769f2-b432-ec12-a2a3-abf5f664c802"), new Guid("55555555-5555-5555-5555-555555555555") },
                    { new Guid("1b25fc03-7ad1-c69c-dae0-25daab39a24b"), new Guid("55555555-5555-5555-5555-555555555555") },
                    { new Guid("7c06b883-0b51-f2eb-74b0-435447264e2f"), new Guid("55555555-5555-5555-5555-555555555555") },
                    { new Guid("91bb8b66-0967-83ce-7056-629707d49a86"), new Guid("55555555-5555-5555-5555-555555555555") },
                    { new Guid("a1bb2e7a-c3c4-31c0-8a79-931946540a5e"), new Guid("55555555-5555-5555-5555-555555555555") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("008769f2-b432-ec12-a2a3-abf5f664c802"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("0798cb38-d699-bcc8-774f-87ac16dcb56d"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("1b25fc03-7ad1-c69c-dae0-25daab39a24b"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("416c3b00-b88a-f2d6-0f98-edd2530f4cfd"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("531335c1-be81-604d-927f-38492c2a3bdb"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("79a651a3-d1ae-03dd-9d16-6378fa5a8da3"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("7c06b883-0b51-f2eb-74b0-435447264e2f"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("91bb8b66-0967-83ce-7056-629707d49a86"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("98a8f386-db29-0ea2-98df-a340c0787c08"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("a1bb2e7a-c3c4-31c0-8a79-931946540a5e"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("a2da381b-d201-15a4-1949-e2e85417c86a"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("ec601469-ca78-db72-cd02-248fd76a3558"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("fea911ea-a325-1e7e-8feb-866ad4a9a5c4"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("21bd6ce0-657d-9610-2fc5-34caf572fbe8"), new Guid("22222222-2222-2222-2222-222222222222") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("416c3b00-b88a-f2d6-0f98-edd2530f4cfd"), new Guid("22222222-2222-2222-2222-222222222222") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("4c77f9fc-e86f-78b8-c88b-a2df097f6bd3"), new Guid("22222222-2222-2222-2222-222222222222") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("62bf81bb-b05b-3dab-ab8e-46f87cf16bbf"), new Guid("22222222-2222-2222-2222-222222222222") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("91bb8b66-0967-83ce-7056-629707d49a86"), new Guid("22222222-2222-2222-2222-222222222222") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("ec601469-ca78-db72-cd02-248fd76a3558"), new Guid("22222222-2222-2222-2222-222222222222") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("fea911ea-a325-1e7e-8feb-866ad4a9a5c4"), new Guid("22222222-2222-2222-2222-222222222222") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("21bd6ce0-657d-9610-2fc5-34caf572fbe8"), new Guid("33333333-3333-3333-3333-333333333333") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("416c3b00-b88a-f2d6-0f98-edd2530f4cfd"), new Guid("33333333-3333-3333-3333-333333333333") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("4c77f9fc-e86f-78b8-c88b-a2df097f6bd3"), new Guid("33333333-3333-3333-3333-333333333333") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("67c04dad-c1c4-c165-628f-62f4f39794d1"), new Guid("33333333-3333-3333-3333-333333333333") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("91bb8b66-0967-83ce-7056-629707d49a86"), new Guid("33333333-3333-3333-3333-333333333333") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("db454c1f-a726-589d-5585-7ee8c79c3434"), new Guid("33333333-3333-3333-3333-333333333333") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("ec601469-ca78-db72-cd02-248fd76a3558"), new Guid("33333333-3333-3333-3333-333333333333") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("62bf81bb-b05b-3dab-ab8e-46f87cf16bbf"), new Guid("44444444-4444-4444-4444-444444444444") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("91bb8b66-0967-83ce-7056-629707d49a86"), new Guid("44444444-4444-4444-4444-444444444444") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("98a8f386-db29-0ea2-98df-a340c0787c08"), new Guid("44444444-4444-4444-4444-444444444444") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("db454c1f-a726-589d-5585-7ee8c79c3434"), new Guid("44444444-4444-4444-4444-444444444444") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("008769f2-b432-ec12-a2a3-abf5f664c802"), new Guid("55555555-5555-5555-5555-555555555555") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("08965e65-eaba-3846-f3ca-5de5a9894e08"), new Guid("55555555-5555-5555-5555-555555555555") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("1b25fc03-7ad1-c69c-dae0-25daab39a24b"), new Guid("55555555-5555-5555-5555-555555555555") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("3e1115c6-b4da-0b86-6140-c77889a5dec1"), new Guid("55555555-5555-5555-5555-555555555555") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("496fc662-2331-b5ab-2dba-803c897dd134"), new Guid("55555555-5555-5555-5555-555555555555") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("7c06b883-0b51-f2eb-74b0-435447264e2f"), new Guid("55555555-5555-5555-5555-555555555555") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("9153963b-fc31-2e9b-2a99-d243b6081845"), new Guid("55555555-5555-5555-5555-555555555555") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("91bb8b66-0967-83ce-7056-629707d49a86"), new Guid("55555555-5555-5555-5555-555555555555") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("a1bb2e7a-c3c4-31c0-8a79-931946540a5e"), new Guid("55555555-5555-5555-5555-555555555555") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("c5355ae4-569e-3834-47be-60ae6c73b9c5"), new Guid("55555555-5555-5555-5555-555555555555") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("db454c1f-a726-589d-5585-7ee8c79c3434"), new Guid("55555555-5555-5555-5555-555555555555") });

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("008769f2-b432-ec12-a2a3-abf5f664c802"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("0798cb38-d699-bcc8-774f-87ac16dcb56d"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("1b25fc03-7ad1-c69c-dae0-25daab39a24b"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("416c3b00-b88a-f2d6-0f98-edd2530f4cfd"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("531335c1-be81-604d-927f-38492c2a3bdb"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("79a651a3-d1ae-03dd-9d16-6378fa5a8da3"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("7c06b883-0b51-f2eb-74b0-435447264e2f"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("91bb8b66-0967-83ce-7056-629707d49a86"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("98a8f386-db29-0ea2-98df-a340c0787c08"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("a1bb2e7a-c3c4-31c0-8a79-931946540a5e"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("a2da381b-d201-15a4-1949-e2e85417c86a"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("ec601469-ca78-db72-cd02-248fd76a3558"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("fea911ea-a325-1e7e-8feb-866ad4a9a5c4"));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("00d42e12-16d8-1161-d9c3-8198d016b61d"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7298), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7283) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("01fbb69d-2fe0-cfe2-60cc-9ddefdb59221"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7081), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7066) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("08965e65-eaba-3846-f3ca-5de5a9894e08"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7800), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7788) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("18637eb6-dd2d-4916-90e3-d190753fe651"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7377), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7365) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("19bddbc6-bc3d-33b5-9a93-ac104667687f"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7129), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7116) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("19fc086c-165d-67c5-668f-56661a36164d"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7145), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7132) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("1cf712ea-bf75-5da7-0315-f6abf938c90c"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7058), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7044) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("1f20e50d-312e-e12d-bbe3-9b176f4055dc"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7630), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7617) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("21bd6ce0-657d-9610-2fc5-34caf572fbe8"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7460), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7447) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("2821a053-365e-d274-2f88-c10912fd142b"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7199), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7185) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("28fc09cf-1d97-d3cc-66b9-e7a6210a3ced"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7232), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7218) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("2f7e4df2-8507-4ef5-625a-2f7959334de6"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7018), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(6955) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("35f5f086-45fa-c7c6-8a5e-ea8f9c97cea8"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(6932), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(6916) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("3e1115c6-b4da-0b86-6140-c77889a5dec1"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7786), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7773) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("40486d09-d640-93b6-125b-742b3fb9c30a"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7393), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7380) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("428a7672-6e0e-5486-0493-dd54a9d9ecb0"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7585), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7569) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("454b45b2-c9b0-ba27-b071-88c7ce4f9821"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7551), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7538) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("462495bb-1b19-10a6-a40f-a2f15ed27874"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7042), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7026) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("496fc662-2331-b5ab-2dba-803c897dd134"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7756), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7718) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("4a5e57f9-7371-ff34-a7c0-e6cf47f912c8"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(6885), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(6460) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("4c77f9fc-e86f-78b8-c88b-a2df097f6bd3"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7517), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7463) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("54d379ae-ad74-040a-c946-f56cd58ef72a"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7600), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7587) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("55674d3f-5e0b-2f4e-0123-849bc32efd83"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7097), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7084) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("62bf81bb-b05b-3dab-ab8e-46f87cf16bbf"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7651), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7638) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("641ccf8e-48a4-63f9-ea51-46a8f84e8275"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(6914), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(6895) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("676d33fc-eacf-fcc4-00a5-09c3521386f3"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7281), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7267) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("67c04dad-c1c4-c165-628f-62f4f39794d1"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7114), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7101) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("6bc3f860-b68c-8568-6564-78b6a17735fa"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7411), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7398) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("6f6232a5-e70c-eb97-a1e8-cfb0190dfc80"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7328), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7314) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("83bc6004-6954-1c64-f355-b54fa1e8f7cc"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7312), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7300) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("87a96f18-d58d-24eb-e677-ca0abfb99bec"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7361), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7348) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("89c37a1c-4545-d211-ab83-1785370f3c10"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7535), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7519) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("9153963b-fc31-2e9b-2a99-d243b6081845"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7815), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7802) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("ba2e8da4-a36d-4bc8-25fc-ae51d41fea7d"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7346), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7333) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("c4bdb36c-4205-1966-10fc-6b5701a69c42"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7699), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7686) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("c501c51b-6061-b652-d47f-8a593e35e5bb"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7615), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7602) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("c5355ae4-569e-3834-47be-60ae6c73b9c5"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7771), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7758) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("ccc21865-8cbd-f136-7f56-5c9551c5c120"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7161), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7147) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d2865fa6-328c-da22-aad5-12cd86505b1b"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7216), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7203) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("db454c1f-a726-589d-5585-7ee8c79c3434"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7445), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7432) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e35c101b-a88c-8804-7ccb-6f0c2a5cf1e1"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7713), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7701) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e6b7aa38-22c2-348d-5827-efeb05b4b45e"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7426), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7412) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e8031eea-0eef-f077-9aa5-c44e18fef35f"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7565), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7553) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e827ae98-f118-1f4e-e890-4cb036aba6b8"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7665), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7653) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f1d8a62c-3192-969d-5d75-025c2928d679"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(6953), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(6934) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f8edc86c-8f35-bc18-75a5-17cfec2310f0"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7684), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7669) });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[] { new Guid("9a3ba0a0-2914-e381-87df-4a00988bf036"), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7183), "Quyền cho Restaurants - ViewProfile", "Permissions.Restaurants.ViewProfile", new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7169) });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { new Guid("9a3ba0a0-2914-e381-87df-4a00988bf036"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("9a3ba0a0-2914-e381-87df-4a00988bf036"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("9a3ba0a0-2914-e381-87df-4a00988bf036"), new Guid("33333333-3333-3333-3333-333333333333") }
                });
        }
    }
}
