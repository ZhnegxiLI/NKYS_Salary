using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NKYS.Models
{
    public class Salary: BaseObject
    {
        public long CycleId { get; set; }

        public long EmployeId { get; set; }
        public decimal? WorkingHours { get; set; }// 工时
        public decimal? WorkingScore { get; set; }// 工作评分
        public decimal? AbsentHours { get; set; }// 缺勤工时
        public decimal? SocialSercurityFee { get; set; } // 社保费用
        public decimal? HousingReserves { get; set; } // 公积金费用

        public decimal? FullPresencePay { get; set; } // 全勤奖
        public decimal? OvertimePay { get; set; } // 加班费
        public decimal? AbsentDeduct { get; set; } // 缺勤
        public decimal? DormFee { get; set; } // 住宿费用
        public decimal? TransportFee { get; set; }// 交通费用
        public decimal? OtherFee { get; set; } // 其他费用
        public string Comment { get; set; }// 解释说明

        public Cycle Cycle { get; set; }
        public Employe Employe { get; set; }
    }
}
