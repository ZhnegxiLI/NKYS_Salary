using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NKYS.Models
{
    public class Salary: BaseObject
    {
        public long CycleId { get; set; }

        public long EmployeId { get; set; }

        [DisplayName("工时")]
        public decimal? WorkingHours { get; set; }// 工时
        [DisplayName("分数")]
        public decimal? WorkingScore { get; set; }// 工作评分
        [DisplayName("缺勤工时")]
        public decimal? AbsentHours { get; set; }// 缺勤工时
        [DisplayName("缺勤扣费")]
        public decimal? AbsentDeduct { get; set; } // 缺勤
        [DisplayName("加班费")]
        public decimal? OvertimePay { get; set; } // 加班费
        [DisplayName("可补休工时")]
        public decimal? DeferredHolidayHours { get; set; }// 可补休工时
        [DisplayName("个人社保费")]
        public decimal? SocialSercurityFee { get; set; } // 社保费用
        [DisplayName("社保自理")]
        public decimal? SelfPaySocialSercurityFee { get; set; } // 社保自理
        [DisplayName("个人公积金")]
        public decimal? HousingReserves { get; set; } // 公积金费用
        [DisplayName("全勤奖")]
        public decimal? FullPresencePay { get; set; } // 全勤奖

        [DisplayName("工龄奖")]
        public decimal? SeniorityPay { get; set; } // 加班费
        [DisplayName("交通及延时补贴")]
        public decimal? TransportFee { get; set; }// 交通费用
        [DisplayName("住宿费")]
        public decimal? DormFee { get; set; } // 住宿费用
        [DisplayName("水、电费")]
        public decimal? DormOtherFee { get; set; } // 住宿杂费用
        [DisplayName("其他费用")]
        public decimal? OtherFee { get; set; } // 其他费用
        [DisplayName("解释说明")]
        public string Comment { get; set; }// 解释说明

        public Cycle Cycle { get; set; }
        public Employe Employe { get; set; }
    }
}
