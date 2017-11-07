using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ContestEACA.Migrations
{
    public partial class ProvisionAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProvisionId",
                table: "Contests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contests_ProvisionId",
                table: "Contests",
                column: "ProvisionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contests_Files_ProvisionId",
                table: "Contests",
                column: "ProvisionId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contests_Files_ProvisionId",
                table: "Contests");

            migrationBuilder.DropIndex(
                name: "IX_Contests_ProvisionId",
                table: "Contests");

            migrationBuilder.DropColumn(
                name: "ProvisionId",
                table: "Contests");
        }
    }
}
