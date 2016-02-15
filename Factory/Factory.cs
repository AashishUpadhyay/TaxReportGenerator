using Services.Interfaces;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxReportGenerator.DataObjects;
using TaxReportGenerator.Domain;
using TaxReportGenerator.Domain.Interfaces;
using TaxReportGenerator.Domain.Validations;
using TaxReportGenerator.Services;
using System.Linq.Expressions;

namespace TaxReportGenerator.Factory
{
    public static class Factory<T>
    {
        public static Container container;

        public static T Create(string key)
        {
            if (container == null)
            {
                container = new Container(x =>
                {
                    x.For<IInvoiceRetriever<Invoice>>().Add<CSVInvoicesRetriever>().Named("CSVInvoicesRetriever");
                    x.For<ITaxService<Tax>>().Add<ConfiguredTaxRateService>().Named("ConfiguredTaxRateService");
                    x.For<IDomesticTaxCalculation<Invoice, Tax, InvoiceTax>>().Add<DomesticTaxCalculation>().Named("DomesticTaxCalculation");
                    x.For<IInternationalTaxCalculation<Invoice, Tax, InvoiceTax>>().Add<InternationalTaxCalculation>().Named("InternationalTaxCalculation");
                    x.For<IValidationStrategy<Invoice>>().Add<InvoiceValidations>().Named("InvoiceValidations");

                    x.For<IInvoiceTaxDetail<InvoiceTaxDetails>>().Use<InvoiceBO>().Ctor<IValidationStrategy<Invoice>>().Is(
    c => c.GetInstance<IValidationStrategy<Invoice>>("InvoiceValidations")).Ctor<IDomesticTaxCalculation<Invoice, Tax, InvoiceTax>>().Is(c => c.GetInstance<IDomesticTaxCalculation<Invoice, Tax, InvoiceTax>>("DomesticTaxCalculation")).Ctor<IInternationalTaxCalculation<Invoice, Tax, InvoiceTax>>().Is(c => c.GetInstance<IInternationalTaxCalculation<Invoice, Tax, InvoiceTax>>("InternationalTaxCalculation")).Ctor<IInvoiceRetriever<Invoice>>().Is(c => c.GetInstance<IInvoiceRetriever<Invoice>>("CSVInvoicesRetriever")).Ctor<ITaxService<Tax>>().Is(c => c.GetInstance<ITaxService<Tax>>("ConfiguredTaxRateService")).Ctor<Dictionary<string, string>>().Is(GetServiceSources()).Named("InvoiceBOReport");

                    x.For<ITaxReportGenerator>().Add<Domain.TaxReportGenerator>().Ctor<IInvoiceTaxDetail<InvoiceTaxDetails>>().Is(c => c.GetInstance<IInvoiceTaxDetail<InvoiceTaxDetails>>("InvoiceBOReport")).Named("TaxReportGenerator");
                });
            }
            return container.GetInstance<T>(key);
        }

        private static Dictionary<string, string> GetServiceSources()
        {
            var ReturnValue = new Dictionary<string, string>();
            ReturnValue.Add("InvoiceFilePath", ConfigurationManager.AppSettings["InvoiceFilePath"]);
            ReturnValue.Add("TaxRatesFilePath", ConfigurationManager.AppSettings["TaxRatesFilePath"]);
            return ReturnValue;
        }
    }
}
