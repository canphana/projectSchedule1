using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace projectSchedule1.Migrations
{
    public partial class Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Employee_Status_StatusId", table: "Employee");
            migrationBuilder.DropForeignKey(name: "FK_EmployeeTask_Employee_EmployeeId", table: "EmployeeTask");
            migrationBuilder.DropForeignKey(name: "FK_EmployeeTask_Task_TaskId", table: "EmployeeTask");
            migrationBuilder.DropForeignKey(name: "FK_File_Employee_EmployeeId", table: "File");
            migrationBuilder.DropForeignKey(name: "FK_LoginAccount_AccountType_AccountTypeId", table: "LoginAccount");
            migrationBuilder.DropForeignKey(name: "FK_LoginAccount_Employee_EmployeeId", table: "LoginAccount");
            migrationBuilder.DropForeignKey(name: "FK_Project_Client_ClientId", table: "Project");
            migrationBuilder.DropForeignKey(name: "FK_Project_Status_StatusId", table: "Project");
            migrationBuilder.DropForeignKey(name: "FK_ProjectReport_Employee_EmployeeId", table: "ProjectReport");
            migrationBuilder.DropForeignKey(name: "FK_Task_Project_ProjectId", table: "Task");
            migrationBuilder.DropForeignKey(name: "FK_Task_Status_StatusId", table: "Task");
            migrationBuilder.AlterColumn<string>(
                name: "ProjectName",
                table: "Project",
                type: "nvarchar(200)",
                nullable: false);
            migrationBuilder.AlterColumn<string>(
                name: "ProjectDesc",
                table: "Project",
                type: "nvarchar(200)",
                nullable: false);
            migrationBuilder.AlterColumn<string>(
                name: "EmployeePhoneNo",
                table: "Employee",
                nullable: false);
            migrationBuilder.AlterColumn<string>(
                name: "EmployeeEmail",
                table: "Employee",
                nullable: false);
            migrationBuilder.AlterColumn<string>(
                name: "EmployeeAddr",
                table: "Employee",
                nullable: false);
            migrationBuilder.AlterColumn<string>(
                name: "ClientPhoneNo",
                table: "Client",
                nullable: false);
            migrationBuilder.AlterColumn<string>(
                name: "ClientName",
                table: "Client",
                nullable: false);
            migrationBuilder.AlterColumn<string>(
                name: "ClientEmail",
                table: "Client",
                nullable: false);
            migrationBuilder.AlterColumn<string>(
                name: "ClientAddress",
                table: "Client",
                nullable: false);
            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Status_StatusId",
                table: "Employee",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "StatusId",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeTask_Employee_EmployeeId",
                table: "EmployeeTask",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeTask_Task_TaskId",
                table: "EmployeeTask",
                column: "TaskId",
                principalTable: "Task",
                principalColumn: "TaskId",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_File_Employee_EmployeeId",
                table: "File",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_LoginAccount_AccountType_AccountTypeId",
                table: "LoginAccount",
                column: "AccountTypeId",
                principalTable: "AccountType",
                principalColumn: "AccountTypeId",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_LoginAccount_Employee_EmployeeId",
                table: "LoginAccount",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_Project_Client_ClientId",
                table: "Project",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "ClientId",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_Project_Status_StatusId",
                table: "Project",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "StatusId",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_ProjectReport_Employee_EmployeeId",
                table: "ProjectReport",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_Task_Project_ProjectId",
                table: "Task",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.NoAction);
            migrationBuilder.AddForeignKey(
                name: "FK_Task_Status_StatusId",
                table: "Task",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "StatusId",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Employee_Status_StatusId", table: "Employee");
            migrationBuilder.DropForeignKey(name: "FK_EmployeeTask_Employee_EmployeeId", table: "EmployeeTask");
            migrationBuilder.DropForeignKey(name: "FK_EmployeeTask_Task_TaskId", table: "EmployeeTask");
            migrationBuilder.DropForeignKey(name: "FK_File_Employee_EmployeeId", table: "File");
            migrationBuilder.DropForeignKey(name: "FK_LoginAccount_AccountType_AccountTypeId", table: "LoginAccount");
            migrationBuilder.DropForeignKey(name: "FK_LoginAccount_Employee_EmployeeId", table: "LoginAccount");
            migrationBuilder.DropForeignKey(name: "FK_Project_Client_ClientId", table: "Project");
            migrationBuilder.DropForeignKey(name: "FK_Project_Status_StatusId", table: "Project");
            migrationBuilder.DropForeignKey(name: "FK_ProjectReport_Employee_EmployeeId", table: "ProjectReport");
            migrationBuilder.DropForeignKey(name: "FK_Task_Project_ProjectId", table: "Task");
            migrationBuilder.DropForeignKey(name: "FK_Task_Status_StatusId", table: "Task");
            migrationBuilder.AlterColumn<string>(
                name: "ProjectName",
                table: "Project",
                type: "nvarchar(200)",
                nullable: true);
            migrationBuilder.AlterColumn<string>(
                name: "ProjectDesc",
                table: "Project",
                type: "nvarchar(200)",
                nullable: true);
            migrationBuilder.AlterColumn<string>(
                name: "EmployeePhoneNo",
                table: "Employee",
                nullable: true);
            migrationBuilder.AlterColumn<string>(
                name: "EmployeeEmail",
                table: "Employee",
                nullable: true);
            migrationBuilder.AlterColumn<string>(
                name: "EmployeeAddr",
                table: "Employee",
                nullable: true);
            migrationBuilder.AlterColumn<string>(
                name: "ClientPhoneNo",
                table: "Client",
                nullable: true);
            migrationBuilder.AlterColumn<string>(
                name: "ClientName",
                table: "Client",
                nullable: true);
            migrationBuilder.AlterColumn<string>(
                name: "ClientEmail",
                table: "Client",
                nullable: true);
            migrationBuilder.AlterColumn<string>(
                name: "ClientAddress",
                table: "Client",
                nullable: true);
            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Status_StatusId",
                table: "Employee",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "StatusId",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeTask_Employee_EmployeeId",
                table: "EmployeeTask",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeTask_Task_TaskId",
                table: "EmployeeTask",
                column: "TaskId",
                principalTable: "Task",
                principalColumn: "TaskId",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_File_Employee_EmployeeId",
                table: "File",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_LoginAccount_AccountType_AccountTypeId",
                table: "LoginAccount",
                column: "AccountTypeId",
                principalTable: "AccountType",
                principalColumn: "AccountTypeId",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_LoginAccount_Employee_EmployeeId",
                table: "LoginAccount",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_Project_Client_ClientId",
                table: "Project",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "ClientId",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_Project_Status_StatusId",
                table: "Project",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "StatusId",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_ProjectReport_Employee_EmployeeId",
                table: "ProjectReport",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_Task_Project_ProjectId",
                table: "Task",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_Task_Status_StatusId",
                table: "Task",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "StatusId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
