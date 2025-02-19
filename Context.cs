﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFramwork.net5
{
    internal class Context :DbContext
    {
        public Context()
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<Empolyee> Empolyee { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Branch> Branchs { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Project> Projects { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data source = SABER ; Initial catalog = frameCore ; integrated security = true ");

            optionsBuilder.LogTo(log => Debug.WriteLine(log));


            // lazy loading
           //optionsBuilder.UseLazyLoadingProxies(true);

            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Writing Fluent API configurations 

            modelBuilder.Entity<Department>().ToTable("Departments"); // fluent API

            //modelBuilder.Entity<Attendance>().ToTable("Attendances");

            modelBuilder.Entity<Department>().Property(d => d.ID).IsRequired(true);

            modelBuilder.Entity<Attendance>().HasKey(e => new { e.EmployeeID ,e.Date });//composet key

            //modelBuilder.Entity<Empolyee>().HasQueryFilter(e => !e.Deleted);// to add condition in query to filter data


            //modelBuilder.Entity<Department>()
            //    .Property<bool>("Deleted").IsRequired(true).HasDefaultValue(false);
            foreach(var item in modelBuilder.Model.GetEntityTypes())
            {
                modelBuilder.Entity(item.Name)  
                    .Property<bool>("Deleted").IsRequired(true).HasDefaultValue(false);
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
