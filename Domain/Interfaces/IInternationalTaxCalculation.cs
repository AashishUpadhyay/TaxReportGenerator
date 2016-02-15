using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxReportGenerator.Domain.Interfaces
{
    public interface IInternationalTaxCalculation<T1, T2, R>
    {
        R CalculateTax(T1 input1, List<T2> input2);
        List<R> CalculateTax(List<T1> input, List<T2> input2);
    }
}
