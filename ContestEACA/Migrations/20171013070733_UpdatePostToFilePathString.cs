using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ContestEACA.Migrations
{
    public partial class UpdatePostToFilePathString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_File_FileId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_FileId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "Posts");

            migrationBuilder.AddColumn<string>(
                name: "File",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "File",
                table: "Posts");

            migrationBuilder.AddColumn<int>(
                name: "FileId",
                table: "Posts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_FileId",
                table: "Posts",
                column: "FileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_File_FileId",
                table: "Posts",
                column: "FileId",
                principalTable: "File",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
