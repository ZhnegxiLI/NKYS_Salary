using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NKYS.Models
{
    public class Department:BaseObject
    {
        [DisplayName("部门名")]
        [Required]
        public string Name { get; set; }

        [DisplayName("解释说明")]
        public string Comment { get; set; }
        public List<Groups> Groups { get; set; }
    }
}
