using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NKYS.Account.Model
{
    public class LoginViewModel
    {

        [Display(Name = "用户名")]
        [Required]
        public string Username { get; set; }
        [Display(Name = "密码")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "记住我")]
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}
