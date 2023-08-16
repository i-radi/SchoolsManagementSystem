using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Migrations
{
    /// <inheritdoc />
    public partial class ChangeReferentialRelation2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityClassrooms_ClassRooms_ClassroomId",
                table: "ActivityClassrooms");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityClassrooms_ClassRooms_ClassroomId",
                table: "ActivityClassrooms",
                column: "ClassroomId",
                principalTable: "ClassRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityClassrooms_ClassRooms_ClassroomId",
                table: "ActivityClassrooms");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityClassrooms_ClassRooms_ClassroomId",
                table: "ActivityClassrooms",
                column: "ClassroomId",
                principalTable: "ClassRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
