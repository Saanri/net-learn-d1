using System.Configuration;

namespace FileSorter.Configuration
{
    public class ViewedDirElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ViewedDirElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ViewedDirElement)element).Dir;
        }
    }
}
