using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ContestEACA.Migrations
{
    public partial class PostAddLinkWork : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Work",
                table: "Posts");

            migrationBuilder.AddColumn<string>(
                name: "LinkWork",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TextWork",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LinkWork",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "TextWork",
                table: "Posts");

            migrationBuilder.AddColumn<string>(
                name: "Work",
                table: "Posts",
                nullable: true);
        }
    }
}
