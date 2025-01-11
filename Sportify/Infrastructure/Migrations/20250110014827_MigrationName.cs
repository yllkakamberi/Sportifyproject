using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MigrationName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductBrands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductBrands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductTypeId = table.Column<int>(type: "int", nullable: false),
                    ProductBrandId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_ProductBrands_ProductBrandId",
                        column: x => x.ProductBrandId,
                        principalTable: "ProductBrands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_ProductTypes_ProductTypeId",
                        column: x => x.ProductTypeId,
                        principalTable: "ProductTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ProductBrands",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { -7, "Babolat" },
                    { -6, "Nike" },
                    { -5, "Puma" },
                    { -4, "Yonex" },
                    { -3, "Victor" },
                    { -2, "ASICS" },
                    { -1, "Adidas" }
                });

            migrationBuilder.InsertData(
                table: "ProductTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { -4, "Kit Bags" },
                    { -3, "Football" },
                    { -2, "Rackets" },
                    { -1, "Shoes" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "Name", "PictureUrl", "Price", "ProductBrandId", "ProductTypeId" },
                values: new object[,]
                {
                    { -35, "Yonex SUNR LRB05 MS BT6 S Badminton Kit Bag...", "Yonex SUNR LRB05 MS BT6 S Badminton Kit Bag", "images/products/yonex_kitback-3.png", 1499m, -4, -4 },
                    { -34, "Yonex SUNR LRB05 MS BT6 S Badminton Kit Bag...", "Yonex SUNR LRB05 MS BT6 S Badminton Kit Bag", "images/products/yonex_kitback-2.png", 1499m, -4, -4 },
                    { -33, "Yonex SUNR 4826TK BT6-SR Badminton Kit Bag...", "Yonex SUNR 4826TK BT6-SR Badminton Kit Bag", "images/products/yonex_kitback-1.png", 1999m, -4, -4 },
                    { -32, "Babolat Team Line 12 Racquet Kit Bag...", "Babolat Team Line 12 Racquet Kit Bag", "images/products/babolat_kitback-3.png", 4550m, -7, -4 },
                    { -31, "Babolat Pure Strike RH X12 Kit Bag...", "Babolat Pure Strike RH X12 Kit Bag", "images/products/babolat_kitback-2.png", 9799m, -7, -4 },
                    { -30, "The Babolat Team Line racquet bag is highly durable...", "Babolat Team Line Racket 12 Kit Bag", "images/products/babolat_kitback-1.png", 4550m, -7, -4 },
                    { -29, "Nike Mercurial Veer Football...", "Nike Mercurial Veer Football", "images/products/Nike_Football-3.png", 1450m, -6, -3 },
                    { -28, "Nike Manchester City Supporters Football...", "Nike Manchester City Supporters Football", "images/products/Nike_Football-2.png", 1525m, -6, -3 },
                    { -27, "Nike Pitch Premier League Football...", "Nike Pitch Premier League Football", "images/products/Nike_Football-1.png", 1525m, -6, -3 },
                    { -26, "Featuring an innovative surface panel design...", "Adidas FIFA World Cup Top Glider Ball", "images/products/adidas_football-3.png", 2499m, -1, -3 },
                    { -25, "Featuring an innovative surface panel design...", "Adidas FIFA World Cup 2018 OMB Football", "images/products/adidas_football-2.png", 3200m, -1, -3 },
                    { -24, "Featuring an innovative surface panel design...", "Adidas FIFA World Cup 2018 OMB Football", "images/products/adidas_football-1.png", 2499m, -1, -3 },
                    { -23, "Babolat Pure Drive VS Tennis Racquet...", "Babolat Pure Drive VS Tennis Racquet", "images/products/babolat_racket-3.png", 34000m, -7, -2 },
                    { -22, "Babolat Pure Aero 2019 Superlite Tennis Racquet...", "Buy Babolat Pure Aero 2019 Superlite Tennis Racquet...", "images/products/babolat_racket-2.png", 13000m, -7, -2 },
                    { -21, "Babolat Boost D Tennis Racquet...", "Babolat Boost D Tennis Racquet", "images/products/babolat_racket-1.png", 6999m, -7, -2 },
                    { -20, "For offensive players looking to win with game-changing spin...", "Yonex VCORE Pro 100 2019 Tennis Racquet", "images/products/yonex_racket-3.png", 13299m, -4, -2 },
                    { -19, "For offensive players looking to win with game-changing spin...", "Yonex VCORE Pro 100 A Tennis Racquet", "images/products/yonex_racket-2.png", 6399m, -4, -2 },
                    { -18, "For offensive players looking to win with game-changing spin...", "Yonex VCORE Pro 100 A Tennis Racquet", "images/products/yonex_racket-1.png", 6099m, -4, -2 },
                    { -17, "Babolat Shadow Team Womens Badminton Shoes...", "Babolat Shadow Team Womens Badminton Shoes", "images/products/babolat_shoe-3.png", 2999m, -7, -1 },
                    { -16, "Babolat Shadow Tour Mens Badminton Shoes...", "Babolat Shadow Tour Mens Badminton Shoes", "images/products/babolat_shoe-2.png", 5249m, -7, -1 },
                    { -15, "Babolat Shadow Spirit Mens Badminton Shoes...", "Babolat Shadow Spirit Mens Badminton Shoes", "images/products/babolat_shoe-1.png", 4125m, -7, -1 },
                    { -14, "With features and functions designed to withstand long hours...", "Puma 19 FH Rubber Spike Cricket Shoes", "images/products/puma_shoe-3.png", 5700m, -5, -1 },
                    { -13, "With features and functions designed to withstand long hours...", "Puma 19 FH Rubber Spike Cricket Shoes", "images/products/puma_shoe-2.png", 5200m, -5, -1 },
                    { -12, "With features and functions designed to withstand long hours...", "Puma 19 FH Rubber Spike Cricket Shoes", "images/products/puma_shoe-1.png", 4999m, -5, -1 },
                    { -11, "Rule the game with Super Ace Light...", "YONEX Super Ace Light Badminton Shoes", "images/products/yonex_shoe-3.png", 3700m, -4, -1 },
                    { -10, "Rule the game with Super Ace Light...", "YONEX Super Ace Light Badminton Shoes", "images/products/yonex_shoe-2.png", 3500m, -4, -1 },
                    { -9, "Rule the game with Super Ace Light...", "YONEX Super Ace Light Badminton Shoes", "images/products/yonex_shoe-1.png", 2590m, -4, -1 },
                    { -8, "PU Leather, Mesh, EVA, ENERGY MAX, Nylon sheet, Rubber", "Victor SHW503 F Badminton Shoes", "images/products/victor_shoe-2.png", 3000m, -3, -1 },
                    { -7, "PU Leather, Mesh, EVA, ENERGY MAX, Nylon sheet, Rubber", "Victor SHW503 F Badminton Shoes", "images/products/victor_shoe-1.png", 2392m, -3, -1 },
                    { -6, "The Asics Gel Rocket 8 Indoor Court Shoes...", "Asics Gel Rocket 8 Indoor Court Shoes", "images/products/asics_shoe-3.png", 3499m, -2, -1 },
                    { -5, "The Asics Gel Rocket 8 Indoor Court Shoes...", "Asics Gel Rocket 8 Indoor Court Shoes", "images/products/asics_shoe-2.png", 3499m, -2, -1 },
                    { -4, "The Asics Gel Rocket 8 Indoor Court Shoes...", "Asics Gel Rocket 8 Indoor Court Shoes", "images/products/asics_shoe-1.png", 4249m, -2, -1 },
                    { -3, "Designed for professional as well as amateur badminton players...", "Adidas Quick Force Indoor Badminton Shoes", "images/products/adidas_shoe-3.png", 3375m, -1, -1 },
                    { -2, "Designed for professional as well as amateur badminton players...", "Adidas Quick Force Indoor Badminton Shoes", "images/products/adidas_shoe-2.png", 3375m, -1, -1 },
                    { -1, "Designed for professional as well as amateur badminton players...", "Adidas Quick Force Indoor Badminton Shoes", "images/products/adidas_shoe-1.png", 3500m, -1, -1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductBrandId",
                table: "Products",
                column: "ProductBrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductTypeId",
                table: "Products",
                column: "ProductTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "ProductBrands");

            migrationBuilder.DropTable(
                name: "ProductTypes");
        }
    }
}
