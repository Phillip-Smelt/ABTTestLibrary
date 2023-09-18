using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;

namespace ABT.TestSpace.TestExec.AppConfig {
    public enum SI_UNITS { amperes, celcius, farads, henries, hertz, NotApplicable, ohms, seconds, siemens, volt_amperes, volts, watts }
    public enum SI_UNITS_MODIFIER { AC, DC, Peak, PP, NotApplicable, RMS }

    public abstract class MeasurementAbstract {
        public const String ClassName = nameof(MeasurementAbstract);
        public const Char SA = '|'; // Arguments separator character.  Must match Arguments separator character used in TestExecutor's App.Config.
        public const Char SK = '='; // Key/Values separator character.  Must match Key/Values separator character used in TestExecutor's App.Config.

        private protected MeasurementAbstract() { }

        public abstract String ArgumentsGet();

        public static Dictionary<String, String> ArgumentsSplit(String Arguments) {
            Dictionary<String, String> argDictionary = new Dictionary<String, String>();
            if (String.Equals(Arguments, MeasurementCustom.NOT_APPLICABLE)) argDictionary.Add(MeasurementCustom.NOT_APPLICABLE, MeasurementCustom.NOT_APPLICABLE);
            else {
                String[] args = Arguments.Split(SA);
                String[] kvp;
                for (Int32 i = 0; i < args.Length; i++) {
                    kvp = args[i].Split(SK);
                    argDictionary.Add(kvp[0].Trim(), kvp[1].Trim());
                }
            }
            return argDictionary;
        }

        internal abstract void ArgumentsValidate(String id, String arguments, Dictionary<String, String> argsDict);
    }

    public class MeasurementCustom : MeasurementAbstract {
        public new const String ClassName = nameof(MeasurementCustom);
        public readonly String Arguments;
        public static readonly String NOT_APPLICABLE = Enum.GetName(typeof(SI_UNITS), SI_UNITS.NotApplicable);

        public MeasurementCustom(String ID, String Arguments) {
            Dictionary<String, String> argsDict = ArgumentsSplit(Arguments);
            ArgumentsValidate(ID, Arguments, argsDict);
            this.Arguments = Arguments;
        }

        public override String ArgumentsGet() { return Arguments; }

        internal override void ArgumentsValidate(String id, String arguments, Dictionary<String, String> argsDict) {
            if (argsDict.Count == 0) throw new ArgumentException($"{ClassName} ID '{id}' requires 1 or more case-sensitive arguments:{Environment.NewLine}" +
                $"   Example: '{NOT_APPLICABLE}'{Environment.NewLine}" +
                $"   Or     : 'Key1{SK}Value1'{Environment.NewLine}" +
                $"   Or     : 'Key1{SK}Value1{SA}{Environment.NewLine}" +
                $"             Key2{SK}Value2{SA}{Environment.NewLine}" +
                $"             Key3{SK}Value3'{Environment.NewLine}" +
                $"   Actual : '{arguments}'");
        }
    }

    public class MeasurementISP : MeasurementAbstract {
        public new const String ClassName = nameof(MeasurementISP);
        public readonly String ISPExecutableFolder;         private const String _ISP_EXECUTABLE_FOLDER = nameof(ISPExecutableFolder);
        public readonly String ISPExecutable;               private const String _ISP_EXECUTABLE = nameof(ISPExecutable);
        public readonly String ISPExecutableArguments;      private const String _ISP_EXECUTABLE_ARGUMENTS = nameof(ISPExecutableArguments);
        public readonly String ISPExpected;                 private const String _ISP_EXPECTED = nameof(ISPExpected);

        public MeasurementISP(String ID, String Arguments) {
            Dictionary<String, String> argsDict = ArgumentsSplit(Arguments);
            ArgumentsValidate(ID, Arguments, argsDict);
            ISPExecutableFolder = argsDict[_ISP_EXECUTABLE_FOLDER];
            ISPExecutable = argsDict[_ISP_EXECUTABLE];
            ISPExecutableArguments = argsDict[_ISP_EXECUTABLE_ARGUMENTS];
            ISPExpected = argsDict[_ISP_EXPECTED];
        }

