using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using projectSchedule1.Models;

namespace projectSchedule1.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("projectSchedule1.Models.AccountType", b =>
                {
                    b.Property<int>("AccountTypeId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountTypeDesc")
                        .HasAnnotation("Relational:ColumnType", "nvarchar(200)");

                    b.Property<string>("AccountTypeName")
                        .HasAnnotation("Relational:ColumnType", "nvarchar(200)");

                    b.HasKey("AccountTypeId");
                });

            modelBuilder.Entity("projectSchedule1.Models.Client", b =>
                {
                    b.Property<int>("ClientId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClientAddress")
                        .IsRequired();

                    b.Property<string>("ClientEmail")
                        .IsRequired();

                    b.Property<string>("ClientName")
                        .IsRequired();

                    b.Property<string>("ClientPhoneNo")
                        .IsRequired();

                    b.HasKey("ClientId");
                });

            modelBuilder.Entity("projectSchedule1.Models.Employee", b =>
                {
                    b.Property<int>("EmployeeId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("EmployeeAddr")
                        .IsRequired();

                    b.Property<DateTime>("EmployeeBirthDate");

                    b.Property<string>("EmployeeEmail")
                        .IsRequired();

                    b.Property<string>("EmployeeFirstName")
                        .IsRequired()
                        .HasAnnotation("Relational:ColumnType", "nvarchar(200)");

                    b.Property<string>("EmployeeLastName")
                        .IsRequired()
                        .HasAnnotation("Relational:ColumnType", "nvarchar(200)");

                    b.Property<string>("EmployeePhoneNo")
                        .IsRequired();

                    b.Property<int>("StatusId");

                    b.HasKey("EmployeeId");
                });

            modelBuilder.Entity("projectSchedule1.Models.EmployeeProject", b =>
                {
                    b.Property<int>("EmployeeId");

                    b.Property<int>("ProjectId");

                    b.HasKey("EmployeeId", "ProjectId");
                });

            modelBuilder.Entity("projectSchedule1.Models.EmployeeTask", b =>
                {
                    b.Property<int>("EmployeeId");

                    b.Property<int>("TaskId");

                    b.HasKey("EmployeeId", "TaskId");
                });

            modelBuilder.Entity("projectSchedule1.Models.File", b =>
                {
                    b.Property<int>("FileId")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Content");

                    b.Property<string>("ContentType")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<int>("EmployeeId");

                    b.Property<string>("FileName")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<int>("FileType");

                    b.Property<int?>("ProjectReportProjectReportId");

                    b.HasKey("FileId");
                });

            modelBuilder.Entity("projectSchedule1.Models.LoginAccount", b =>
                {
                    b.Property<int>("LoginAccountId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccountTypeId");

                    b.Property<int>("EmployeeId");

                    b.Property<string>("LoginAccountName")
                        .IsRequired();

                    b.Property<string>("LoginAccountPassword")
                        .IsRequired();

                    b.HasKey("LoginAccountId");
                });

            modelBuilder.Entity("projectSchedule1.Models.Project", b =>
                {
                    b.Property<int>("ProjectId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ClientId");

                    b.Property<string>("ProjectDesc")
                        .IsRequired()
                        .HasAnnotation("Relational:ColumnType", "nvarchar(200)");

                    b.Property<DateTime>("ProjectEndDate");

                    b.Property<string>("ProjectName")
                        .IsRequired()
                        .HasAnnotation("Relational:ColumnType", "nvarchar(200)");

                    b.Property<DateTime>("ProjectStartDate");

                    b.Property<int>("StatusId");

                    b.HasKey("ProjectId");
                });

            modelBuilder.Entity("projectSchedule1.Models.ProjectReport", b =>
                {
                    b.Property<int>("ProjectReportId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("EmployeeId");

                    b.Property<int>("ProjectId");

                    b.Property<DateTime>("ProjectReportDate");

                    b.Property<string>("ProjectReportDesc");

                    b.HasKey("ProjectReportId");
                });

            modelBuilder.Entity("projectSchedule1.Models.Status", b =>
                {
                    b.Property<int>("StatusId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("StatusDesc")
                        .HasAnnotation("Relational:ColumnType", "nvarchar(200)");

                    b.Property<string>("StatusName")
                        .HasAnnotation("Relational:ColumnType", "nvarchar(200)");

                    b.HasKey("StatusId");
                });

            modelBuilder.Entity("projectSchedule1.Models.Task", b =>
                {
                    b.Property<int>("TaskId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ProjectId");

                    b.Property<int>("StatusId");

                    b.Property<string>("TaskDesc")
                        .HasAnnotation("Relational:ColumnType", "nvarchar(200)");

                    b.Property<DateTime>("TaskEndDate");

                    b.Property<DateTime>("TaskStartDate");

                    b.HasKey("TaskId");
                });

            modelBuilder.Entity("projectSchedule1.Models.Employee", b =>
                {
                    b.HasOne("projectSchedule1.Models.Status")
                        .WithOne()
                        .HasForeignKey("projectSchedule1.Models.Employee", "StatusId");
                });

            modelBuilder.Entity("projectSchedule1.Models.EmployeeProject", b =>
                {
                    b.HasOne("projectSchedule1.Models.Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId");

                    b.HasOne("projectSchedule1.Models.Project")
                        .WithMany()
                        .HasForeignKey("ProjectId");
                });

            modelBuilder.Entity("projectSchedule1.Models.EmployeeTask", b =>
                {
                    b.HasOne("projectSchedule1.Models.Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId");

                    b.HasOne("projectSchedule1.Models.Task")
                        .WithMany()
                        .HasForeignKey("TaskId");
                });

            modelBuilder.Entity("projectSchedule1.Models.File", b =>
                {
                    b.HasOne("projectSchedule1.Models.Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId");

                    b.HasOne("projectSchedule1.Models.ProjectReport")
                        .WithMany()
                        .HasForeignKey("ProjectReportProjectReportId");
                });

            modelBuilder.Entity("projectSchedule1.Models.LoginAccount", b =>
                {
                    b.HasOne("projectSchedule1.Models.AccountType")
                        .WithMany()
                        .HasForeignKey("AccountTypeId");

                    b.HasOne("projectSchedule1.Models.Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId");
                });

            modelBuilder.Entity("projectSchedule1.Models.Project", b =>
                {
                    b.HasOne("projectSchedule1.Models.Client")
                        .WithMany()
                        .HasForeignKey("ClientId");

                    b.HasOne("projectSchedule1.Models.Status")
                        .WithOne()
                        .HasForeignKey("projectSchedule1.Models.Project", "StatusId");
                });

            modelBuilder.Entity("projectSchedule1.Models.ProjectReport", b =>
                {
                    b.HasOne("projectSchedule1.Models.Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId");

                    b.HasOne("projectSchedule1.Models.Project")
                        .WithMany()
                        .HasForeignKey("ProjectId");
                });

            modelBuilder.Entity("projectSchedule1.Models.Task", b =>
                {
                    b.HasOne("projectSchedule1.Models.Project")
                        .WithMany()
                        .HasForeignKey("ProjectId");

                    b.HasOne("projectSchedule1.Models.Status")
                        .WithOne()
                        .HasForeignKey("projectSchedule1.Models.Task", "StatusId");
                });
        }
    }
}
