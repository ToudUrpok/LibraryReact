using Microsoft.EntityFrameworkCore.Migrations;

namespace Library.Services.Migrations
{
    public partial class Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Authors",
                table: "Books",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Genre",
                table: "Books",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Authors",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Genre",
                table: "Books");
        }
    }
}
