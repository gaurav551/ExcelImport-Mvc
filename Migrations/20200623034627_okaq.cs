using Microsoft.EntityFrameworkCore.Migrations;

namespace ExcelImport.Migrations
{
    public partial class okaq : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "UserInfos",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Phone",
                table: "UserInfos",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "UserInfos");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "UserInfos");
        }
    }
}
