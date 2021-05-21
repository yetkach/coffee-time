using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoffeeTime.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CheckTimes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckTimes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoffeeData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoffeeData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuidId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserFirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserLastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsCheckedOut = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VolumeData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CoffeeDataId = table.Column<int>(type: "int", nullable: false),
                    Volume = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VolumeData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VolumeData_CoffeeData_CoffeeDataId",
                        column: x => x.CoffeeDataId,
                        principalTable: "CoffeeData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Coffees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Volume = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sugar = table.Column<int>(type: "int", nullable: false),
                    Milk = table.Column<bool>(type: "bit", nullable: false),
                    CupCap = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coffees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Coffees_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PriceData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VolumeDataId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceData_VolumeData_VolumeDataId",
                        column: x => x.VolumeDataId,
                        principalTable: "VolumeData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CheckTimes",
                columns: new[] { "Id", "DeletionTime" },
                values: new object[] { 1, new DateTime(2021, 5, 12, 19, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "CoffeeData",
                columns: new[] { "Id", "Image", "Name" },
                values: new object[,]
                {
                    { 1, "/img/americano.png", "Americano" },
                    { 2, "/img/latte.png", "Latte" },
                    { 3, "/img/espresso.png", "Espresso" }
                });

            migrationBuilder.InsertData(
                table: "VolumeData",
                columns: new[] { "Id", "CoffeeDataId", "Volume" },
                values: new object[,]
                {
                    { 1, 1, "0.133 L" },
                    { 2, 1, "0.250 L" },
                    { 3, 1, "0.500 L" },
                    { 4, 2, "0.133 L" },
                    { 5, 2, "0.250 L" },
                    { 6, 3, "0.133 L" },
                    { 7, 3, "0.250 L" }
                });

            migrationBuilder.InsertData(
                table: "PriceData",
                columns: new[] { "Id", "Price", "VolumeDataId" },
                values: new object[,]
                {
                    { 1, 3.5m, 1 },
                    { 2, 4m, 2 },
                    { 3, 4.5m, 3 },
                    { 4, 4.4m, 4 },
                    { 5, 5.2m, 5 },
                    { 6, 4.2m, 6 },
                    { 7, 5.4m, 7 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Coffees_OrderId",
                table: "Coffees",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceData_VolumeDataId",
                table: "PriceData",
                column: "VolumeDataId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VolumeData_CoffeeDataId",
                table: "VolumeData",
                column: "CoffeeDataId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckTimes");

            migrationBuilder.DropTable(
                name: "Coffees");

            migrationBuilder.DropTable(
                name: "PriceData");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "VolumeData");

            migrationBuilder.DropTable(
                name: "CoffeeData");
        }
    }
}
