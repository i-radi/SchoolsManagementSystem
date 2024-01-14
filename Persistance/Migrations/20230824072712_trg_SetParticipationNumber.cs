using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Migrations
{
    /// <inheritdoc />
    public partial class trg_SetParticipationNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                    CREATE TRIGGER trg_SetParticipationNumber
                    ON [Users]
                    AFTER INSERT
                    AS
                    BEGIN
                        UPDATE u
                        SET u.ParticipationNumber = i.Id
                        FROM [Users] u
                        INNER JOIN inserted i ON u.Id = i.Id;
                    END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_SetParticipationNumber ON [Users];");
        }
    }
}
