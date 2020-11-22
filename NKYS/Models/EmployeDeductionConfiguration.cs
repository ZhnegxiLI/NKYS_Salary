using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NKYS.Models
{
    public class EmployeDeductionConfiguration:BaseObject
    {
        public long EmployeId { get; set; }
        public ProductionValueType? LinkedProductionValueTypeId { get; set; } // 成套或钣金或收款
        public Employe Employe { get; set; }
    }
}
