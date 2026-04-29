using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FoodDelivery.Migrations
{
    /// <inheritdoc />
    public partial class AddPermission_v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                keyValue: new Guid("40486d09-d640-93b6-125b-742b3fb9c30a"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7393), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7380) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("462495bb-1b19-10a6-a40f-a2f15ed27874"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7042), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7026) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("4a5e57f9-7371-ff34-a7c0-e6cf47f912c8"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(6885), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(6460) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("55674d3f-5e0b-2f4e-0123-849bc32efd83"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7097), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7084) });

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
                keyValue: new Guid("9a3ba0a0-2914-e381-87df-4a00988bf036"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7183), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7169) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("ba2e8da4-a36d-4bc8-25fc-ae51d41fea7d"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7346), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7333) });

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
                keyValue: new Guid("e6b7aa38-22c2-348d-5827-efeb05b4b45e"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7426), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7412) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f1d8a62c-3192-969d-5d75-025c2928d679"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(6953), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(6934) });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("08965e65-eaba-3846-f3ca-5de5a9894e08"), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7800), "Quyền cho Cart - RemoveItem", "Permissions.Cart.RemoveItem", new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7788) },
                    { new Guid("1f20e50d-312e-e12d-bbe3-9b176f4055dc"), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7630), "Quyền cho Accounts - AssignRole", "Permissions.Accounts.AssignRole", new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7617) },
                    { new Guid("21bd6ce0-657d-9610-2fc5-34caf572fbe8"), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7460), "Quyền cho Accounts - UpdateOwn", "Permissions.Accounts.UpdateOwn", new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7447) },
                    { new Guid("3e1115c6-b4da-0b86-6140-c77889a5dec1"), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7786), "Quyền cho Cart - UpdateItem", "Permissions.Cart.UpdateItem", new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7773) },
                    { new Guid("428a7672-6e0e-5486-0493-dd54a9d9ecb0"), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7585), "Quyền cho Accounts - Delete", "Permissions.Accounts.Delete", new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7569) },
                    { new Guid("454b45b2-c9b0-ba27-b071-88c7ce4f9821"), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7551), "Quyền cho Accounts - View", "Permissions.Accounts.View", new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7538) },
                    { new Guid("496fc662-2331-b5ab-2dba-803c897dd134"), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7756), "Quyền cho Cart - View", "Permissions.Cart.View", new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7718) },
                    { new Guid("4c77f9fc-e86f-78b8-c88b-a2df097f6bd3"), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7517), "Quyền cho Accounts - ChangePassword", "Permissions.Accounts.ChangePassword", new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7463) },
                    { new Guid("54d379ae-ad74-040a-c946-f56cd58ef72a"), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7600), "Quyền cho Accounts - Lock", "Permissions.Accounts.Lock", new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7587) },
                    { new Guid("62bf81bb-b05b-3dab-ab8e-46f87cf16bbf"), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7651), "Quyền cho Address - View", "Permissions.Address.View", new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7638) },
                    { new Guid("89c37a1c-4545-d211-ab83-1785370f3c10"), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7535), "Quyền cho Accounts - ViewAll", "Permissions.Accounts.ViewAll", new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7519) },
                    { new Guid("9153963b-fc31-2e9b-2a99-d243b6081845"), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7815), "Quyền cho Cart - ClearCart", "Permissions.Cart.ClearCart", new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7802) },
                    { new Guid("c4bdb36c-4205-1966-10fc-6b5701a69c42"), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7699), "Quyền cho Address - Delete", "Permissions.Address.Delete", new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7686) },
                    { new Guid("c501c51b-6061-b652-d47f-8a593e35e5bb"), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7615), "Quyền cho Accounts - Unlock", "Permissions.Accounts.Unlock", new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7602) },
                    { new Guid("c5355ae4-569e-3834-47be-60ae6c73b9c5"), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7771), "Quyền cho Cart - AddItem", "Permissions.Cart.AddItem", new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7758) },
                    { new Guid("db454c1f-a726-589d-5585-7ee8c79c3434"), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7445), "Quyền cho Accounts - ViewOwn", "Permissions.Accounts.ViewOwn", new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7432) },
                    { new Guid("e35c101b-a88c-8804-7ccb-6f0c2a5cf1e1"), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7713), "Quyền cho Address - SetDefault", "Permissions.Address.SetDefault", new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7701) },
                    { new Guid("e8031eea-0eef-f077-9aa5-c44e18fef35f"), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7565), "Quyền cho Accounts - Edit", "Permissions.Accounts.Edit", new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7553) },
                    { new Guid("e827ae98-f118-1f4e-e890-4cb036aba6b8"), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7665), "Quyền cho Address - Create", "Permissions.Address.Create", new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7653) },
                    { new Guid("f8edc86c-8f35-bc18-75a5-17cfec2310f0"), new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7684), "Quyền cho Address - Edit", "Permissions.Address.Edit", new DateTime(2026, 4, 29, 10, 53, 14, 256, DateTimeKind.Utc).AddTicks(7669) }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { new Guid("6bc3f860-b68c-8568-6564-78b6a17735fa"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("ccc21865-8cbd-f136-7f56-5c9551c5c120"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("e6b7aa38-22c2-348d-5827-efeb05b4b45e"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("462495bb-1b19-10a6-a40f-a2f15ed27874"), new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("9a3ba0a0-2914-e381-87df-4a00988bf036"), new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("19fc086c-165d-67c5-668f-56661a36164d"), new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("08965e65-eaba-3846-f3ca-5de5a9894e08"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("1f20e50d-312e-e12d-bbe3-9b176f4055dc"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("21bd6ce0-657d-9610-2fc5-34caf572fbe8"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("3e1115c6-b4da-0b86-6140-c77889a5dec1"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("428a7672-6e0e-5486-0493-dd54a9d9ecb0"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("454b45b2-c9b0-ba27-b071-88c7ce4f9821"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("496fc662-2331-b5ab-2dba-803c897dd134"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("4c77f9fc-e86f-78b8-c88b-a2df097f6bd3"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("54d379ae-ad74-040a-c946-f56cd58ef72a"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("62bf81bb-b05b-3dab-ab8e-46f87cf16bbf"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("89c37a1c-4545-d211-ab83-1785370f3c10"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("9153963b-fc31-2e9b-2a99-d243b6081845"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("c4bdb36c-4205-1966-10fc-6b5701a69c42"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("c501c51b-6061-b652-d47f-8a593e35e5bb"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("c5355ae4-569e-3834-47be-60ae6c73b9c5"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("db454c1f-a726-589d-5585-7ee8c79c3434"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("e35c101b-a88c-8804-7ccb-6f0c2a5cf1e1"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("e8031eea-0eef-f077-9aa5-c44e18fef35f"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("e827ae98-f118-1f4e-e890-4cb036aba6b8"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("f8edc86c-8f35-bc18-75a5-17cfec2310f0"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("454b45b2-c9b0-ba27-b071-88c7ce4f9821"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("454b45b2-c9b0-ba27-b071-88c7ce4f9821"), new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("21bd6ce0-657d-9610-2fc5-34caf572fbe8"), new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("454b45b2-c9b0-ba27-b071-88c7ce4f9821"), new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("4c77f9fc-e86f-78b8-c88b-a2df097f6bd3"), new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("21bd6ce0-657d-9610-2fc5-34caf572fbe8"), new Guid("55555555-5555-5555-5555-555555555555") },
                    { new Guid("454b45b2-c9b0-ba27-b071-88c7ce4f9821"), new Guid("55555555-5555-5555-5555-555555555555") },
                    { new Guid("4c77f9fc-e86f-78b8-c88b-a2df097f6bd3"), new Guid("55555555-5555-5555-5555-555555555555") },
                    { new Guid("62bf81bb-b05b-3dab-ab8e-46f87cf16bbf"), new Guid("55555555-5555-5555-5555-555555555555") },
                    { new Guid("c4bdb36c-4205-1966-10fc-6b5701a69c42"), new Guid("55555555-5555-5555-5555-555555555555") },
                    { new Guid("e35c101b-a88c-8804-7ccb-6f0c2a5cf1e1"), new Guid("55555555-5555-5555-5555-555555555555") },
                    { new Guid("e827ae98-f118-1f4e-e890-4cb036aba6b8"), new Guid("55555555-5555-5555-5555-555555555555") },
                    { new Guid("f8edc86c-8f35-bc18-75a5-17cfec2310f0"), new Guid("55555555-5555-5555-5555-555555555555") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("08965e65-eaba-3846-f3ca-5de5a9894e08"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("1f20e50d-312e-e12d-bbe3-9b176f4055dc"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("21bd6ce0-657d-9610-2fc5-34caf572fbe8"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("3e1115c6-b4da-0b86-6140-c77889a5dec1"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("428a7672-6e0e-5486-0493-dd54a9d9ecb0"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("454b45b2-c9b0-ba27-b071-88c7ce4f9821"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("496fc662-2331-b5ab-2dba-803c897dd134"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("4c77f9fc-e86f-78b8-c88b-a2df097f6bd3"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("54d379ae-ad74-040a-c946-f56cd58ef72a"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("62bf81bb-b05b-3dab-ab8e-46f87cf16bbf"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("89c37a1c-4545-d211-ab83-1785370f3c10"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("9153963b-fc31-2e9b-2a99-d243b6081845"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("c4bdb36c-4205-1966-10fc-6b5701a69c42"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("c501c51b-6061-b652-d47f-8a593e35e5bb"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("c5355ae4-569e-3834-47be-60ae6c73b9c5"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("db454c1f-a726-589d-5585-7ee8c79c3434"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("e35c101b-a88c-8804-7ccb-6f0c2a5cf1e1"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("e8031eea-0eef-f077-9aa5-c44e18fef35f"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("e827ae98-f118-1f4e-e890-4cb036aba6b8"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("f8edc86c-8f35-bc18-75a5-17cfec2310f0"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("454b45b2-c9b0-ba27-b071-88c7ce4f9821"), new Guid("22222222-2222-2222-2222-222222222222") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("6bc3f860-b68c-8568-6564-78b6a17735fa"), new Guid("22222222-2222-2222-2222-222222222222") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("ccc21865-8cbd-f136-7f56-5c9551c5c120"), new Guid("22222222-2222-2222-2222-222222222222") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("e6b7aa38-22c2-348d-5827-efeb05b4b45e"), new Guid("22222222-2222-2222-2222-222222222222") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("454b45b2-c9b0-ba27-b071-88c7ce4f9821"), new Guid("33333333-3333-3333-3333-333333333333") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("462495bb-1b19-10a6-a40f-a2f15ed27874"), new Guid("33333333-3333-3333-3333-333333333333") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("9a3ba0a0-2914-e381-87df-4a00988bf036"), new Guid("33333333-3333-3333-3333-333333333333") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("19fc086c-165d-67c5-668f-56661a36164d"), new Guid("44444444-4444-4444-4444-444444444444") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("21bd6ce0-657d-9610-2fc5-34caf572fbe8"), new Guid("44444444-4444-4444-4444-444444444444") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("454b45b2-c9b0-ba27-b071-88c7ce4f9821"), new Guid("44444444-4444-4444-4444-444444444444") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("4c77f9fc-e86f-78b8-c88b-a2df097f6bd3"), new Guid("44444444-4444-4444-4444-444444444444") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("21bd6ce0-657d-9610-2fc5-34caf572fbe8"), new Guid("55555555-5555-5555-5555-555555555555") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("454b45b2-c9b0-ba27-b071-88c7ce4f9821"), new Guid("55555555-5555-5555-5555-555555555555") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("4c77f9fc-e86f-78b8-c88b-a2df097f6bd3"), new Guid("55555555-5555-5555-5555-555555555555") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("62bf81bb-b05b-3dab-ab8e-46f87cf16bbf"), new Guid("55555555-5555-5555-5555-555555555555") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("c4bdb36c-4205-1966-10fc-6b5701a69c42"), new Guid("55555555-5555-5555-5555-555555555555") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("e35c101b-a88c-8804-7ccb-6f0c2a5cf1e1"), new Guid("55555555-5555-5555-5555-555555555555") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("e827ae98-f118-1f4e-e890-4cb036aba6b8"), new Guid("55555555-5555-5555-5555-555555555555") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("f8edc86c-8f35-bc18-75a5-17cfec2310f0"), new Guid("55555555-5555-5555-5555-555555555555") });

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("08965e65-eaba-3846-f3ca-5de5a9894e08"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("1f20e50d-312e-e12d-bbe3-9b176f4055dc"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("21bd6ce0-657d-9610-2fc5-34caf572fbe8"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("3e1115c6-b4da-0b86-6140-c77889a5dec1"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("428a7672-6e0e-5486-0493-dd54a9d9ecb0"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("454b45b2-c9b0-ba27-b071-88c7ce4f9821"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("496fc662-2331-b5ab-2dba-803c897dd134"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("4c77f9fc-e86f-78b8-c88b-a2df097f6bd3"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("54d379ae-ad74-040a-c946-f56cd58ef72a"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("62bf81bb-b05b-3dab-ab8e-46f87cf16bbf"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("89c37a1c-4545-d211-ab83-1785370f3c10"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("9153963b-fc31-2e9b-2a99-d243b6081845"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("c4bdb36c-4205-1966-10fc-6b5701a69c42"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("c501c51b-6061-b652-d47f-8a593e35e5bb"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("c5355ae4-569e-3834-47be-60ae6c73b9c5"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("db454c1f-a726-589d-5585-7ee8c79c3434"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e35c101b-a88c-8804-7ccb-6f0c2a5cf1e1"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e8031eea-0eef-f077-9aa5-c44e18fef35f"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e827ae98-f118-1f4e-e890-4cb036aba6b8"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f8edc86c-8f35-bc18-75a5-17cfec2310f0"));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("00d42e12-16d8-1161-d9c3-8198d016b61d"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6779), new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6766) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("01fbb69d-2fe0-cfe2-60cc-9ddefdb59221"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6568), new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6555) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("18637eb6-dd2d-4916-90e3-d190753fe651"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6854), new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6842) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("19bddbc6-bc3d-33b5-9a93-ac104667687f"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6613), new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6600) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("19fc086c-165d-67c5-668f-56661a36164d"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6627), new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6615) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("1cf712ea-bf75-5da7-0315-f6abf938c90c"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6547), new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6534) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("2821a053-365e-d274-2f88-c10912fd142b"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6713), new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6699) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("28fc09cf-1d97-d3cc-66b9-e7a6210a3ced"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6745), new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6732) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("2f7e4df2-8507-4ef5-625a-2f7959334de6"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6511), new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6480) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("35f5f086-45fa-c7c6-8a5e-ea8f9c97cea8"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6462), new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6448) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("40486d09-d640-93b6-125b-742b3fb9c30a"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6868), new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6856) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("462495bb-1b19-10a6-a40f-a2f15ed27874"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6532), new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6518) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("4a5e57f9-7371-ff34-a7c0-e6cf47f912c8"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6384), new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6071) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("55674d3f-5e0b-2f4e-0123-849bc32efd83"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6582), new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6570) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("641ccf8e-48a4-63f9-ea51-46a8f84e8275"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6446), new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6427) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("676d33fc-eacf-fcc4-00a5-09c3521386f3"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6765), new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6751) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("67c04dad-c1c4-c165-628f-62f4f39794d1"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6598), new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6585) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("6bc3f860-b68c-8568-6564-78b6a17735fa"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6886), new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6872) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("6f6232a5-e70c-eb97-a1e8-cfb0190dfc80"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6807), new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6795) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("83bc6004-6954-1c64-f355-b54fa1e8f7cc"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6793), new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6780) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("87a96f18-d58d-24eb-e677-ca0abfb99bec"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6839), new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6827) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("9a3ba0a0-2914-e381-87df-4a00988bf036"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6697), new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6647) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("ba2e8da4-a36d-4bc8-25fc-ae51d41fea7d"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6825), new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6812) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("ccc21865-8cbd-f136-7f56-5c9551c5c120"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6642), new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6629) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d2865fa6-328c-da22-aad5-12cd86505b1b"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6730), new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6717) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e6b7aa38-22c2-348d-5827-efeb05b4b45e"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6925), new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6888) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f1d8a62c-3192-969d-5d75-025c2928d679"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6479), new DateTime(2026, 4, 7, 15, 26, 3, 9, DateTimeKind.Utc).AddTicks(6464) });
        }
    }
}
