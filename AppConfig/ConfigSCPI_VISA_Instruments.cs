using System;
using System.Collections.Generic;
using System.Configuration;
using Agilent.CommandExpert.ScpiNet.Ag33500B_33600A_2_09;
using Agilent.CommandExpert.ScpiNet.Ag3466x_2_08;
using Agilent.CommandExpert.ScpiNet.AgE3610XB_1_0_0_1_00;
using Agilent.CommandExpert.ScpiNet.AgE36200_1_0_0_1_0_2_1_00;
using Agilent.CommandExpert.ScpiNet.AgEL30000_1_2_5_1_0_6_17_114;
using Agilent.CommandExpert.ScpiNet.AgSCPI99_1_0;
using TestLibrary.SCPI_VISA;
using TestLibrary.Logging;

namespace TestLibrary.AppConfig {
    public class SCPI_VISA_InstrumentElement : ConfigurationElement {
        [ConfigurationProperty("ID", IsKey = true, IsRequired = true)] public String ID { get { return ((String)base["ID"]).Trim(); } }
        [ConfigurationProperty("Description", IsKey = false, IsRequired = true)] public String Description { get { return ((String)base["Description"]).Trim(); } }
        [ConfigurationProperty("Address", IsKey = false, IsRequired = true)] public String Address { get { return ((String)base["Address"]).Trim(); } }
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

    public class SCPI_VISA_Instrument {
        public String ID { get; private set; }
        public String Description { get; private set; }
        public String Address { get; private set; }
        public SCPI_VISA_CATEGORIES Category { get; private set; }
        public object Instance { get; private set; }

        private SCPI_VISA_Instrument(String id, String description, String address) {
            this.ID = id;
            this.Description = description;
            this.Address = address;

            try {
                String instrumentModel = SCPI99.GetModel(this.Address);
                switch (instrumentModel) {
                    case "EL34143A":
                        this.Category = SCPI_VISA_CATEGORIES.ElectronicLoad;
                        this.Instance = new AgEL30000(this.Address);
                        EL_34143A.Initialize(this);
                        break;
                    case "34461A":
                        this.Category = SCPI_VISA_CATEGORIES.MultiMeter;
                        this.Instance = new Ag3466x(this.Address);
                        MM_34661A.Initialize(this);
                        break;
                    case "E36103B":
                    case "E36105B":
                        this.Category = SCPI_VISA_CATEGORIES.PowerSupply;
                        this.Instance = new AgE3610XB(this.Address);
                        PS_E3610xB.Initialize(this);
                        break;
                    case "E36234A":
                        this.Category = SCPI_VISA_CATEGORIES.PowerSupply;
                        this.Instance = new AgE36200(this.Address);
                        PS_E36234A.Initialize(this);
                        break;
                    case "33509B":
                        this.Category = SCPI_VISA_CATEGORIES.WaveformGenerator;
                        this.Instance = new Ag33500B_33600A(this.Address);
                        WG_33509B.Initialize(this);
                        break;
                    default:
                        this.Category = SCPI_VISA_CATEGORIES.SCPI;
                        this.Instance = new AgSCPI99(this.Address);
                        SCPI99.Initialize(this);
                        Logger.UnexpectedErrorHandler(SCPI99.GetMessage(this, $"Unrecognized SCPI VISA Instrument!  Update Class SCPI_VISA_Instrument, adding '{instrumentModel}'"));
                        break;
                }
            } catch (Exception e) {
                String[] a = address.Split(':');
                throw new InvalidOperationException(SCPI99.GetMessage(this, $"Check to see if SCPI VISA Instrument is powered and it's {a[0]} bus is communicating."), e);
            }
        }

        public static Dictionary<SCPI_VISA_IDs, SCPI_VISA_Instrument> Get() {
            Dictionary<SCPI_VISA_IDs, (String id, String description, String address)> visaInstrumentElements = GetVISA_InstrumentElements();
            Dictionary<SCPI_VISA_IDs, SCPI_VISA_Instrument> instruments = new Dictionary<SCPI_VISA_IDs, SCPI_VISA_Instrument>();
            foreach (KeyValuePair<SCPI_VISA_IDs, (String id, String description, String address)> kvp in visaInstrumentElements) instruments.Add(kvp.Key, new SCPI_VISA_Instrument(kvp.Value.id, kvp.Value.description, kvp.Value.address));
            return instruments;
        }

        private static Dictionary<SCPI_VISA_IDs, (String id, String description, String address)> GetVISA_InstrumentElements() {
            SCPI_VISA_InstrumentsSection viSection = (SCPI_VISA_InstrumentsSection)ConfigurationManager.GetSection("SCPI_VISA_InstrumentsSection");
            SCPI_VISA_InstrumentElements viElements = viSection.SCPI_VISA_InstrumentElements;
            Dictionary<SCPI_VISA_IDs, (String id, String description, String address)> visaInstrumentElements = new Dictionary<SCPI_VISA_IDs, (String id, String description, String address)>();
            SCPI_VISA_IDs ids;
            String addresses = String.Empty;
            foreach (SCPI_VISA_InstrumentElement viElement in viElements) {
                ids = (SCPI_VISA_IDs)Enum.Parse(typeof(SCPI_VISA_IDs), viElement.ID);
                if (!Enum.IsDefined(typeof(SCPI_VISA_IDs), ids)) throw new ArgumentException($"App.config's ID '{viElement.ID}' not present in SCPI_VISA_IDs enum.  ID's Description is '{viElement.Description}.'");
                if (visaInstrumentElements.ContainsKey(ids)) throw new ArgumentException($"App.config's ID '{viElement.ID}' duplicated; must be unique.  ID's Description is '{viElement.Description}.'");
                if (addresses.Contains(viElement.Address)) throw new ArgumentException($"App.config's Address '{viElement.Address}' duplicated; must be unique.  Address' ID is '{viElement.ID}'.");
                addresses += viElement.Address;
                visaInstrumentElements.Add(ids, (viElement.ID, viElement.Description, viElement.Address));
            }
            return visaInstrumentElements;
        }
    }
}
