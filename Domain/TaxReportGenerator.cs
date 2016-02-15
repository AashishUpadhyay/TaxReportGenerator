using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxReportGenerator.DataObjects;
using TaxReportGenerator.Domain.Interfaces;

namespace TaxReportGenerator.Domain
{
    public class TaxReportGenerator : ITaxReportGenerator
    {
        private List<string> _columnNames = new List<string>();
        private IInvoiceTaxDetail<InvoiceTaxDetails> _InvoiceTaxDetailsIns = null;

        public TaxReportGenerator(IInvoiceTaxDetail<InvoiceTaxDetails> invoiceTaxDetails)
        {
            _InvoiceTaxDetailsIns = invoiceTaxDetails;
        }

        public string GenerateTaxReport()
        {
            var reportBuilder = new StringBuilder();
            var invoiceTaxDetails = _InvoiceTaxDetailsIns.GetInvoiceTaxDetails();

            _columnNames.Add("Month");
            _columnNames.Add("InvoiceAmount");

            invoiceTaxDetails.AllTaxTypes.ForEach(u =>
            {
                _columnNames.Add(u.Taxtype);
            });

            var groupedInvoices = (from x in invoiceTaxDetails.InvoiceTaxList
                                   group x by new { x.InvoiceDate.Month, x.InvoiceDate.Year } into x_grouped
                                   select new { Month = x_grouped.Key.Month + "-" + x_grouped.Key.Year, InvoiceAmount = x_grouped.Sum(y => y.TotalInvoiceAmount), TaxList = x_grouped.Select(z => z.TaxList) }).ToList();

            var firstReportLine = "Month";
            _columnNames.Where(t => t != "Month").ToList().ForEach(u =>
                {
                    firstReportLine += " | " + u;
                });
            reportBuilder.AppendLine(firstReportLine);
            groupedInvoices.ForEach(u =>
            {
                var reportLine = u.Month + " | " + u.InvoiceAmount;
                invoiceTaxDetails.AllTaxTypes.ForEach(v =>
                {
                    var amountForTaxType = 0.0;
                    u.TaxList.ToList().ForEach(w =>
                    {
                        w.ForEach(x =>
                        {
                            if (x.Taxtype == v.Taxtype)
                            {
                                amountForTaxType += x.TaxAmount;
                            }
                        });
                    });

                    reportLine += " | " + amountForTaxType;
                });

                reportBuilder.AppendLine(reportLine);
            });

            return reportBuilder.ToString();
        }
    }
}
