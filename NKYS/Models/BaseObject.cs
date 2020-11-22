using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NKYS.Models
{
    public class BaseObject
    {
        [Key]
        public long Id { get; set; }
        public long? CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
