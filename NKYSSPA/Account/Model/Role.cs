using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NKYSSPA.Account.Model
{
    public class Role: IdentityRole<long>
    {
        public Role()
        {

        }

        public string  Label { get; set; }
    }
}
