using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class Tags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberAdresses_AspNetUsers_UserId",
                table: "MemberAdresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MemberAdresses",
                table: "MemberAdresses");

            migrationBuilder.RenameTable(
                name: "MemberAdresses",
                newName: "UserAdresses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAdresses",
                table: "UserAdresses",
                column: "UserId");

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagName = table.Column<string>(type: "nvarchar(75)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tags_TagName",
                table: "Tags",
                column: "TagName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAdresses_AspNetUsers_UserId",
                table: "UserAdresses",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAdresses_AspNetUsers_UserId",
                table: "UserAdresses");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAdresses",
                table: "UserAdresses");

            migrationBuilder.RenameTable(
                name: "UserAdresses",
                newName: "MemberAdresses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemberAdresses",
                table: "MemberAdresses",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberAdresses_AspNetUsers_UserId",
                table: "MemberAdresses",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
