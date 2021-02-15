using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NKYS.Models.ViewModel
{
    public class UserUpdateOrCreateModel
    {
        public long Id { get; set; }

        [Required, Display(Name ="用户名")]
        public string Username { get; set; }
        [Display(Name = "是否有效")]
        public bool Validity { get; set; }

        [Required, Display(Name = "权限")]
        public long RoleId { get; set; }
    }
}
