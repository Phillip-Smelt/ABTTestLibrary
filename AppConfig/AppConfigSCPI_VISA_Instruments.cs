using System;
using System.Collections.Generic;
using System.Configuration;
using Agilent.CommandExpert.ScpiNet.Ag33500B_33600A_2_09;
using Agilent.CommandExpert.ScpiNet.Ag3466x_2_08;
using Agilent.CommandExpert.ScpiNet.AgE3610XB_1_0_0_1_00;
using Agilent.CommandExpert.ScpiNet.AgE36200_1_0_0_1_0_2_1_00;
using Agilent.CommandExpert.ScpiNet.AgEL30000_1_2_5_1_0_6_17_114;
using Agilent.CommandExpert.ScpiNet.AgSCPI99_1_0;
using ABT.TestSpace.SCPI_VISA_Instruments;
using ABT.TestSpace.Logging;
using System.Reflection;

namespace ABT.TestSpace.AppConfig {
    // NOTE: https://forums.ni.com/t5/Instrument-Control-GPIB-Serial/IVI-Drivers-Pros-and-Cons/td-p/4165671.

    public enum SCPI_IDENTITY { Manufacturer, Model, SerialNumber, FirmwareRevision }
    // Example: "Keysight Technologies,E36103B,MY61001983,1.0.2-1.02".

    public class SCPI_VISA_InstrumentElement : ConfigurationElement {
        [ConfigurationProperty("ID", IsKey = true, IsRequired = true)] public String ID { get { return ((String)base["ID"]).Trim(); } }
        [ConfigurationProperty("Description", IsKey = false, IsRequired = true)] public String Description { get { return ((String)base["Description"]).Trim(); } }
        [ConfigurationProperty("Address", IsKey = false, IsRequired = true)] public String Address { get { return ((String)base["Address"]).Trim(); } }
    }

    [ConfigurationCollection(typeof(SCPI_VISA_InstrumentElement))]
    public class SCPI_VISA_InstrumentElements : ConfigurationElementCollection {
        public const String PropertyName = "SCPI_VISA_InstrumentElement";
        public SCPI_VISA_InstrumentElement this[Int32 idx] { get { return (SCPI_VISA_InstrumentElement)this.BaseGet(idx); } }
        public override ConfigurationElementCollectionType CollectionType { get { return ConfigurationElementCollectionType.BasicMapAlternate; } }
        protected override String ElementName { get { return PropertyName; } }
        protected override Boolean IsElementName(String elementName) { return elementName.Equals(PropertyName, StringComparison.InvariantCultureIgnoreCase); }
        public override Boolean IsReadOnly() { return false; }
        protected override ConfigurationElement CreateNewElement() { return new SCPI_VISA_InstrumentElement(); }
        protected override Object GetElementKey(ConfigurationElement element) { return ((SCPI_VISA_InstrumentElement)(element)).ID; }
    }

    public class SCPI_VISA_InstrumentsSection : ConfigurationSection {
        [ConfigurationProperty("SCPI_VISA_InstrumentElements")] public SCPI_VISA_InstrumentElements SCPI_VISA_InstrumentElements { get { return ((SCPI_VISA_InstrumentElements)(base["SCPI_VISA_InstrumentElements"])); } }
    }

    //public class AppConfigSCPI_VISA {
    //    public SCPI_VISA_InstrumentsSection SCPI_VISA_InstrumentsSection { get { return (SCPI_VISA_InstrumentsSection)ConfigurationManager.GetSection("SCPI_VISA_InstrumentsSection"); } }
    //    public SCPI_VISA_InstrumentElements SCPI_VISA_InstrumentElements { get { return this.SCPI_VISA_InstrumentsSection.SCPI_VISA_InstrumentElements; } }
    //    public IEnumerable<SCPI_VISA_InstrumentElement> SCPI_VISA_InstrumentElement { get { foreach (SCPI_VISA_InstrumentElement svie in this.SCPI_VISA_InstrumentElements) if (svie != null) yield return svie; } }
    //}

