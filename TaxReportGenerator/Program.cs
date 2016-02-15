using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Exceptions;
using TaxReportGenerator.Factory;
using TaxReportGenerator.Domain.Interfaces;

namespace TaxReportGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var TaxReportGeneratorIns = Factory<ITaxReportGenerator>.Create("TaxReportGenerator");
                var TaxReport = TaxReportGeneratorIns.GenerateTaxReport();
                DisplayResults(TaxReport);
            }
            catch (TaxGeneratorReportBusinessException ex)
            {
                var ErrorMessageBuilder = new StringBuilder();
                ErrorMessageBuilder.AppendLine("The input file has invalid data please fix following errors and try again:");
                ErrorMessageBuilder.AppendLine(ex.Message);
                DisplayResults(ErrorMessageBuilder.ToString());
          
            }
            catch (Exception ex)
            {
                DisplayResults("Application Failure:" + ex.Message);
            }
        }
        private static void DisplayResults(string errorMessage)
        {
            Console.WriteLine(errorMessage);
            Console.WriteLine("Press enter to exit.");
            Console.Read();
        }
    }
}
