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
        public decimal? WorkingHours { get; set; }
        [DisplayName("日班工时")]
        public decimal? WorkingHoursDay { get; set; }
        [DisplayName("夜班工时")]
        public decimal? WorkingHoursNight { get; set; }
        [DisplayName("假日工时")]
        public decimal? WorkingHoursHoliday{ get; set; }



        [DisplayName("分数")]
        public decimal? WorkingScore { get; set; }// 工作评分
        [DisplayName("缺勤工时")]
        public decimal? AbsentHours { get; set; }// 缺勤工时
        [DisplayName("可补休工时")]
        public decimal? DeferredHolidayHours { get; set; }// 可补休工时

        [DisplayName("出勤天数")]
        public decimal? WorkingDays { get; set; }// TODO: 改成日班夜班假日工时




        [DisplayName("解释说明")]
        public string Comment { get; set; }// 解释说明


        // 工资计算列
        [DisplayName("计件工资/固定工资")]
        public decimal? BasicSalary { get; set; }
        [DisplayName("加班工资")]
        public decimal? OvertimePay { get; set; } // 加班费

        [DisplayName("缺勤扣费")]
        public decimal? AbsentDeduct { get; set; } // 缺勤

        [DisplayName("职务津贴")]
        public decimal? PositionPay { get; set; } // 岗位补贴
        [DisplayName("交通及延时补贴")]
        public decimal? TransportFee { get; set; }// 交通费用
        [DisplayName("全勤奖")]
        public decimal? FullPresencePay { get; set; } // 全勤奖
        [DisplayName("工龄奖")]
        public decimal? SeniorityPay { get; set; } // 加班费
        [DisplayName("房租费")]
        public decimal? DormFee { get; set; } // 住宿费用
        [DisplayName("水、电费")]
        public decimal? DormOtherFee { get; set; } // 住宿杂费用

        [DisplayName("其他奖励费用")]
        public decimal? OtherRewardFee { get; set; } // 其他费用
        [DisplayName("其他扣罚费用")]
        public decimal? OtherPenaltyFee { get; set; } // 其他费用


        [DisplayName("个人社保费")]
        public decimal? SocialSercurityFee { get; set; } // TODO: 社保费用-357.91

        [DisplayName("社保自理")]
        public decimal? SelfPaySocialSercurityFee { get; set; } //TODO：确认 社保自理退300
        [DisplayName("个人公积金")]
        public decimal? HousingReservesFee { get; set; } // TODO 公积金费用 大部分人退200



        [DisplayName("税前工资")]
        public decimal? NetSalary { get; set; }
        [DisplayName("工资税")]
        public decimal? SalaryTax { get; set; }
        [DisplayName("税后实发工资")]
        public decimal? FinalSalary { get; set; }


        [DisplayName("确认人")]
        public long? ValidatedBy { get; set; }
        [DisplayName("确认时间")]
        public DateTime? ValidatedOn { get; set; }
        [DisplayName("确认")]
        public bool Validity { get; set; } = false;


        [DisplayName("经理确认人")]
        public long? FinalValidatedBy { get; set; }
        [DisplayName("经理确认时间")]
        public DateTime? FinalValidatedOn { get; set; }
        [DisplayName("经理确认")]
        public bool FinalValidity { get; set; } = false;


        public Cycle Cycle { get; set; }
        public Employe Employe { get; set; }
    }
}
