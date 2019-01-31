using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication12_bulkExtensions.Data.Migrations
{
    public partial class MakeIntsNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Region",
                table: "States",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "Division",
                table: "States",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Region",
                table: "States",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Division",
                table: "States",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
