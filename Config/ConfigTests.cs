using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using Agilent.CommandExpert.ScpiNet.Ag33500B_33600A_2_09.SCPI.MMEMory.CDIRectory;
using TestLibrary.TestSupport;

namespace TestLibrary.Config {
    public class TestElement : ConfigurationElement {
        [ConfigurationProperty("ID", IsKey = true, IsRequired = true)] public String ID { get { return (String)base["ID"]; } }
        [ConfigurationProperty("Description", IsKey = false, IsRequired = true)] public String Description { get { return (String)base["Description"]; } }
        [ConfigurationProperty("Type", IsKey = false, IsRequired = true)] public String Type { get { return (String)base["Type"]; } }
        [ConfigurationProperty("Revision", IsKey = false, IsRequired = true)] public String Revision { get { return (String)base["Revision"]; } }
        [ConfigurationProperty("Arguments", IsKey = false, IsRequired = true)] public String Arguments { get { return (String)base["Arguments"]; } }
    }

    [ConfigurationCollection(typeof(TestElement))]
    public class TestElements : ConfigurationElementCollection {
        public const String PropertyName = "TestElement";
        public TestElement this[Int32 idx] { get { return (TestElement)BaseGet(idx); } }
        public override ConfigurationElementCollectionType CollectionType { get { return ConfigurationElementCollectionType.BasicMapAlternate; } }
        protected override String ElementName { get { return PropertyName; } }
        protected override bool IsElementName(String elementName) { return elementName.Equals(PropertyName, StringComparison.InvariantCultureIgnoreCase); }
        public override bool IsReadOnly() { return false; }
        protected override ConfigurationElement CreateNewElement() { return new TestElement(); }
        protected override object GetElementKey(ConfigurationElement element) { return ((TestElement)(element)).ID; }
    }

    public class TestElementsSection : ConfigurationSection {
        [ConfigurationProperty("TestElements")] public TestElements TestElements { get { return ((TestElements)(base["TestElements"])); } }
    }

    public class ConfigTests {
        public TestElementsSection TestElementsSection { get { return (TestElementsSection)ConfigurationManager.GetSection("TestElementsSection"); } }
        public TestElements TestElements { get { return this.TestElementsSection.TestElements; } }
        public IEnumerable<TestElement> TestElement { get { foreach (TestElement te in this.TestElements) if (te != null) yield return te; } }
    }

    internal abstract class TestAbstract {
        private protected const String Type = "Abstract";
        private protected TestAbstract() { }

        internal static Dictionary<String, String> SplitArguments(String Arguments) {
            String[] args = Arguments.Split(Test.SPLIT_ARGUMENTS_CHAR);
            String[] kvp;
            Dictionary<String, String> argDictionary = new Dictionary<String, String>();
            for (int i = 0; i < args.Length; i++) {
                kvp = args[i].Split('=');
                argDictionary.Add(kvp[0], kvp[1]);
            }
            return argDictionary;
        }
    }

    internal class TestCustomized : TestAbstract {
        internal new const String Type = "Custom";
        internal Dictionary<String, String> Arguments;

        internal TestCustomized(String ID, String Arguments) : base() {
            this.Arguments = TestAbstract.SplitArguments(Arguments);
            if (this.Arguments.Count == 0) throw new ArgumentException($"TestElement ID '{ID}' with Type '{Type}' requires 1 or more internally formatted arguments:{Environment.NewLine}" +
                    $"   Example: 'Key1=Value1|Key2=Value2|Key3=Value3'{Environment.NewLine}" +
                    $"   Actual : '{Arguments}'");
        }
    }

    internal class TestProgrammed : TestAbstract {
        internal new const String Type = "Program";
        internal String AppFolder;
        internal String AppFile;
        internal String AppArguments;
        internal String FirmwareFolder;
        internal String FirmwareFile;
        internal String FirmwareCRC;

