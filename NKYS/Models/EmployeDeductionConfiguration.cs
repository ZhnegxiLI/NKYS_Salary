using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NKYS.Models
{
    public class EmployeDeductionConfiguration:BaseObject
    {
        public long EmployeId { get; set; }
        [DisplayName("分成比例")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal DeductionSharePropotion { get; set; }
        [DisplayName("产值类型")]
        public ProductionValueType? LinkedProductionValueTypeId { get; set; } // 成套或钣金或收款
        public Employe Employe { get; set; }
    }
}
