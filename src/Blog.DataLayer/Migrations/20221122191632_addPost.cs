using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class addPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Content = table.Column<string>(type: "ntext", nullable: false),
                    CoverImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "85e50f0f-b4ef-43bf-9de9-186ca018015e", "AQAAAAIAAYagAAAAEILgaodvRKRrkMkPdeOkKbgqbIKjEWDUmovRBUJv2HySRaRhfrwW7oCPixPbSjDttQ==", "757b447a-cb9d-470e-84ce-2177e432c89d" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9eb0c4f7-7754-4ba2-a63c-7f4db7188815", "AQAAAAIAAYagAAAAEGBIBr6wq9jhZ+KdkEHJaBpiWFmY681eW8bMigfSwA2TOtXgD4Xj8xhb3bnCAWXwrw==", "849b4154-1b9e-4cf8-b050-8ecaccef31ab" });
        }
    }
}
