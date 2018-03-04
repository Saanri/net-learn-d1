using System.Configuration;

namespace FileSorter.Configuration
{
    public class ViewedDirElement : ConfigurationElement
    {
        [ConfigurationProperty("dir", IsKey = true, IsRequired = true)]
        public string Dir
        {
            get { return (string)base["dir"]; }
        }
    }
}
