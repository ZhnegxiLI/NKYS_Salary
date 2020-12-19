using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace NKYS.Models.ViewModel
{
    public class SalarieSearchModel: Salary
    {
        public SalarieSearchModel()
        {
            Salaries = new List<Salary>();
        }

        public List<Salary> Salaries { get; set; }

        [DisplayName("部门")]
        public long? DepartmentId { get; set; }
        [DisplayName("小组")]
        public long? GroupsId { get; set; }
        [DisplayName("周期")]
        public long? PeriodId { get; set; }
    }
}
