using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;

namespace projectSchedule1.Models
{
    public class AppDbContext: DbContext
    {
        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
 
            modelBuilder.Entity<EmployeeTask>().HasKey(x => new { x.EmployeeId, x.TaskId });

            modelBuilder.Entity<EmployeeTask>()
                .HasOne(et => et.Employee)
                 .WithMany(e => e.EmployeeTasks)
                 .HasForeignKey(et => et.EmployeeId);

            modelBuilder.Entity<EmployeeTask>()
                .HasOne(et => et.ProjectTask)
                .WithMany(t => t.EmployeeTasks)
                .HasForeignKey(et => et.TaskId);

            modelBuilder.Entity<EmployeeProject>().HasKey(x => new { x.EmployeeId,x.ProjectId });
            modelBuilder.Entity<EmployeeProject>()
            .HasOne(ep => ep.Employee)
             .WithMany(e => e.EmployeeProjects)
             .HasForeignKey(et => et.EmployeeId)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EmployeeProject>()
                .HasOne(ep => ep.Project)
                .WithMany(t => t.EmployeeProjects)
                .HasForeignKey(et => et.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProjectReport>()
               .HasOne(pr => pr.Project)
               .WithMany(p => p.ProjectReports)
               .HasForeignKey(pr => pr.ProjectId)
               .OnDelete(DeleteBehavior.Restrict);


        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<ProjectReport> ProjectReports { get; set; }
        public DbSet<LoginAccount> LoginAcounts { get; set; }

        public DbSet<EmployeeProject> EmployeeProjects { get; set; }
        public DbSet<EmployeeTask> EmployeeTasks { get; set; }

    }

}
