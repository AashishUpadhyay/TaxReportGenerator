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
    public class DomesticTaxCalculation : IDomesticTaxCalculation<Invoice, Tax, InvoiceTax>
    {
        public List<InvoiceTax> CalculateTax(List<Invoice> input1, List<Tax> input2)
        {
            var returnValue = new List<InvoiceTax>();

            var ServiceTax = input2.Where(u => u.Taxtype == "ST");
            var EducationCess = input2.Where(u => u.Taxtype == "EC");

            if (ServiceTax.Any())
            {
                returnValue = CalculateServiceTaxOnInvoices(input1, ServiceTax.FirstOrDefault(), EducationCess);
            }

            return returnValue;
        }

        public InvoiceTax CalculateTax(Invoice input, List<Tax> input2)
        {
            var returnValue = new InvoiceTax();

            var ServiceTaxEnumerable = input2.Where(u => u.Taxtype == "ST");
            var EducationCessEnumerable = input2.Where(u => u.Taxtype == "EC");

            if (ServiceTaxEnumerable.Any())
            {
                var applyEducationCess = false;
                Tax educationCess = null;
                if (EducationCessEnumerable.Any())
                {
                    applyEducationCess = true;
                    educationCess = EducationCessEnumerable.FirstOrDefault();
                }

                returnValue = GetInvoiceTaxDetails(input, ServiceTaxEnumerable.FirstOrDefault(), educationCess, applyEducationCess);
            }

            return returnValue;
        }

        private List<InvoiceTax> CalculateServiceTaxOnInvoices(List<Invoice> input1, Tax serviceTax, IEnumerable<Tax> educationCessEnumerable)
        {
            var returnValue = new List<InvoiceTax>();
            var applyEducationCess = false;
            Tax educationCess = null;
            if (educationCessEnumerable.Any())
            {
                applyEducationCess = true;
                educationCess = educationCessEnumerable.FirstOrDefault();
            }

            input1.ForEach(u =>
            {
                var invoiceReport = GetInvoiceTaxDetails(u, serviceTax, educationCess, applyEducationCess);
                returnValue.Add(invoiceReport);
            });

            return returnValue;
        }

        private InvoiceTax GetInvoiceTaxDetails(Invoice u, Tax serviceTax, Tax educationCess, bool applyEducationCess)
        {
            var invoiceTaxDetails = new InvoiceTax();

            var invoiceAmount = Convert.ToDouble(u.InvoiceAmount);
            serviceTax.TaxAmount = invoiceAmount * Convert.ToDouble(serviceTax.Rate);
            if (applyEducationCess)
            {
                educationCess.TaxAmount= serviceTax.TaxAmount * Convert.ToDouble(educationCess.Rate);
            }

            DateTime invoiceDate;
            var formats = new[] { ConfigurationManager.AppSettings["DateFormat"] };
            if ((DateTime.TryParseExact(u.InvoiceDate, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out invoiceDate)))
            {
                invoiceTaxDetails.InvoiceDate = invoiceDate;
            }
            invoiceTaxDetails.TotalInvoiceAmount = invoiceAmount;

            invoiceTaxDetails.TaxList.Add(educationCess);
            invoiceTaxDetails.TaxList.Add(serviceTax);

            return invoiceTaxDetails;
        }
    }
}
