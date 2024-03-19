using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Globamantics.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeUserLazy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDo_Users_AssignedToId",
                table: "ToDo");

            migrationBuilder.DropForeignKey(
                name: "FK_ToDo_Users_Bug_AssignedToId",
                table: "ToDo");

            migrationBuilder.DropForeignKey(
                name: "FK_ToDo_Users_CreatedById",
                table: "ToDo");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedById",
                table: "ToDo",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDo_Users_AssignedToId",
                table: "ToDo",
                column: "AssignedToId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDo_Users_Bug_AssignedToId",
                table: "ToDo",
                column: "Bug_AssignedToId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDo_Users_CreatedById",
                table: "ToDo",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDo_Users_AssignedToId",
                table: "ToDo");

            migrationBuilder.DropForeignKey(
                name: "FK_ToDo_Users_Bug_AssignedToId",
                table: "ToDo");

            migrationBuilder.DropForeignKey(
                name: "FK_ToDo_Users_CreatedById",
                table: "ToDo");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedById",
                table: "ToDo",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ToDo_Users_AssignedToId",
                table: "ToDo",
                column: "AssignedToId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ToDo_Users_Bug_AssignedToId",
                table: "ToDo",
                column: "Bug_AssignedToId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ToDo_Users_CreatedById",
                table: "ToDo",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