        public override String ArgumentsGet() {
            return $"{_ISP_EXECUTABLE_FOLDER}{SK}{ISPExecutableFolder}{SA}{_ISP_EXECUTABLE}{SK}{ISPExecutable}{SA}{_ISP_EXECUTABLE_ARGUMENTS}{SK}{ISPExecutableArguments}{SA}{_ISP_EXPECTED}{SK}{ISPExpected}";
        }

        internal override void ArgumentsValidate(String id, String arguments, Dictionary<String, String> argsDict) {
            if (argsDict.Count != 4) throw new ArgumentException($"{ClassName} ID '{id}' requires 4 case-sensitive arguments:{Environment.NewLine}" +
                $@"   Example: '{_ISP_EXECUTABLE}{SK}ipecmd.exe{SA}{Environment.NewLine}
                                {_ISP_EXECUTABLE_FOLDER}{SK}C:\Program Files\Microchip\MPLABX\v6.05\mplab_platform\mplab_ipe\{SA}{Environment.NewLine}
                                {_ISP_EXECUTABLE_ARGUMENTS}{SK}C:\TBD\U1_Firmware.hex{SA}{Environment.NewLine}
                                {_ISP_EXPECTED}{SK}0xAC0E'{Environment.NewLine}" +
                 $"   Actual : '{arguments}'");
            if (!argsDict.ContainsKey(_ISP_EXECUTABLE_FOLDER)) throw new ArgumentException($"{ClassName} ID '{id}' does not contain '{_ISP_EXECUTABLE_FOLDER}' key-value pair.");
            if (!argsDict.ContainsKey(_ISP_EXECUTABLE)) throw new ArgumentException($"{ClassName} ID '{id}' does not contain '{_ISP_EXECUTABLE}' key-value pair.");
            if (!argsDict.ContainsKey(_ISP_EXECUTABLE_ARGUMENTS)) throw new ArgumentException($"{ClassName} ID '{id}' does not contain '{_ISP_EXECUTABLE_ARGUMENTS}' key-value pair.");
            if (!argsDict.ContainsKey(_ISP_EXPECTED)) throw new ArgumentException($"{ClassName} ID '{id}' does not contain '{_ISP_EXPECTED}' key-value pair.");
            if (!argsDict[_ISP_EXECUTABLE_FOLDER].EndsWith(@"\")) throw new ArgumentException($"{ClassName} ID '{id}' {_ISP_EXECUTABLE_FOLDER} '{argsDict[_ISP_EXECUTABLE_FOLDER]}' does not end with '\\'.");
            if (!Directory.Exists(argsDict[_ISP_EXECUTABLE_FOLDER])) throw new ArgumentException($"{ClassName} ID '{id}' {_ISP_EXECUTABLE_FOLDER} '{argsDict[_ISP_EXECUTABLE_FOLDER]}' does not exist.");
            if (!File.Exists(argsDict[_ISP_EXECUTABLE_FOLDER] + argsDict[_ISP_EXECUTABLE])) throw new ArgumentException($"{ClassName} ID '{id}' {_ISP_EXECUTABLE} '{argsDict[_ISP_EXECUTABLE_FOLDER] + argsDict[_ISP_EXECUTABLE]}' does not exist.");
        }
    }

    public class MeasurementNumeric : MeasurementAbstract {
        public new const String ClassName = nameof(MeasurementNumeric);
        public readonly Double Low;                                                                 private const String _LOW = nameof(Low);
        public readonly Double High;                                                                private const String _HIGH = nameof(High);
        public readonly SI_UNITS SI_Units = SI_UNITS.NotApplicable;                                   private const String _SI_UNITS = nameof(SI_Units);
        public readonly SI_UNITS_MODIFIER SI_Units_Modifier = SI_UNITS_MODIFIER.NotApplicable;        private const String _SI_UNITS_MODIFIER = nameof(SI_Units_Modifier);

        public MeasurementNumeric(String ID, String Arguments) {
            Dictionary<String, String> argsDict = ArgumentsSplit(Arguments);
            ArgumentsValidate(ID, Arguments, argsDict);
            High = Double.Parse(argsDict[_HIGH], NumberStyles.Float, CultureInfo.CurrentCulture);
            Low = Double.Parse(argsDict[_LOW], NumberStyles.Float, CultureInfo.CurrentCulture);

            String[] si_units = Enum.GetNames(typeof(SI_UNITS)).Select(s => s.ToLower()).ToArray();
            if (si_units.Any(argsDict[_SI_UNITS].ToLower().Contains)) {
                SI_Units = (SI_UNITS)Enum.Parse(typeof(SI_UNITS), argsDict[_SI_UNITS], ignoreCase: true);
                String[] si_units_modifiers = Enum.GetNames(typeof(SI_UNITS_MODIFIER)).Select(s => s.ToLower()).ToArray();
                if (si_units_modifiers.Any(argsDict[_SI_UNITS_MODIFIER].ToLower().Contains)) {
                    SI_Units_Modifier = (SI_UNITS_MODIFIER)Enum.Parse(typeof(SI_UNITS_MODIFIER), argsDict[_SI_UNITS_MODIFIER], ignoreCase: true);
                }
            }
        }

        public override String ArgumentsGet() {
            return $"{_HIGH}{SK}{High}{SA}{_LOW}{SK}{Low}{SA}{_SI_UNITS}{SK}{SI_Units}{SA}{_SI_UNITS_MODIFIER}{SK}{SI_Units_Modifier}";
        }

        internal override void ArgumentsValidate(String id, String arguments, Dictionary<String, String> argsDict) {
            if (argsDict.Count != 4) throw new ArgumentException($"{ClassName} ID '{id}' requires 4 case-sensitive arguments:{Environment.NewLine}" +
                $"   Example: '{_HIGH}{SK}0.004{SA}{Environment.NewLine}" +
                $"             {_LOW}{SK}0.002{SA}{Environment.NewLine}" +
                $"             {_SI_UNITS}{SK}volts{SA}{Environment.NewLine}" +
                $"             {_SI_UNITS_MODIFIER}{SK}DC'{Environment.NewLine}" +
                $"   Actual : '{arguments}'");
            if (!argsDict.ContainsKey(_HIGH)) throw new ArgumentException($"{ClassName} ID '{id}' does not contain '{_HIGH}' key-value pair.");
            if (!argsDict.ContainsKey(_LOW)) throw new ArgumentException($"{ClassName} ID '{id  }' does not contain '{_LOW}' key-value pair.");
            if (!argsDict.ContainsKey(_SI_UNITS)) throw new ArgumentException($"{ClassName} ID '{id}' does not contain '{_SI_UNITS}' key-value pair.");
            if (!argsDict.ContainsKey(_SI_UNITS_MODIFIER)) throw new ArgumentException($"{ClassName} ID '{id}' does not contain '{_SI_UNITS_MODIFIER}' key-value pair.");
            if (!Double.TryParse(argsDict[_HIGH], NumberStyles.Float, CultureInfo.CurrentCulture, out Double high)) throw new ArgumentException($"{ClassName} ID '{id}' {_HIGH} '{argsDict[_HIGH]}' ≠ System.Double.");
            if (!Double.TryParse(argsDict[_LOW], NumberStyles.Float, CultureInfo.CurrentCulture, out Double low)) throw new ArgumentException($"{ClassName} ID '{id}' {_LOW} '{argsDict[_LOW]}' ≠ System.Double.");
            if (low > high) throw new ArgumentException($"{ClassName} ID '{id}' {_LOW} '{low}' > {_HIGH} '{high}'.");
        }
    }

    public class MeasurementTextual : MeasurementAbstract {
        public new const String ClassName = nameof(MeasurementTextual);
        public readonly String Text;                                private const String _TEXT = nameof(Text);

        public MeasurementTextual(String ID, String Arguments) {
            Dictionary<String, String> argsDict = ArgumentsSplit(Arguments);
            ArgumentsValidate(ID, Arguments, argsDict);
            Text = argsDict[_TEXT];
        }

        public override String ArgumentsGet() {
            return $"{_TEXT}{SK}{Text}";
        }

        internal override void ArgumentsValidate(String id, String arguments, Dictionary<String, String> argsDict) {
            if (argsDict.Count != 1) throw new ArgumentException($"{ClassName} ID '{id}' requires 1 case-sensitive argument:{Environment.NewLine}" +
                $"   Example: '{_TEXT}{SK}The quick brown fox jumps over the lazy dog.'{Environment.NewLine}" +
                $"   Actual : '{arguments}'");
            if (!argsDict.ContainsKey(_TEXT)) throw new ArgumentException($"{ClassName} ID '{id}' does not contain '{_TEXT}' key-value pair.");
        }
    }

    public class AppConfigTest {
        public readonly String TestElementID;
        public readonly Boolean IsOperation;
        public readonly String TestElementDescription;
        public readonly String TestElementRevision;
        public readonly List<String> GroupIDsSequence = new List<String>();
        public readonly Dictionary<String, Group> Groups = new Dictionary<String, Group>();
        public readonly Dictionary<String, List<String>> GroupIDsToMeasurementIDs = new Dictionary<String, List<String>>();
        public readonly List<String> TestMeasurementIDsSequence = new List<String>();
        public readonly Dictionary<String, Measurement> Measurements = new Dictionary<String, Measurement>();
        public readonly Int32 FormattingLengthGroupID = 0;
        public readonly Int32 FormattingLengthMeasurementID = 0;

        private AppConfigTest() {
            Dictionary<String, Operation> allOperations = Operation.Get();
            Dictionary<String, Group> allGroups = Group.Get();
            Dictionary<String, Measurement> allMeasurements = Measurement.Get();
            (TestElementID, IsOperation) = SelectTests.Get(allOperations, allGroups);

            if (IsOperation) {
                TestElementDescription = allOperations[TestElementID].Description;
                TestElementRevision = allOperations[TestElementID].Revision;
                GroupIDsSequence = allOperations[TestElementID].TestGroupIDs.Split(MeasurementAbstract.SA).Select(id => id.Trim()).ToList();
                foreach (String groupID in GroupIDsSequence) {
                    Groups.Add(groupID, allGroups[groupID]);
                    GroupIDsToMeasurementIDs.Add(groupID, allGroups[groupID].TestMeasurementIDs.Split(MeasurementAbstract.SA).ToList());

                    if (groupID.Length > FormattingLengthGroupID) FormattingLengthGroupID = groupID.Length;
                    foreach (String measurementID in GroupIDsToMeasurementIDs[groupID]) {
                        Measurements.Add(measurementID, allMeasurements[measurementID]);
                        Measurements[measurementID].GroupID = groupID;
                        if (measurementID.Length > FormattingLengthMeasurementID) FormattingLengthMeasurementID = measurementID.Length;
                    }
                }
            } else {
                TestElementDescription = allGroups[TestElementID].Description;
                TestElementRevision = allGroups[TestElementID].Revision;
                GroupIDsSequence.Add(TestElementID);
                Groups.Add(TestElementID, allGroups[TestElementID]);
                GroupIDsToMeasurementIDs.Add(TestElementID, allGroups[TestElementID].TestMeasurementIDs.Split(MeasurementAbstract.SA).ToList());

                foreach (String measurementID in GroupIDsToMeasurementIDs[TestElementID]) {
                    Measurements.Add(measurementID, allMeasurements[measurementID]);
                    Measurements[measurementID].GroupID = TestElementID;
                    if (measurementID.Length > FormattingLengthMeasurementID) FormattingLengthMeasurementID = measurementID.Length;
                }
            }
            foreach (String groupID in GroupIDsSequence) TestMeasurementIDsSequence.AddRange(GroupIDsToMeasurementIDs[groupID]);

            IEnumerable<String> duplicateIDs = TestMeasurementIDsSequence.GroupBy(x => x).Where(g => g.Count() > 1).Select(x => x.Key);
            if (duplicateIDs.Count() !=0) throw new InvalidOperationException($"Duplicated TestMeasurementIDs '{String.Join("', '", duplicateIDs)}'.");
        }

        public static AppConfigTest Get() { return new AppConfigTest(); }
    }
}
