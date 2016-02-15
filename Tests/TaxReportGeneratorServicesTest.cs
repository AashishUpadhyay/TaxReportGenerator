using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaxReportGenerator.Services;
using Services.Interfaces;
using System.Configuration;
using TaxReportGenerator.Factory;
using TaxReportGenerator.Domain.Interfaces;
using System.Linq;
using TaxReportGenerator.Domain;
using TaxReportGenerator.DataObjects;

namespace Tests
{
    [TestClass]
    public class TaxReportGeneratorServicesTest
    {
        [TestMethod]
        public void can_read_invoices_file_from_file_path()
        {
            var filePath = ConfigurationManager.AppSettings["InvoiceFilePath"];
            var InvoiceRetriever = Factory<IInvoiceRetriever<Invoice>>.Create("CSVInvoicesRetriever");
            var Invoices = InvoiceRetriever.RetrieveInvoices(filePath).ToList();
        }

        [TestMethod]
        public void throw_error_invoices_when_file_not_present()
        {
            var errorOccured = false;
            try
            {
                var filePath = "ASDF";
                var InvoiceRetriever = Factory<IInvoiceRetriever<Invoice>>.Create("CSVInvoicesRetriever");
                var Invoices = InvoiceRetriever.RetrieveInvoices(filePath);
            }
            catch
            {
                errorOccured = true;
            }

            if (!errorOccured)
            {
                throw new Exception();
            }
        }

        [TestMethod]
        public void can_read_tax_rate_file_from_file_path()
        {
            var filePath = ConfigurationManager.AppSettings["TaxRatesFilePath"];
            var TaxRateRetriever = Factory<ITaxService<Tax>>.Create("ConfiguredTaxRateService");
            var TaxRates = TaxRateRetriever.RetrieveTaxes(filePath);
        }

        [TestMethod]
        public void throw_error_when_taxrate_file_not_present()
        {
            var errorOccured = false;
            try
            {
                var filePath = "ASDF";
            var TaxRateRetriever = Factory<ITaxService<Tax>>.Create("ConfiguredTaxRateService");
            var TaxRates = TaxRateRetriever.RetrieveTaxes(filePath);
            }
            catch
            {
                errorOccured = true;
            }

            if (!errorOccured)
            {
                throw new Exception();
            }
        }
    }
}
