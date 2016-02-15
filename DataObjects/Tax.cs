using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxReportGenerator.DataObjects
{
    public class Tax
    {
        public string Taxtype { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Rate { get; set; }
        public double TaxAmount{ get; set; }
    }
}
