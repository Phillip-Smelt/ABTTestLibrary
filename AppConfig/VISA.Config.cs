using System;
using System.Collections.Generic;
using System.Configuration;
using static TestLibrary.VISA.Instrument;

namespace TestLibrary.AppConfig {
    public class VISA_InstrumentElement : ConfigurationElement {
        [ConfigurationProperty("ID", IsKey = true, IsRequired = true)] public String ID { get { return ((String)base["ID"]).Trim(); } }
        [ConfigurationProperty("Address", IsKey = false, IsRequired = true)] public String Address { get { return ((String)base["Address"]).Trim(); } }
    }

    [ConfigurationCollection(typeof(VISA_InstrumentElement))]
    public class VISA_InstrumentElements : ConfigurationElementCollection {
        public const String PropertyName = "VISA_InstrumentElement";
        public VISA_InstrumentElement this[Int32 idx] { get { return (VISA_InstrumentElement)BaseGet(idx); } }
        public override ConfigurationElementCollectionType CollectionType { get { return ConfigurationElementCollectionType.BasicMapAlternate; } }
        protected override String ElementName { get { return PropertyName; } }
        protected override bool IsElementName(String elementName) { return elementName.Equals(PropertyName, StringComparison.InvariantCultureIgnoreCase); }
        public override bool IsReadOnly() { return false; }
        protected override ConfigurationElement CreateNewElement() { return new VISA_InstrumentElement(); }
        protected override object GetElementKey(ConfigurationElement element) { return ((VISA_InstrumentElement)(element)).ID; }
    }

    public class VISA_InstrumentsSection : ConfigurationSection {
        [ConfigurationProperty("VISA_InstrumentElements")] public VISA_InstrumentElements VISA_InstrumentElements { get { return ((VISA_InstrumentElements)(base["VISA_InstrumentElements"])); } }
    }

    public class ConfigVISA {
        public VISA_InstrumentsSection VISA_InstrumentsSection { get { return (VISA_InstrumentsSection)ConfigurationManager.GetSection("VISA_InstrumentsSection"); } }
        public VISA_InstrumentElements VISA_InstrumentElements { get { return this.VISA_InstrumentsSection.VISA_InstrumentElements; } }
        public IEnumerable<VISA_InstrumentElement> VISA_InstrumentElement { get { foreach (VISA_InstrumentElement vie in this.VISA_InstrumentElements) if (vie != null) yield return vie; } }
    }

    public class VISA_Instrument {
        public IDs ID { get; private set; }
        public String Address { get; private set; }

        private VISA_Instrument(IDs id, String address) {
            this.ID = id;
            this.Address = address;
        }

        public static Dictionary<IDs, String> Get() {
            VISA_InstrumentsSection viSection = (VISA_InstrumentsSection)ConfigurationManager.GetSection("VISA_InstrumentsSection");
            VISA_InstrumentElements viElements = viSection.VISA_InstrumentElements;
            Dictionary<IDs, String> instrumentsToAddresses = new Dictionary<IDs, String>();
            IDs id;
            foreach (VISA_InstrumentElement viElement in viElements) {
                id = (IDs)Enum.Parse(typeof(IDs), viElement.ID);
                if (!Enum.IsDefined(typeof(IDs), id)) throw new InvalidOperationException($"VISA_Config.xml's ID '{viElement.ID}' not present in VISA.IDs enum.");
                if (instrumentsToAddresses.ContainsKey(id)) throw new InvalidOperationException($"VISA_Config.xml's ID '{viElement.ID}' duplicated; must be unique.");
                if (instrumentsToAddresses.ContainsValue(viElement.Address)) throw new InvalidOperationException($"VISA_Config.xml's Address '{viElement.Address}' duplicated; must be unique.");
                instrumentsToAddresses.Add(id, viElement.Address);
            }
            return instrumentsToAddresses;
        }
    }
}
