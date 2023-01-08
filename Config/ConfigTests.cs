using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using TestLibrary.TestSupport;

namespace TestLibrary.Config {
    public class TestElement : ConfigurationElement {
        [ConfigurationProperty("ID", IsKey = true, IsRequired = true)] public String ID { get { return (String)base["ID"]; } }
        [ConfigurationProperty("Revision", IsKey = false, IsRequired = true)] public String Revision { get { return (String)base["Revision"]; } }
        [ConfigurationProperty("Summary", IsKey = false, IsRequired = true)] public String Summary { get { return (String)base["Summary"]; } }
        [ConfigurationProperty("Detail", IsKey = false, IsRequired = false)] public String Detail { get { return (String)base["Detail"]; } }
        [ConfigurationProperty("LimitLow", IsKey = false, IsRequired = false)] public String LimitLow { get { return (String)base["LimitLow"]; } }
        [ConfigurationProperty("LimitHigh", IsKey = false, IsRequired = false)] public String LimitHigh { get { return (String)base["LimitHigh"]; } }
        [ConfigurationProperty("Units", IsKey = false, IsRequired = false)] public String Units { get { return (String)base["Units"]; } }
        [ConfigurationProperty("UnitType", IsKey = false, IsRequired = false)] public String UnitType { get { return (String)base["UnitType"]; } }
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

    public class Test {
        public String ID { get; private set; }
        public String Revision { get; private set; }
        public String Summary { get; private set; }
        public String Detail { get; private set; }
        public String LimitLow { get; private set; }
        public String LimitHigh { get; private set; }
        public String Units { get; private set; }
        public String UnitType { get; private set; }
        public String Measurement { get; set; }
        public String Result { get; set; }

        private Test(String ID, String Revision, String Summary, String Detail, String LimitLow, String LimitHigh, String Units, String UnitType, String Measurement, String Result) {
            this.ID = ID;
            this.Revision = Revision;
            this.Summary = Summary;
            this.Detail = Detail;
            this.LimitLow = LimitLow;
            this.LimitHigh = LimitHigh;
            this.Units = Units;
            this.UnitType = UnitType;
            this.Measurement = Measurement;
            this.Result = Result;
        }

        public static Dictionary<String, Test> Get() {
            TestElementsSection s = (TestElementsSection)ConfigurationManager.GetSection("TestElementsSection");
            TestElements e = s.TestElements;
            Dictionary<String, Test> d = new Dictionary<String, Test>();
            foreach (TestElement te in e) d.Add(te.ID, new Test(te.ID, te.Revision, te.Summary, te.Detail, te.LimitLow, te.LimitHigh, te.Units, te.UnitType, String.Empty, Result: EventCodes.UNSET));
            // Pre-load Tests with EventCodes.UNSET results, which will be replaced as the tests are executed with EventCodes.ABORT, EventCodes.ERROR, EventCodes.FAIL or (hopefully!) EventCodes.PASS.
            return d;
        }
    }

    public class ConfigTest {
        public Group Group { get; private set; }
        public Dictionary<String, Test> Tests { get; private set; }

        private ConfigTest () {
            Dictionary<String, Group> Groups = Group.Get();
            String GroupSelected = GroupSelect.Get(Groups);
            this.Group = Groups[GroupSelected];
            // Operator selects the Group they want to test, from the Dictionary of all Groups.
            // GroupSelected is Dictionary Groups' Key.

            Dictionary<String, Test> tests = Test.Get();
            this.Tests = new Dictionary<String, Test>();
            String[] g = this.Group.TestIDs.Split('|');
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
