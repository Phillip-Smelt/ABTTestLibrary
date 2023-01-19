using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using TestLibrary.TestSupport;

namespace TestLibrary.Config {
    public class TestElement : ConfigurationElement {
        [ConfigurationProperty("ID", IsKey = true, IsRequired = true)] public String ID { get { return ((String)base["ID"]).Trim(); } }
        [ConfigurationProperty("Description", IsKey = false, IsRequired = true)] public String Description { get { return ((String)base["Description"]).Trim(); } }
        [ConfigurationProperty("Revision", IsKey = false, IsRequired = true)] public String Revision { get { return ((String)base["Revision"]).Trim(); } }
        [ConfigurationProperty("ClassName", IsKey = false, IsRequired = true)] public String ClassName { get { return ((String)base["ClassName"]).Trim(); } }
        [ConfigurationProperty("Arguments", IsKey = false, IsRequired = true)] public String Arguments { get { return ((String)base["Arguments"]).Trim(); } }
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

    public abstract class TestAbstract {
        public const String ClassName = nameof(TestAbstract);
        private protected TestAbstract() { }

        public static Dictionary<String, String> SplitArguments(String Arguments) {
            String[] args = Arguments.Split(Test.SPLIT_ARGUMENTS_CHAR);
            String[] kvp;
            Dictionary<String, String> argDictionary = new Dictionary<String, String>();
            for (int i = 0; i < args.Length; i++) {
                kvp = args[i].Split('=');
                argDictionary.Add(kvp[0].Trim(), kvp[1].Trim());
            }
            return argDictionary;
        }
    }

    public class TestCustomizable : TestAbstract {
        public new const String ClassName = nameof(TestCustomizable);
        public Dictionary<String, String> Arguments;

        public TestCustomizable(String ID, String Arguments) {
            this.Arguments = TestAbstract.SplitArguments(Arguments);
            if (this.Arguments.Count == 0) throw new ArgumentException($"TestElement ID '{ID}' with ClassName '{ClassName}' requires 1 or more key=value arguments:{Environment.NewLine}" +
                    $"   Example: 'NameFirst=Harry|" +
                    $"             NameLast=Potter|" +
                    $"             Occupation=Wizard'{Environment.NewLine}" +
                    $"   Actual : '{Arguments}'");
        }
    }

    public class TestISP : TestAbstract {
        public new const String ClassName = nameof(TestISP);
        public String ISPExecutableFolder;
        public String ISPExecutable;
        public String ISPExecutableArguments;
        public String ISPResult;

