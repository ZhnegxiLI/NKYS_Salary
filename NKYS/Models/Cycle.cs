using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NKYS.Models
{
    public class Cycle: BaseObject
    {
        [Required]
        [DisplayName("周期名字")]
        public string Label { get; set; }
        [DisplayName("年")]
        public int Year { get; set; }
        [DisplayName("月")]
        public int Month { get; set; }
        [Required]
        [DisplayName("起始日期")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]
        public DateTime FromDate { get; set; }
        [DisplayName("结束日期")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]
        [Required]
        public DateTime ToDate { get; set; }
        [DisplayName("是否有效")]
        public bool Validity { get; set; } = true;
        [DisplayName("标准工作时")]
        [Required]
        public decimal? StandardWorkingHours { get; set; } // 该月标准工时
        [DisplayName("解释说明")]
        public string Comment { get; set; }// 解释说明

        public DateTime? LastCalculedSalaryTime { get; set; } // 最后一次进行计算时间, 以确保没有上一次的excel作废
        public DateTime? ValidationTime { get; set; } // 确认时间，确认后该Cycle 内所有数据应该被锁定,一般在发完工资之后

    }
}
