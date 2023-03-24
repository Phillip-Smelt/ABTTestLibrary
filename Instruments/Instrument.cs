using System;
using System.Collections.Generic;
using System.Reflection;
using Agilent.CommandExpert.ScpiNet.Ag33500B_33600A_2_09;
using Agilent.CommandExpert.ScpiNet.AgE3610XB_1_0_0_1_00;
using Agilent.CommandExpert.ScpiNet.AgE36200_1_0_0_1_0_2_1_00;
using Agilent.CommandExpert.ScpiNet.AgEL30000_1_2_5_1_0_6_17_114;
using Agilent.CommandExpert.ScpiNet.Ag3466x_2_08;
// All Agilent.CommandExpert.ScpiNet drivers are created by adding new instruments in Keysight's Command Expert app software.
//  - Command Expert literally downloads & installs Agilent.CommandExpert.ScpiNet drivers when new instruments are added.
//  - The Agilent.CommandExpert.ScpiNet dirvers are installed into folder C:\ProgramData\Keysight\Command Expert\ScpiNetDrivers.
//
// https://www.keysight.com/us/en/lib/software-detail/computer-software/command-expert-downloads-2151326.html
//
// Recommend using Command Expert to generate SCPI & IVI driver commands, which are directly exportable as .Net statements.
//
using TestLibrary.Instruments.Keysight;

// TODO: Implement Rohde-Schwarz' recommendations from below link when time permit. 
// https://www.rohde-schwarz.com/webhelp/Remote_Control_SCPI_HTML_GettingStarted/Content/welcome.htm
// NOTE: Following notes apply to namespace TestLibrary.Instruments.Keysight:
// NOTE: Consider using Keysight's IVI drivers instead of wrapping SCPI driver's calls; IVI drivers might prove preferable.
// NOTE: Wrapper methods hopefully prove useful for the most commonly used SCPI commands.
// NOTE: But wrapper methods are strictly conveniences, not necessities.
// NOTE: Won't ever complete wrappers for the full set of SCPI commands, just some of the most commonly used & useful ones.

namespace TestLibrary.Instruments {
    public class Instrument {
        // TODO: Replace _instrumentAddresses with an XML app.config configuration file defining each Test System's instruments.
        //  - Permitting dynamic configuration of Test Systems, without requiring re-compilation.
        //  - Moving each Test System's configuration out of global Test Library, into a local XML configuration file.
        // NOTE: Add/remove instruments as needed.
        public static String CHANNEL_1 = "(@1)";
        public static String CHANNEL_2 = "(@2)";
        public static String CHANNEL_1_2 = "(@1:2)";

        public enum CATEGORIES {
            CounterTimer,
            ElectronicLoad,
            LogicAnalyzer,
            MultiMeter,
            OscilloScope,
            PowerSupply,
            WaveformGenerator
        }

        public enum IDs {
            EL_EL34143A,
            MM_33461A,
            PS_E36103B_1,
            PS_E36103B_2,
            PS_E36105B,
            PS_E36234A,
            WG_E33509B
        }

        private static readonly Dictionary<Instrument.IDs, String> _instrumentAddresses = new Dictionary<Instrument.IDs, String> {
        // VISA (Virtual Instrument Software Architecture) Resource Names.
        // - https://www.ivifoundation.org/specifications/default.aspx
        // - Technically, these are actually VISA 'Resource Names' instead of VISA 'Addresses',
        //   but 'Address' has widespread usage and is more descriptive than 'Resource Name'.
            { Instrument.IDs.MM_33461A, "USB0::0x2A8D::0x1301::MY60049978::0::INSTR" },
         // { Instrument.IDs.WG_E33509B, "USB0::0x0957::0x2507::MY59003604::0::INSTR" },
            { Instrument.IDs.PS_E36103B_1, "USB0::0x2A8D::0x1602::MY61001983::0::INSTR" },
            { Instrument.IDs.PS_E36103B_2, "USB0::0x2A8D::0x1602::MY61001958::0::INSTR" },
         // { Instrument.IDs.PS_E36105B, "USB0::0x2A8D::0x1802::MY61001696::0::INSTR" },
            { Instrument.IDs.PS_E36234A, "USB0::0x2A8D::0x3402::MY61002598::0::INSTR" },
            { Instrument.IDs.EL_EL34143A, "USB0::0x2A8D::0x3802::MY61001295::0::INSTR" }
            };
        public Instrument.IDs ID { get; private set; }
        public Instrument.CATEGORIES Category { get; private set; }
        public String Address { get; private set; }
        public object Instance { get; private set; }
        public String Manufacturer { get; private set; }
        public String Model { get; private set; }

        private Instrument(Instrument.IDs id, String address) {
            this.ID = id;
            this.Address = address;
            this.Manufacturer = SCPI99.GetManufacturer(this.Address);
            this.Model = SCPI99.GetModel(this.Address);
            SCPI99.SelfTest(this.Address);
            try {
                switch (ID) {
                    case Instrument.IDs.EL_EL34143A:
                        this.Category = Instrument.CATEGORIES.ElectronicLoad;
                        this.Instance = new AgEL30000(this.Address);
                        EL34143A.RemoteLock(this);
                        EL34143A.ResetClear(this);

                        break;
                    case Instrument.IDs.MM_33461A:
                        this.Category = Instrument.CATEGORIES.MultiMeter;
                        this.Instance = new Ag3466x(this.Address);
                        KS34661A.ResetClear(this);
                        break;
                    case Instrument.IDs.PS_E36103B_1:
                    case Instrument.IDs.PS_E36103B_2:
                    case Instrument.IDs.PS_E36105B:
                        this.Category = Instrument.CATEGORIES.PowerSupply;
                        this.Instance = new AgE3610XB(this.Address);
                        E3610xB.RemoteLock(this);
                        E3610xB.ResetClear(this);
                        break;
                    case Instrument.IDs.PS_E36234A:
                        this.Category = Instrument.CATEGORIES.PowerSupply;
                        this.Instance = new AgE36200(this.Address);
                        E36234A.RemoteLock(this);
                        E36234A.ResetClear(this);
                        break;
                    case Instrument.IDs.WG_E33509B:
                        this.Category = Instrument.CATEGORIES.WaveformGenerator;
                        this.Instance = new Ag33500B_33600A(this.Address);
                        KS33509B.ResetClear(this);
                        break;
                    default:
                        throw new NotImplementedException($"Unrecognized Instrument!{Environment.NewLine}{Environment.NewLine}" +
                            $"Update Class TestLibrary.Instrument.Instrument, adding '{ID}'.");
                }
            } catch (NotImplementedException) {
                throw;
            } catch (Exception e) {
                String[] a = this.Address.Split(':');
                throw new InvalidOperationException($"Check to see if {Enum.GetName(typeof(Instrument.CATEGORIES), this.Category)} with VISA Address '{this.Address}' is powered and it's {a[0]} bus is communicating.", e);
            }
        }

        public static Dictionary<Instrument.IDs, Instrument> Get() {
            Dictionary<Instrument.IDs, Instrument> d = new Dictionary<Instrument.IDs, Instrument>();
            foreach (KeyValuePair<Instrument.IDs, String> ia in _instrumentAddresses) d.Add(ia.Key, new Instrument(ia.Key, ia.Value));
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
