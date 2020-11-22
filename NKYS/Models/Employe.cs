using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NKYS.Models
{
    public class Employe: BaseObject
    {
        [DisplayName("小组")]
        public long GroupsId { get; set; }
        [DisplayName("姓名")]
        public string Name { get; set; }
        [DisplayName("入职时间")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]
        public DateTime? EntreEntrepriseDate { get; set; }
        [DisplayName("打卡机编号")]
        public long? ExternalId { get; set; } // 打卡机上的序号

        [DisplayName("技术级别")]
        public decimal? TechnicalLevel { get; set; } // 技术级别

        [DisplayName("社保是否自理")]
        public bool SelfPaySocialSercurity { get; set; } //社保自理
        [DisplayName("住房公积金是否自理")]
        public bool SelfPayHousingReserves { get; set; } //住房公积金自理
        [DisplayName("是否住宿")]
        public bool HasDorm { get; set; } // 是否住宿
        [DisplayName("交通补贴")]
        public decimal? TransportFee { get; set; } // 交通补贴
        [DisplayName("岗位补贴")]
        public decimal? PositionPay { get; set; } // 岗位补贴
        [DisplayName("是否为小组组长")]
        public bool IsChefOfGroup { get; set; } // 是否为组长

        [DisplayName("工龄奖")]
        public decimal? SeniorityPay { get; set; } // 工龄奖
        [DisplayName("固定工资")]
        public decimal? FixSalary { get; set; } // 固定工资

        [DisplayName("工资提成百分比")]
        public decimal? DeductionPercentage { get; set; } // 100% 提成百分比
        [DisplayName("是否为零时工")]
        public bool IsTemporaryEmploye { get; set; } // 临时工
        [DisplayName("所属小组")]
        public Groups Groups { get; set; }
        [DisplayName("离职日期")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]
        public DateTime? DepartDate { get; set; }// 离职日期

        public List<EmployeDeductionConfiguration> EmployeDeductionConfiguration { get; set; }
    }
}
