using Microsoft.EntityFrameworkCore;
using NKYS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NKYS.Models
{
    public class Context: DbContext
    {
        public Context(DbContextOptions<Context> options)
         : base(options)
        {
        }

        public virtual DbSet<Cycle> Cycle { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<Employe> Employe { get; set; }
        public virtual DbSet<EmployeDeductionConfiguration> EmployeDeductionConfiguration { get; set; }
        public virtual DbSet<Groups> Groups { get; set; }
        public virtual DbSet<ProductionValue> ProductionValue { get; set; }
        public virtual DbSet<Salary> Salary { get; set; }
    }
}
