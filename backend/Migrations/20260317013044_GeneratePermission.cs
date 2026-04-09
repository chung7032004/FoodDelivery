using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FoodDelivery.Migrations
{
    /// <inheritdoc />
    public partial class GeneratePermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("1904d778-9ee3-4694-96fa-ad549bca5d1e"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("1920c0d7-57a7-4406-85fe-2454a559cc6c"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("1eac68e9-3305-4df4-ba8c-57968085255a"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("418bbdd8-43ac-431d-84b0-8a5a03e219c9"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("41faab76-d17c-4deb-90d7-b07106313c5b"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("431e3490-7ad6-498c-8e0c-59e9c7f69e8a"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("49eaebda-e018-4394-949a-a0cc69a2be83"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("54647cca-cbfc-457c-a2e4-5f43995dec67"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("58ca0ab1-aa99-4932-acd5-ae64ef428c2f"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("5a59adf8-a6dd-426a-abb6-dfeef0eed8ad"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("6970c7f5-8f97-43d7-85c7-d4455f9f59e4"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("765c626e-3a4a-4f19-9597-1239a83adddc"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("78c27066-25f7-4a3f-a0ce-a8de48b183e3"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("7917282e-a262-4fd6-9635-b6beba4df935"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("7c93ed24-6057-4750-b3c3-0e97b81084e3"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("8887fffa-3861-4486-84da-af74b04c0335"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("9a9ec8ae-c51c-4193-a3f0-97e9ca2369e9"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("9f39ca4b-8575-423c-8161-faf41dc04968"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("a2d247b8-1c62-4425-9dbc-ccc5d367ef2d"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("b13e14f2-6930-4a2e-b36c-fd8e52980808"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("cf5533f7-87d1-447f-a578-9de964bc187c"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d57f7ce1-6196-40b2-99a4-75671e59e294"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e2cd2710-5e18-4adb-b193-2cfe5242fa5e"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e544f7f8-fbb2-4427-ac0a-1c6f6668851a"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("edb9dbea-8e6d-4bd8-8787-12bab3fe3c2a"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f1cbafe9-3b35-4159-8746-96e93512e350"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f7aafeae-b0ec-4f2f-82da-e6e34cda690d"));

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("00d42e12-16d8-1161-d9c3-8198d016b61d"), new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(3052), "Quyền cho Staffs - Create", "Permissions.Staffs.Create", new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(3039) },
                    { new Guid("01fbb69d-2fe0-cfe2-60cc-9ddefdb59221"), new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(2850), "Quyền cho Orders - View", "Permissions.Orders.View", new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(2837) },
                    { new Guid("18637eb6-dd2d-4916-90e3-d190753fe651"), new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(3126), "Quyền cho Shippers - Block", "Permissions.Shippers.Block", new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(3114) },
                    { new Guid("19bddbc6-bc3d-33b5-9a93-ac104667687f"), new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(2899), "Quyền cho Orders - Cancel", "Permissions.Orders.Cancel", new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(2885) },
                    { new Guid("19fc086c-165d-67c5-668f-56661a36164d"), new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(2915), "Quyền cho Orders - UpdateStatus", "Permissions.Orders.UpdateStatus", new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(2900) },
                    { new Guid("1cf712ea-bf75-5da7-0315-f6abf938c90c"), new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(2833), "Quyền cho Categories - Manage", "Permissions.Categories.Manage", new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(2820) },
                    { new Guid("2821a053-365e-d274-2f88-c10912fd142b"), new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(2986), "Quyền cho Restaurants - UpdateProfile", "Permissions.Restaurants.UpdateProfile", new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(2974) },
                    { new Guid("28fc09cf-1d97-d3cc-66b9-e7a6210a3ced"), new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(3015), "Quyền cho Restaurants - ViewRevenue", "Permissions.Restaurants.ViewRevenue", new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(3002) },
                    { new Guid("2f7e4df2-8507-4ef5-625a-2f7959334de6"), new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(2795), "Quyền cho Products - ToggleAvailability", "Permissions.Products.ToggleAvailability", new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(2757) },
                    { new Guid("35f5f086-45fa-c7c6-8a5e-ea8f9c97cea8"), new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(2738), "Quyền cho Products - Edit", "Permissions.Products.Edit", new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(2724) },
                    { new Guid("40486d09-d640-93b6-125b-742b3fb9c30a"), new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(3206), "Quyền cho Shippers - AcceptOrder", "Permissions.Shippers.AcceptOrder", new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(3128) },
                    { new Guid("462495bb-1b19-10a6-a40f-a2f15ed27874"), new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(2817), "Quyền cho Categories - View", "Permissions.Categories.View", new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(2804) },
                    { new Guid("4a5e57f9-7371-ff34-a7c0-e6cf47f912c8"), new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(2684), "Quyền cho Products - View", "Permissions.Products.View", new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(2324) },
                    { new Guid("55674d3f-5e0b-2f4e-0123-849bc32efd83"), new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(2865), "Quyền cho Orders - Create", "Permissions.Orders.Create", new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(2852) },
                    { new Guid("641ccf8e-48a4-63f9-ea51-46a8f84e8275"), new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(2712), "Quyền cho Products - Create", "Permissions.Products.Create", new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(2691) },
                    { new Guid("676d33fc-eacf-fcc4-00a5-09c3521386f3"), new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(3036), "Quyền cho Staffs - View", "Permissions.Staffs.View", new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(3022) },
                    { new Guid("67c04dad-c1c4-c165-628f-62f4f39794d1"), new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(2882), "Quyền cho Orders - Confirm", "Permissions.Orders.Confirm", new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(2868) },
                    { new Guid("6bc3f860-b68c-8568-6564-78b6a17735fa"), new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(3225), "Quyền cho Users - View", "Permissions.Users.View", new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(3211) },
                    { new Guid("6f6232a5-e70c-eb97-a1e8-cfb0190dfc80"), new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(3081), "Quyền cho Staffs - Delete", "Permissions.Staffs.Delete", new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(3069) },
                    { new Guid("83bc6004-6954-1c64-f355-b54fa1e8f7cc"), new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(3067), "Quyền cho Staffs - Edit", "Permissions.Staffs.Edit", new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(3054) },
                    { new Guid("87a96f18-d58d-24eb-e677-ca0abfb99bec"), new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(3113), "Quyền cho Shippers - Verify", "Permissions.Shippers.Verify", new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(3100) },
                    { new Guid("9a3ba0a0-2914-e381-87df-4a00988bf036"), new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(2972), "Quyền cho Restaurants - ViewProfile", "Permissions.Restaurants.ViewProfile", new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(2956) },
                    { new Guid("ba2e8da4-a36d-4bc8-25fc-ae51d41fea7d"), new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(3098), "Quyền cho Shippers - View", "Permissions.Shippers.View", new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(3086) },
                    { new Guid("ccc21865-8cbd-f136-7f56-5c9551c5c120"), new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(2950), "Quyền cho Orders - AssignShipper", "Permissions.Orders.AssignShipper", new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(2917) },
                    { new Guid("d2865fa6-328c-da22-aad5-12cd86505b1b"), new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(3000), "Quyền cho Restaurants - ManageSettings", "Permissions.Restaurants.ManageSettings", new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(2988) },
                    { new Guid("e6b7aa38-22c2-348d-5827-efeb05b4b45e"), new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(3241), "Quyền cho Users - ManageReviews", "Permissions.Users.ManageReviews", new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(3228) },
                    { new Guid("f1d8a62c-3192-969d-5d75-025c2928d679"), new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(2756), "Quyền cho Products - Delete", "Permissions.Products.Delete", new DateTime(2026, 3, 17, 1, 30, 43, 774, DateTimeKind.Utc).AddTicks(2740) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("00d42e12-16d8-1161-d9c3-8198d016b61d"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("01fbb69d-2fe0-cfe2-60cc-9ddefdb59221"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("18637eb6-dd2d-4916-90e3-d190753fe651"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("19bddbc6-bc3d-33b5-9a93-ac104667687f"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("19fc086c-165d-67c5-668f-56661a36164d"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("1cf712ea-bf75-5da7-0315-f6abf938c90c"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("2821a053-365e-d274-2f88-c10912fd142b"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("28fc09cf-1d97-d3cc-66b9-e7a6210a3ced"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("2f7e4df2-8507-4ef5-625a-2f7959334de6"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("35f5f086-45fa-c7c6-8a5e-ea8f9c97cea8"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("40486d09-d640-93b6-125b-742b3fb9c30a"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("462495bb-1b19-10a6-a40f-a2f15ed27874"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("4a5e57f9-7371-ff34-a7c0-e6cf47f912c8"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("55674d3f-5e0b-2f4e-0123-849bc32efd83"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("641ccf8e-48a4-63f9-ea51-46a8f84e8275"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("676d33fc-eacf-fcc4-00a5-09c3521386f3"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("67c04dad-c1c4-c165-628f-62f4f39794d1"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("6bc3f860-b68c-8568-6564-78b6a17735fa"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("6f6232a5-e70c-eb97-a1e8-cfb0190dfc80"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("83bc6004-6954-1c64-f355-b54fa1e8f7cc"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("87a96f18-d58d-24eb-e677-ca0abfb99bec"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("9a3ba0a0-2914-e381-87df-4a00988bf036"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("ba2e8da4-a36d-4bc8-25fc-ae51d41fea7d"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("ccc21865-8cbd-f136-7f56-5c9551c5c120"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d2865fa6-328c-da22-aad5-12cd86505b1b"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e6b7aa38-22c2-348d-5827-efeb05b4b45e"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f1d8a62c-3192-969d-5d75-025c2928d679"));

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("1904d778-9ee3-4694-96fa-ad549bca5d1e"), new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2829), "Quyền cho Staffs - Delete", "Permissions.Staffs.Delete", new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2827) },
                    { new Guid("1920c0d7-57a7-4406-85fe-2454a559cc6c"), new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2846), "Quyền cho Shippers - Block", "Permissions.Shippers.Block", new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2843) },
                    { new Guid("1eac68e9-3305-4df4-ba8c-57968085255a"), new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2672), "Quyền cho Products - View", "Permissions.Products.View", new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2640) },
                    { new Guid("418bbdd8-43ac-431d-84b0-8a5a03e219c9"), new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2770), "Quyền cho Orders - Create", "Permissions.Orders.Create", new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2767) },
                    { new Guid("41faab76-d17c-4deb-90d7-b07106313c5b"), new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2862), "Quyền cho Users - ManageReviews", "Permissions.Users.ManageReviews", new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2859) },
                    { new Guid("431e3490-7ad6-498c-8e0c-59e9c7f69e8a"), new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2788), "Quyền cho Orders - AssignShipper", "Permissions.Orders.AssignShipper", new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2785) },
                    { new Guid("49eaebda-e018-4394-949a-a0cc69a2be83"), new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2780), "Quyền cho Orders - Cancel", "Permissions.Orders.Cancel", new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2777) },
                    { new Guid("54647cca-cbfc-457c-a2e4-5f43995dec67"), new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2699), "Quyền cho Products - Delete", "Permissions.Products.Delete", new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2696) },
                    { new Guid("58ca0ab1-aa99-4932-acd5-ae64ef428c2f"), new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2776), "Quyền cho Orders - Confirm", "Permissions.Orders.Confirm", new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2772) },
                    { new Guid("5a59adf8-a6dd-426a-abb6-dfeef0eed8ad"), new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2795), "Quyền cho Restaurants - ViewProfile", "Permissions.Restaurants.ViewProfile", new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2791) },
                    { new Guid("6970c7f5-8f97-43d7-85c7-d4455f9f59e4"), new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2818), "Quyền cho Staffs - View", "Permissions.Staffs.View", new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2814) },
                    { new Guid("765c626e-3a4a-4f19-9597-1239a83adddc"), new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2822), "Quyền cho Staffs - Create", "Permissions.Staffs.Create", new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2819) },
                    { new Guid("78c27066-25f7-4a3f-a0ce-a8de48b183e3"), new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2749), "Quyền cho Categories - View", "Permissions.Categories.View", new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2708) },
                    { new Guid("7917282e-a262-4fd6-9635-b6beba4df935"), new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2851), "Quyền cho Shippers - AcceptOrder", "Permissions.Shippers.AcceptOrder", new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2848) },
                    { new Guid("7c93ed24-6057-4750-b3c3-0e97b81084e3"), new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2858), "Quyền cho Users - View", "Permissions.Users.View", new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2854) },
                    { new Guid("8887fffa-3861-4486-84da-af74b04c0335"), new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2704), "Quyền cho Products - ToggleAvailability", "Permissions.Products.ToggleAvailability", new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2701) },
                    { new Guid("9a9ec8ae-c51c-4193-a3f0-97e9ca2369e9"), new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2826), "Quyền cho Staffs - Edit", "Permissions.Staffs.Edit", new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2823) },
                    { new Guid("9f39ca4b-8575-423c-8161-faf41dc04968"), new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2842), "Quyền cho Shippers - Verify", "Permissions.Shippers.Verify", new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2839) },
                    { new Guid("a2d247b8-1c62-4425-9dbc-ccc5d367ef2d"), new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2805), "Quyền cho Restaurants - ManageSettings", "Permissions.Restaurants.ManageSettings", new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2801) },
                    { new Guid("b13e14f2-6930-4a2e-b36c-fd8e52980808"), new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2681), "Quyền cho Products - Create", "Permissions.Products.Create", new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2675) },
                    { new Guid("cf5533f7-87d1-447f-a578-9de964bc187c"), new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2837), "Quyền cho Shippers - View", "Permissions.Shippers.View", new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2832) },
                    { new Guid("d57f7ce1-6196-40b2-99a4-75671e59e294"), new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2800), "Quyền cho Restaurants - UpdateProfile", "Permissions.Restaurants.UpdateProfile", new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2797) },
                    { new Guid("e2cd2710-5e18-4adb-b193-2cfe5242fa5e"), new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2810), "Quyền cho Restaurants - ViewRevenue", "Permissions.Restaurants.ViewRevenue", new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2806) },
                    { new Guid("e544f7f8-fbb2-4427-ac0a-1c6f6668851a"), new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2783), "Quyền cho Orders - UpdateStatus", "Permissions.Orders.UpdateStatus", new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2781) },
                    { new Guid("edb9dbea-8e6d-4bd8-8787-12bab3fe3c2a"), new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2755), "Quyền cho Categories - Manage", "Permissions.Categories.Manage", new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2752) },
                    { new Guid("f1cbafe9-3b35-4159-8746-96e93512e350"), new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2766), "Quyền cho Orders - View", "Permissions.Orders.View", new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2762) },
                    { new Guid("f7aafeae-b0ec-4f2f-82da-e6e34cda690d"), new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2694), "Quyền cho Products - Edit", "Permissions.Products.Edit", new DateTime(2026, 3, 17, 1, 17, 9, 920, DateTimeKind.Utc).AddTicks(2683) }
                });
        }
    }
}
