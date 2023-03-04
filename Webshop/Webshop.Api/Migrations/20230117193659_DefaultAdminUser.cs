using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webshop.Api.Migrations
{
    /// <inheritdoc />
    public partial class DefaultAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AddressId", "CreatedAt", "Email", "FullName", "IsActive", "PasswordHash", "PasswordSalt", "Role" },
                values: new object[] { new Guid("e8fcd35c-3ef4-4d16-ab60-0d566b08d8a4"), null, new DateTime(2023, 1, 17, 19, 36, 59, 551, DateTimeKind.Utc).AddTicks(4088), "admin@gunthers-sim-gear.com", "Admin", true, "Q5KKdW6MLadjfHGHX0jpYie/xsGD16wbPiRYArQgIxT7QSjf4cnppd49Brpda0OHKa3JPtKOen40r/pdq78q+Q==", "474d507c2ace4669856fc4fe24b95859", 2 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("e8fcd35c-3ef4-4d16-ab60-0d566b08d8a4"));
        }
    }
}
