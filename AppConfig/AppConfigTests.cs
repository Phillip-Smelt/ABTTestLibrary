using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;

namespace ABT.TestSpace.TestExec.AppConfig {
    public enum SI_UNITS { amperes, celcius, farads, henries, hertz, NotApplicable, ohms, seconds, siemens, volt_amperes, volts, watts }
    public enum SI_UNITS_MODIFIERS { AC, DC, Peak, PP, NotApplicable, RMS }

    public abstract class MeasurementAbstract {
        public const String ClassName = nameof(MeasurementAbstract);
        public const Char SA = '|'; // Arguments separator character.  Must match Arguments separator character used in TestExecutor's App.Config.
        public const Char SK = '='; // Key/Values separator character.  Must match Key/Values separator character used in TestExecutor's App.Config.
        private protected MeasurementAbstract() { }

        public static Dictionary<String, String> SplitArguments(String Arguments) {
            Dictionary<String, String> argDictionary = new Dictionary<String, String>();
            if (String.Equals(Arguments, MeasurementCustomizable.NOT_APPLICABLE)) argDictionary.Add(MeasurementCustomizable.NOT_APPLICABLE, MeasurementCustomizable.NOT_APPLICABLE);
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

        internal abstract void ValidateArguments(String id, String arguments, Dictionary<String, String> argsDict);

        public abstract String GetArguments();
    }

    public class MeasurementCustomizable : MeasurementAbstract {
        public new const String ClassName = nameof(MeasurementCustomizable);
        public readonly String Arguments;
        public const String NOT_APPLICABLE = "NotApplicable";

        public MeasurementCustomizable(String ID, String Arguments) {
            Dictionary<String, String> argsDict = SplitArguments(Arguments);
            ValidateArguments(ID, Arguments, argsDict);
            this.Arguments = Arguments;
        }

        internal override void ValidateArguments(String id, String arguments, Dictionary<String, String> argsDict) {
            if (argsDict.Count == 0) throw new ArgumentException($"{ClassName} ID '{id}' requires 1 or more case-sensitive arguments:{Environment.NewLine}" +
                $"   Example: '{NOT_APPLICABLE}'{Environment.NewLine}" +
                $"   Or     : 'Key1{SK}Value1'{Environment.NewLine}" +
                $"   Or     : 'Key1{SK}Value1{SA}{Environment.NewLine}" +
                $"             Key2{SK}Value2{SA}{Environment.NewLine}" +
                $"             Key3{SK}Value3'{Environment.NewLine}" +
                $"   Actual : '{arguments}'");
        }

        public override String GetArguments() { return this.Arguments; }
    }

    public class MeasurementISP : MeasurementAbstract {
        public new const String ClassName = nameof(MeasurementISP);
        public readonly String ISPExecutableFolder;         private const String _ISP_EXECUTABLE_FOLDER = nameof(ISPExecutableFolder);
        public readonly String ISPExecutable;               private const String _ISP_EXECUTABLE = nameof(ISPExecutable);
        public readonly String ISPExecutableArguments;      private const String _ISP_EXECUTABLE_ARGUMENTS = nameof(ISPExecutableArguments);
        public readonly String ISPExpected;                 private const String _ISP_EXPECTED = nameof(ISPExpected);

        public MeasurementISP(String ID, String Arguments) {
            Dictionary<String, String> argsDict = SplitArguments(Arguments);
            ValidateArguments(ID, Arguments, argsDict);
            this.ISPExecutableFolder = argsDict[_ISP_EXECUTABLE_FOLDER];
            this.ISPExecutable = argsDict[_ISP_EXECUTABLE];
            this.ISPExecutableArguments = argsDict[_ISP_EXECUTABLE_ARGUMENTS];
            this.ISPExpected = argsDict[_ISP_EXPECTED];
        }

        internal override void ValidateArguments(String id, String arguments, Dictionary<String, String> argsDict) {
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

        public override String GetArguments() {
            return $"{_ISP_EXECUTABLE_FOLDER}{SK}{this.ISPExecutableFolder}{SA}{_ISP_EXECUTABLE}{SK}{this.ISPExecutable}{SA}{_ISP_EXECUTABLE_ARGUMENTS}{SK}{this.ISPExecutableArguments}{SA}{_ISP_EXPECTED}{SK}{this.ISPExpected}";
        }
    }

    public class MeasurementNumerical : MeasurementAbstract {
        public new const String ClassName = nameof(MeasurementNumerical);
        public readonly Double Low;                                                                 private const String _LOW = nameof(Low);
        public readonly Double High;                                                                private const String _HIGH = nameof(High);
        public readonly SI_UNITS SI_Units = SI_UNITS.NotApplicable;                                 private const String _SI_UNITS = nameof(SI_Units);
        public readonly SI_UNITS_MODIFIERS SI_Units_Modifier = SI_UNITS_MODIFIERS.NotApplicable;    private const String _SI_UNITS_MODIFIER = nameof(SI_Units_Modifier);

