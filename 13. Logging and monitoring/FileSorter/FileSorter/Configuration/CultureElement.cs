using System.Configuration;
using System.Globalization;

namespace FileSorter.Configuration
{
    public class CultureElement : ConfigurationElement
    {
        [ConfigurationProperty("culture")]
        public CultureInfo cultureInfo
        {
            get { return (CultureInfo)this["culture"]; }
        }
    }
}
