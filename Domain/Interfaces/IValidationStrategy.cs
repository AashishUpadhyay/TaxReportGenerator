using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxReportGenerator.Domain.Interfaces
{
    public interface IValidationStrategy<T>
    {
        void Validate(T input);
        void Validate(List<T> input);
    }
}
