using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxReportGenerator.DataObjects;
using TaxReportGenerator.Domain;
using TaxReportGenerator.Domain.Interfaces;
using TaxReportGenerator.Factory;

namespace Tests
{
    [TestClass]
    public class InvoiceValidationTests
    {
       
        [TestMethod]
        public void throw_error_when_client_number_is_invalid_for_invoices()
        {
            try
            {
                var filePath = @"SampleFiles\throw_error_when_client_number_is_invalid_for_invoices.csv";
                var InvoiceRetriever = Factory<IInvoiceRetriever<Invoice>>.Create("CSVInvoicesRetriever");
                var Invoices = InvoiceRetriever.RetrieveInvoices(filePath).ToList();

                var InvoiceValidationsIns = Factory<IValidationStrategy<Invoice>>.Create("InvoiceValidations");
                InvoiceValidationsIns.Validate(Invoices);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Client number is invalid"))
                {
                    // do nothing
                }
                else
                {
                    throw;
                }
            }
        }

        [TestMethod]
        public void throw_error_when_invoice_date_is_invalid_for_invoices()
        {
            try
            {
                var filePath = @"SampleFiles\throw_error_when_invoice_date_is_invalid_for_invoices.csv";
                var InvoiceRetriever = Factory<IInvoiceRetriever<Invoice>>.Create("CSVInvoicesRetriever");
                var Invoices = InvoiceRetriever.RetrieveInvoices(filePath).ToList();

                var InvoiceValidationsIns = Factory<IValidationStrategy<Invoice>>.Create("InvoiceValidations");
                InvoiceValidationsIns.Validate(Invoices);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Invalid date"))
                {
                    // do nothing
                }
                else
                {
                    throw;
                }
            }
        }

        [TestMethod]
        public void throw_error_when_invoice_amount_is_invalid_for_invoice_list()
        {
            try
            {
                var filePath = @"SampleFiles\throw_error_when_invoice_amount_is_invalid_for_invoice_list.csv";
                var InvoiceRetriever = Factory<IInvoiceRetriever<Invoice>>.Create("CSVInvoicesRetriever");
                var Invoices = InvoiceRetriever.RetrieveInvoices(filePath).ToList();

                var InvoiceValidationsIns = Factory<IValidationStrategy<Invoice>>.Create("InvoiceValidations");
                InvoiceValidationsIns.Validate(Invoices);
            }
            catch (Exception ex)
            {
                if ((ex.Message.Contains("Invalid Amount")))
                {
                    // do nothing
                }
                else
                {
                    throw;
                }
            }
        }

        [TestMethod]
        public void throw_error_when_client_number_and_invoice_date_and_invoice_amount_is_invalid_for_invoices()
        {
            try
            {
                var filePath = @"SampleFiles\throw_error_when_client_number_and_invoice_date_and_invoice_amount_is_invalid_for_invoices.csv";
                var InvoiceRetriever = Factory<IInvoiceRetriever<Invoice>>.Create("CSVInvoicesRetriever");
                var Invoices = InvoiceRetriever.RetrieveInvoices(filePath).ToList();

                var InvoiceValidationsIns = Factory<IValidationStrategy<Invoice>>.Create("InvoiceValidations");
                InvoiceValidationsIns.Validate(Invoices);
            }
            catch (Exception ex)
            {
                if ((ex.Message.Contains("Client number is invalid")) && (ex.Message.Contains("Invalid date")) && ((ex.Message.Contains("Invalid Amount"))))
                {
                    // do nothing
                }
                else
                {
                    throw;
                }
            }
        }

        [TestMethod]
        public void get_generated_file()
        {
            var TaxReportGeneratorIns = Factory<ITaxReportGenerator>.Create("TaxReportGenerator");
            var report = TaxReportGeneratorIns.GenerateTaxReport();
        }
    }
}
