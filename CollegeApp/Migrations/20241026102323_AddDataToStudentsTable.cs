using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CollegeApp.Migrations
{
    /// <inheritdoc />
    public partial class AddDataToStudentsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "Address", "DOB", "Email", "StudentName" },
                values: new object[,]
                {
                    { 1, "123,4,qasjh", new DateTime(2022, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "aih@gmail.com", "AVc" },
                    { 2, "wnx.s8,we", new DateTime(2022, 8, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "iyt@gmail.com", "Iyt" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
