using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NKYS.Models
{
    public class ExportConfiguration: BaseObject
    {
        public string ExportName { get; set; }

        public string ExportModel { get; set; }
    }
}
