using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxReportGenerator.DataObjects;
using TaxReportGenerator.Domain.Interfaces;

namespace TaxReportGenerator.Domain
{
    public class InternationalTaxCalculation : IInternationalTaxCalculation<Invoice, Tax, InvoiceTax>
    {
        public List<InvoiceTax> CalculateTax(List<Invoice> input1, List<Tax> input2)
        {
            var returnValue = new List<InvoiceTax>();

            var FRT = input2.Where(u => u.Taxtype == "FRT");

            if (FRT.Any())
            {
                returnValue = CalculateTax(input1, FRT.FirstOrDefault());
            }

            return returnValue;
        }

        public InvoiceTax CalculateTax(Invoice input, List<Tax> input2)
        {
            var returnValue = new InvoiceTax();

            var FRT = input2.Where(u => u.Taxtype == "FRT");

            if (FRT.Any())
            {
                returnValue = GetInvoiceTaxDetails(input, FRT.FirstOrDefault());
            }

            return returnValue;
        }

        private List<InvoiceTax> CalculateTax(List<Invoice> input1, Tax frt)
        {
            var returnValue = new List<InvoiceTax>();

            input1.ForEach(u =>
            {
                var invoiceReport = GetInvoiceTaxDetails(u, frt);
                returnValue.Add(invoiceReport);
            });

            return returnValue;
        }

        private InvoiceTax GetInvoiceTaxDetails(Invoice u, Tax frt)
        {
            var invoiceReport = new InvoiceTax();

            var invoiceAmount = Convert.ToDouble(u.InvoiceAmount);
            frt.TaxAmount = invoiceAmount * Convert.ToDouble(frt.Rate);

            DateTime invoiceDate;
            var formats = new[] { ConfigurationManager.AppSettings["DateFormat"] };
            if ((DateTime.TryParseExact(u.InvoiceDate, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out invoiceDate)))
            {
                invoiceReport.InvoiceDate = invoiceDate;
            }
            invoiceReport.TotalInvoiceAmount = invoiceAmount;
            invoiceReport.TaxList.Add(frt);
            return invoiceReport;
        }
    }
}
