using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Library.Services.Migrations
{
    public partial class BookAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ISBN = table.Column<string>(maxLength: 13, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    Year = table.Column<short>(nullable: false),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Copies",
                columns: table => new
                {
                    InventoryNumber = table.Column<Guid>(nullable: false),
                    IsAvailable = table.Column<bool>(nullable: false),
                    BookId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Copies", x => x.InventoryNumber);
                    table.ForeignKey(
                        name: "FK_Copies_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Copies_BookId",
                table: "Copies",
                column: "BookId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Copies");

            migrationBuilder.DropTable(
                name: "Books");
        }
    }
}
