using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ContestEACA.Migrations
{
    public partial class NewStory2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Nominations_NominationId",
                table: "Posts");

            migrationBuilder.AlterColumn<int>(
                name: "NominationId",
                table: "Posts",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Nominations_NominationId",
                table: "Posts",
                column: "NominationId",
                principalTable: "Nominations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Nominations_NominationId",
                table: "Posts");

            migrationBuilder.AlterColumn<int>(
                name: "NominationId",
                table: "Posts",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Nominations_NominationId",
                table: "Posts",
                column: "NominationId",
                principalTable: "Nominations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
