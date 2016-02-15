using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IInvoiceRetriever<T>
    {
        IEnumerable<T> RetrieveInvoices(string source);
    }
}
