using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Migrations
{
    /// <inheritdoc />
    public partial class changeClassroomImageNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TeacherImage",
                table: "Classrooms",
                newName: "TeacherImagePath");

            migrationBuilder.RenameColumn(
                name: "StudentImage",
                table: "Classrooms",
                newName: "StudentImagePath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TeacherImagePath",
                table: "Classrooms",
                newName: "TeacherImage");

            migrationBuilder.RenameColumn(
                name: "StudentImagePath",
                table: "Classrooms",
                newName: "StudentImage");
        }
    }
}
