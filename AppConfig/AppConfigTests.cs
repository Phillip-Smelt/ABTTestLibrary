using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Policy;

namespace ABT.TestSpace.AppConfig {
    public enum SI_UNITS { amperes, celcius, farads, henries, hertz, NotApplicable, ohms, seconds, siemens, volt_amperes, volts, watts }
    public enum SI_UNITS_MODIFIERS { AC, DC, Peak, PP, NotApplicable, RMS }

    public abstract class TestAbstract {
        public const String ClassName = nameof(TestAbstract);
        public const String NOT_APPLICABLE = "NotApplicable";
        private protected TestAbstract() { }

        public static Dictionary<String, String> SplitArguments(String Arguments) {
            String[] args = Arguments.Split(Test.SPLIT_ARGUMENTS_CHAR);
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

        public TestCustomizable(String _, String Arguments) { if (!String.Equals(Arguments, TestAbstract.NOT_APPLICABLE)) this.Arguments = TestAbstract.SplitArguments(Arguments); }
    }

    public class TestISP : TestAbstract {
        public new const String ClassName = nameof(TestISP);
        public readonly String ISPExecutableFolder;         private const String _ISP_EXECUTABLE_FOLDER = nameof(ISPExecutableFolder);
        public readonly String ISPExecutable;               private const String _ISP_EXECUTABLE = nameof(ISPExecutable);
        public readonly String ISPExecutableArguments;      private const String _ISP_EXECUTABLE_ARGUMENTS = nameof(ISPExecutableArguments);
        public readonly String ISPExpected;                 private const String _ISP_EXPECTED = nameof(ISPExpected);

        public TestISP(String ID, String Arguments) {
            Dictionary<String, String> argsDict = TestAbstract.SplitArguments(Arguments);
            ValidateTestISP(ID, Arguments, argsDict);
            this.ISPExecutableFolder = argsDict[_ISP_EXECUTABLE_FOLDER];
            this.ISPExecutable = argsDict[_ISP_EXECUTABLE];
            this.ISPExecutableArguments = argsDict[_ISP_EXECUTABLE_ARGUMENTS];
            this.ISPExpected = argsDict[_ISP_EXPECTED];
        }

        public static void ValidateTestISP(String ID, String Arguments) { ValidateTestISP(ID, Arguments, TestAbstract.SplitArguments(Arguments)); }

