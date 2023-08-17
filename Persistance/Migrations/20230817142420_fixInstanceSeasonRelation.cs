using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Migrations
{
    /// <inheritdoc />
    public partial class fixInstanceSeasonRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityInstanceSeasons");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Seasons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SeasonId",
                table: "ActivityInstances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ActivityInstances_SeasonId",
                table: "ActivityInstances",
                column: "SeasonId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityInstances_Seasons_SeasonId",
                table: "ActivityInstances",
                column: "SeasonId",
                principalTable: "Seasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityInstances_Seasons_SeasonId",
                table: "ActivityInstances");

            migrationBuilder.DropIndex(
                name: "IX_ActivityInstances_SeasonId",
                table: "ActivityInstances");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Seasons");

            migrationBuilder.DropColumn(
                name: "SeasonId",
                table: "ActivityInstances");

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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityInstanceSeasons_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityInstanceSeasons_ActivityInstanceId",
                table: "ActivityInstanceSeasons",
                column: "ActivityInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityInstanceSeasons_SeasonId",
                table: "ActivityInstanceSeasons",
                column: "SeasonId");
        }
    }
}
