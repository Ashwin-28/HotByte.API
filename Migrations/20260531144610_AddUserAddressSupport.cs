using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotByte.API.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAddressSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserAddresses",
                columns: table => new
                {
                    AddressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AddressLabel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AddressLine = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    State = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAddresses", x => x.AddressId);
                    table.ForeignKey(
                        name: "FK_UserAddresses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$Ie..HR8H5/6YIpBb4et6P.TwHp62a4NBuWfp7wG.akenaTpHPpljW");

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$Ie..HR8H5/6YIpBb4et6P.TwHp62a4NBuWfp7wG.akenaTpHPpljW");

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 3,
                column: "PasswordHash",
                value: "$2a$11$Ie..HR8H5/6YIpBb4et6P.TwHp62a4NBuWfp7wG.akenaTpHPpljW");

            migrationBuilder.InsertData(
                table: "UserAddresses",
                columns: new[] { "AddressId", "AddressLabel", "AddressLine", "City", "CreatedAt", "IsActive", "PostalCode", "State", "UserId" },
                values: new object[] { 1, "Home", "123 Main Street", "Chennai", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "600001", "Tamil Nadu", 2 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$APYGzGiIm7dO4oWMit/ACOOuvPNcsRSI6dYlTnq3Mz0Zyo3McMdpa");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$75yFIf0UbgpoUlb5e9biuOF.pBpl79K6oK6VtRGP/0/gwiENyTKje");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddresses_UserId",
                table: "UserAddresses",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAddresses");

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$Fzqgehxgb0JDCA5IFnAVveKRN0fMIedQL8ZHh9J4bRYT9aonvV4R.");

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$.aNOyVy.pwvR.UD6kmLvhOwwLVBaaOG.yf6i7JGFQh8/WaP136bT6");

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 3,
                column: "PasswordHash",
                value: "$2a$11$IT/4YTel2ToRCybV2ByKl.ovCynR9UlsVvc2c6G5q3GYSkIwi8fgG");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$LFxh.zCNZaUWfxhf/tNRHu4YiCYk.VuVOt/8bmYCH4O308cZfmURO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$R2KmvB4Ze6.CbBoEbRCqg.nD8tJottggzbn78gLkCwnZvMDTyQo/G");
        }
    }
}
