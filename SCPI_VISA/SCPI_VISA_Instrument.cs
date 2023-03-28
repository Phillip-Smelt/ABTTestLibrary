using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using Agilent.CommandExpert.ScpiNet.Ag33500B_33600A_2_09;
using Agilent.CommandExpert.ScpiNet.Ag3466x_2_08;
using Agilent.CommandExpert.ScpiNet.AgE3610XB_1_0_0_1_00;
using Agilent.CommandExpert.ScpiNet.AgE36200_1_0_0_1_0_2_1_00;
using Agilent.CommandExpert.ScpiNet.AgEL30000_1_2_5_1_0_6_17_114;
using TestLibrary.AppConfig;
using TestLibrary.Logging;
// All Agilent.CommandExpert.ScpiNet drivers are created by adding new instruments in Keysight's Command Expert app software.
//  - Command Expert literally downloads & installs Agilent.CommandExpert.ScpiNet drivers when new instruments are added.
//  - The Agilent.CommandExpert.ScpiNet dirvers are installed into folder C:\ProgramData\Keysight\Command Expert\ScpiNetDrivers.
//
// https://www.keysight.com/us/en/lib/software-detail/computer-software/command-expert-downloads-2151326.html
//
// Recommend using Command Expert to generate SCPI commands, which are directly exportable as .Net statements.
//
// TODO: Implement Rohde-Schwarz' recommendations from below link when time permit. 
// https://www.rohde-schwarz.com/webhelp/Remote_Control_SCPI_HTML_GettingStarted/Content/welcome.htm
// NOTE: Following notes apply to namespace TestLibrary.SCPI_VISA:
// NOTE: Wrapper methods hopefully prove useful for the most commonly used SCPI commands.
// NOTE: But wrapper methods are strictly conveniences, not necessities.
// NOTE: Won't ever complete wrappers for the full set of SCPI commands, just some of the most commonly used & useful ones.

namespace TestLibrary.SCPI_VISA {
    // TODO: Refactor class Instrument to class SCPI_VISA_Instrument.
    // TODO: Refactor class SCPI_VISA_Instrument into file ConfigSCPI_VISA_Instruments.
    public class Instrument {
        public static String CHANNEL_1 = "(@1)";
        public static String CHANNEL_2 = "(@2)";
        public static String CHANNEL_1_2 = "(@1:2)";

        public enum CATEGORIES {// Abbreviations:
            CounterTimer,       // CT
            ElectronicLoad,     // EL
            LogicAnalyzer,      // LA
            MultiMeter,         // MM
            OscilloScope,       // OS
            PowerSupply,        // PS
            WaveformGenerator   // WG
        }

        public enum IDs {
            // NOTE: Not all SCPI VISA Instrument IDs are necessarily present/installed; actual configuration defined in file App.config.
            //  - IDs has *vastly* more capacity than needed, but doing so costs little.
            CT1, CT2, CT3, CT4, CT5, CT6, CT7, CT8, CT9,    // Counter Timers 1 - 9.
            EL1, EL2, EL3, EL4, EL5, EL6, EL7, EL8, EL9,    // Electronic Loads 1 - 9.
            LA1, LA2, LA3, LA4, LA5, LA6, LA7, LA8, LA9,    // Logic Analyzers 1 - 9.
            MM1, MM2, MM3, MM4, MM5, MM6, MM7, MM8, MM9,    // Multi-Meters 1 - 9.
            OS1, OS2, OS3, OS4, OS5, OS6, OS7, OS8, OS9,    // OscilloScopes 1 - 9.
            PS1, PS2, PS3, PS4, PS5, PS6, PS7, PS8, PS9,    // Power Supplies 1 - 9.
            WG1, WG2, WG3, WG4, WG5, WG6, WG7, WG8, WG9     // Waveform Generators 1 - 9.
        }

        public String Address { get; private set; }
        public String Description { get; private set; }
        public Instrument.CATEGORIES Category { get; private set; }
        public object Instance { get; private set; }

