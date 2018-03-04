using System.Configuration;

namespace FileSorter.Configuration
{
    public class SimpleConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("culture")]
        public CultureElement Culture
        {
            get { return (CultureElement)this["culture"]; }
        }

        [ConfigurationCollection(typeof(RuleElement), AddItemName = "rule")]
        [ConfigurationProperty("rules")]
        public RuleElementCollection Rules
        {
            get { return (RuleElementCollection)this["rules"]; }
        }

        [ConfigurationCollection(typeof(ViewedDirElement), AddItemName = "viewedDir")]
        [ConfigurationProperty("viewedDirs")]
        public ViewedDirElementCollection ViewedDirs
        {
            get { return (ViewedDirElementCollection)this["viewedDirs"]; }
        }

        [ConfigurationProperty("defaultDestinationDir")]
        public DefaultDestinationDirElement DefaultDestinationDir
        {
            get { return (DefaultDestinationDirElement)this["defaultDestinationDir"]; }
        }
    }
}
