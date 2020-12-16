using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace NKYS.Models.ViewModel
{
    public class GroupsIndex : Groups
    {
        public GroupsIndex()
        {
            Groups = new List<Groups>();
        }
        public List<Groups> Groups { get; set; }
    }

}
