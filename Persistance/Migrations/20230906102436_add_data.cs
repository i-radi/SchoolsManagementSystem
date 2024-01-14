using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistance.Migrations
{
    /// <inheritdoc />
    public partial class add_data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Organizations",
                columns: new[] { "Id", "Name", "PicturePath" },
                values: new object[] { 1, "Cairo Organization", "" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1, null, "SuperAdmin", "SUPERADMIN" },
                    { 2, null, "OrganizationAdmin", "ORGANIZATIONADMIN" },
                    { 3, null, "SchoolAdmin", "SCHOOLADMIN" },
                    { 4, null, "FullAccessActivity", "FULLACCESSACTIVITY" },
                    { 5, null, "ReadAccessActivity", "READACCESSACTIVITY" }
                });

            migrationBuilder.InsertData(
                table: "UserTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Teacher" },
                    { 2, "Student" },
                    { 3, "Parent" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "AccessToken", "Address", "Birthdate", "ConcurrencyStamp", "Email", "EmailConfirmed", "FatherMobile", "FirstMobile", "Gender", "GpsLocation", "LockoutEnabled", "LockoutEnd", "MentorName", "MotherMobile", "Name", "NationalID", "NormalizedEmail", "NormalizedUserName", "Notes", "ParticipationNumber", "ParticipationQRCodePath", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PlainPassword", "PositionType", "ProfilePicturePath", "RefreshToken", "RefreshTokenExpiryDate", "SchoolUniversityJob", "SecondMobile", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { 1, 0, null, null, null, "721a882b-410e-46e0-b878-625aaa8bbd75", "admin@sms.com", false, null, null, null, "", false, null, null, null, "Admin User", null, "ADMIN@SMS.COM", "ADMIN", null, 0, null, "AQAAAAIAAYagAAAAEEEvuzV4blWESxZcSEnPlgLgae4bQZgB6A29NU/zj9FS91zzZKF9odfHtexpQHlzGg==", null, false, "123456", null, "emptyAvatar.png", new Guid("9ed43c18-ceae-422a-b474-673329ad9b8f"), new DateTime(2023, 9, 26, 10, 24, 36, 429, DateTimeKind.Utc).AddTicks(1210), null, null, "XCGDJZV44O4PZ47TD7MFQCD27H5DO4MB", false, "admin" });

            migrationBuilder.InsertData(
                table: "Schools",
                columns: new[] { "Id", "Description", "Name", "Order", "OrganizationId", "PicturePath" },
                values: new object[] { 1, "desc. of Cairo 1 School", "Cairo 1 School", 0, 1, "" });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Id", "ActivityId", "OrganizationId", "RoleId", "SchoolId", "UserId" },
                values: new object[] { 1, null, null, 1, null, 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "UserTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "UserTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "UserTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Organizations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
