using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Migrations
{
    /// <inheritdoc />
    public partial class ChangeReferentialRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityInstanceSeasons_Seasons_SeasonId",
                table: "ActivityInstanceSeasons");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityInstanceSeasons_Seasons_SeasonId",
                table: "ActivityInstanceSeasons",
                column: "SeasonId",
                principalTable: "Seasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityInstanceSeasons_Seasons_SeasonId",
                table: "ActivityInstanceSeasons");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityInstanceSeasons_Seasons_SeasonId",
                table: "ActivityInstanceSeasons",
                column: "SeasonId",
                principalTable: "Seasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
