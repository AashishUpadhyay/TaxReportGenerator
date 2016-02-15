using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxReportGenerator.DataObjects;
using System.IO;
using Core.Exceptions;
using System.Text;
using System.Collections;

namespace TaxReportGenerator.Services
{
    public class CSVInvoicesRetriever : IInvoiceRetriever<Invoice>
    {
        public IEnumerable<Invoice> RetrieveInvoices(string source)
        {
            CheckFileExists(source);

            var InvoiceList = ReadFileContents(source);

            return InvoiceList;
        }

        private IEnumerable<Invoice> ReadFileContents(string source)
        {
            var returnValue = (from l in File.ReadLines(source)
                               let x = l.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                               select new Invoice
                               {
                                   InvoiceNumber = ValidateArrayLength(x, 0),
                                   ClientNumber = ValidateArrayLength(x, 1),
                                   InvoiceDate = ValidateArrayLength(x, 2),
                                   InvoiceAmount = ValidateArrayLength(x, 3)
                               });

            return returnValue;
        }

        private string ValidateArrayLength(string[] x, int v)
        {
            if (x.Length > v)
            {
                return x[v];
            }
            return string.Empty;
        }

        private void CheckFileExists(string source)
        {
            if (!File.Exists(source))
            {
                var errorMessage = "No file found at this location: " + source;
                throw new TaxGeneratorReportBusinessException(errorMessage);
            }
        }
    }
}
