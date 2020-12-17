using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NKYS.Account.Model
{
    public class User: IdentityUser<long>
    {
        public User()
        {

        }
    }
}