        private Instrument(String address,  String description) {
            this.Address = address;
            this.Description = description;

            try {
                String instrumentModel = SCPI99.GetModel(this.Address);
                switch (instrumentModel) {
                    case "EL34143A":
                        this.Category = Instrument.CATEGORIES.ElectronicLoad;
                        this.Instance = new AgEL30000(this.Address);
                        EL_34143A.SpecificInitialization(this);
                        break;
                    case "34461A":
                        this.Category = Instrument.CATEGORIES.MultiMeter;
                        this.Instance = new Ag3466x(this.Address);
                        MM_34661A.SpecificInitialization(this);
                        break;
                    case "E36103B":
                    case "E36105B":
                        this.Category = Instrument.CATEGORIES.PowerSupply;
                        this.Instance = new AgE3610XB(this.Address);
                        PS_E3610xB.SpecificInitialization(this);
                        break;
                    case "E36234A":
                        this.Category = Instrument.CATEGORIES.PowerSupply;
                        this.Instance = new AgE36200(this.Address);
                        PS_E36234A.SpecificInitialization(this);
                        break;
                    case "33509B":
                        this.Category = Instrument.CATEGORIES.WaveformGenerator;
                        this.Instance = new Ag33500B_33600A(this.Address);
                        WG_33509B.SpecificInitialization(this);
                        break;
                    default:
                        // *TST, *RST & *CLS are generic Instrument Initializations that work universly on all SCPI VISA Instruments.
                        // This switch adds specific Initializations via .SpecificInitialzation() methods for recognized Instruments.
                        SCPI99.SelfTest(this.Address); // SCPI99.SelfTest() issues a Factory Reset (*RST) command after its *TST completes.
                        SCPI99.Clear(this.Address);    // SCPI99.Clear() issues SCPI *CLS.
                        Logger.UnexpectedErrorHandler($"Unrecognized Instrument!{Environment.NewLine}{Environment.NewLine}" +
                            $"Description : {this.Description}{Environment.NewLine}{Environment.NewLine}" +
                            $"Address     : {this.Address}{Environment.NewLine}{Environment.NewLine}" +
                            $"Update Class TestLibrary.SCPI_VISA.Instrument, adding '{instrumentModel}'.");
                        break;
                }
            } catch (Exception e) {
                String[] a = address.Split(':');
                throw new InvalidOperationException($"Check to see if SCPI Instrument with Description '{this.Description}' & VISA Address '{address}' is powered and it's {a[0]} bus is communicating.", e);
            }
        }

        public static Dictionary<IDs, Instrument> Get() {
            Dictionary<IDs, (String address, String description)> visaInstrumentElements = GetVISA_InstrumentElements();
            Dictionary<IDs, Instrument> instruments = new Dictionary<IDs, Instrument>();
            foreach (KeyValuePair<IDs, (String address, String description)> kvp in visaInstrumentElements) instruments.Add(kvp.Key, new Instrument(kvp.Value.address, kvp.Value.description));
            return instruments;
        }

        private static Dictionary<IDs, (String address, String description)> GetVISA_InstrumentElements() {
            SCPI_VISA_InstrumentsSection viSection = (SCPI_VISA_InstrumentsSection)ConfigurationManager.GetSection("SCPI_VISA_InstrumentsSection");
            SCPI_VISA_InstrumentElements viElements = viSection.SCPI_VISA_InstrumentElements;
            Dictionary<IDs, (String address, String description)> visaInstrumentElements = new Dictionary<IDs, (String address, String description)>();
            IDs id;
            String addresses = String.Empty;
            foreach (SCPI_VISA_InstrumentElement viElement in viElements) {
                id = (IDs)Enum.Parse(typeof(IDs), viElement.ID);
                if (!Enum.IsDefined(typeof(IDs), id)) throw new ArgumentException($"App.config's ID '{viElement.ID}' not present in SCPI_VISA.IDs enum.");
                if (visaInstrumentElements.ContainsKey(id)) throw new ArgumentException($"App.config's ID '{viElement.ID}' duplicated; must be unique.");
                if (addresses.Contains(viElement.Address)) throw new ArgumentException($"App.config's Address '{viElement.Address}' duplicated; must be unique.  Address' ID is '{viElement.ID}'.");
                addresses += viElement.Address;
                visaInstrumentElements.Add(id, (viElement.Address, viElement.Description));
            }
            return visaInstrumentElements;
        }

        public static String GetMessage(Instrument instrument, String optionalHeader = "") {
            String Message = (optionalHeader == "") ? "" : optionalHeader += Environment.NewLine;
            foreach (PropertyInfo pi in instrument.GetType().GetProperties()) Message += $"{pi.Name,-14}: {pi.GetValue(instrument)}{Environment.NewLine}";
            return Message;
        }
    }
}
