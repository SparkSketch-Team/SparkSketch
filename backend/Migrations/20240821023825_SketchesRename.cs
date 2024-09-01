using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    public partial class SketchesRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Sketchs_PostID",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Sketchs_PostID",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_Sketchs_Users_UserId",
                table: "Sketchs");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Permissions_UserPermissionPermissionId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sketchs",
                table: "Sketchs");

            migrationBuilder.RenameTable(
                name: "Sketchs",
                newName: "Sketches");

            migrationBuilder.RenameIndex(
                name: "IX_Sketchs_UserId",
                table: "Sketches",
                newName: "IX_Sketches_UserId");

            migrationBuilder.AlterColumn<int>(
                name: "UserPermissionPermissionId",
                table: "Users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sketches",
                table: "Sketches",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Sketches_PostID",
                table: "Comments",
                column: "PostID",
                principalTable: "Sketches",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Sketches_PostID",
                table: "Likes",
                column: "PostID",
                principalTable: "Sketches",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sketches_Users_UserId",
                table: "Sketches",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Permissions_UserPermissionPermissionId",
                table: "Users",
                column: "UserPermissionPermissionId",
                principalTable: "Permissions",
                principalColumn: "PermissionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Sketches_PostID",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Sketches_PostID",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_Sketches_Users_UserId",
                table: "Sketches");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Permissions_UserPermissionPermissionId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sketches",
                table: "Sketches");

            migrationBuilder.RenameTable(
                name: "Sketches",
                newName: "Sketchs");

            migrationBuilder.RenameIndex(
                name: "IX_Sketches_UserId",
                table: "Sketchs",
                newName: "IX_Sketchs_UserId");

            migrationBuilder.AlterColumn<int>(
                name: "UserPermissionPermissionId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sketchs",
                table: "Sketchs",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Sketchs_PostID",
                table: "Comments",
                column: "PostID",
                principalTable: "Sketchs",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Sketchs_PostID",
                table: "Likes",
                column: "PostID",
                principalTable: "Sketchs",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sketchs_Users_UserId",
                table: "Sketchs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Permissions_UserPermissionPermissionId",
                table: "Users",
                column: "UserPermissionPermissionId",
                principalTable: "Permissions",
                principalColumn: "PermissionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
