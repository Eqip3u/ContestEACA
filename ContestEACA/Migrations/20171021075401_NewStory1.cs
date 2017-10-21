using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ContestEACA.Migrations
{
    public partial class NewStory1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Nominations_Posts_PostId",
                table: "Nominations");

            migrationBuilder.DropIndex(
                name: "IX_Nominations_PostId",
                table: "Nominations");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Nominations");

            migrationBuilder.AddColumn<int>(
                name: "NominationId",
                table: "Posts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_NominationId",
                table: "Posts",
                column: "NominationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Nominations_NominationId",
                table: "Posts",
                column: "NominationId",
                principalTable: "Nominations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Nominations_NominationId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_NominationId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "NominationId",
                table: "Posts");

            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "Nominations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Nominations_PostId",
                table: "Nominations",
                column: "PostId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Nominations_Posts_PostId",
                table: "Nominations",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
