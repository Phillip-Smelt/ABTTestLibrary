using System;
using System.Collections.Generic;
using System.Configuration;
using Agilent.CommandExpert.ScpiNet.Ag33500B_33600A_2_09;
using Agilent.CommandExpert.ScpiNet.Ag3466x_2_08;
using Agilent.CommandExpert.ScpiNet.AgE3610XB_1_0_0_1_00;
using Agilent.CommandExpert.ScpiNet.AgE36200_1_0_0_1_0_2_1_00;
using Agilent.CommandExpert.ScpiNet.AgEL30000_1_2_5_1_0_6_17_114;
using Agilent.CommandExpert.ScpiNet.AgSCPI99_1_0;
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
        public String ID { get; private set; }
        public String Description { get; private set; }
        public String Address { get; private set; }
        public SCPI_VISA_CATEGORIES Category { get; private set; }
        public object Instance { get; private set; }

        private Instrument(String id, String description, String address) {
            this.ID = id;
            this.Description = description;
            this.Address= address;

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
                        Logger.UnexpectedErrorHandler(SCPI99.GetMessage(this, $"Unrecognized SCPI VISA Instrument!  Update Class TestLibrary.SCPI_VISA.Instrument, adding '{instrumentModel}'"));
                        break;
                }
            } catch (Exception e) {
                String[] a = address.Split(':');
                throw new InvalidOperationException(SCPI99.GetMessage(this, $"Check to see if SCPI VISA Instrument is powered and it's {a[0]} bus is communicating."), e);
            }
        }

        public static Dictionary<SCPI_VISA_IDs, Instrument> Get() {
            Dictionary<SCPI_VISA_IDs, (String id, String description, String address)> visaInstrumentElements = GetVISA_InstrumentElements();
            Dictionary<SCPI_VISA_IDs, Instrument> instruments = new Dictionary<SCPI_VISA_IDs, Instrument>();
            foreach (KeyValuePair<SCPI_VISA_IDs, (String id, String description, String address)> kvp in visaInstrumentElements) instruments.Add(kvp.Key, new Instrument(kvp.Value.id ,kvp.Value.description, kvp.Value.address));
            return instruments;
        }

        private static Dictionary<SCPI_VISA_IDs, (String id, String description, String address)> GetVISA_InstrumentElements() {
            SCPI_VISA_InstrumentsSection viSection = (SCPI_VISA_InstrumentsSection)ConfigurationManager.GetSection("SCPI_VISA_InstrumentsSection");
            SCPI_VISA_InstrumentElements viElements = viSection.SCPI_VISA_InstrumentElements;
            Dictionary<SCPI_VISA_IDs, (String id, String description, String address)> visaInstrumentElements = new Dictionary<SCPI_VISA_IDs, (String id, String description, String address)> ();
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
