using System;
using System.Collections.Generic;
using System.Reflection;
using Agilent.CommandExpert.ScpiNet.Ag33500B_33600A_2_09;
using Agilent.CommandExpert.ScpiNet.AgE3610XB_1_0_0_1_00;
using Agilent.CommandExpert.ScpiNet.AgE36200_1_0_0_1_0_2_1_00;
// All Agilent.CommandExpert.ScpiNet drivers are created by adding new instruments in Keysight's Command Expert app software.
//  - Command Expert literally downloads & installs Agilent.CommandExpert.ScpiNet drivers when new instruments are added.
//  - The Agilent.CommandExpert.ScpiNet dirvers are installed into folder C:\ProgramData\Keysight\Command Expert\ScpiNetDrivers.
//
// https://www.keysight.com/us/en/lib/software-detail/computer-software/command-expert-downloads-2151326.html
//
// Recommend using Command Expert to generate SCPI & IVI driver commands, which are directly exportable as .Net statements.
//
using Keysight.Kt34400; // https://www.keysight.com/us/en/lib/software-detail/driver/34400-digital-multimeters-ivi-instrument-drivers.html
using Keysight.KtEL30000; // https://www.keysight.com/us/en/lib/software-detail/driver/el30000a-dc-electronic-loads-ivi-instrument-drivers.html
using TestLibrary.Instruments.Keysight;

// NOTE: Consider adding ScpiQuery & ScpiCommand methods to all Instrument classes, to pass raw SCPI strings to the Instrument.
// Eliminates need to wrap every SCPI command, and allows all Instrument specific commands/queries to be handled by the Instrument
// classes, rather than directly invoking them from TestPrograms.
namespace TestLibrary.Instruments {
    public static class InstrumentTasks {
        public static String GetMessage(Instrument instrument, String OptionalHeader = "") {
            String Message = (OptionalHeader == "") ? "" : OptionalHeader += Environment.NewLine;
            foreach (PropertyInfo pi in instrument.GetType().GetProperties()) Message += $"{pi.Name,-14}: {pi.GetValue(instrument)}{Environment.NewLine}";
            return Message;
        }

        public static void SCPI99Reset(Dictionary<String, Instrument> instruments) {
            foreach (KeyValuePair<String, Instrument> i in instruments) SCPI99.Reset(i.Value.Address); }

        public static void SCPI99Clear(Dictionary<String, Instrument> instruments) {
            foreach (KeyValuePair<String, Instrument> i in instruments) SCPI99.Clear(i.Value.Address); }

        public static void SCPI99ResetClear(Dictionary<String, Instrument> instruments) {
            foreach (KeyValuePair<String, Instrument> i in instruments) {
                SCPI99.Clear(i.Value.Address);
                SCPI99.Reset(i.Value.Address);
            }
        }

        public static void SCPI99Test(Dictionary<String, Instrument> instruments) {
            Int32 SelfTestResult;
            foreach (KeyValuePair<String, Instrument> i in instruments) {
                SelfTestResult = SCPI99.SelfTest(i.Value.Address);
                if (SelfTestResult != 0) throw new InvalidOperationException(GetMessage(i.Value));
            }
        }

        public static void InstrumentResetClear(Dictionary<String, Instrument> instruments) {
            // TODO: Use Reflection instead of below switch to invoke ResetClear() below, as
            // then won't have to update below switch every time another Instrument is added.
            foreach (KeyValuePair<String, Instrument> i in instruments) {
                switch (i.Value.ID) {
                    case Instrument.POWER_PRELIMINARY:
                    case Instrument.POWER_PRIMARY:
                    case Instrument.POWER_SECONDARY:
                        E3610xB.ResetClear(i.Value);
                        break;
                    case Instrument.POWER_MAIN:
                        E36234A.ResetClear(i.Value);
                        break;
                    case Instrument.LOAD:
                        // TODO: case Instrument.LOAD:
                        break;
                    case Instrument.WAVE_GENERATOR:
                        // TODO: case Instrument.WAVE_GENERATOR:
                        break;
                    case Instrument.MULTI_METER:
                        // TODO: case Instrument.MULTI_METER:
                        break;
                    default:
                        throw new NotImplementedException($"Unrecognized Instrument!{Environment.NewLine}{Environment.NewLine}" +
                            $"Update Class TestLibrary.InstrumentTasks.Instrument, adding '{i.Value.ID}'.");
                }
            }
        }
    }

