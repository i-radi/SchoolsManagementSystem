using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Migrations
{
    /// <inheritdoc />
    public partial class AddActivitiesTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Activities",
                newName: "Name");

            migrationBuilder.AddColumn<bool>(
                name: "ForStudents",
                table: "Activities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ForTeachers",
                table: "Activities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Activities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Activities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Activities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ActivityClassrooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityId = table.Column<int>(type: "int", nullable: false),
                    ClassroomId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityClassrooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityClassrooms_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityClassrooms_ClassRooms_ClassroomId",
                        column: x => x.ClassroomId,
                        principalTable: "ClassRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ActivityInstances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ForDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityInstances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityInstances_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivityTimes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityId = table.Column<int>(type: "int", nullable: false),
                    Day = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FromTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityTimes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityTimes_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivityInstanceSeasons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityInstanceId = table.Column<int>(type: "int", nullable: false),
                    SeasonId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityInstanceSeasons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityInstanceSeasons_ActivityInstances_ActivityInstanceId",
                        column: x => x.ActivityInstanceId,
                        principalTable: "ActivityInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction,
                        onUpdate: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ActivityInstanceSeasons_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction,
                        onUpdate: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ActivityInstanceUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityInstanceId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityInstanceUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityInstanceUsers_ActivityInstances_ActivityInstanceId",
                        column: x => x.ActivityInstanceId,
                        principalTable: "ActivityInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityInstanceUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityClassrooms_ActivityId",
                table: "ActivityClassrooms",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityClassrooms_ClassroomId",
                table: "ActivityClassrooms",
                column: "ClassroomId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityInstances_ActivityId",
                table: "ActivityInstances",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityInstanceSeasons_ActivityInstanceId",
                table: "ActivityInstanceSeasons",
                column: "ActivityInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityInstanceSeasons_SeasonId",
                table: "ActivityInstanceSeasons",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityInstanceUsers_ActivityInstanceId",
                table: "ActivityInstanceUsers",
                column: "ActivityInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityInstanceUsers_UserId",
                table: "ActivityInstanceUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityTimes_ActivityId",
                table: "ActivityTimes",
                column: "ActivityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityClassrooms");

            migrationBuilder.DropTable(
                name: "ActivityInstanceSeasons");

            migrationBuilder.DropTable(
                name: "ActivityInstanceUsers");

            migrationBuilder.DropTable(
                name: "ActivityTimes");

            migrationBuilder.DropTable(
                name: "ActivityInstances");

            migrationBuilder.DropColumn(
                name: "ForStudents",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "ForTeachers",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "Activities");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Activities",
                newName: "Title");
        }
    }
}
