using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommentManagement.Infrastructure.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class updatecomentmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_ParentId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "WebSite",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "Comments",
                newName: "ChildId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_ParentId",
                table: "Comments",
                newName: "IX_Comments_ChildId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_ChildId",
                table: "Comments",
                column: "ChildId",
                principalTable: "Comments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_ChildId",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "ChildId",
                table: "Comments",
                newName: "ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_ChildId",
                table: "Comments",
                newName: "IX_Comments_ParentId");

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "Comments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "WebSite",
                table: "Comments",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_ParentId",
                table: "Comments",
                column: "ParentId",
                principalTable: "Comments",
                principalColumn: "Id");
        }
    }
}
