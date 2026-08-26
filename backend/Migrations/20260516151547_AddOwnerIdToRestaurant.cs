using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDelivery.Migrations
{
    /// <inheritdoc />
    public partial class AddOwnerIdToRestaurant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "Restaurants",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("008769f2-b432-ec12-a2a3-abf5f664c802"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(662), new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(650) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("00d42e12-16d8-1161-d9c3-8198d016b61d"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(78), new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(43) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("01fbb69d-2fe0-cfe2-60cc-9ddefdb59221"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9366), new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9354) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("0798cb38-d699-bcc8-774f-87ac16dcb56d"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9553), new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9539) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("08965e65-eaba-3846-f3ca-5de5a9894e08"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(614), new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(601) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("18637eb6-dd2d-4916-90e3-d190753fe651"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(162), new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(151) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("1972a37c-ad35-0c73-ec42-8f1e18925e33"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(790), new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(777) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("19bddbc6-bc3d-33b5-9a93-ac104667687f"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9449), new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9438) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("19fc086c-165d-67c5-668f-56661a36164d"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9466), new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9453) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("1b25fc03-7ad1-c69c-dae0-25daab39a24b"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(677), new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(665) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("1cf712ea-bf75-5da7-0315-f6abf938c90c"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9344), new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9330) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("1f20e50d-312e-e12d-bbe3-9b176f4055dc"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(449), new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(436) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("21bd6ce0-657d-9610-2fc5-34caf572fbe8"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(250), new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(238) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("2821a053-365e-d274-2f88-c10912fd142b"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9654), new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9642) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("28fc09cf-1d97-d3cc-66b9-e7a6210a3ced"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9966), new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9823) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("2e9b21fa-0456-1c67-b02f-77c8ffbdd1bc"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(706), new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(694) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("2f7e4df2-8507-4ef5-625a-2f7959334de6"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9299), new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9245) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("35f5f086-45fa-c7c6-8a5e-ea8f9c97cea8"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9223), new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9205) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("3e1115c6-b4da-0b86-6140-c77889a5dec1"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(599), new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(587) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("40486d09-d640-93b6-125b-742b3fb9c30a"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(180), new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(167) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("408a4c2e-b781-797e-1ac0-462ac106ba27"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(825), new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(812) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("416c3b00-b88a-f2d6-0f98-edd2530f4cfd"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9572), new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9558) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("428a7672-6e0e-5486-0493-dd54a9d9ecb0"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(402), new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(390) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("454b45b2-c9b0-ba27-b071-88c7ce4f9821"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(372), new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(310) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("462495bb-1b19-10a6-a40f-a2f15ed27874"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9327), new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9312) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("496fc662-2331-b5ab-2dba-803c897dd134"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(567), new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(554) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("4a5e57f9-7371-ff34-a7c0-e6cf47f912c8"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9135), new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(8795) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("4c77f9fc-e86f-78b8-c88b-a2df097f6bd3"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(290), new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(273) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("5273f960-e88a-b69b-c87c-aa16332b9d5d"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(774), new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(746) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("531335c1-be81-604d-927f-38492c2a3bdb"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9621), new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9608) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("54d379ae-ad74-040a-c946-f56cd58ef72a"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(416), new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(405) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("55674d3f-5e0b-2f4e-0123-849bc32efd83"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9420), new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9408) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("62bf81bb-b05b-3dab-ab8e-46f87cf16bbf"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(468), new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(455) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("641ccf8e-48a4-63f9-ea51-46a8f84e8275"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9201), new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9182) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("676d33fc-eacf-fcc4-00a5-09c3521386f3"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(40), new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(27) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("67c04dad-c1c4-c165-628f-62f4f39794d1"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9435), new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9423) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("6bc3f860-b68c-8568-6564-78b6a17735fa"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(198), new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(186) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("6f6232a5-e70c-eb97-a1e8-cfb0190dfc80"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(115), new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(101) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("745be01f-d2c8-8cca-6684-f322171229ff"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9639), new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9626) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("79a651a3-d1ae-03dd-9d16-6378fa5a8da3"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9387), new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9375) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("7c06b883-0b51-f2eb-74b0-435447264e2f"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9498), new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9484) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("83bc6004-6954-1c64-f355-b54fa1e8f7cc"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(96), new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(81) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("87a96f18-d58d-24eb-e677-ca0abfb99bec"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(148), new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(136) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("89c37a1c-4545-d211-ab83-1785370f3c10"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(307), new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(294) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("9153963b-fc31-2e9b-2a99-d243b6081845"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(629), new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(617) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("91bb8b66-0967-83ce-7056-629707d49a86"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9405), new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9392) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("924c4e8c-1963-ec6b-7602-4a7700971a14"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(727), new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(714) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("98a8f386-db29-0ea2-98df-a340c0787c08"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9604), new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9592) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("a1bb2e7a-c3c4-31c0-8a79-931946540a5e"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(647), new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(635) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("a2da381b-d201-15a4-1949-e2e85417c86a"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9535), new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9501) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("ba2e8da4-a36d-4bc8-25fc-ae51d41fea7d"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(133), new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(120) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("c4bdb36c-4205-1966-10fc-6b5701a69c42"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(513), new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(500) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("c501c51b-6061-b652-d47f-8a593e35e5bb"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(432), new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(418) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("c5355ae4-569e-3834-47be-60ae6c73b9c5"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(583), new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(570) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("ccc21865-8cbd-f136-7f56-5c9551c5c120"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9481), new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9469) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d2865fa6-328c-da22-aad5-12cd86505b1b"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9820), new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9657) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d8d648d0-c0f5-523a-3dda-d95613982d96"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(809), new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(797) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("db454c1f-a726-589d-5585-7ee8c79c3434"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(233), new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(219) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e35c101b-a88c-8804-7ccb-6f0c2a5cf1e1"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(548), new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(536) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e6b7aa38-22c2-348d-5827-efeb05b4b45e"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(212), new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(201) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e8031eea-0eef-f077-9aa5-c44e18fef35f"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(387), new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(375) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e827ae98-f118-1f4e-e890-4cb036aba6b8"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(483), new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(471) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("ec601469-ca78-db72-cd02-248fd76a3558"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9589), new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9576) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("ee00f29c-4500-640d-cb1d-8909ae281747"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(743), new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(731) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f1d8a62c-3192-969d-5d75-025c2928d679"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9242), new DateTime(2026, 5, 16, 15, 15, 46, 535, DateTimeKind.Utc).AddTicks(9226) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f8edc86c-8f35-bc18-75a5-17cfec2310f0"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(498), new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(485) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("fea911ea-a325-1e7e-8feb-866ad4a9a5c4"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(692), new DateTime(2026, 5, 16, 15, 15, 46, 536, DateTimeKind.Utc).AddTicks(679) });

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_OwnerId",
                table: "Restaurants",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Users_OwnerId",
                table: "Restaurants",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Users_OwnerId",
                table: "Restaurants");

            migrationBuilder.DropIndex(
                name: "IX_Restaurants_OwnerId",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Restaurants");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("008769f2-b432-ec12-a2a3-abf5f664c802"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4923), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4910) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("00d42e12-16d8-1161-d9c3-8198d016b61d"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4400), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4386) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("01fbb69d-2fe0-cfe2-60cc-9ddefdb59221"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4023), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4010) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("0798cb38-d699-bcc8-774f-87ac16dcb56d"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4215), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4201) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("08965e65-eaba-3846-f3ca-5de5a9894e08"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4874), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4861) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("18637eb6-dd2d-4916-90e3-d190753fe651"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4477), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4465) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("1972a37c-ad35-0c73-ec42-8f1e18925e33"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(5069), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(5057) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("19bddbc6-bc3d-33b5-9a93-ac104667687f"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4109), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4095) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("19fc086c-165d-67c5-668f-56661a36164d"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4124), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4111) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("1b25fc03-7ad1-c69c-dae0-25daab39a24b"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4939), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4927) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("1cf712ea-bf75-5da7-0315-f6abf938c90c"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4002), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(3989) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("1f20e50d-312e-e12d-bbe3-9b176f4055dc"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4713), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4701) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("21bd6ce0-657d-9610-2fc5-34caf572fbe8"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4567), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4554) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("2821a053-365e-d274-2f88-c10912fd142b"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4314), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4301) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("28fc09cf-1d97-d3cc-66b9-e7a6210a3ced"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4343), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4331) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("2e9b21fa-0456-1c67-b02f-77c8ffbdd1bc"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4969), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4956) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("2f7e4df2-8507-4ef5-625a-2f7959334de6"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(3963), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(3930) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("35f5f086-45fa-c7c6-8a5e-ea8f9c97cea8"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(3885), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(3873) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("3e1115c6-b4da-0b86-6140-c77889a5dec1"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4859), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4845) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("40486d09-d640-93b6-125b-742b3fb9c30a"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4494), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4481) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("408a4c2e-b781-797e-1ac0-462ac106ba27"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(5109), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(5096) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("416c3b00-b88a-f2d6-0f98-edd2530f4cfd"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4234), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4220) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("428a7672-6e0e-5486-0493-dd54a9d9ecb0"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4670), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4657) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("454b45b2-c9b0-ba27-b071-88c7ce4f9821"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4638), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4625) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("462495bb-1b19-10a6-a40f-a2f15ed27874"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(3986), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(3971) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("496fc662-2331-b5ab-2dba-803c897dd134"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4824), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4811) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("4a5e57f9-7371-ff34-a7c0-e6cf47f912c8"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(3813), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(3483) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("4c77f9fc-e86f-78b8-c88b-a2df097f6bd3"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4608), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4593) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("5273f960-e88a-b69b-c87c-aa16332b9d5d"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(5054), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(5041) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("531335c1-be81-604d-927f-38492c2a3bdb"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4281), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4267) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("54d379ae-ad74-040a-c946-f56cd58ef72a"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4684), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4672) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("55674d3f-5e0b-2f4e-0123-849bc32efd83"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4079), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4066) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("62bf81bb-b05b-3dab-ab8e-46f87cf16bbf"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4733), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4720) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("641ccf8e-48a4-63f9-ea51-46a8f84e8275"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(3860), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(3845) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("676d33fc-eacf-fcc4-00a5-09c3521386f3"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4384), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4350) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("67c04dad-c1c4-c165-628f-62f4f39794d1"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4093), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4081) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("6bc3f860-b68c-8568-6564-78b6a17735fa"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4516), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4503) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("6f6232a5-e70c-eb97-a1e8-cfb0190dfc80"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4430), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4418) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("745be01f-d2c8-8cca-6684-f322171229ff"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4299), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4286) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("79a651a3-d1ae-03dd-9d16-6378fa5a8da3"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4044), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4030) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("7c06b883-0b51-f2eb-74b0-435447264e2f"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4180), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4141) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("83bc6004-6954-1c64-f355-b54fa1e8f7cc"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4416), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4403) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("87a96f18-d58d-24eb-e677-ca0abfb99bec"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4462), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4450) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("89c37a1c-4545-d211-ab83-1785370f3c10"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4623), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4610) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("9153963b-fc31-2e9b-2a99-d243b6081845"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4888), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4876) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("91bb8b66-0967-83ce-7056-629707d49a86"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4062), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4048) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("924c4e8c-1963-ec6b-7602-4a7700971a14"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4988), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4975) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("98a8f386-db29-0ea2-98df-a340c0787c08"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4264), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4252) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("a1bb2e7a-c3c4-31c0-8a79-931946540a5e"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4908), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4894) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("a2da381b-d201-15a4-1949-e2e85417c86a"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4197), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4183) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("ba2e8da4-a36d-4bc8-25fc-ae51d41fea7d"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4447), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4435) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("c4bdb36c-4205-1966-10fc-6b5701a69c42"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4788), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4764) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("c501c51b-6061-b652-d47f-8a593e35e5bb"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4698), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4686) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("c5355ae4-569e-3834-47be-60ae6c73b9c5"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4843), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4829) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("ccc21865-8cbd-f136-7f56-5c9551c5c120"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4139), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4126) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d2865fa6-328c-da22-aad5-12cd86505b1b"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4329), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4316) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d8d648d0-c0f5-523a-3dda-d95613982d96"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(5092), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(5078) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("db454c1f-a726-589d-5585-7ee8c79c3434"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4552), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4539) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e35c101b-a88c-8804-7ccb-6f0c2a5cf1e1"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4806), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4792) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e6b7aa38-22c2-348d-5827-efeb05b4b45e"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4533), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4519) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e8031eea-0eef-f077-9aa5-c44e18fef35f"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4653), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4640) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e827ae98-f118-1f4e-e890-4cb036aba6b8"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4747), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4735) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("ec601469-ca78-db72-cd02-248fd76a3558"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4249), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4237) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("ee00f29c-4500-640d-cb1d-8909ae281747"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(5038), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4991) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f1d8a62c-3192-969d-5d75-025c2928d679"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(3927), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(3888) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f8edc86c-8f35-bc18-75a5-17cfec2310f0"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4762), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4750) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("fea911ea-a325-1e7e-8feb-866ad4a9a5c4"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4954), new DateTime(2026, 5, 10, 17, 31, 31, 331, DateTimeKind.Utc).AddTicks(4942) });
        }
    }
}
