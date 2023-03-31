using System;
using System.Collections.Generic;
using System.Configuration;
using Agilent.CommandExpert.ScpiNet.Ag33500B_33600A_2_09;
using Agilent.CommandExpert.ScpiNet.Ag3466x_2_08;
using Agilent.CommandExpert.ScpiNet.AgE3610XB_1_0_0_1_00;
using Agilent.CommandExpert.ScpiNet.AgE36200_1_0_0_1_0_2_1_00;
using Agilent.CommandExpert.ScpiNet.AgEL30000_1_2_5_1_0_6_17_114;
using Agilent.CommandExpert.ScpiNet.AgSCPI99_1_0;
using TestLibrary.SCPI_VISA_Instruments;
using TestLibrary.Logging;
using System.Reflection;

namespace TestLibrary.AppConfig {
    // NOTE: https://forums.ni.com/t5/Instrument-Control-GPIB-Serial/IVI-Drivers-Pros-and-Cons/td-p/4165671.
    public enum SCPI_VISA_CATEGORIES {
        CounterTimer,     // CT
        ElectronicLoad,   // EL
        LogicAnalyzer,    // LA
        MultiMeter,       // MM
        OscilloScope,     // OS
        PowerSupply,      // PS
        WaveformGenerator // WG
    }

    public enum SCPI_VISA_IDs {
        // NOTE: Not all SCPI_VISA_IDs are necessarily present/installed; actual configuration defined in file App.config.
        //  - SCPI_VISA_IDs has vastly more capacity than needed, but excess capacity costs little.
        CT1, CT2, CT3, CT4, CT5, CT6, CT7, CT8, CT9, // Counter Timers 1 - 9.
        EL1, EL2, EL3, EL4, EL5, EL6, EL7, EL8, EL9, // Electronic Loads 1 - 9.
        LA1, LA2, LA3, LA4, LA5, LA6, LA7, LA8, LA9, // Logic Analyzers 1 - 9.
        MM1, MM2, MM3, MM4, MM5, MM6, MM7, MM8, MM9, // Multi-Meters 1 - 9.
        OS1, OS2, OS3, OS4, OS5, OS6, OS7, OS8, OS9, // OscilloScopes 1 - 9.
        PS1, PS2, PS3, PS4, PS5, PS6, PS7, PS8, PS9, // Power Supplies 1 - 9.
        WG1, WG2, WG3, WG4, WG5, WG6, WG7, WG8, WG9  // Waveform Generators 1 - 9.
    }

    // TODO? enum SCPI_VISA_IDs_RENAMED { PRI_BIAS, SEC_BIAS, PRE_BIAS, MAIN, MM_34461A, EL_34143A, WG_33509B }
    //
    //  Dictionary<SCPI_VISA_IDs_RENAMED, SCPI_VISA_Ids> Instruments = new Dictionary<SCPI_VISA_IDs_RENAMED, SCPI_VISA_Ids>() {
    //  SCPI_VISA_IDs_RENAMED.PRI_BIAS,  SCPI_VISA_IDs.PS1;
    //  SCPI_VISA_IDs_RENAMED.SEC_BIAS,  SCPI_VISA_IDs.PS2;
    //  SCPI_VISA_IDs_RENAMED.PRE_BIAS,  SCPI_VISA_IDs.PS3;
    //  SCPI_VISA_IDs_RENAMED.MAIN,      SCPI_VISA_IDs.PS4;
    //  SCPI_VISA_IDs_RENAMED.MM_34461A, SCPI_VISA_IDs.MM1;
    //  SCPI_VISA_IDs_RENAMED.EL_34143A, SCPI_VISA_IDs.EL1;
    //  SCPI_VISA_IDs_RENAMED.WG_33509B, SCPI_VISA_IDs.WG1; }
    //
    //  Thus, TestProgram's SVIs[Instruments[PRI_BIAS]] == TestLibrary's SVIs[SCPI_VISA_IDs.PS1]
    //  
    //  Or, could override class SCPI_VISA_Instrument, copying SVIs into a new Dictionary with renamed Keys from SCPI_VISA_IDs_RENAMED instead of SCPI_VISA_IDs.
    //  
    public enum SCPI_IDENTITY { Manufacturer, Model, SerialNumber, FirmwareRevision }
    // Example: "Keysight Technologies,E36103B,MY61001983,1.0.2-1.02".

    public class SCPI_VISA_InstrumentElement : ConfigurationElement {
        [ConfigurationProperty("ID", IsKey = true, IsRequired = true)] public SCPI_VISA_IDs ID { get { return ((SCPI_VISA_IDs)base["ID"]); } }
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
        protected override Object GetElementKey(ConfigurationElement element) { return ((SCPI_VISA_InstrumentElement)(element)).ID; }
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
        public SCPI_VISA_IDs ID { get; private set; }
        public String Description { get; private set; }
        public String Address { get; private set; }
        public SCPI_VISA_CATEGORIES Category { get; private set; }
        public String Identity { get; private set; }
        public Object Instrument { get; private set; }
        public const Int32 FORMAT_WIDTH = -16;

