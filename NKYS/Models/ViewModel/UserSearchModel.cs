using Microsoft.AspNetCore.Identity;
using NKYS.Account.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NKYS.Models.ViewModel
{
    public class UserSearchModel
    {
        public long Id { get; set; }
        [Display(Name = "用户名")]
        public string Username { get; set; }
        [Display(Name = "是否有效")]
        public bool Validity { get; set; }

        [Display(Name = "权限")]
        public string Role { get; set; }
    }
}
