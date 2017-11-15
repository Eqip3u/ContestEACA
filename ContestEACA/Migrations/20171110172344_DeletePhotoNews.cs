using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ContestEACA.Migrations
{
    public partial class DeletePhotoNews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_News_Files_PhotoId",
                table: "News");

            migrationBuilder.DropIndex(
                name: "IX_News_PhotoId",
                table: "News");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "News");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PhotoId",
                table: "News",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_News_PhotoId",
                table: "News",
                column: "PhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_News_Files_PhotoId",
                table: "News",
                column: "PhotoId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
