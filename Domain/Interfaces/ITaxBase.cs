using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxReportGenerator.Domain.Interfaces
{
    public enum TaxtType { FRT, ST, EC, WC }

    public interface ITaxBase : IBusinessObject
    {
        TaxtType Taxtype { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        double Rate { get; set; }
    }
}
