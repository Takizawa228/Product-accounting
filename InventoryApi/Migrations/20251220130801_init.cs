using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryApi.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Клиенты",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Passport = table.Column<int>(type: "int", nullable: false),
                    Birth = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Клиенты", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Продукция",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    Supplier = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Продукция", x => x.Id);
                    table.CheckConstraint("CheckCount", "Quantity > 0");
                    table.CheckConstraint("ValidPrice", "Price > 0");
                });

            migrationBuilder.CreateTable(
                name: "Сотрудники",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Post = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Сотрудники", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Транзакции",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Sum = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    WorkerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Транзакции", x => x.Id);
                    table.CheckConstraint("CheckCount1", "Quantity > 0");
                    table.ForeignKey(
                        name: "FK_Транзакции_Клиенты_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Клиенты",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Транзакции_Сотрудники_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Сотрудники",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductsTransactions",
                columns: table => new
                {
                    productsId = table.Column<int>(type: "int", nullable: false),
                    transactionsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsTransactions", x => new { x.productsId, x.transactionsId });
                    table.ForeignKey(
                        name: "FK_ProductsTransactions_Продукция_productsId",
                        column: x => x.productsId,
                        principalTable: "Продукция",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsTransactions_Транзакции_transactionsId",
                        column: x => x.transactionsId,
                        principalTable: "Транзакции",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Транзакции_ClientId",
                table: "Транзакции",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Транзакции_WorkerId",
                table: "Транзакции",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsTransactions_transactionsId",
                table: "ProductsTransactions",
                column: "transactionsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductsTransactions");

            migrationBuilder.DropTable(
                name: "Продукция");

            migrationBuilder.DropTable(
                name: "Транзакции");

            migrationBuilder.DropTable(
                name: "Клиенты");

            migrationBuilder.DropTable(
                name: "Сотрудники");
        }
    }
}
