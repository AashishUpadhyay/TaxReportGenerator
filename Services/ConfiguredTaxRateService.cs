using Core.Exceptions;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxReportGenerator.DataObjects;
using TaxReportGenerator.Core;

namespace TaxReportGenerator.Services
{
    public class ConfiguredTaxRateService : ITaxService<Tax>
    {
        public IEnumerable<Tax> RetrieveTaxes(string source)
        {
            CheckFileExists(source);

            var fileContents = File.ReadAllText(source);

            var returnValue = fileContents.ToObject<IEnumerable<Tax>>();

            return returnValue;
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