    public class SCPI_VISA_Instrument {
        public readonly String ID;
        public readonly String Description;
        public readonly String Address;
        public readonly String Identity;
        public readonly Object Instrument; // NOTE: The assumption, thus far proven correct, is that Keysight's SCPI drivers don't contain state, thus can be readonly.
        // This assumption may fail with other Keysight drivers, or other manufacturers SCPI drivers, so may need re-visiting.
        public const Int32 FORMAT_WIDTH = -16;

        private SCPI_VISA_Instrument(String id, String description, String address) {
            this.ID = id;
            this.Description = description;
            this.Address = address;

            try {
                this.Identity = SCPI99.GetIdentity(address, SCPI_IDENTITY.Model);

                switch (this.Identity) {
                    case EL_34143A.MODEL:
                        this.Instrument = new AgEL30000(this.Address);
                        EL_34143A.Initialize(this);
                        break;
                    case MM_34661A.MODEL:
                        this.Instrument = new Ag3466x(this.Address);
                        MM_34661A.Initialize(this);
                        break;
                    case PS_E36103B.MODEL:
                    case PS_E36105B.MODEL:
                        this.Instrument = new AgE3610XB(this.Address);
                        PS_E3610xB.Initialize(this);
                        break;
                    case PS_E36234A.MODEL:
                        this.Instrument = new AgE36200(this.Address);
                        PS_E36234A.Initialize(this);
                        break;
                    case WG_33509B.MODEL:
                        this.Instrument = new Ag33500B_33600A(this.Address);
                        WG_33509B.Initialize(this);
                        break;
                    default:
                        this.Instrument = new AgSCPI99(this.Address);
                        SCPI99.Initialize(this);
                        Logger.UnexpectedErrorHandler(SCPI.GetErrorMessage(this, $"Unrecognized SCPI VISA Instrument.  Functionality limited to SCPI99 commands only."));
                        break;
                }
            } catch (Exception e) {
                throw new InvalidOperationException(SCPI.GetErrorMessage(this, "Check to see if SCPI VISA Instrument is powered and its interface communicating."), e);
            }
        }

        public static Dictionary<String, SCPI_VISA_Instrument> Get() {
            Dictionary<String, (String id, String description, String address)> visaInstrumentElements = GetVISA_InstrumentElements();
            Dictionary<String, SCPI_VISA_Instrument> SVIs = new Dictionary<String, SCPI_VISA_Instrument>();
            foreach (KeyValuePair<String, (String id, String description, String address)> kvp in visaInstrumentElements) SVIs.Add(kvp.Key, new SCPI_VISA_Instrument(kvp.Value.id, kvp.Value.description, kvp.Value.address));
            return SVIs;
        }

        public static String GetInfo(SCPI_VISA_Instrument SVI, String optionalHeader = "") {
            String info = (optionalHeader == "") ? "" : optionalHeader += Environment.NewLine;
            foreach (PropertyInfo pi in SVI.GetType().GetProperties()) info += $"{pi.Name,FORMAT_WIDTH}: '{pi.GetValue(SVI)}'{Environment.NewLine}";
            return info;
        }

        // TODO:
        private static Dictionary<String, (String id, String description, String address)> GetVISA_InstrumentElements() {
            SCPI_VISA_InstrumentsSection viSection = (SCPI_VISA_InstrumentsSection)ConfigurationManager.GetSection("SCPI_VISA_InstrumentsSection"); 
            SCPI_VISA_InstrumentElements viElements = viSection.SCPI_VISA_InstrumentElements;
            Dictionary<String, (String id, String description, String address)> visaInstrumentElements = new Dictionary<String, (String id, String description, String address)>();
            String addresses = String.Empty;
            foreach (SCPI_VISA_InstrumentElement viElement in viElements) {
                if (visaInstrumentElements.ContainsKey(viElement.ID)) throw new ArgumentException($"App.config's ID '{viElement.ID}' duplicated; must be unique.  ID's Description is '{viElement.Description}.'");
                if (addresses.Contains(viElement.Address)) throw new ArgumentException($"App.config's Address '{viElement.Address}' duplicated; must be unique.  Address' ID is '{viElement.ID}'.");
                addresses += viElement.Address;
                visaInstrumentElements.Add(viElement.ID, (viElement.ID, viElement.Description, viElement.Address));
            }
            return visaInstrumentElements;
        }
    }
}
