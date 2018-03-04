using System.Configuration;

namespace FileSorter.Configuration
{
    public class RuleElement : ConfigurationElement
    {
        [ConfigurationProperty("rule", IsKey = true, IsRequired = true)]
        public string Rule
        {
            get { return (string)base["rule"]; }
        }

        [ConfigurationProperty("targetFolder", IsRequired = false)]
        public string TargetFolder
        {
            get { return (string)base["targetFolder"]; }
        }

        [ConfigurationProperty("isAddNumber", IsRequired = false, DefaultValue = true)]
        public bool IsAddNumber
        {
            get { return (bool)base["isAddNumber"]; }
        }

        [ConfigurationProperty("isAddMoveDate", IsRequired = false, DefaultValue = false)]
        public bool IsAddMoveDate
        {
            get { return (bool)base["isAddMoveDate"]; }
        }

    }
}
