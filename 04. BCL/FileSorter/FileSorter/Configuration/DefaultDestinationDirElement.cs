using System.Configuration;

namespace FileSorter.Configuration
{
    public class DefaultDestinationDirElement : ConfigurationElement
    {
        [ConfigurationProperty("dir")]
        public string Dir
        {
            get { return (string)this["dir"]; }
        }
    }
}
