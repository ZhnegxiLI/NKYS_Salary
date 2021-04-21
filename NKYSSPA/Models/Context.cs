using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NKYSSPA.Account.Model;

namespace NKYSSPA.Models
{
    public class Context : IdentityDbContext<User, Role, long>
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
        public virtual DbSet<SalaryCalculLog> SalaryCalculLog { get; set; }

        public virtual DbSet<ExportConfiguration> ExportConfiguration { get; set; }

    }
}
