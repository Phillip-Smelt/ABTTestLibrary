using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;

namespace ABT.TestSpace.AppConfig {
    public enum SI_UNITS { amperes, celcius, farads, henries, hertz, NotApplicable, ohms, seconds, siemens, volt_amperes, volts, watts }
    public enum SI_UNITS_MODIFIERS { AC, DC, Peak, PP, NotApplicable, RMS }

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
        public TestElement this[Int32 idx] { get { return (TestElement)this.BaseGet(idx); } }
        public override ConfigurationElementCollectionType CollectionType { get { return ConfigurationElementCollectionType.BasicMapAlternate; } }
        protected override String ElementName { get { return PropertyName; } }
        protected override Boolean IsElementName(String elementName) { return elementName.Equals(PropertyName, StringComparison.InvariantCultureIgnoreCase); }
        public override Boolean IsReadOnly() { return false; }
        protected override ConfigurationElement CreateNewElement() { return new TestElement(); }
        protected override Object GetElementKey(ConfigurationElement element) { return ((TestElement)(element)).ID; }
    }

    public class TestsSection : ConfigurationSection {
        [ConfigurationProperty("TestElements")] public TestElements TestElements { get { return ((TestElements)(base["TestElements"])); } }
    }

    public class ConfigTests {
        public TestsSection TestsSection { get { return (TestsSection)ConfigurationManager.GetSection("TestsSection"); } }
        public TestElements TestElements { get { return this.TestsSection.TestElements; } }
        public IEnumerable<TestElement> TestElement { get { foreach (TestElement te in this.TestElements) if (te != null) yield return te; } }
    }

    public abstract class TestAbstract {
        public const String ClassName = nameof(TestAbstract);
        private protected TestAbstract() { }

        public static Dictionary<String, String> SplitArguments(String arguments) {
            String[] args = arguments.Split(Test.SPLIT_ARGUMENTS_CHAR);
            String[] kvp;
            Dictionary<String, String> argDictionary = new Dictionary<String, String>();
            for (Int32 i = 0; i < args.Length; i++) {
                kvp = args[i].Split('=');
                argDictionary.Add(kvp[0].Trim(), kvp[1].Trim());
            }
            return argDictionary;
        }
    }

    public class TestCustomizable : TestAbstract {
        public new const String ClassName = nameof(TestCustomizable);
        public readonly Dictionary<String, String> Arguments;

        public TestCustomizable(String id, String arguments) {
            this.Arguments = TestAbstract.SplitArguments(arguments);
            if (this.Arguments.Count == 0) throw new ArgumentException($"TestElement ID '{id}' with ClassName '{ClassName}' requires 1 or more key=value arguments:{Environment.NewLine}" +
                    $"   Example: 'NameFirst=Harry|" +
                    $"             NameLast=Potter|" +
                    $"             Occupation=Auror'{Environment.NewLine}" +
                    $"   Actual : '{this.Arguments}'");
        }
    }

    public class TestISP : TestAbstract {
        public new const String ClassName = nameof(TestISP);
        public readonly String ISPExecutableFolder;
        public readonly String ISPExecutable;
        public readonly String ISPExecutableArguments;
        public readonly String ISPExpected;

        public TestISP(String id, String arguments) {
            Dictionary<String, String> argsDict = TestAbstract.SplitArguments(arguments);
            if (argsDict.Count != 4) throw new ArgumentException($"TestElement ID '{id}' with ClassName '{ClassName}' requires 4 case-sensitive arguments:{Environment.NewLine}" +
                $@"   Example: 'ISPExecutable=ipecmd.exe|
                                ISPExecutableFolder=C:\Program Files\Microchip\MPLABX\v6.05\mplab_platform\mplab_ipe|
                                ISPExecutableArguments=C:\TBD\U1_Firmware.hex|
                                ISPExpected=0xAC0E'{Environment.NewLine}" +
                $"   Actual : '{arguments}'");
            if (!argsDict.ContainsKey("ISPExecutableFolder")) throw new ArgumentException($"TestElement ID '{id}' does not contain 'ISPExecutableFolder' key-value pair.");
            if (!argsDict.ContainsKey("ISPExecutable")) throw new ArgumentException($"TestElement ID '{id}' does not contain 'ISPExecutable' key-value pair.");
            if (!argsDict.ContainsKey("ISPExecutableArguments")) throw new ArgumentException($"TestElement ID '{id}' does not contain 'ISPExecutableArguments' key-value pair.");
            if (!argsDict.ContainsKey("ISPExpected")) throw new ArgumentException($"TestElement ID '{id}' does not contain 'ISPExpected' key-value pair.");
            if (!argsDict["ISPExecutableFolder"].EndsWith(@"\")) argsDict["ISPExecutableFolder"] += @"\";
            if (!Directory.Exists(argsDict["ISPExecutableFolder"])) throw new ArgumentException($"TestElement ID '{id}' ISPExecutableFolder '{argsDict["ISPExecutableFolder"]}' does not exist.");
            if (!File.Exists(argsDict["ISPExecutableFolder"] + argsDict["ISPExecutable"])) throw new ArgumentException($"TestElement ID '{id}' ISPExecutable '{argsDict["ISPExecutableFolder"] + argsDict["ISPExecutable"]}' does not exist.");

            this.ISPExecutableFolder = argsDict["ISPExecutableFolder"];
            this.ISPExecutable = argsDict["ISPExecutable"];
            this.ISPExecutableArguments = argsDict["ISPExecutableArguments"];
            this.ISPExpected = argsDict["ISPExpected"];
        }
    }

    public class TestNumerical : TestAbstract {
        public new const String ClassName = nameof(TestNumerical);
        public readonly Double High;
        public readonly Double Low;
        public readonly SI_UNITS SI_Units = SI_UNITS.NotApplicable;
        public readonly SI_UNITS_MODIFIERS SI_Units_Modifier = SI_UNITS_MODIFIERS.NotApplicable;

        public TestNumerical(String id, String arguments) {
            Dictionary<String, String> argsDict = TestAbstract.SplitArguments(arguments);
            if (argsDict.Count != 4) throw new ArgumentException($"TestElement ID '{id}' with ClassName '{ClassName}' requires 4 case-sensitive arguments:{Environment.NewLine}" +
                $"   Example: 'High=0.004|" +
                $"             Low=0.002|" +
                $"             SI_Units=volts|" +
                $"             SI_Units_Modifier=DC'{Environment.NewLine}" +
                $"   Actual : '{arguments}'");
            if (!argsDict.ContainsKey("High")) throw new ArgumentException($"TestElement ID '{id}' does not contain 'High' key-value pair.");
            if (!argsDict.ContainsKey("Low")) throw new ArgumentException($"TestElement ID '{id}' does not contain 'Low' key-value pair.");
            if (!argsDict.ContainsKey("SI_Units")) throw new ArgumentException($"TestElement ID '{id}' does not contain 'SI_Units' key-value pair.");
            if (!argsDict.ContainsKey("SI_Units_Modifier")) throw new ArgumentException($"TestElement ID '{id}' does not contain 'SI_Units_Modifier' key-value pair.");

            if (Double.TryParse(argsDict["High"], NumberStyles.Float, CultureInfo.CurrentCulture, out Double high)) this.High = high;
            else throw new ArgumentException($"TestElement ID '{id}' High '{argsDict["High"]}' ≠ System.Double.");

            if (Double.TryParse(argsDict["Low"], NumberStyles.Float, CultureInfo.CurrentCulture, out Double low)) this.Low = low;
            else throw new ArgumentException($"TestElement ID '{id}' Low '{argsDict["Low"]}' ≠ System.Double.");

            if (low > high) throw new ArgumentException($"TestElement ID '{id}' Low '{low}' > High '{high}'.");

            String[] si_units = Enum.GetNames(typeof(SI_UNITS)).Select(s => s.ToLower()).ToArray();
            if (si_units.Any(argsDict["SI_Units"].ToLower().Contains)) {
                this.SI_Units = (SI_UNITS)Enum.Parse(typeof(SI_UNITS), argsDict["SI_Units"], ignoreCase: true);
                String[] si_units_modifiers = Enum.GetNames(typeof(SI_UNITS_MODIFIERS)).Select(s => s.ToLower()).ToArray();
                if (si_units_modifiers.Any(argsDict["SI_Units_Modifier"].ToLower().Contains)) {
                    this.SI_Units_Modifier = (SI_UNITS_MODIFIERS)Enum.Parse(typeof(SI_UNITS_MODIFIERS), argsDict["SI_Units_Modifier"], ignoreCase: true);
                }
            }
        }
    }

    public class TestTextual : TestAbstract {
        public new const String ClassName = nameof(TestTextual);
        public readonly String Text;

        public TestTextual(String id, String arguments) {
            Dictionary<String, String> argsDict = TestAbstract.SplitArguments(arguments);
            if (argsDict.Count != 1) throw new ArgumentException($"TestElement ID '{id}' with ClassName '{ClassName}' requires 1 case-sensitive argument:{Environment.NewLine}" +
                    $"   Example: 'Text=The quick brown fox jumps over the lazy dog.'{Environment.NewLine}" +
                    $"   Actual : '{arguments}'");
            if (!argsDict.ContainsKey("Text")) throw new ArgumentException($"TestElement ID '{id}' does not contain 'Text' key-value pair.");
            this.Text = argsDict["Text"];
        }
    }

    public class Test {
        internal const Char SPLIT_ARGUMENTS_CHAR = '|';
        public readonly String ID;
        public readonly String Description;
        public readonly String Revision;
        public readonly String ClassName;
        public readonly Object ClassObject;
        public String Measurement { get; set; } = String.Empty; // Determined during test.
        public String Result { get; set; } = EventCodes.UNSET; // Determined post-test.

        private Test(String id, String description, String revision, String className, String arguments) {
            this.ID = id;
            this.Description = description;
            this.Revision = revision;
            this.ClassName = className;
            this.ClassObject = Activator.CreateInstance(Type.GetType(this.GetType().Namespace + "." + this.ClassName), new Object[] { this.ID, arguments });
            if (String.Equals(this.ClassName, TestNumerical.ClassName)) this.Measurement = Double.NaN.ToString();
        }

        public static Dictionary<String, Test> Get() {
            TestsSection testElementsSection = (TestsSection)ConfigurationManager.GetSection("TestsSection");
            TestElements testElements = testElementsSection.TestElements;
            Dictionary<String, Test> dictionary = new Dictionary<String, Test>();
            foreach (TestElement testElement in testElements) dictionary.Add(testElement.ID, new Test(testElement.ID, testElement.Description, testElement.Revision, testElement.ClassName, testElement.Arguments));
            return dictionary;
        }
    }

    public class AppConfigTest {
        public readonly Group Group;
        public readonly Dictionary<String, Test> Tests;
        public readonly Int32 LogFormattingLength;

        private AppConfigTest() {
            Dictionary<String, Group> Groups = Group.Get();
            String GroupSelected = GroupSelect.Get(Groups);
            this.Group = Groups[GroupSelected];
            // Operator selects the Group they want to test, from the Dictionary of all Groups.
            // GroupSelected is Dictionary Groups' Key.

            Dictionary<String, Test> allTests = Test.Get();
            this.Tests = new Dictionary<String, Test>();
            String[] groupTestIDs = this.Group.TestIDs.Split(Test.SPLIT_ARGUMENTS_CHAR).Select(TestID => TestID.Trim()).ToArray();
            this.LogFormattingLength = groupTestIDs.OrderByDescending(TestID => TestID.Length).First().Length + 1;
            foreach (String TestID in groupTestIDs) {
                if (!allTests.ContainsKey(TestID)) throw new InvalidOperationException($"Group '{this.Group.ID}' includes Test ID '{TestID}', which isn't present in TestElements in App.config.");
                this.Tests.Add(TestID, allTests[TestID]); // Add only Tests correlated to the Group previously selected by operator.
            }
        }
        public static AppConfigTest Get() { return new AppConfigTest(); }
    }
}
