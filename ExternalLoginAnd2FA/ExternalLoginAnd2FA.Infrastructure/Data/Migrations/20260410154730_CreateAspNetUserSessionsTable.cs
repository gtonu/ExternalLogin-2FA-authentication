using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExternalLoginAnd2FA.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateAspNetUserSessionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetUserSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<byte[]>(type: "bytea", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ExpiresAtTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    SlidingExpirationInSeconds = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserSessions", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetUserSessions");
        }
    }
}
