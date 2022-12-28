using System;
using System.Collections.Generic;
using System.Configuration;
using ABTTestLibrary.TestSupport;

namespace ABTTestLibrary.AppConfig {
    public class TestElement : ConfigurationElement {
        [ConfigurationProperty("ID", IsKey = true, IsRequired = true)] public String ID { get { return (String)base["ID"]; } }
        [ConfigurationProperty("Summary", IsKey = false, IsRequired = true)] public String Summary { get { return (String)base["Summary"]; } }
        [ConfigurationProperty("Detail", IsKey = false, IsRequired = false)] public String Detail { get { return (String)base["Detail"]; } }
        [ConfigurationProperty("LimitLow", IsKey = false, IsRequired = false)] public String LimitLow { get { return (String)base["LimitLow"]; } }
        [ConfigurationProperty("LimitHigh", IsKey = false, IsRequired = false)] public String LimitHigh { get { return (String)base["LimitHigh"]; } }
        [ConfigurationProperty("Units", IsKey = false, IsRequired = true)] public String Units { get { return (String)base["Units"]; } }
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
        public String Summary { get; private set; }
        public String Detail { get; private set; }
        public String LimitLow { get; private set; }
        public String LimitHigh { get; private set; }
        public String Units { get; private set; }
        public String Measurement { get; set; }
        public String Result { get; set; }

        private Test(String ID, String Summary, String Detail, String LimitLow, String LimitHigh, String Units, String Measurement, String Result) {
            this.ID = ID;
            this.Summary = Summary;
            this.Detail = Detail;
            this.LimitLow = LimitLow;
            this.LimitHigh = LimitHigh;
            this.Units = Units;
            this.Measurement = Measurement;
            this.Result = Result;
        }

        public static Dictionary<String, Test> Get() {
            TestElementsSection s = (TestElementsSection)ConfigurationManager.GetSection("TestElementsSection");
            TestElements e = s.TestElements;
            Dictionary<String, Test> d = new Dictionary<String, Test>();
            foreach (TestElement te in e) d.Add(te.ID, new Test(te.ID, te.Summary, te.Detail, te.LimitLow, te.LimitHigh, te.Units, String.Empty, Result: EventCodes.UNSET));
            // Pre-load Tests with EventCodes.UNSET results, which will be replaced as the tests are executed with EventCodes.ABORT, EventCodes.ERROR, EventCodes.FAIL or (hopefully!) EventCodes.PASS.
            return d;
        }
    }
}
