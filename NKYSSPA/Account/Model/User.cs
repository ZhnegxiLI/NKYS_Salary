using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NKYSSPA.Account.Model
{
    public class User: IdentityUser<long>
    {
        public User()
        {

        }
        public bool Validity { get; set; }
    }
}
