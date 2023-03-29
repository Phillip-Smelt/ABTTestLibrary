using System;
using System.Collections.Generic;
using System.Configuration;

namespace TestLibrary.AppConfig {
    public class SCPI_VISA_InstrumentElement : ConfigurationElement {
        [ConfigurationProperty("ID", IsKey = true, IsRequired = true)] public String ID { get { return ((String)base["ID"]).Trim(); } }
        [ConfigurationProperty("Address", IsKey = false, IsRequired = true)] public String Address { get { return ((String)base["Address"]).Trim(); } }
        [ConfigurationProperty("Description", IsKey = false, IsRequired = true)] public String Description { get { return ((String)base["Description"]).Trim(); } }
    }

    [ConfigurationCollection(typeof(SCPI_VISA_InstrumentElement))]
    public class SCPI_VISA_InstrumentElements : ConfigurationElementCollection {
        public const String PropertyName = "SCPI_VISA_InstrumentElement";
        public SCPI_VISA_InstrumentElement this[Int32 idx] { get { return (SCPI_VISA_InstrumentElement)BaseGet(idx); } }
        public override ConfigurationElementCollectionType CollectionType { get { return ConfigurationElementCollectionType.BasicMapAlternate; } }
        protected override String ElementName { get { return PropertyName; } }
        protected override bool IsElementName(String elementName) { return elementName.Equals(PropertyName, StringComparison.InvariantCultureIgnoreCase); }
        public override bool IsReadOnly() { return false; }
        protected override ConfigurationElement CreateNewElement() { return new SCPI_VISA_InstrumentElement(); }
        protected override object GetElementKey(ConfigurationElement element) { return ((SCPI_VISA_InstrumentElement)(element)).ID; }
    }

    public class SCPI_VISA_InstrumentsSection : ConfigurationSection {
        [ConfigurationProperty("SCPI_VISA_InstrumentElements")] public SCPI_VISA_InstrumentElements SCPI_VISA_InstrumentElements { get { return ((SCPI_VISA_InstrumentElements)(base["SCPI_VISA_InstrumentElements"])); } }
    }

    public class ConfigSCPI_VISA {
        public SCPI_VISA_InstrumentsSection SCPI_VISA_InstrumentsSection { get { return (SCPI_VISA_InstrumentsSection)ConfigurationManager.GetSection("SCPI_VISA_InstrumentsSection"); } }
        public SCPI_VISA_InstrumentElements SCPI_VISA_InstrumentElements { get { return this.SCPI_VISA_InstrumentsSection.SCPI_VISA_InstrumentElements; } }
        public IEnumerable<SCPI_VISA_InstrumentElement> SCPI_VISA_InstrumentElement { get { foreach (SCPI_VISA_InstrumentElement svie in this.SCPI_VISA_InstrumentElements) if (svie != null) yield return svie; } }
    }
}