    public class Instrument {
        // TODO: Replace this Instrument class with an XML app.config configuration file defining each Test System's Instruments.
        //  - Will permit dynamic configuration of Test Systems, without requiring re-compilation.
        //  - Also moves each Test System's configuration out of global Test Library and into a local XML configuration file.
        public const String LOAD = "LOAD";
        public const String MULTI_METER = "MULTI_METER";
        public const String WAVE_GENERATOR = "WAVE_GENERATOR";
        public const String POWER_MAIN = "POWER_MAIN";
        public const String POWER_PRIMARY = "POWER_PRIMARY";
        public const String POWER_SECONDARY = "POWER_SECONDARY";
        public const String POWER_PRELIMINARY = "POWER_PRELIMINARY";
        private static Dictionary<String, String> InstrumentsToVISA_Addresses = new Dictionary<String, String> {
            // TODO: Add MULTI_METER's address, enable other Instruments.
            // NOTE: Add/remove instruments as needed.
            // VISA (Virtual Instrument Software Architecture) Resource Names.
            // - https://www.ivifoundation.org/specifications/default.aspx
            // - Technically, these are actually VISA 'Resource Names' instead of VISA 'Addresses',
            //   but 'Address' has widespread usage and is more descriptive than 'Resource Name'.
            // { LOAD, "USB0::0x2A8D::0x3802::MY61001295::0::INSTR" },
            // { MULTI_METER, "TBD" },
            // { WAVE_GENERATOR,"USB0::0x0957::0x2507::MY59003604::0::INSTR" },
            // { POWER_MAIN, "USB0::0x2A8D::0x3402::MY61002598::0::INSTR" },
            { POWER_PRELIMINARY , "USB0::0x2A8D::0x1802::MY61001696::0::INSTR" },
            { POWER_PRIMARY, "USB0::0x2A8D::0x1602::MY61001983::0::INSTR" },
            { POWER_SECONDARY, "USB0::0x2A8D::0x1602::MY61001958::0::INSTR" }
            };
        public String ID { get; private set; }
        public String Category { get; private set; }
        public String Address { get; private set; }
        public object Instance { get; private set; }
        public String Manufacturer { get; private set; }
        public String Model { get; private set; }

        private Instrument(String ID, String Address) {
            this.ID = ID;
            this.Address = Address;

            try {
                switch (ID) {
                    case POWER_PRELIMINARY:
                    case POWER_PRIMARY:
                    case POWER_SECONDARY:
                        this.Category = "Power Supply";
                        this.Instance = new AgE3610XB(this.Address);
                        ((AgE3610XB)this.Instance).SCPI.SYSTem.RWLock.Command();
                        break;
                    case POWER_MAIN:
                        this.Category = "Power Supply";
                        this.Instance = new AgE36200(this.Address);
                        ((AgE36200)this.Instance).SCPI.SYSTem.RWLock.Command();
                        break;
                    case LOAD:
                        this.Category = "Electronic Load";
                        this.Instance = new KtEL30000(this.Address, false, false);
                        break;
                    case WAVE_GENERATOR:
                        this.Category = "Waveform Generator";
                        this.Instance = new Ag33500B_33600A(this.Address);
                        break;
                    case MULTI_METER:
                        this.Category = "Multi-Meter";
                        this.Instance = new Kt34400(this.Address, false, false);
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
                throw new InvalidOperationException($"Check to see if {this.Category} with VISA Address '{this.Address}' is powered and it's {a[0]} bus is communicating.", e);
            }
        }

        public static Dictionary<String, Instrument> Get() {
            Dictionary<String, Instrument> d = new Dictionary<String, Instrument>();
            foreach (KeyValuePair<String, String> itva in InstrumentsToVISA_Addresses) d.Add(itva.Key, new Instrument(itva.Key, itva.Value));
            return d;
        }
    }
}
