using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace COCOA.Migrations
{
    public partial class courseassigments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_AspNetUsers_OwnerId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_OwnerId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Courses");

            migrationBuilder.CreateTable(
                name: "CourseAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CourseAssignmentRole = table.Column<int>(nullable: false),
                    CourseId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseAssignments_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseAssignments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "MaterialPDFs",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MaterialPDFs_UserId",
                table: "MaterialPDFs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseAssignments_CourseId",
                table: "CourseAssignments",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseAssignments_UserId",
                table: "CourseAssignments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialPDFs_AspNetUsers_UserId",
                table: "MaterialPDFs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaterialPDFs_AspNetUsers_UserId",
                table: "MaterialPDFs");

            migrationBuilder.DropIndex(
                name: "IX_MaterialPDFs_UserId",
                table: "MaterialPDFs");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "MaterialPDFs");

            migrationBuilder.DropTable(
                name: "CourseAssignments");

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Courses",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_OwnerId",
                table: "Courses",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_AspNetUsers_OwnerId",
                table: "Courses",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
