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

// NOTE: Consider adding ScpiQuery & ScpiCommand methods to all Instrument classes, to pass raw SCPI strings to the Instrument.
// Eliminates need to wrap every SCPI command, and allows all Instrument specific commands/queries to be handled by the Instrument
// classes, rather than directly invoking them from TestPrograms.
namespace TestLibrary.Instruments {
    public class Instrument {
        // TODO: Replace this Instrument definition/configuration class with an XML app.config configuration file defining each Test System's instruments.
        //  - Permitting dynamic configuration of Test Systems, without requiring re-compilation.
        //  - Moving each Test System's configuration out of global Test Library, into a local XML configuration file.
        // NOTE: Add/remove instruments as needed.
        // VISA (Virtual Instrument Software Architecture) Resource Names.
        // - https://www.ivifoundation.org/specifications/default.aspx
        // - Technically, these are actually VISA 'Resource Names' instead of VISA 'Addresses',
        //   but 'Address' has widespread usage and is more descriptive than 'Resource Name'.
        public static String CHANNEL_1 = "(@1)";
        public static String CHANNEL_2 = "(@2)";
        public static String CHANNEL_1_2 = "(@1:2)";

        public enum CATEGORIES {
            Counter_Timer,
            Electronic_Load,
            Multi_Meter,
            Oscilloscope,
            Power_Suppy,
            Waveform_Generator
        }

        public enum IDs {
            Keysight_33461A_Multi_Meter,
            Keysight_E33509B_WaveForm_Generator,
            Keysight_E36103B_Power_Supply_1,
            Keysight_E36103B_Power_Supply_2,
            Keysight_E36105B_Power_Supply,
            Keysight_E36234A_Power_Supply,
            Keysight_EL34143A_Electronic_Load
        }

        private static readonly Dictionary<Instrument.IDs, String> _instrumentAddresses = new Dictionary<Instrument.IDs, String> {
            { Instrument.IDs.Keysight_33461A_Multi_Meter, "USB0::0x2A8D::0x1301::MY60049978::0::INSTR" },
         // { Instrument.IDs.Keysight_E33509B_WaveForm_Generator, "USB0::0x0957::0x2507::MY59003604::0::INSTR" },
            { Instrument.IDs.Keysight_E36103B_Power_Supply_1, "USB0::0x2A8D::0x1602::MY61001983::0::INSTR" },
            { Instrument.IDs.Keysight_E36103B_Power_Supply_2, "USB0::0x2A8D::0x1602::MY61001958::0::INSTR" },
         // { Instrument.IDs.Keysight_E36105B_Power_Supply, "USB0::0x2A8D::0x1802::MY61001696::0::INSTR" },
            { Instrument.IDs.Keysight_E36234A_Power_Supply, "USB0::0x2A8D::0x3402::MY61002598::0::INSTR" },
            { Instrument.IDs.Keysight_EL34143A_Electronic_Load, "USB0::0x2A8D::0x3802::MY61001295::0::INSTR" }
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

            try {
                switch (ID) {
                    case Instrument.IDs.Keysight_E36103B_Power_Supply_1:
                    case Instrument.IDs.Keysight_E36103B_Power_Supply_2:
                    case Instrument.IDs.Keysight_E36105B_Power_Supply:
                        this.Category = Instrument.CATEGORIES.Power_Suppy;
                        this.Instance = new AgE3610XB(this.Address);
                        ((AgE3610XB)this.Instance).SCPI.SYSTem.RWLock.Command();
                        break;
                    case Instrument.IDs.Keysight_E36234A_Power_Supply:
                        this.Category = Instrument.CATEGORIES.Power_Suppy;
                        this.Instance = new AgE36200(this.Address);
                        ((AgE36200)this.Instance).SCPI.SYSTem.RWLock.Command();
                        break;
                    case Instrument.IDs.Keysight_EL34143A_Electronic_Load:
                        this.Category = Instrument.CATEGORIES.Electronic_Load;
                        this.Instance = new AgEL30000(this.Address);
                        ((AgEL30000)this.Instance).SCPI.SYSTem.RWLock.Command();
                        break;
                    case Instrument.IDs.Keysight_E33509B_WaveForm_Generator:
                        this.Category = Instrument.CATEGORIES.Waveform_Generator;
                        this.Instance = new Ag33500B_33600A(this.Address);
                        break;
                    case Instrument.IDs.Keysight_33461A_Multi_Meter:
                        this.Category = Instrument.CATEGORIES.Multi_Meter;
                        this.Instance = new Ag3466x(this.Address);
                        break;
                    default:
                        throw new NotImplementedException($"Unrecognized Instrument!{Environment.NewLine}{Environment.NewLine}" +
                            $"Update Class TestLibrary.InstrumentTasks.Instrument, adding '{ID}'.");
                }
                this.Manufacturer = SCPI99.GetManufacturer(this.Address);
                this.Model = SCPI99.GetModel(this.Address);
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
    }

    public static class InstrumentTasks {
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
    
        public static void SCPI99_ResetClear(Dictionary<Instrument.IDs, Instrument> instruments) {
            foreach (KeyValuePair<Instrument.IDs, Instrument> i in instruments) {
                SCPI99.Reset(i.Value.Address);
                SCPI99.Clear(i.Value.Address);
            }
        }

        public static void SCPI99_Test(Dictionary<Instrument.IDs, Instrument> instruments) {
            Int32 SelfTestResult;
            foreach (KeyValuePair<Instrument.IDs, Instrument> i in instruments) {
                SelfTestResult = SCPI99.SelfTest(i.Value.Address);
                if (SelfTestResult != 0) throw new InvalidOperationException(GetMessage(i.Value));
            }
        }

        public static void InstrumentResetClear(Dictionary<Instrument.IDs, Instrument> instruments) {
            foreach (KeyValuePair<Instrument.IDs, Instrument> i in instruments) {
                switch (i.Value.ID) {
                    case Instrument.IDs.Keysight_E36103B_Power_Supply_1:
                    case Instrument.IDs.Keysight_E36103B_Power_Supply_2:
                    case Instrument.IDs.Keysight_E36105B_Power_Supply:
                        E3610xB.ResetClear(i.Value);
                        break;
                    case Instrument.IDs.Keysight_E36234A_Power_Supply:
                        E36234A.ResetClear(i.Value);
                        break;
                    case Instrument.IDs.Keysight_EL34143A_Electronic_Load:
                        EL34143A.ResetClear(i.Value);
                        break;
                    case Instrument.IDs.Keysight_E33509B_WaveForm_Generator:
                        KS33509B.ResetClear(i.Value);
                        break;
                    case Instrument.IDs.Keysight_33461A_Multi_Meter:
                        KS34661A.ResetClear(i.Value);
                        break;
                    default:
                        throw new NotImplementedException($"Unrecognized Instrument!{Environment.NewLine}{Environment.NewLine}" +
                            $"Update Class TestLibrary.InstrumentTasks.Instrument, adding '{i.Value.ID}'.");
                }
            }
        }
    }
}