        public TestISP(String ID, String Arguments) {
            Dictionary<String, String> argsDict = TestAbstract.SplitArguments(Arguments);
            if (argsDict.Count != 4) throw new ArgumentException($"TestElement ID '{ID}' with ClassName '{ClassName}' requires 4 case-sensitive arguments:{Environment.NewLine}" +
                $@"   Example: 'ISPExecutable=ipecmd.exe|
                                ISPExecutableFolder=C:\Program Files\Microchip\MPLABX\v6.05\mplab_platform\mplab_ipe|
                                ISPExecutableArguments=C:\TBD\U1_Firmware.hex|
                                ISPResult=0xAC0E'{Environment.NewLine}" +
                $"   Actual : '{Arguments}'");
            if (!argsDict.ContainsKey("ISPExecutableFolder")) throw new ArgumentException($"TestElement ID '{ID}' does not contain 'ISPExecutableFolder' key-value pair.");
            if (!argsDict.ContainsKey("ISPExecutable")) throw new ArgumentException($"TestElement ID '{ID}' does not contain 'ISPExecutable' key-value pair.");
            if (!argsDict.ContainsKey("ISPExecutableArguments")) throw new ArgumentException($"TestElement ID '{ID}' does not contain 'ISPExecutableArguments' key-value pair.");
            if (!argsDict.ContainsKey("ISPResult")) throw new ArgumentException($"TestElement ID '{ID}' does not contain 'ISPResult' key-value pair.");
            if (!argsDict["ISPExecutableFolder"].EndsWith(@"\")) argsDict["ISPExecutableFolder"] += @"\";
            if (!Directory.Exists(argsDict["ISPExecutableFolder"])) throw new ArgumentException($"TestElement ID '{ID}' ISPExecutableFolder '{argsDict["ISPExecutableFolder"]}' does not exist.");
            if (!File.Exists(argsDict["ISPExecutableFolder"] + argsDict["ISPExecutable"])) throw new ArgumentException($"TestElement ID '{ID}' ISPExecutable '{argsDict["ISPExecutableFolder"] + argsDict["ISPExecutable"]}' does not exist.");

            this.ISPExecutableFolder = argsDict["ISPExecutableFolder"];
            this.ISPExecutable = argsDict["ISPExecutable"];
            this.ISPExecutableArguments = argsDict["ISPExecutableArguments"];
            this.ISPResult = argsDict["ISPResult"];
        }
    }

    public class TestNumerical : TestAbstract {
        public new const String ClassName = nameof(TestNumerical);
        public Double Low { get; private set; }
        public Double High { get; private set; }
        public String Unit { get; private set; }
        public String UnitType { get; private set; }

        public TestNumerical(String ID, String Arguments) {
            Dictionary<String, String> argsDict = TestAbstract.SplitArguments(Arguments);
            if (argsDict.Count != 4) throw new ArgumentException($"TestElement ID '{ID}' with ClassName '{ClassName}' requires 4 case-sensitive arguments:{Environment.NewLine}" +
                $"   Example: 'Low=0.002|" +
                $"             High=0.004|" +
                $"             Unit=A|" +
                $"             UnitType=DC'{Environment.NewLine}" +
                $"   Actual : '{Arguments}'");
            if (!argsDict.ContainsKey("Low")) throw new ArgumentException($"TestElement ID '{ID}' does not contain 'Low' key-value pair.");
            if (!argsDict.ContainsKey("High")) throw new ArgumentException($"TestElement ID '{ID}' does not contain 'High' key-value pair.");
            if (!argsDict.ContainsKey("Unit")) throw new ArgumentException($"TestElement ID '{ID}' does not contain 'Unit' key-value pair.");
            if (!argsDict.ContainsKey("UnitType")) throw new ArgumentException($"TestElement ID '{ID}' does not contain 'UnitType' key-value pair.");

            if (Double.TryParse(argsDict["Low"], NumberStyles.Float, CultureInfo.CurrentCulture, out Double low)) this.Low = low;
            else throw new ArgumentException($"TestElement ID '{ID}' Low '{argsDict["Low"]}' ≠ System.Double.");

            if (Double.TryParse(argsDict["High"], NumberStyles.Float, CultureInfo.CurrentCulture, out Double high)) this.High = high;
            else throw new ArgumentException($"TestElement ID '{ID}' High '{argsDict["High"]}' ≠ System.Double.");

            if (low > high) throw new ArgumentException($"TestElement ID '{ID}' Low '{low}' > High '{high}'.");
            this.Unit = argsDict["Unit"];
            this.UnitType = argsDict["UnitType"];
        }
    }

    public class TestTextual : TestAbstract {
        public new const String ClassName = nameof(TestTextual);
        public String Text { get; private set; }
        public TestTextual(String ID, String Arguments) {
            Dictionary<String, String> argsDict = TestAbstract.SplitArguments(Arguments);
            if (argsDict.Count != 1) throw new ArgumentException($"TestElement ID '{ID}' with ClassName '{ClassName}' requires 1 case-sensitive argument:{Environment.NewLine}" +
                    $"   Example: 'Text=The quick brown fox jumps over the lazy dog.'{Environment.NewLine}" +
                    $"   Actual : '{Arguments}'");
            if (!argsDict.ContainsKey("Text")) throw new ArgumentException($"TestElement ID '{ID}' does not contain 'Text' key-value pair.");
            this.Text = argsDict["Text"];
        }
    }

    public class Test {
        internal const Char SPLIT_ARGUMENTS_CHAR = '|';
        public String ID { get; private set; }
        public String Description { get; private set; }
        public String Revision { get; private set; }
        public String ClassName { get; private set; }
        public object ClassObject { get; private set; }
        public String Measurement { get; set; }
        public String Result { get; set; }

        private Test(String ID, String Description, String Revision, String ClassName, String Arguments) {
            this.ID = ID;
            this.Description = Description;
            this.Revision = Revision;
            this.ClassName = ClassName;
            this.Measurement = String.Empty; // Measured during test execution
            this.Result = EventCodes.UNSET;  // Determined during test execution
            this.ClassObject = Activator.CreateInstance(Type.GetType(GetType().Namespace + "." + this.ClassName), new Object[] { this.ID, Arguments });
        }

        public static Dictionary<String, Test> Get() {
            TestElementsSection s = (TestElementsSection)ConfigurationManager.GetSection("TestElementsSection");
            TestElements e = s.TestElements;
            Dictionary<String, Test> d = new Dictionary<String, Test>();
            foreach (TestElement te in e) d.Add(te.ID, new Test(te.ID, te.Description, te.Revision, te.ClassName, te.Arguments));
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
            String sTrim;
            foreach (String s in g) {
                sTrim = s.Trim();
                if (!tests.ContainsKey(sTrim)) throw new InvalidOperationException($"Group '{Group.ID}' includes IDTest '{sTrim}', which isn't present in TestElements in App.config.");
                this.Tests.Add(sTrim, tests[sTrim]);
                // Add only Tests correlated to the Group previously selected by operator.
            }
        }

        public static ConfigTest Get() {
            return new ConfigTest();
        }
    }
}
