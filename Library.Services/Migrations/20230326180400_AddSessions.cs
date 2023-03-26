using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Library.Services.Migrations
{
    public partial class AddSessions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SessionRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsApproved = table.Column<bool>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    BookCopyInventoryNumber = table.Column<Guid>(nullable: true),
                    ReaderId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessionRequests_Copies_BookCopyInventoryNumber",
                        column: x => x.BookCopyInventoryNumber,
                        principalTable: "Copies",
                        principalColumn: "InventoryNumber",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SessionRequests_AspNetUsers_ReaderId",
                        column: x => x.ReaderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RequestId = table.Column<Guid>(nullable: true),
                    ExpireDate = table.Column<DateTime>(nullable: false),
                    LibrarianId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sessions_AspNetUsers_LibrarianId",
                        column: x => x.LibrarianId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sessions_SessionRequests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "SessionRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SessionRequests_BookCopyInventoryNumber",
                table: "SessionRequests",
                column: "BookCopyInventoryNumber");

            migrationBuilder.CreateIndex(
                name: "IX_SessionRequests_ReaderId",
                table: "SessionRequests",
                column: "ReaderId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_LibrarianId",
                table: "Sessions",
                column: "LibrarianId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_RequestId",
                table: "Sessions",
                column: "RequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "SessionRequests");
        }
    }
}
