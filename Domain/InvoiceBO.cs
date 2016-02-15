using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxReportGenerator.DataObjects;
using TaxReportGenerator.Domain.Interfaces;
using TaxReportGenerator.Core;
using Services.Interfaces;

namespace TaxReportGenerator.Domain
{
    public class InvoiceBO : IInvoiceTaxDetail<InvoiceTaxDetails>
    {
        private IValidationStrategy<Invoice> _ValidationType = null;
        private IDomesticTaxCalculation<Invoice, Tax, InvoiceTax> _CalculateDomesticTax = null;
        private IInternationalTaxCalculation<Invoice, Tax, InvoiceTax> _CalculateInternationalTax = null;
        private IInvoiceRetriever<Invoice> _InvoiceRetriever = null;
        private ITaxService<Tax> _TaxService = null;
        private string _InvoiceSource = string.Empty;
        private string _TaxSource = string.Empty;

        public InvoiceBO(IValidationStrategy<Invoice> validate, IDomesticTaxCalculation<Invoice, Tax, InvoiceTax> calculateDomesticTax, IInternationalTaxCalculation<Invoice, Tax, InvoiceTax> calculateInternationalTax, IInvoiceRetriever<Invoice> invoiceRetriever, ITaxService<Tax> taxService, Dictionary<string, string> serviceSources)
        {
            _ValidationType = validate;
            _CalculateDomesticTax = calculateDomesticTax;
            _CalculateInternationalTax = calculateInternationalTax;
            _InvoiceRetriever = invoiceRetriever;
            _TaxService = taxService;
            _InvoiceSource = serviceSources["InvoiceFilePath"];
            _TaxSource = serviceSources["TaxRatesFilePath"];
        }

        public InvoiceTaxDetails GetInvoiceTaxDetails()
        {
            var invoices = _InvoiceRetriever.RetrieveInvoices(_InvoiceSource).ToList();
            var taxes = _TaxService.RetrieveTaxes(_TaxSource).ToList();
            _ValidationType.Validate(invoices);

            var domesticlInvoices = from x in invoices
                                    where x.ClientNumber.MatchesRegex("^[D][0-9][0-9][0-9]$")
                                    select x;

            var internationalInvoices = from x in invoices
                                        where x.ClientNumber.MatchesRegex("^[I][0-9][0-9][0-9]$")
                                        select x;

            var domesticTaxDetails = _CalculateDomesticTax.CalculateTax(domesticlInvoices.ToList(), taxes);
            var internationalTaxDetails = _CalculateInternationalTax.CalculateTax(internationalInvoices.ToList(), taxes);

            internationalTaxDetails.AddRange(domesticTaxDetails);

            var returnValue = new InvoiceTaxDetails();
            returnValue.InvoiceTaxList = internationalTaxDetails;
            returnValue.AllTaxTypes = taxes;
            return returnValue;
        }
    }
}
