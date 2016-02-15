using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxReportGenerator.DataObjects
{
    public class Invoice
    {
        public string InvoiceNumber { get; set; }
        public string ClientNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceAmount { get; set; }
    }

    public class InternationalInvoice
    {
        
    }

    public class DomesticInvoice
    {

    }
}
