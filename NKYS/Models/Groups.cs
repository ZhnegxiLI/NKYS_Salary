using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NKYS.Models
{
    public class Groups: BaseObject
    {
        [DisplayName("小组名")]
        [Required]
        public string Name { get; set; }
        [Required]
        public long DepartmentId { get; set; }
        [DisplayName("分成比例")]
        public decimal? SharePropotion { get; set; }
        [DisplayName("关联产值类型")]
        public ProductionValueType? ProductionValueTypeId { get; set; } // 钣金或成套
        [DisplayName("该小组是否为固定工资模式")]
        public bool IsFixSalary { get; set; }
        [DisplayName("解释说明")]
        public string Comment { get; set; }
        [DisplayName("部门")]
        public Department Department { get; set; }

        public List<Employe> Employes { get; set; }
    }
}
