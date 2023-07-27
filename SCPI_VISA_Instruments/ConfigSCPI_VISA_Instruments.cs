using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Agilent.CommandExpert.ScpiNet.Ag33500B_33600A_2_09;
using Agilent.CommandExpert.ScpiNet.Ag3466x_2_08;
using Agilent.CommandExpert.ScpiNet.AgE3610XB_1_0_0_1_00;
using Agilent.CommandExpert.ScpiNet.AgE36200_1_0_0_1_0_2_1_00;
using Agilent.CommandExpert.ScpiNet.AgEL30000_1_2_5_1_0_6_17_114;
using Agilent.CommandExpert.ScpiNet.AgSCPI99_1_0;
using ABT.TestSpace.TestExec.Logging;

namespace ABT.TestSpace.TestExec.SCPI_VISA_Instruments {
    // NOTE: https://forums.ni.com/t5/Instrument-Control-GPIB-Serial/IVI-Drivers-Pros-and-Cons/td-p/4165671.
    public enum SCPI_IDENTITY { Manufacturer, Model, SerialNumber, FirmwareRevision }
    // Example: "Keysight Technologies,E36103B,MY61001983,1.0.2-1.02".

    public class SCPI_VISA_Instrument {
        public readonly Alias ID;
        public readonly String Description;
        public readonly String Address;
        public readonly String Identity;
        public readonly Object Instrument; // NOTE: The assumption, thus far proven correct, is that Keysight's SCPI drivers don't contain state, thus can be readonly.

        private SCPI_VISA_Instrument(Alias id, String description, String address) {
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
                        Logger.LogError(SCPI.GetErrorMessage(this, $"Unrecognized SCPI VISA Instrument.  Functionality limited to SCPI99 commands only."));
                        break;
                }
            } catch (Exception e) {
                throw new InvalidOperationException(SCPI.GetErrorMessage(this, "Check to see if SCPI VISA Instrument is powered and its interface communicating."), e);
            }
        }

        public static Dictionary<Alias, SCPI_VISA_Instrument> Get() {
            IEnumerable<SCPI_VISA_Instrument> svis =
                from svi in XElement.Load("TestExecutive.config.xml").Elements("SCPI_VISA_Instrument")
                select new SCPI_VISA_Instrument(new Alias(svi.Element("ID").Value), svi.Element("Description").Value, svi.Element("Address").Value);
            Dictionary<Alias, SCPI_VISA_Instrument> SVIs = new Dictionary<Alias, SCPI_VISA_Instrument>();
            foreach (SCPI_VISA_Instrument svi in svis) SVIs.Add(new Alias(svi.ID.ToString()), svi);
            return SVIs;
        }

        public static String GetInfo(SCPI_VISA_Instrument SVI, String optionalHeader = "") {
            String info = (optionalHeader == "") ? optionalHeader : optionalHeader += Environment.NewLine;
            foreach (PropertyInfo pi in SVI.GetType().GetProperties()) info += $"{pi.Name.PadLeft(Logger.SPACES_21.Length)}: '{pi.GetValue(SVI)}'{Environment.NewLine}";
            return info;
        }

        public class Alias {
            public readonly String ID;

            public Alias(String name) { this.ID = name; }

            public override Boolean Equals(Object obj) {
                Alias a = obj as Alias;
                if (ReferenceEquals(this, a)) return true;
                return a != null && this.ID == a.ID;
            }

            public override Int32 GetHashCode() { return 3 * this.ID.GetHashCode(); }

            public override string ToString() { return this.ID; }
        }
    }
}
