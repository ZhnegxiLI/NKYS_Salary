using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace NKYS.Models.ViewModel
{
    public class ProductionValuesIndex: ProductionValue
    {
        public ProductionValuesIndex()
        {
            ProductionValues = new List<ProductionValue>();
        }
        public List<ProductionValue> ProductionValues { get; set; }
    }

}
