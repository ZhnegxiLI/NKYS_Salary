using NKYSSPA.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NKYSSPA.Models
{
    public class ProductionValue: BaseObject
    {
        [DisplayName("周期")]
        [Required]
        public long CycleId { get; set; }
        [DisplayName("产值(元)")]
        [Required]
        public decimal Value { get; set; }// 产值
        [DisplayName("解释说明")]
        public string Comment { get; set; } // 解释说明
        [DisplayName("产值类型")]
        [Required]
        public ProductionValueType ProductionValueTypeId { get; set; } // 产值种类: 钣金， 成套， 收款
        [DisplayName("周期")]
        public Cycle Cycle { get; set; }
        [DisplayName("是否锁定")]
        public bool Validity { get; set; } // Valid 之后不可再次修改
    }
}
