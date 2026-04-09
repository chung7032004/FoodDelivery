using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FoodDelivery.Migrations
{
    /// <inheritdoc />
    public partial class SeedRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("0b4f1ade-5fa8-4a85-bf1c-e9d8f92418cd"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("114aa2d6-ad63-476d-97bb-88ba1891c95c"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("220b1967-31f4-4616-aed1-1d5b5e1302a8"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("2d6da130-d2a1-4663-99fb-ed7023bb8bfb"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("38e9c4d2-3cad-477f-8939-9a0469aca114"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("3e536c8f-b6ce-4412-852d-80626cc5ea55"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("448e8d7e-fb5a-461c-b546-338e2971bdf2"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("45808413-4d98-46a6-8d9a-e5e455c8353b"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("578ba995-4c1c-45f4-acfd-db7584f55882"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("600002fd-a447-404d-ade6-fd26ca9cacd7"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("68d51e6e-90ae-486d-8f95-7331a51b7631"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("7f2063fb-e882-4f6e-83d1-11abf38bd0f3"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("a0c4bb98-9d5f-489c-b905-b07f06ccf424"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("a1a70ea5-2744-42e3-81e8-970236167053"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("b03bfdfa-ffe4-49bd-b987-45ada9a913f2"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("be2913a6-55d2-473e-bf70-d688eac05c9e"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("c956fc39-2f27-43ab-8926-ed753b9d80c1"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("ce35f751-f68c-46c9-8733-5e1d9c1a9010"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d7397a26-f326-45c9-8170-626675e80582"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d82a2e62-b83b-4e2a-b315-ec884ee62a7a"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d9f6b42e-c7e5-43ee-b231-baff04e877ea"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e0bc922a-4eb3-43aa-8bab-8f17db001a41"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e6838849-8d2c-43d3-89a4-a2a4fb8ab93b"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e6bd2478-101d-4c8c-9718-6ceba34209de"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("ee5c2867-02be-44ea-ab5a-1c471b0e45e5"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f6ed5a23-7eb1-4cfe-a4a5-0591fad4ac0f"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("fd0eb88a-9f3c-45e8-9658-0be7c5642ef7"));

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

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

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Admin" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "RestaurantOwner" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "RestaurantStaff" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), "Shipper" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), "Customer" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"));

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("0b4f1ade-5fa8-4a85-bf1c-e9d8f92418cd"), new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7370), "Quyền cho Shippers - Verify", "Permissions.Shippers.Verify", new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7367) },
                    { new Guid("114aa2d6-ad63-476d-97bb-88ba1891c95c"), new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7239), "Quyền cho Orders - View", "Permissions.Orders.View", new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7235) },
                    { new Guid("220b1967-31f4-4616-aed1-1d5b5e1302a8"), new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7232), "Quyền cho Categories - Manage", "Permissions.Categories.Manage", new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7229) },
                    { new Guid("2d6da130-d2a1-4663-99fb-ed7023bb8bfb"), new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7350), "Quyền cho Staffs - Create", "Permissions.Staffs.Create", new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7347) },
                    { new Guid("38e9c4d2-3cad-477f-8939-9a0469aca114"), new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7336), "Quyền cho Restaurants - ViewRevenue", "Permissions.Restaurants.ViewRevenue", new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7332) },
                    { new Guid("3e536c8f-b6ce-4412-852d-80626cc5ea55"), new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7315), "Quyền cho Orders - AssignShipper", "Permissions.Orders.AssignShipper", new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7312) },
                    { new Guid("448e8d7e-fb5a-461c-b546-338e2971bdf2"), new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7310), "Quyền cho Orders - UpdateStatus", "Permissions.Orders.UpdateStatus", new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7307) },
                    { new Guid("45808413-4d98-46a6-8d9a-e5e455c8353b"), new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7387), "Quyền cho Users - View", "Permissions.Users.View", new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7384) },
                    { new Guid("578ba995-4c1c-45f4-acfd-db7584f55882"), new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7327), "Quyền cho Restaurants - UpdateProfile", "Permissions.Restaurants.UpdateProfile", new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7324) },
                    { new Guid("600002fd-a447-404d-ade6-fd26ca9cacd7"), new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7395), "Quyền cho Users - ManageReviews", "Permissions.Users.ManageReviews", new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7390) },
                    { new Guid("68d51e6e-90ae-486d-8f95-7331a51b7631"), new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7244), "Quyền cho Orders - Create", "Permissions.Orders.Create", new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7240) },
                    { new Guid("7f2063fb-e882-4f6e-83d1-11abf38bd0f3"), new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7354), "Quyền cho Staffs - Edit", "Permissions.Staffs.Edit", new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7352) },
                    { new Guid("a0c4bb98-9d5f-489c-b905-b07f06ccf424"), new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7063), "Quyền cho Products - View", "Permissions.Products.View", new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7026) },
                    { new Guid("a1a70ea5-2744-42e3-81e8-970236167053"), new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7071), "Quyền cho Products - Create", "Permissions.Products.Create", new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7067) },
                    { new Guid("b03bfdfa-ffe4-49bd-b987-45ada9a913f2"), new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7358), "Quyền cho Staffs - Delete", "Permissions.Staffs.Delete", new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7356) },
                    { new Guid("be2913a6-55d2-473e-bf70-d688eac05c9e"), new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7251), "Quyền cho Orders - Confirm", "Permissions.Orders.Confirm", new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7247) },
                    { new Guid("c956fc39-2f27-43ab-8926-ed753b9d80c1"), new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7225), "Quyền cho Categories - View", "Permissions.Categories.View", new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7222) },
                    { new Guid("ce35f751-f68c-46c9-8733-5e1d9c1a9010"), new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7381), "Quyền cho Shippers - AcceptOrder", "Permissions.Shippers.AcceptOrder", new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7377) },
                    { new Guid("d7397a26-f326-45c9-8170-626675e80582"), new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7158), "Quyền cho Products - Edit", "Permissions.Products.Edit", new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7154) },
                    { new Guid("d82a2e62-b83b-4e2a-b315-ec884ee62a7a"), new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7305), "Quyền cho Orders - Cancel", "Permissions.Orders.Cancel", new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7254) },
                    { new Guid("d9f6b42e-c7e5-43ee-b231-baff04e877ea"), new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7321), "Quyền cho Restaurants - ViewProfile", "Permissions.Restaurants.ViewProfile", new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7318) },
                    { new Guid("e0bc922a-4eb3-43aa-8bab-8f17db001a41"), new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7364), "Quyền cho Shippers - View", "Permissions.Shippers.View", new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7362) },
                    { new Guid("e6838849-8d2c-43d3-89a4-a2a4fb8ab93b"), new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7211), "Quyền cho Products - Delete", "Permissions.Products.Delete", new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7159) },
                    { new Guid("e6bd2478-101d-4c8c-9718-6ceba34209de"), new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7215), "Quyền cho Products - ToggleAvailability", "Permissions.Products.ToggleAvailability", new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7212) },
                    { new Guid("ee5c2867-02be-44ea-ab5a-1c471b0e45e5"), new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7344), "Quyền cho Staffs - View", "Permissions.Staffs.View", new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7341) },
                    { new Guid("f6ed5a23-7eb1-4cfe-a4a5-0591fad4ac0f"), new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7331), "Quyền cho Restaurants - ManageSettings", "Permissions.Restaurants.ManageSettings", new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7328) },
                    { new Guid("fd0eb88a-9f3c-45e8-9658-0be7c5642ef7"), new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7375), "Quyền cho Shippers - Block", "Permissions.Shippers.Block", new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(7371) }
                });

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "Id", "Address", "CloseTime", "ClosingMessage", "CreatedAt", "IsOpen", "Latitude", "Longitude", "Name", "OpenTime", "Phone", "UpdatedAt" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), "319 Hùng Vương, P. Vĩnh Trung, Q. Thanh Khê, Đà Nẵng", new TimeSpan(0, 22, 0, 0, 0), null, new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(6754), true, 16.067771, 108.214287, "Food Delivery Shop", new TimeSpan(0, 8, 0, 0, 0), "0909123456", new DateTime(2026, 3, 15, 9, 7, 41, 641, DateTimeKind.Utc).AddTicks(6755) });
        }
    }
}