        internal TestProgrammed(String ID, String Arguments) : base() {
            Dictionary<String, String> argsDict = TestAbstract.SplitArguments(Arguments);
            if (argsDict.Count != 5) throw new ArgumentException($"TestElement ID '{ID}' with Type '{Type}' requires 5 internally formatted arguments:{Environment.NewLine}" +
                $@"   Example: 'AppFile=ipecmd.exe|AppFolder=C:\Program Files\Microchip\MPLABX\v6.05\mplab_platform\mplab_ipe|AppArguments=|FirmwareFile=U1_Firmware.hex|FirmwareCRC=0xAC0E'{Environment.NewLine}" +
                $"   Actual : '{Arguments}'");
            if (!argsDict.ContainsKey("AppFolder")) throw new ArgumentException($"TestElement ID '{ID}' does not contain 'AppFolder' key-value pair.");
            if (!argsDict.ContainsKey("AppFile")) throw new ArgumentException($"TestElement ID '{ID}' does not contain 'AppFile' key-value pair.");
            if (!argsDict.ContainsKey("AppArguments")) throw new ArgumentException($"TestElement ID '{ID}' does not contain 'AppArguments' key-value pair.");
            if (!argsDict.ContainsKey("FirmwareFolder")) throw new ArgumentException($"TestElement ID '{ID}' does not contain 'FirmwareFolder' key-value pair.");
            if (!argsDict.ContainsKey("FirmwareFile")) throw new ArgumentException($"TestElement ID '{ID}' does not contain 'FirmwareFile' key-value pair.");
            if (!argsDict.ContainsKey("FirmwareCRC")) throw new ArgumentException($"TestElement ID '{ID}' does not contain 'FirmwareCRC' key-value pair.");

            if (!argsDict["AppFolder"].EndsWith(@"\")) argsDict["AppFolder"] += @"\";
            if (!argsDict["FirmwareFolder"].EndsWith(@"\")) argsDict["FirmwareFolder"] += @"\";

            if (!File.Exists(argsDict["AppFolder"])) throw new ArgumentException($"TestElement ID '{ID}' AppFolder '{argsDict["AppFolder"]}' does not exist.");
            if (!File.Exists(argsDict["AppFolder"] + argsDict["AppFile"])) throw new ArgumentException($"TestElement ID '{ID}' AppFile '{argsDict["AppFolder"] + argsDict["AppFile"]}' does not exist.");

            if (!File.Exists(argsDict["FirmwareFolder"])) throw new ArgumentException($"TestElement ID '{ID}' FirmwareFolder '{argsDict["FirmwareFolder"]}' does not exist.");
            if (!File.Exists(argsDict["FirmwareFolder"] + argsDict["FirmwareFile"])) throw new ArgumentException($"TestElement ID '{ID}' FirmwareFile '{argsDict["FirmwareFolder"] + argsDict["FirmwareFile"]}' does not exist.");

            if (String.Equals(argsDict["FirmwareCRC"], String.Empty)) throw new ArgumentException($"TestElement ID '{ID}' CRC '{argsDict["FirmwareCRC"]}' = String.Empty.");
            this.AppFolder = argsDict["AppFolder"];
            this.AppFile = argsDict["AppFile"];
            this.AppArguments = argsDict["AppArguments"];
            this.FirmwareFile = argsDict["FirmwareFile"];
            this.FirmwareFolder = argsDict["FirmwareFolder"];
            this.FirmwareCRC = argsDict["FirmwareCRC"];
        }
    }

    internal class TestRanged : TestAbstract {
        internal new const String Type = "Range";
        internal Double Low { get; private set; }
        internal Double High { get; private set; }
        internal String Unit { get; private set; }
        internal String UnitType { get; private set; }

        internal TestRanged(String ID, String Arguments) : base() {
            Dictionary<String, String> argsDict = TestAbstract.SplitArguments(Arguments);
            if (argsDict.Count != 4) throw new ArgumentException($"TestElement ID '{ID}' with Type '{Type}' requires 4 internally formatted arguments:{Environment.NewLine}" +
                $"   Example: 'Low=0.002|High=0.004|Unit=A|UnitType=DC'{Environment.NewLine}" +
                $"   Actual : '{Arguments}'");
            if (!argsDict.ContainsKey("Low")) throw new ArgumentException($"TestElement ID '{ID}' does not contain 'Low' key-value pair.");
            if (!argsDict.ContainsKey("High")) throw new ArgumentException($"TestElement ID '{ID}' does not contain 'High' key-value pair.");
            if (!argsDict.ContainsKey("Unit")) throw new ArgumentException($"TestElement ID '{ID}' does not contain 'Unit' key-value pair.");
            if (!argsDict.ContainsKey("UnitType")) throw new ArgumentException($"TestElement ID '{ID}' does not contain 'UnitType' key-value pair.");

            if (String.Equals(argsDict["Low"], String.Empty) && String.Equals(argsDict["High"], String.Empty)) throw new ArgumentException($"TestElement ID '{ID}' 'Low' = 'High' = String.Empty.");

            Double low, high;
            if (String.Equals(argsDict["Low"], String.Empty)) {
                this.Low = Double.NegativeInfinity;
                if (!TryDouble(argsDict["High"], out high)) throw new ArgumentException($"TestElement ID '{ID}' High '{argsDict["High"]}' ≠ System.Double.");
                else this.High = high;
            }

            if (String.Equals(argsDict["High"], String.Empty)) {
                this.High = Double.PositiveInfinity;
                if (!TryDouble(argsDict["Low"], out low)) throw new ArgumentException($"TestElement ID '{ID}' Low '{argsDict["Low"]}' ≠ System.Double.");
                else this.Low = low;
            }

            if (TryDouble(argsDict["Low"], out low) && TryDouble(argsDict["High"], out high)) {
                this.Low = low;
                this.High = High;
            } else throw new ArgumentException($"TestElement ID '{ID}' Low '{argsDict["Low"]}' or High '{argsDict["High"]}' ≠ System.Double.");

            if (low > high) throw new ArgumentException($"TestElement ID '{ID}' Low '{low}' > High '{high}'.");

            this.Unit = argsDict["Unit"];
            this.UnitType = argsDict["UnitType"];
        }

        private static Boolean TryDouble(String s, out Double d) {
            return Double.TryParse(s, NumberStyles.Float, CultureInfo.CurrentCulture, out d);
            // Convenience wrapper method to add NumberStyles.Float & CultureInfo.CurrentCulture to Double.TryParse().
            // NumberStyles.Float best for parsing floating point decimal values, including scientific/exponential notation.
        }
    }

    public class Test {
        internal const Char SPLIT_ARGUMENTS_CHAR = '|';
        public String ID { get; private set; }
        public String Description { get; private set; }
        public String Type { get; private set; }
        public String Revision { get; private set; }
        public object TestClass { get; private set; }
        public String Measurement { get; set; }
        public String Result { get; set; }

        private Test(String ID, String Description, String Type, String Revision, String Arguments) {
            this.ID = ID;
            this.Description = Description;
            this.Type = Type;
            this.Revision = Revision;
            this.Measurement = String.Empty; // Measured during test execution
            this.Result = EventCodes.UNSET;  // Determined during test execution
            switch (this.Type) {
                case TestCustomized.Type:
                    this.TestClass = new TestCustomized(this.ID, Arguments);
                    break;
                case TestProgrammed.Type:
                    this.TestClass = new TestProgrammed(this.ID, Arguments);
                    break;
                case TestRanged.Type:
                    this.TestClass = new TestRanged(this.ID, Arguments);
                    break;
                default:
                    throw new NotImplementedException($"TestElement ID '{ID}' with Type '{Type}' not implemented.");
            }
        }

        public static Dictionary<String, Test> Get() {
            TestElementsSection s = (TestElementsSection)ConfigurationManager.GetSection("TestElementsSection");
            TestElements e = s.TestElements;
            Dictionary<String, Test> d = new Dictionary<String, Test>();
            foreach (TestElement te in e) d.Add(te.ID, new Test(te.ID, te.Description, te.Type, te.Revision, te.Arguments));
            return d;
        }
    }

    public class ConfigTest {
        public Group Group { get; private set; }
        public Dictionary<String, Test> Tests { get; private set; }

        private ConfigTest() {
            Dictionary<String, Group> Groups = Group.Get();
            String GroupSelected = GroupSelect.Get(Groups);
            this.Group = Groups[GroupSelected];
            // Operator selects the Group they want to test, from the Dictionary of all Groups.
            // GroupSelected is Dictionary Groups' Key.

            Dictionary<String, Test> tests = Test.Get();
            this.Tests = new Dictionary<String, Test>();
            String[] g = this.Group.TestIDs.Split(Test.SPLIT_ARGUMENTS_CHAR);
            foreach (String s in g) {
                if (!tests.ContainsKey(s)) throw new InvalidOperationException($"Group '{Group.ID}' includes IDTest '{s}', which isn't present in TestElements in App.config.");
                this.Tests.Add(s, tests[s]);
                // Add only Tests correlated to the Group previously selected by operator.
            }
        }

        public static ConfigTest Get() {
            return new ConfigTest();
        }
    }
}
