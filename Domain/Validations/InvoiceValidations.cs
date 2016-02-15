using Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TaxReportGenerator.DataObjects;
using TaxReportGenerator.Domain.Interfaces;

namespace TaxReportGenerator.Domain.Validations
{
    public class InvoiceValidations : IValidationStrategy<Invoice>
    {
        public void Validate(Invoice input)
        {
            var errorMessageBuilder = new StringBuilder();

            var errorMessage = ValidateInput(input);
            if (!(string.IsNullOrEmpty(errorMessage)))
            {
                errorMessage += "Line " + input.InvoiceNumber + " " + errorMessage;
                errorMessageBuilder.AppendLine(errorMessage);
            }

            if (errorMessageBuilder.ToString().Trim().Length > 0)
            {
                var exceptionMessage = errorMessageBuilder.ToString().Trim();
                throw new TaxGeneratorReportBusinessException(exceptionMessage);
            }
        }

        public void Validate(List<Invoice> input)
        {
            var errorMessageBuilder = new StringBuilder();
            input.ForEach(u =>
            {
                var errorMessage = ValidateInput(u);
                if (!(string.IsNullOrEmpty(errorMessage)))
                {
                    errorMessage = "Line " + u.InvoiceNumber + " " + errorMessage;
                    errorMessageBuilder.AppendLine(errorMessage);
                }
            });

            if (errorMessageBuilder.ToString().Trim().Length > 0)
            {
                var exceptionMessage = errorMessageBuilder.ToString().Trim();
                throw new TaxGeneratorReportBusinessException(exceptionMessage);
            }
        }

        private string ValidateInput(Invoice input)
        {
            var errorMessage = string.Empty;

            if (string.IsNullOrEmpty(input.ClientNumber) || !Regex.IsMatch(input.ClientNumber, "^[DI][0-9][0-9][0-9]$"))
            {
                errorMessage = "Client number is invalid";
            }

            DateTime invoiceDate;
            var formats = new[] { ConfigurationManager.AppSettings["DateFormat"] };
            if (!(DateTime.TryParseExact(input.InvoiceDate, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out invoiceDate)))
            {
                errorMessage += ", Invalid date";
            }

            double invoiceAmount;
            if ((!(double.TryParse(input.InvoiceAmount, out invoiceAmount)))
                || (!((invoiceAmount % 1) == 0)))
            {
                errorMessage += ", Invalid Amount";
            }

            return errorMessage;
        }
    }
}
