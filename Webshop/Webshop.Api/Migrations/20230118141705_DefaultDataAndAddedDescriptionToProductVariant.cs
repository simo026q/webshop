using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Webshop.Api.Migrations
{
    /// <inheritdoc />
    public partial class DefaultDataAndAddedDescriptionToProductVariant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("e8fcd35c-3ef4-4d16-ab60-0d566b08d8a4"));

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getutcdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ProductVariants",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ProductVariants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getutcdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getutcdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.InsertData(
                table: "Categories",
                column: "Id",
                value: "Wheel Base");

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "ImageUrl", "IsActive", "Name" },
                values: new object[] { new Guid("9c5f329d-1ed1-4d09-8a15-d056a5df6599"), "All the variants of Simucube 2 use industrial quality electronics, advanced software and firmware, and are built with high-quality materials for durability and long-lasting performance. They also have a wide range of compatibility with various sim racing software and games.", null, true, "Simucube 2" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AddressId", "CreatedAt", "Email", "FullName", "IsActive", "PasswordHash", "PasswordSalt", "Role" },
                values: new object[] { new Guid("401fa43f-1cde-4554-90b5-3ac178128e60"), null, new DateTime(2023, 1, 18, 14, 17, 5, 646, DateTimeKind.Utc).AddTicks(8400), "admin@gunthers-sim-gear.com", "Admin", true, "5DFB28JuB/syGSoMjtb4uck33gOopQYNOEJ6hnK2ikG1d11qv8eeFgLk9f6DjJ/1fQGxhSq4dLb0p0mTPtVdug==", "988be5f2ae2d46df83a71cd0b7799557", 2 });

            migrationBuilder.InsertData(
                table: "ProductCategories",
                columns: new[] { "CategoryId", "ProductId" },
                values: new object[] { "Wheel Base", new Guid("9c5f329d-1ed1-4d09-8a15-d056a5df6599") });

            migrationBuilder.InsertData(
                table: "ProductVariants",
                columns: new[] { "Id", "Description", "IsActive", "Name", "ProductId", "PurchasePrice", "SellingPrice", "Stock" },
                values: new object[,]
                {
                    { new Guid("24e6a157-10ae-4188-ac26-cdf2a88903b7"), "The SimuCube 2 Pro is designed for professional and semi-professional sim racers. It features a torque of up to 20Nm and a higher level of customization options.", true, "Pro", new Guid("9c5f329d-1ed1-4d09-8a15-d056a5df6599"), 1000.0, 1495.0, 10 },
                    { new Guid("9b3a5373-35ec-4283-aec3-b53a708dc706"), "The SimuCube 2 Sport is the entry-level variant and is designed for entry-level and club-level sim racers. It features a compact design and a torque of up to 10Nm.", true, "Sport", new Guid("9c5f329d-1ed1-4d09-8a15-d056a5df6599"), 800.0, 1295.0, 25 },
                    { new Guid("f820dd70-109e-49b5-b06b-11043779d073"), "The SimuCube 2 Ultimate is the top-of-the-line variant and is designed for elite sim racers. It features a torque of up to 32Nm and the most advanced customization options.", true, "Ultimate", new Guid("9c5f329d-1ed1-4d09-8a15-d056a5df6599"), 2000.0, 3295.0, 4 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { "Wheel Base", new Guid("9c5f329d-1ed1-4d09-8a15-d056a5df6599") });

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

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: "Wheel Base");

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("9c5f329d-1ed1-4d09-8a15-d056a5df6599"));

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ProductVariants");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Users",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getutcdate()");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ProductVariants",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Products",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Products",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getutcdate()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getutcdate()");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AddressId", "CreatedAt", "Email", "FullName", "IsActive", "PasswordHash", "PasswordSalt", "Role" },
                values: new object[] { new Guid("e8fcd35c-3ef4-4d16-ab60-0d566b08d8a4"), null, new DateTime(2023, 1, 17, 19, 36, 59, 551, DateTimeKind.Utc).AddTicks(4088), "admin@gunthers-sim-gear.com", "Admin", true, "Q5KKdW6MLadjfHGHX0jpYie/xsGD16wbPiRYArQgIxT7QSjf4cnppd49Brpda0OHKa3JPtKOen40r/pdq78q+Q==", "474d507c2ace4669856fc4fe24b95859", 2 });
        }
    }
}