        private SCPI_VISA_Instrument(SCPI_VISA_IDs id, String description, String address) {
            this.ID = id;
            this.Description = description;
            this.Address = address;

            switch (this.ID) {
                case SCPI_VISA_IDs svi when (SCPI_VISA_IDs.CT1 <= svi && svi <= SCPI_VISA_IDs.CT9):
                    this.Category = SCPI_VISA_CATEGORIES.CounterTimer;
                    break;
                case SCPI_VISA_IDs svi when (SCPI_VISA_IDs.EL1 <= svi && svi <= SCPI_VISA_IDs.EL9):
                    this.Category = SCPI_VISA_CATEGORIES.ElectronicLoad;
                    break;
                case SCPI_VISA_IDs svi when (SCPI_VISA_IDs.LA1 <= svi && svi <= SCPI_VISA_IDs.LA9):
                    this.Category = SCPI_VISA_CATEGORIES.LogicAnalyzer;
                    break;
                case SCPI_VISA_IDs svi when (SCPI_VISA_IDs.MM1 <= svi && svi <= SCPI_VISA_IDs.MM9):
                    this.Category = SCPI_VISA_CATEGORIES.MultiMeter; 
                    break;
                case SCPI_VISA_IDs svi when (SCPI_VISA_IDs.OS1 <= svi && svi <= SCPI_VISA_IDs.OS9):
                    this.Category = SCPI_VISA_CATEGORIES.OscilloScope;
                    break;
                case SCPI_VISA_IDs svi when (SCPI_VISA_IDs.PS1 <= svi && svi <= SCPI_VISA_IDs.PS9):
                    this.Category = SCPI_VISA_CATEGORIES.PowerSupply;
                    break;
                case SCPI_VISA_IDs svi when (SCPI_VISA_IDs.WG1 <= svi && svi <= SCPI_VISA_IDs.WG9):
                    this.Category = SCPI_VISA_CATEGORIES.WaveformGenerator;
                    break;
            }

            try {
                this.Identity = SCPI_VISA.GetIdentity(address);
                String model = this.Identity.Split(SCPI_VISA.IDENTITY_SEPARATOR)[(Int32)SCPI_IDENTITY.Model];

                switch (model) {
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
                        PI_SCPI99.Initialize(this);
                        Logger.UnexpectedErrorHandler(SCPI_VISA.GetErrorMessage(this, $"Unrecognized SCPI VISA Instrument.  Functionality limited to SCPI99 commands only."));
                        break;
                }
            } catch (Exception e) {
                throw new InvalidOperationException(SCPI_VISA.GetErrorMessage(this, "Check to see if SCPI VISA Instrument is powered and its interface communicating."), e);
            }
        }

        public static Dictionary<SCPI_VISA_IDs, SCPI_VISA_Instrument> Get() {
            Dictionary<SCPI_VISA_IDs, (SCPI_VISA_IDs, String description, String address)> visaInstrumentElements = GetVISA_InstrumentElements();
            Dictionary<SCPI_VISA_IDs, SCPI_VISA_Instrument> SVIs = new Dictionary<SCPI_VISA_IDs, SCPI_VISA_Instrument>();
            foreach (KeyValuePair<SCPI_VISA_IDs, (SCPI_VISA_IDs id, String description, String address)> kvp in visaInstrumentElements) SVIs.Add(kvp.Key, new SCPI_VISA_Instrument(kvp.Value.id, kvp.Value.description, kvp.Value.address));
            return SVIs;
        }

        public static String GetInfo(SCPI_VISA_Instrument SVI, String optionalHeader = "") {
            String info = (optionalHeader == "") ? "" : optionalHeader += Environment.NewLine;
            foreach (PropertyInfo pi in SVI.GetType().GetProperties()) {
                info += $"{pi.Name,FORMAT_WIDTH}: ";
                if(pi.PropertyType.IsEnum) {
                    if (String.Equals(pi.Name, "ID")) info += $"'{Enum.GetName(typeof(SCPI_VISA_IDs), SVI.ID)}'";
                    if (String.Equals(pi.Name, "Category")) info += $"'{Enum.GetName(typeof(SCPI_VISA_CATEGORIES), SVI.Category)}'";
                }
                else info += $"'{pi.GetValue(SVI)}'";
                info += Environment.NewLine;
            }
            return info;
        }

        private static Dictionary<SCPI_VISA_IDs, (SCPI_VISA_IDs id, String description, String address)> GetVISA_InstrumentElements() {
            SCPI_VISA_InstrumentsSection viSection = (SCPI_VISA_InstrumentsSection)ConfigurationManager.GetSection("SCPI_VISA_InstrumentsSection");
            SCPI_VISA_InstrumentElements viElements = viSection.SCPI_VISA_InstrumentElements;
            Dictionary<SCPI_VISA_IDs, (SCPI_VISA_IDs id, String description, String address)> visaInstrumentElements = new Dictionary<SCPI_VISA_IDs, (SCPI_VISA_IDs id, String description, String address)>();
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
