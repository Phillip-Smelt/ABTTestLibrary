using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;

namespace ABT.TestSpace.AppConfig {
    public enum SI_UNITS { amperes, celcius, farads, henries, hertz, NotApplicable, ohms, seconds, siemens, volt_amperes, volts, watts }
    public enum SI_UNITS_MODIFIERS { AC, DC, Peak, PP, NotApplicable, RMS }

    public abstract class TestAbstract {
        public const String ClassName = nameof(TestAbstract);
        public const String NOT_APPLICABLE = "NotApplicable";
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
        public readonly Dictionary<String, String> Arguments = null;

        public TestCustomizable(String _, String arguments) { if (!String.Equals(arguments, TestAbstract.NOT_APPLICABLE)) this.Arguments = TestAbstract.SplitArguments(arguments); }
    }

    public class TestISP : TestAbstract {
        public new const String ClassName = nameof(TestISP);
        public readonly String ISPExecutableFolder;
        public readonly String ISPExecutable;
        public readonly String ISPExecutableArguments;
        public readonly String ISPExpected;

        public TestISP(String id, String arguments) {
            Dictionary<String, String> argsDict = TestAbstract.SplitArguments(arguments);
            if (argsDict.Count != 4) throw new ArgumentException($"TestISP ID '{id}' with ClassName '{ClassName}' requires 4 case-sensitive arguments:{Environment.NewLine}" +
                $@"   Example: 'ISPExecutable=ipecmd.exe|{Environment.NewLine}
                                ISPExecutableFolder=C:\Program Files\Microchip\MPLABX\v6.05\mplab_platform\mplab_ipe|{Environment.NewLine}
                                ISPExecutableArguments=C:\TBD\U1_Firmware.hex|{Environment.NewLine}
                                ISPExpected=0xAC0E'{Environment.NewLine}" +
                $"   Actual : '{arguments}'");
            if (!argsDict.ContainsKey("ISPExecutableFolder")) throw new ArgumentException($"TestISP ID '{id}' does not contain 'ISPExecutableFolder' key-value pair.");
            if (!argsDict.ContainsKey("ISPExecutable")) throw new ArgumentException($"TestISP ID '{id}' does not contain 'ISPExecutable' key-value pair.");
            if (!argsDict.ContainsKey("ISPExecutableArguments")) throw new ArgumentException($"TestISP ID '{id}' does not contain 'ISPExecutableArguments' key-value pair.");
            if (!argsDict.ContainsKey("ISPExpected")) throw new ArgumentException($"TestISP ID '{id}' does not contain 'ISPExpected' key-value pair.");
            if (!argsDict["ISPExecutableFolder"].EndsWith(@"\")) argsDict["ISPExecutableFolder"] += @"\";
            if (!Directory.Exists(argsDict["ISPExecutableFolder"])) throw new ArgumentException($"TestISP ID '{id}' ISPExecutableFolder '{argsDict["ISPExecutableFolder"]}' does not exist.");
            if (!File.Exists(argsDict["ISPExecutableFolder"] + argsDict["ISPExecutable"])) throw new ArgumentException($"TestISP ID '{id}' ISPExecutable '{argsDict["ISPExecutableFolder"] + argsDict["ISPExecutable"]}' does not exist.");

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
            if (argsDict.Count != 4) throw new ArgumentException($"TestNumerical ID '{id}' with ClassName '{ClassName}' requires 4 case-sensitive arguments:{Environment.NewLine}" +
                $"   Example: 'High=0.004|{Environment.NewLine}" +
                $"             Low=0.002|{Environment.NewLine}" +
                $"             SI_Units=volts|{Environment.NewLine}" +
                $"             SI_Units_Modifier=DC'{Environment.NewLine}" +
                $"   Actual : '{arguments}'");
            if (!argsDict.ContainsKey("High")) throw new ArgumentException($"TestNumerical ID '{id}' does not contain 'High' key-value pair.");
            if (!argsDict.ContainsKey("Low")) throw new ArgumentException($"TestNumerical ID '{id}' does not contain 'Low' key-value pair.");
            if (!argsDict.ContainsKey("SI_Units")) throw new ArgumentException($"TestNumerical ID '{id}' does not contain 'SI_Units' key-value pair.");
            if (!argsDict.ContainsKey("SI_Units_Modifier")) throw new ArgumentException($"TestNumerical ID '{id}' does not contain 'SI_Units_Modifier' key-value pair.");

            if (Double.TryParse(argsDict["High"], NumberStyles.Float, CultureInfo.CurrentCulture, out Double high)) this.High = high;
            else throw new ArgumentException($"TestNumerical ID '{id}' High '{argsDict["High"]}' ≠ System.Double.");

            if (Double.TryParse(argsDict["Low"], NumberStyles.Float, CultureInfo.CurrentCulture, out Double low)) this.Low = low;
            else throw new ArgumentException($"TestNumerical ID '{id}' Low '{argsDict["Low"]}' ≠ System.Double.");

            if (low > high) throw new ArgumentException($"TestNumerical ID '{id}' Low '{low}' > High '{high}'.");

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
            if (argsDict.Count != 1) throw new ArgumentException($"TestTextual ID '{id}' with ClassName '{ClassName}' requires 1 case-sensitive argument:{Environment.NewLine}" +
                    $"   Example: 'Text=The quick brown fox jumps over the lazy dog.'{Environment.NewLine}" +
                    $"   Actual : '{arguments}'");
            if (!argsDict.ContainsKey("Text")) throw new ArgumentException($"TestTextual ID '{id}' does not contain 'Text' key-value pair.");
            this.Text = argsDict["Text"];
        }
    }

    public class Test {
        internal const Char SPLIT_ARGUMENTS_CHAR = '|';
        public readonly String ID;
        public readonly String Description;
        public readonly String Revision;
        public readonly String ClassName;
        public readonly String Arguments;
        public String Measurement { get; set; } = String.Empty; // Determined during test.
        public String Result { get; set; } = EventCodes.UNSET; // Determined post-test.
#if DEBUG
        public String DebugMessage { get; set; } = String.Empty; // Determined during test, only available for Debug compilations.
#endif
        private Test(String id, String description, String revision, String className, String arguments) {
            this.ID = id;
            this.Description = description;
            this.Revision = revision;
            this.ClassName = className;
            this.Arguments = arguments;
            if (String.Equals(this.ClassName, TestNumerical.ClassName)) this.Measurement = Double.NaN.ToString();
            Object _ = Activator.CreateInstance(Type.GetType(this.GetType().Namespace + "." + this.ClassName), new Object[] { this.ID, arguments });
            // Create throwaway instance of className to validate its arguments; better to throw obvious Exception before testing than cryptic Exception during testing.
        }

        public static Dictionary<String, Test> Get() {
            TestMeasurementsSection testMeasurementsSection = (TestMeasurementsSection)ConfigurationManager.GetSection("TestMeasurementsSection");
            TestMeasurements testMeasurements = testMeasurementsSection.TestMeasurements;
            Dictionary<String, Test> dictionary = new Dictionary<String, Test>();
            foreach (TestMeasurement tm in testMeasurements) { dictionary.Add(tm.ID, new Test(tm.ID, tm.Description, tm.Revision, tm.ClassName, tm.Arguments)); }
            return dictionary;
        }

        public static Test Get(String TestMeasurementID) { return Get()[TestMeasurementID]; }
    }

    public class AppConfigTest {
        public readonly String TestElementID;
        public readonly Boolean IsOperation;
        public readonly String TestElementDescription;
        public readonly String TestElementRevision;
        public readonly List<String> TestMeasurementIDsSequence;
        public readonly Dictionary<String, Test> Tests;
        public readonly Int32 FormattingLengthTestGroup = 0;
        public readonly Int32 FormattingLengthTestMeasurement = 0;

        private AppConfigTest() {
            Dictionary<String, Operation> testOperations = Operation.Get();
            Dictionary<String, Group> testGroups = Group.Get();

            (this.TestElementID, this.IsOperation) = SelectTests.Get(testOperations, testGroups);

            this.TestMeasurementIDsSequence = new List<String>();
            if (this.IsOperation) {
                this.TestElementDescription = testOperations[this.TestElementID].Description;
                this.TestElementRevision = testOperations[this.TestElementID].Revision;
                List<String> testGroupIDs = testOperations[this.TestElementID].TestGroupIDs.Split(Test.SPLIT_ARGUMENTS_CHAR).Select(id => id.Trim()).ToList();
                foreach (String testGroupID in testGroupIDs) {
                    this.TestMeasurementIDsSequence.AddRange(testGroups[testGroupID].TestMeasurementIDs.Split(Test.SPLIT_ARGUMENTS_CHAR).Select(id => id.Trim()).ToList());
                    if (testGroupID.Length > this.FormattingLengthTestGroup) this.FormattingLengthTestGroup = testGroupID.Length;
                }
            } else {
                this.TestElementDescription = testGroups[this.TestElementID].Description;
                this.TestElementRevision = testGroups[this.TestElementID].Revision;
                this.TestMeasurementIDsSequence = testGroups[this.TestElementID].TestMeasurementIDs.Split(Test.SPLIT_ARGUMENTS_CHAR).Select(id => id.Trim()).ToList(); 
            }
            IEnumerable<String> duplicateIDs = this.TestMeasurementIDsSequence.GroupBy(x => x).Where(g => g.Count() > 1).Select(x => x.Key);
            if (duplicateIDs.Count() !=0) throw new InvalidOperationException($"Duplicated TestMeasurementIDs '{String.Join("', '", duplicateIDs)}'.");

            Dictionary<String, Test> testMeasurements = Test.Get();
            this.Tests = new Dictionary<String, Test>();

            foreach (String testMeasurementID in this.TestMeasurementIDsSequence) {
                this.Tests.Add(testMeasurementID, testMeasurements[testMeasurementID]); // Add only TestMeasurements correlated to the TestElementID selected by operator.
                if (testMeasurementID.Length > this.FormattingLengthTestMeasurement) this.FormattingLengthTestMeasurement = testMeasurementID.Length;
            }
        }

        public static AppConfigTest Get() { return new AppConfigTest(); }
    }
}
