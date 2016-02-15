using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxReportGenerator.DataObjects
{
    public class InvoiceTaxDetails
    {
        public List<Tax> AllTaxTypes { get; set; }
        public List<InvoiceTax> InvoiceTaxList { get; set; }
    }

    public class InvoiceTax
    {
        public InvoiceTax()
        {
            TaxList = new List<Tax>();
        }
        public DateTime InvoiceDate { get; set; }

        private double _TotalInvoiceAmount;
        public double TotalInvoiceAmount { get { return Math.Round(_TotalInvoiceAmount); } set { _TotalInvoiceAmount = value; } }

        public List<Tax> TaxList { get; set; }
    }
}