        private static void ValidateTestISP(String id, String arguments, Dictionary<String, String> argsDict) {
            if (argsDict.Count != 4) throw new ArgumentException($"{ClassName} ID '{id}' requires 4 case-sensitive arguments:{Environment.NewLine}" +
                $@"   Example: '{_ISP_EXECUTABLE}=ipecmd.exe|{Environment.NewLine}
                                {_ISP_EXECUTABLE_FOLDER}=C:\Program Files\Microchip\MPLABX\v6.05\mplab_platform\mplab_ipe\|{Environment.NewLine}
                                {_ISP_EXECUTABLE_ARGUMENTS}=C:\TBD\U1_Firmware.hex|{Environment.NewLine}
                                {_ISP_EXPECTED}=0xAC0E'{Environment.NewLine}" +
                $"   Actual : '{arguments}'");
            if (!argsDict.ContainsKey(_ISP_EXECUTABLE_FOLDER)) throw new ArgumentException($"{ClassName} ID '{id}' does not contain '{_ISP_EXECUTABLE_FOLDER}' key-value pair.");
            if (!argsDict.ContainsKey(_ISP_EXECUTABLE)) throw new ArgumentException($"{ClassName} ID '{id}' does not contain '{_ISP_EXECUTABLE}' key-value pair.");
            if (!argsDict.ContainsKey(_ISP_EXECUTABLE_ARGUMENTS)) throw new ArgumentException($"{ClassName} ID '{id}' does not contain '{_ISP_EXECUTABLE_ARGUMENTS}' key-value pair.");
            if (!argsDict.ContainsKey(_ISP_EXPECTED)) throw new ArgumentException($"{ClassName} ID '{id}' does not contain '{_ISP_EXPECTED}' key-value pair.");
            if (!argsDict[_ISP_EXECUTABLE_FOLDER].EndsWith(@"\")) throw new ArgumentException($"{ClassName} ID '{id}' {_ISP_EXECUTABLE_FOLDER} '{argsDict[_ISP_EXECUTABLE_FOLDER]}' does not end with '\\'.");
            if (!Directory.Exists(argsDict[_ISP_EXECUTABLE_FOLDER])) throw new ArgumentException($"{ClassName} ID '{id}' {_ISP_EXECUTABLE_FOLDER} '{argsDict[_ISP_EXECUTABLE_FOLDER]}' does not exist.");
            if (!File.Exists(argsDict[_ISP_EXECUTABLE_FOLDER] + argsDict[_ISP_EXECUTABLE])) throw new ArgumentException($"{ClassName} ID '{id}' {_ISP_EXECUTABLE} '{argsDict[_ISP_EXECUTABLE_FOLDER] + argsDict[_ISP_EXECUTABLE]}' does not exist.");
        }

        public String GetArguments() {
            return $"{_ISP_EXECUTABLE_FOLDER}={this.ISPExecutableFolder}|{_ISP_EXECUTABLE}={this.ISPExecutable}|{_ISP_EXECUTABLE_ARGUMENTS}={this.ISPExecutableArguments}|{_ISP_EXPECTED}={this.ISPExpected}";
        }
    }

    public class TestNumerical : TestAbstract {
        public new const String ClassName = nameof(TestNumerical);
        public readonly Double High;
        public readonly Double Low;
        public readonly SI_UNITS SI_Units = SI_UNITS.NotApplicable;
        public readonly SI_UNITS_MODIFIERS SI_Units_Modifier = SI_UNITS_MODIFIERS.NotApplicable;

        public TestNumerical(String ID, String Arguments) {
            Dictionary<String, String> argsDict = TestAbstract.SplitArguments(Arguments);
            if (argsDict.Count != 4) throw new ArgumentException($"TestNumerical ID '{ID}' with ClassName '{ClassName}' requires 4 case-sensitive arguments:{Environment.NewLine}" +
                $"   Example: 'High=0.004|{Environment.NewLine}" +
                $"             Low=0.002|{Environment.NewLine}" +
                $"             SI_Units=volts|{Environment.NewLine}" +
                $"             SI_Units_Modifier=DC'{Environment.NewLine}" +
                $"   Actual : '{Arguments}'");
            if (!argsDict.ContainsKey("High")) throw new ArgumentException($"TestNumerical ID '{ID}' does not contain 'High' key-value pair.");
            if (!argsDict.ContainsKey("Low")) throw new ArgumentException($"TestNumerical ID '{ID}' does not contain 'Low' key-value pair.");
            if (!argsDict.ContainsKey("SI_Units")) throw new ArgumentException($"TestNumerical ID '{ID}' does not contain 'SI_Units' key-value pair.");
            if (!argsDict.ContainsKey("SI_Units_Modifier")) throw new ArgumentException(ID);

            if (Double.TryParse(argsDict["High"], NumberStyles.Float, CultureInfo.CurrentCulture, out Double high)) this.High = high;
            else throw new ArgumentException($"TestNumerical ID '{ID}' High '{argsDict["High"]}' ≠ System.Double.");

            if (Double.TryParse(argsDict["Low"], NumberStyles.Float, CultureInfo.CurrentCulture, out Double low)) this.Low = low;
            else throw new ArgumentException($"TestNumerical ID '{ID}' Low '{argsDict["Low"]}' ≠ System.Double.");

            if (low > high) throw new ArgumentException($"TestNumerical ID '{ID}' Low '{low}' > High '{high}'.");

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

        public TestTextual(String ID, String Arguments) {
            Dictionary<String, String> argsDict = TestAbstract.SplitArguments(Arguments);
            if (argsDict.Count != 1) throw new ArgumentException($"TestTextual ID '{ID}' with ClassName '{ClassName}' requires 1 case-sensitive argument:{Environment.NewLine}" +
                    $"   Example: 'Text=The quick brown fox jumps over the lazy dog.'{Environment.NewLine}" +
                    $"   Actual : '{Arguments}'");
            if (!argsDict.ContainsKey("Text")) throw new ArgumentException($"TestTextual ID '{ID}' does not contain 'Text' key-value pair.");
            this.Text = argsDict["Text"];
        }
    }

    public class Test {
        internal const Char SPLIT_ARGUMENTS_CHAR = '|';
        public readonly String ID;
        public readonly String Description;
        public readonly String Revision;
        public readonly String ClassName;
        public readonly Boolean CancelOnFailure;
        public readonly String Arguments;
        public String Measurement { get; set; } = String.Empty; // Determined during test.
        public String Result { get; set; } = EventCodes.UNSET; // Determined post-test.
#if DEBUG
        public String DebugMessage { get; set; } = String.Empty; // Determined during test, only available for Debug compilations.
#endif
        private Test(String id, String description, String revision, String className, Boolean cancelOnFailure, String arguments) {
            this.ID = id;
            this.Description = description;
            this.Revision = revision;
            this.ClassName = className;
            this.CancelOnFailure = cancelOnFailure;
            this.Arguments = arguments;
            if (String.Equals(this.ClassName, TestNumerical.ClassName)) this.Measurement = Double.NaN.ToString();
            Object _ = Activator.CreateInstance(Type.GetType(this.GetType().Namespace + "." + this.ClassName), new Object[] { this.ID, arguments });
            // Create throwaway instance of className to validate its arguments before testing, rather than during:
            //  - Better to incur a comprehensible Exception when consequences are minimal than an incomprehensible Exception when consequences are maximal.
        }

        public static Dictionary<String, Test> Get() {
            TestMeasurementsSection testMeasurementsSection = (TestMeasurementsSection)ConfigurationManager.GetSection("TestMeasurementsSection");
            TestMeasurements testMeasurements = testMeasurementsSection.TestMeasurements;
            Dictionary<String, Test> dictionary = new Dictionary<String, Test>();
            foreach (TestMeasurement tm in testMeasurements) { dictionary.Add(tm.ID, new Test(tm.ID, tm.Description, tm.Revision, tm.ClassName, tm.CancelOnFailure, tm.Arguments)); }
            return dictionary;
        }

        public static Test Get(String TestMeasurementIDPresent) { return Get()[TestMeasurementIDPresent]; }
    }

    public class AppConfigTest {
        public readonly String TestElementID;
        public readonly Boolean IsOperation;
        public readonly String TestElementDescription;
        public readonly String TestElementRevision;
        public readonly List<String> TestIDsSequence;
        public readonly Dictionary<String, Test> Tests;
        public readonly Int32 FormattingLengthTestGroup = 0;
        public readonly Int32 FormattingLengthTestMeasurement = 0;

        private AppConfigTest() {
            Dictionary<String, Operation> testOperations = Operation.Get();
            Dictionary<String, Group> testGroups = Group.Get();

            (this.TestElementID, this.IsOperation) = SelectTests.Get(testOperations, testGroups);

            this.TestIDsSequence = new List<String>();
            if (this.IsOperation) {
                this.TestElementDescription = testOperations[this.TestElementID].Description;
                this.TestElementRevision = testOperations[this.TestElementID].Revision;
                List<String> testGroupIDs = testOperations[this.TestElementID].TestGroupIDs.Split(Test.SPLIT_ARGUMENTS_CHAR).Select(id => id.Trim()).ToList();
                foreach (String testGroupID in testGroupIDs) {
                    this.TestIDsSequence.AddRange(testGroups[testGroupID].TestMeasurementIDs.Split(Test.SPLIT_ARGUMENTS_CHAR).Select(id => id.Trim()).ToList());
                    if (testGroupID.Length > this.FormattingLengthTestGroup) this.FormattingLengthTestGroup = testGroupID.Length;
                }
            } else {
                this.TestElementDescription = testGroups[this.TestElementID].Description;
                this.TestElementRevision = testGroups[this.TestElementID].Revision;
                this.TestIDsSequence = testGroups[this.TestElementID].TestMeasurementIDs.Split(Test.SPLIT_ARGUMENTS_CHAR).Select(id => id.Trim()).ToList(); 
            }
            IEnumerable<String> duplicateIDs = this.TestIDsSequence.GroupBy(x => x).Where(g => g.Count() > 1).Select(x => x.Key);
            if (duplicateIDs.Count() !=0) throw new InvalidOperationException($"Duplicated TestMeasurementIDs '{String.Join("', '", duplicateIDs)}'.");

            Dictionary<String, Test> testMeasurements = Test.Get();
            this.Tests = new Dictionary<String, Test>();

            foreach (String testMeasurementID in this.TestIDsSequence) {
                this.Tests.Add(testMeasurementID, testMeasurements[testMeasurementID]); // Add only TestMeasurements correlated to the TestElementID selected by operator.
                if (testMeasurementID.Length > this.FormattingLengthTestMeasurement) this.FormattingLengthTestMeasurement = testMeasurementID.Length;
            }
        }

        public static AppConfigTest Get() { return new AppConfigTest(); }
    }
}
