using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace LibLibrary
{
    class XMLValidator
    {
        private XmlSchema _schema;
        private XmlReaderSettings _settings;

        public XmlReaderSettings SettingsVal
        {
            get { return _settings; }
        }

        public XMLValidator()
        {
            ValidationEventHandler validationEventHandler = new ValidationEventHandler(ValidationCallBack);
            _schema = XmlSchema.Read(new StringReader(XElement.Load(@"XMLSchemaLU.xsd").ToString()), validationEventHandler);

            _settings = new XmlReaderSettings();
            _settings.Schemas.Add(_schema);
            _settings.ValidationType = ValidationType.Schema;
            _settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
            _settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            _settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            _settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
        }

        private void ValidationCallBack(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning)
            {
                string warning = String.Format("Validation xml warning: {0}", args.Message);
                throw new Exception(warning);
            }
            else
            {
                string error = String.Format("Validation xml error: {0}", args.Message);
                throw new Exception(error);
            }

        }
    }
}
