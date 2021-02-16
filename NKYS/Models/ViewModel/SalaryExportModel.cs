using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NKYS.Models.ViewModel
{
    public class SalaryExportModel: Salary
    {
        public SalaryExportModel()
        {
        }

        public string EmployeeName { get; set; }
        public string ExternalId { get; set; }

        public string DepartmentName { get; set; }

        public string GroupName { get; set; }

    }
}
