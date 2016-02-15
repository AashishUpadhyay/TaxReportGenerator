using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exceptions
{
    /// <summary>
    /// To handle Custom Business Exception
    /// </summary>
    public class TaxGeneratorReportBusinessException : Exception
    {
        public TaxGeneratorReportBusinessException(string message) : base(message)
        {

        }

        public TaxGeneratorReportBusinessException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