        public MeasurementNumerical(String ID, String Arguments) {
            Dictionary<String, String> argsDict = SplitArguments(Arguments);
            ValidateArguments(ID, Arguments, argsDict);
            this.High = Double.Parse(argsDict[_HIGH], NumberStyles.Float, CultureInfo.CurrentCulture);
            this.Low = Double.Parse(argsDict[_LOW], NumberStyles.Float, CultureInfo.CurrentCulture);

            String[] si_units = Enum.GetNames(typeof(SI_UNITS)).Select(s => s.ToLower()).ToArray();
            if (si_units.Any(argsDict[_SI_UNITS].ToLower().Contains)) {
                this.SI_Units = (SI_UNITS)Enum.Parse(typeof(SI_UNITS), argsDict[_SI_UNITS], ignoreCase: true);
                String[] si_units_modifiers = Enum.GetNames(typeof(SI_UNITS_MODIFIERS)).Select(s => s.ToLower()).ToArray();
                if (si_units_modifiers.Any(argsDict[_SI_UNITS_MODIFIER].ToLower().Contains)) {
                    this.SI_Units_Modifier = (SI_UNITS_MODIFIERS)Enum.Parse(typeof(SI_UNITS_MODIFIERS), argsDict[_SI_UNITS_MODIFIER], ignoreCase: true);
                }
            }
        }

        internal override void ValidateArguments(String id, String arguments, Dictionary<String, String> argsDict) {
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

        public override String GetArguments() {
            return $"{_HIGH}{SK}{this.High}{SA}{_LOW}{SK}{this.Low}{SA}{_SI_UNITS}{SK}{this.SI_Units}{SA}{_SI_UNITS_MODIFIER}{SK}{this.SI_Units_Modifier}";
        }
    }

    public class MeasurementTextual : MeasurementAbstract {
        public new const String ClassName = nameof(MeasurementTextual);
        public readonly String Text;                                private const String _TEXT = nameof(Text);

        public MeasurementTextual(String ID, String Arguments) {
            Dictionary<String, String> argsDict = SplitArguments(Arguments);
            ValidateArguments(ID, Arguments, argsDict);
            this.Text = argsDict[_TEXT];
        }

        internal override void ValidateArguments(String id, String arguments, Dictionary<String, String> argsDict) {
            if (argsDict.Count != 1) throw new ArgumentException($"{ClassName} ID '{id}' requires 1 case-sensitive argument:{Environment.NewLine}" +
                $"   Example: '{_TEXT}{SK}The quick brown fox jumps over the lazy dog.'{Environment.NewLine}" +
                $"   Actual : '{arguments}'");
            if (!argsDict.ContainsKey(_TEXT)) throw new ArgumentException($"{ClassName} ID '{id}' does not contain '{_TEXT}' key-value pair.");
        }

        public override String GetArguments() {
            return $"{_TEXT}{SK}{this.Text}";
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
            (this.TestElementID, this.IsOperation) = SelectTests.Get(allOperations, allGroups);

            if (this.IsOperation) {
                this.TestElementDescription = allOperations[this.TestElementID].Description;
                this.TestElementRevision = allOperations[this.TestElementID].Revision;
                this.GroupIDsSequence = allOperations[this.TestElementID].TestGroupIDs.Split(MeasurementAbstract.SA).Select(id => id.Trim()).ToList();
                foreach (String groupID in this.GroupIDsSequence) {
                    this.Groups.Add(groupID, allGroups[groupID]);
                    this.GroupIDsToMeasurementIDs.Add(groupID, allGroups[groupID].TestMeasurementIDs.Split(MeasurementAbstract.SA).ToList());

                    this.HandleAnyDuplicatedIDs(this.GroupIDsToMeasurementIDs[groupID]);

                    if (groupID.Length > this.FormattingLengthGroupID) this.FormattingLengthGroupID = groupID.Length;
                    foreach (String measurementID in this.GroupIDsToMeasurementIDs[groupID]) {
                        this.Measurements.Add(measurementID, allMeasurements[measurementID]);
                        this.Measurements[measurementID].GroupID = groupID;
                        if (measurementID.Length > this.FormattingLengthMeasurementID) this.FormattingLengthMeasurementID = measurementID.Length;
                    }
                }
            } else {
                this.TestElementDescription = allGroups[this.TestElementID].Description;
                this.TestElementRevision = allGroups[this.TestElementID].Revision;
                this.GroupIDsSequence.Add(this.TestElementID);

                this.Groups.Add(this.TestElementID, allGroups[this.TestElementID]);
                this.GroupIDsToMeasurementIDs.Add(this.TestElementID, allGroups[this.TestElementID].TestMeasurementIDs.Split(MeasurementAbstract.SA).ToList());

                this.HandleAnyDuplicatedIDs(this.GroupIDsToMeasurementIDs[this.TestElementID]);

                foreach (String measurementID in this.GroupIDsToMeasurementIDs[this.TestElementID]) {
                    this.Measurements.Add(measurementID, allMeasurements[measurementID]);
                    this.Measurements[measurementID].GroupID = this.TestElementID;
                    if (measurementID.Length > this.FormattingLengthMeasurementID) this.FormattingLengthMeasurementID = measurementID.Length;
                }
            }
            foreach (String groupID in this.GroupIDsSequence) this.TestMeasurementIDsSequence.AddRange(this.GroupIDsToMeasurementIDs[groupID]);
             this.HandleAnyDuplicatedIDs(this.TestMeasurementIDsSequence);
        }

        private void HandleAnyDuplicatedIDs(List<String> IDs) {
            IEnumerable<String> duplicateIDs = IDs.GroupBy(x => x).Where(g => g.Count() > 1).Select(x => x.Key);
            if (duplicateIDs.Count() !=0) throw new InvalidOperationException($"Duplicated TestMeasurementIDs '{String.Join("', '", duplicateIDs)}'.");
        }

        public static AppConfigTest Get() { return new AppConfigTest(); }
    }
}
