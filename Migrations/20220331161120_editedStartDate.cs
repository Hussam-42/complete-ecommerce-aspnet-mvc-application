using Microsoft.EntityFrameworkCore.Migrations;

namespace eTicket.Migrations
{
    public partial class editedStartDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "startDate",
                table: "Movies",
                newName: "StartDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Movies",
                newName: "startDate");
        }
    }
}
