using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxReportGenerator.DataObjects;

namespace Services.Interfaces
{
    public interface ITaxService<T>
    {
        IEnumerable<T> RetrieveTaxes(string source);
    }
}
