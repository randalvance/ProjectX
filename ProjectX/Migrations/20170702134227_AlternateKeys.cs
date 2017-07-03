using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectX.Migrations
{
    public partial class AlternateKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AccountNumber",
                table: "BankAccounts",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "AccountName",
                table: "BankAccounts",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddUniqueConstraint(
                name: "AK_BankAccounts_AccountName",
                table: "BankAccounts",
                column: "AccountName");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_BankAccounts_AccountNumber",
                table: "BankAccounts",
                column: "AccountNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_BankAccounts_AccountName",
                table: "BankAccounts");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_BankAccounts_AccountNumber",
                table: "BankAccounts");

            migrationBuilder.AlterColumn<string>(
                name: "AccountNumber",
                table: "BankAccounts",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "AccountName",
                table: "BankAccounts",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
