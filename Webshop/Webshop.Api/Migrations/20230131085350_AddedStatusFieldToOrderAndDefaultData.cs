using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Webshop.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddedStatusFieldToOrderAndDefaultData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: new Guid("24e6a157-10ae-4188-ac26-cdf2a88903b7"));

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: new Guid("9b3a5373-35ec-4283-aec3-b53a708dc706"));

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: new Guid("f820dd70-109e-49b5-b06b-11043779d073"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("401fa43f-1cde-4554-90b5-3ac178128e60"));

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Users",
                type: "bit",
                nullable: true,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ProductVariants",
                type: "bit",
                nullable: true,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Products",
                type: "bit",
                nullable: true,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Categories",
                column: "Id",
                values: new object[]
                {
                    "Pedals",
                    "Steering Wheels"
                });

            migrationBuilder.InsertData(
                table: "ProductVariants",
                columns: new[] { "Id", "Description", "IsActive", "Name", "ProductId", "PurchasePrice", "SellingPrice", "Stock" },
                values: new object[,]
                {
                    { new Guid("5833c2fb-0aec-4435-b16c-e1146502ce62"), "The SimuCube 2 Ultimate is the top-of-the-line variant and is designed for elite sim racers. It features a torque of up to 32Nm and the most advanced customization options.", true, "Ultimate", new Guid("9c5f329d-1ed1-4d09-8a15-d056a5df6599"), 2000.0, 3295.0, 4 },
                    { new Guid("835ec654-43e5-49c6-abe6-ffb20d274f52"), "The SimuCube 2 Pro is designed for professional and semi-professional sim racers. It features a torque of up to 20Nm and a higher level of customization options.", true, "Pro", new Guid("9c5f329d-1ed1-4d09-8a15-d056a5df6599"), 1000.0, 1495.0, 10 },
                    { new Guid("de880055-3788-4802-8875-4a4dd0a09e5b"), "The SimuCube 2 Sport is the entry-level variant and is designed for entry-level and club-level sim racers. It features a compact design and a torque of up to 10Nm.", true, "Sport", new Guid("9c5f329d-1ed1-4d09-8a15-d056a5df6599"), 800.0, 1295.0, 25 }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "ImageUrl", "IsActive", "Name" },
                values: new object[,]
                {
                    { new Guid("297c7eda-33c4-47d3-ab1a-60f5d477dd13"), "The Fanatec Clubsport v3 pedals are a high-end set of pedals designed for use with racing simulators. They feature a durable aluminum construction and are fully adjustable to fit the user's preferences. The pedals include a load cell brake, which provides precise braking force feedback, as well as an adjustable brake stiffness. The pedals also feature a realistic and adjustable clutch, allowing for smooth gear changes. Additionally, the pedals have a vibration motor built-in to provide even more immersive feedback. These pedals are compatible with most racing simulators and are a great choice for serious sim racers looking for the most realistic and immersive experience.", "https://fanatec.com/media/image/60/ef/a1/CSP_V3_prime2_1280x1280.jpg", true, "ClubSport Pedals V3" },
                    { new Guid("c0b5b2f5-0b1f-4b5e-8b1a-5b5b5b5b5b5b"), null, "https://fanatec.com/media/image/9c/70/21/Product_Page_top_banner_Podium_SW_BMW_M4_GT3_Front_1280x1280.jpg", true, "Podium BMW M4 GT3" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AddressId", "CreatedAt", "Email", "FullName", "IsActive", "PasswordHash", "PasswordSalt", "Role" },
                values: new object[] { new Guid("4ac4752c-45a0-4677-845c-0b9849312bde"), null, new DateTime(2023, 1, 31, 8, 53, 50, 725, DateTimeKind.Utc).AddTicks(6719), "admin@gunthers-sim-gear.com", "Admin", true, "jkDmQg/7UaOaQzQNRyOxS6T9hrgh00nCIE+VSzAZoEBf6OG1hOvMsEaSrEwJrLt779Mpizvm6omg7VWLAz3ieQ==", "2e961c5efd494449b0125a68fadd45dc", 2 });

            migrationBuilder.InsertData(
                table: "ProductCategories",
                columns: new[] { "CategoryId", "ProductId" },
                values: new object[,]
                {
                    { "Pedals", new Guid("297c7eda-33c4-47d3-ab1a-60f5d477dd13") },
                    { "Steering Wheels", new Guid("c0b5b2f5-0b1f-4b5e-8b1a-5b5b5b5b5b5b") }
                });

            migrationBuilder.InsertData(
                table: "ProductVariants",
                columns: new[] { "Id", "Description", "IsActive", "Name", "ProductId", "PurchasePrice", "SellingPrice", "Stock" },
                values: new object[,]
                {
                    { new Guid("3be4fb1f-edd1-4ac7-b64e-d10eaf1f2b1e"), null, true, "Inverted", new Guid("297c7eda-33c4-47d3-ab1a-60f5d477dd13"), 400.0, 599.0, 4 },
                    { new Guid("b954ba3b-5668-4e43-b183-4e3af98cf74c"), null, true, "Default", new Guid("297c7eda-33c4-47d3-ab1a-60f5d477dd13"), 300.0, 399.0, 25 },
                    { new Guid("ec2e9348-3b59-4232-9094-2597ce777cfe"), null, true, "Default", new Guid("c0b5b2f5-0b1f-4b5e-8b1a-5b5b5b5b5b5b"), 1100.0, 1499.0, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { "Pedals", new Guid("297c7eda-33c4-47d3-ab1a-60f5d477dd13") });

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { "Steering Wheels", new Guid("c0b5b2f5-0b1f-4b5e-8b1a-5b5b5b5b5b5b") });

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: new Guid("3be4fb1f-edd1-4ac7-b64e-d10eaf1f2b1e"));

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: new Guid("5833c2fb-0aec-4435-b16c-e1146502ce62"));

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: new Guid("835ec654-43e5-49c6-abe6-ffb20d274f52"));

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: new Guid("b954ba3b-5668-4e43-b183-4e3af98cf74c"));

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: new Guid("de880055-3788-4802-8875-4a4dd0a09e5b"));

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: new Guid("ec2e9348-3b59-4232-9094-2597ce777cfe"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("4ac4752c-45a0-4677-845c-0b9849312bde"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: "Pedals");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: "Steering Wheels");

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("297c7eda-33c4-47d3-ab1a-60f5d477dd13"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("c0b5b2f5-0b1f-4b5e-8b1a-5b5b5b5b5b5b"));

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Orders");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ProductVariants",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValue: true);

            migrationBuilder.InsertData(
                table: "ProductVariants",
                columns: new[] { "Id", "Description", "IsActive", "Name", "ProductId", "PurchasePrice", "SellingPrice", "Stock" },
                values: new object[,]
                {
                    { new Guid("24e6a157-10ae-4188-ac26-cdf2a88903b7"), "The SimuCube 2 Pro is designed for professional and semi-professional sim racers. It features a torque of up to 20Nm and a higher level of customization options.", true, "Pro", new Guid("9c5f329d-1ed1-4d09-8a15-d056a5df6599"), 1000.0, 1495.0, 10 },
                    { new Guid("9b3a5373-35ec-4283-aec3-b53a708dc706"), "The SimuCube 2 Sport is the entry-level variant and is designed for entry-level and club-level sim racers. It features a compact design and a torque of up to 10Nm.", true, "Sport", new Guid("9c5f329d-1ed1-4d09-8a15-d056a5df6599"), 800.0, 1295.0, 25 },
                    { new Guid("f820dd70-109e-49b5-b06b-11043779d073"), "The SimuCube 2 Ultimate is the top-of-the-line variant and is designed for elite sim racers. It features a torque of up to 32Nm and the most advanced customization options.", true, "Ultimate", new Guid("9c5f329d-1ed1-4d09-8a15-d056a5df6599"), 2000.0, 3295.0, 4 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AddressId", "CreatedAt", "Email", "FullName", "IsActive", "PasswordHash", "PasswordSalt", "Role" },
                values: new object[] { new Guid("401fa43f-1cde-4554-90b5-3ac178128e60"), null, new DateTime(2023, 1, 18, 14, 17, 5, 646, DateTimeKind.Utc).AddTicks(8400), "admin@gunthers-sim-gear.com", "Admin", true, "5DFB28JuB/syGSoMjtb4uck33gOopQYNOEJ6hnK2ikG1d11qv8eeFgLk9f6DjJ/1fQGxhSq4dLb0p0mTPtVdug==", "988be5f2ae2d46df83a71cd0b7799557", 2 });
        }
    }
}
