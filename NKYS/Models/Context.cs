using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NKYS.Account.Model;
using NKYS.Models;
using NKYS.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NKYS.Models
{
    public class Context: IdentityDbContext<User, IdentityRole<long>, long>
    {
        public Context(DbContextOptions<Context> options)
         : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SalariesCalculModel>().HasNoKey();
        }

        public virtual DbSet<Cycle> Cycle { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<Employe> Employe { get; set; }
        public virtual DbSet<EmployeDeductionConfiguration> EmployeDeductionConfiguration { get; set; }
        public virtual DbSet<Groups> Groups { get; set; }
        public virtual DbSet<ProductionValue> ProductionValue { get; set; }
        public virtual DbSet<Salary> Salary { get; set; }
        public virtual DbSet<SalaryCalculLog> SalaryCalculLog { get; set; }
        public virtual DbSet<SalariesCalculModel> SalariesCalculModel { get; set; }
        
    }
}
