using NKYSSPA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NKYSSPA.Models
{
    public class SalaryCalculLog: BaseObject
    {
        public long? CalculTime { get; set; }
        public long? UserId { get; set; }
        public long PeriodId { get; set; }
        public bool StatusSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }
}
