using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NKYS.Models
{
    public class Employe: BaseObject
    {
        [DisplayName("小组")]
        public long GroupsId { get; set; }
        [DisplayName("所属小组")]
        public Groups Groups { get; set; }
        [DisplayName("姓名")]
        public string Name { get; set; }
        [DisplayName("入职时间")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]
        public DateTime? EntreEntrepriseDate { get; set; }
        [DisplayName("打卡机编号")]
        public string ExternalId { get; set; } // 打卡机上的序号


        [DisplayName("技术级别")]
        public decimal? TechnicalLevel { get; set; } // 技术级别
        [DisplayName("固定工资")]
        public decimal? FixSalary { get; set; } // 固定工资
        [DisplayName("是否为零时工")]
        public bool IsTemporaryEmploye { get; set; } // 临时工
        [DisplayName("离职日期")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]
        public DateTime? DepartDate { get; set; }// 离职日期


        [DisplayName("是否住宿")]
        public bool HasDorm { get; set; } = false; // 是否住宿
        [DisplayName("是否拥有交通补贴")]
        public bool HasTransportFee { get; set; } = false; // 交通补贴
        
        [DisplayName("岗位补贴")]
        public decimal? PositionPay { get; set; } // 岗位补贴
        [DisplayName("是否为小组组长")]
        public bool IsChefOfGroup { get; set; } // 是否为组长

        [DisplayName("个人社保费")]
        public decimal? SocialSercurityFee { get; set; } // TODO: 社保费用-357.91
        [DisplayName("社保自理")]
        public decimal? SelfPaySocialSercurityFee { get; set; } //TODO：确认 社保自理退300
        [DisplayName("住房公积金")]
        public decimal? HousingReservesFee { get; set; } // TODO 公积金费用 大部分人退200


        [DisplayName("工龄奖")]
        public decimal? SeniorityPay { get; set; } // 工龄奖


        [DisplayName("工资提成百分比")]
        public decimal? DeductionPercentage { get; set; } // 100% 提成百分比




        [DisplayName("提成产值类型")]
        [NotMapped]
        public List<ProductionValueType> ProductionValueTypeId { get; set; }
        public List<EmployeDeductionConfiguration> EmployeDeductionConfiguration { get; set; }
    }
}
