using System;
using System.Collections.Generic;
using System.Reflection;
using Agilent.CommandExpert.ScpiNet.Ag33500B_33600A_2_09;
using Agilent.CommandExpert.ScpiNet.Ag3466x_2_08;
using Agilent.CommandExpert.ScpiNet.AgE3610XB_1_0_0_1_00;
using Agilent.CommandExpert.ScpiNet.AgE36200_1_0_0_1_0_2_1_00;
using Agilent.CommandExpert.ScpiNet.AgEL30000_1_2_5_1_0_6_17_114;
// All Agilent.CommandExpert.ScpiNet drivers are created by adding new instruments in Keysight's Command Expert app software.
//  - Command Expert literally downloads & installs Agilent.CommandExpert.ScpiNet drivers when new instruments are added.
//  - The Agilent.CommandExpert.ScpiNet dirvers are installed into folder C:\ProgramData\Keysight\Command Expert\ScpiNetDrivers.
//
// https://www.keysight.com/us/en/lib/software-detail/computer-software/command-expert-downloads-2151326.html
//
// Recommend using Command Expert to generate SCPI & IVI driver commands, which are directly exportable as .Net statements.
//
// TODO: Implement Rohde-Schwarz' recommendations from below link when time permit. 
// https://www.rohde-schwarz.com/webhelp/Remote_Control_SCPI_HTML_GettingStarted/Content/welcome.htm
// NOTE: Following notes apply to namespace TestLibrary.VISA:
// NOTE: Consider using Keysight's IVI drivers instead of wrapping SCPI driver's calls; IVI drivers might prove preferable.
// NOTE: Wrapper methods hopefully prove useful for the most commonly used SCPI commands.
// NOTE: But wrapper methods are strictly conveniences, not necessities.
// NOTE: Won't ever complete wrappers for the full set of SCPI commands, just some of the most commonly used & useful ones.
// NOTE: Update as necessary.

namespace TestLibrary.VISA {
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
            // NOTE: Not all VISA Instrument IDs are necessarily present/installed; actual configuration defined in file VISA.Config.xml.
            CT1, CT2, CT3,                                  // Counter Timer
            EL1, EL2, EL3,                                  // Electronic Load
            LA1, LA2, LA3,                                  // Logic Analyzer
            MM1, MM2, MM3,                                  // Multi-Meter
            OS1, OS2, OS3,                                  // OscilloScope
            PS1, PS2, PS3, PS4, PS5, PS6, PS7, PS8, PS9,    // Power Supply
            WG1, WG2, WG3, WG4, WG5, WG6, WG7, WG8, WG9     // Waveform Generator
        }

        private static readonly Dictionary<IDs, String> _instrumentAddresses = VISA_Instrument.Get();
        // TODO: public Instrument.IDs ID { get; private set; }
        public String Address { get; private set; }
        public Instrument.CATEGORIES Category { get; private set; }
        public object Instance { get; private set; }

        private Instrument(String address) {
            // TODO: this.ID = id;
            this.Address = address;
            SCPI99.SelfTest(this.Address); // SCPI99.SelfTest() issues a Factory Reset (*RST) command after its *TST completes.
            SCPI99.Reset(this.Address); 

            try {
                String instrumentModel = SCPI99.GetModel(this.Address);
                switch (instrumentModel) {
                    case "EL34143A":
                        this.Category = Instrument.CATEGORIES.ElectronicLoad;
                        this.Instance = new AgEL30000(this.Address);
                        EL_34143A.ModelSpecificInitialization(this);
                        break;
                    case "34461A":
                        this.Category = Instrument.CATEGORIES.MultiMeter;
                        this.Instance = new Ag3466x(this.Address);
                        MM_34661A.ModelSpecificInitialization(this);
                        break;
                    case "E36103B":
                    case "E36105B":
                        this.Category = Instrument.CATEGORIES.PowerSupply;
                        this.Instance = new AgE3610XB(this.Address);
                        PS_E3610xB.ModelSpecificInitialization(this);
                        break;
                    case "E36234A":
                        this.Category = Instrument.CATEGORIES.PowerSupply;
                        this.Instance = new AgE36200(this.Address);
                        PS_E36234A.ModelSpecificInitialization(this);
                        break;
                    case "33509B":
                        this.Category = Instrument.CATEGORIES.WaveformGenerator;
                        this.Instance = new Ag33500B_33600A(this.Address);
                        WG_33509B.ModelSpecificInitialization(this);
                        break;
                    default:
                        throw new NotImplementedException($"Unrecognized Instrument!{Environment.NewLine}{Environment.NewLine}" +
                            $"Update Class TestLibrary.VISA.Instrument, adding '{instrumentModel}'.");
                }
            } catch (Exception e) {
                String[] a = address.Split(':');
                throw new InvalidOperationException($"Check to see if {Enum.GetName(typeof(Instrument.CATEGORIES), this.Category)} with VISA Address '{address}' is powered and it's {a[0]} bus is communicating.", e);
            }
        }

        public static Dictionary<IDs, Instrument> Get() {
            Dictionary<IDs, Instrument> d = new Dictionary<IDs, Instrument>();
            foreach (KeyValuePair<IDs, String> ia in _instrumentAddresses) d.Add(ia.Key, new Instrument(ia.Value));
            return d;
        }

        public static String GetMessage(Instrument instrument, String optionalHeader = "") {
            String Message = (optionalHeader == "") ? "" : optionalHeader += Environment.NewLine;
            foreach (PropertyInfo pi in instrument.GetType().GetProperties()) Message += $"{pi.Name,-14}: {pi.GetValue(instrument)}{Environment.NewLine}";
            return Message;
        }

        public static void SCPI99_Reset(Dictionary<Instrument.IDs, Instrument> instruments) {
            foreach (KeyValuePair<Instrument.IDs, Instrument> i in instruments) SCPI99.Reset(i.Value.Address);
        }

        public static void SCPI99_Clear(Dictionary<Instrument.IDs, Instrument> instruments) {
            foreach (KeyValuePair<Instrument.IDs, Instrument> i in instruments) SCPI99.Clear(i.Value.Address);
        }

        public static void SCPI99_Test(Dictionary<Instrument.IDs, Instrument> instruments) {
            Int32 SelfTestResult;
            foreach (KeyValuePair<Instrument.IDs, Instrument> i in instruments) {
                SelfTestResult = SCPI99.SelfTest(i.Value.Address);
                if (SelfTestResult != 0) throw new InvalidOperationException(GetMessage(i.Value));
            }
        }
    }
}
