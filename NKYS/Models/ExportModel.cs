using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NKYS.Models
{
    public class ExportModel:BaseObject
    {
        public string Name { get; set; }
        public int Order { get; set; }
        public string DisplayName { get; set; }

    }
}
