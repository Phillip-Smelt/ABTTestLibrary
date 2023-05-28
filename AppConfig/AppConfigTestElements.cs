using System;
using System.Collections.Generic;
using System.Configuration;

namespace ABT.TestSpace.AppConfig {
    [ConfigurationCollection(typeof(TestElement))]
    public class TestElements : ConfigurationElementCollection {
        public const String PropertyName = "Element";
        public TestElement this[Int32 idx] { get { return (TestElement)this.BaseGet(idx); } }
        public override ConfigurationElementCollectionType CollectionType { get { return ConfigurationElementCollectionType.BasicMapAlternate; } }
        protected override String ElementName { get { return PropertyName; } }
        protected override Boolean IsElementName(String elementName) { return elementName.Equals(PropertyName, StringComparison.InvariantCultureIgnoreCase); }
        public override Boolean IsReadOnly() { return false; }
        protected override ConfigurationElement CreateNewElement() { return new TestElement(); }
        protected override Object GetElementKey(ConfigurationElement element) { return ((TestElement)(element)).ID; }
    }
    public class TestElement : ConfigurationElement {
        [ConfigurationProperty("ID", IsKey = true, IsRequired = true)] public String ID { get { return ((String)base["ID"]).Trim(); } }
        [ConfigurationProperty("Revision", IsKey = false, IsRequired = true)] public String Revision { get { return ((String)base["Revision"]).Trim(); } }
        [ConfigurationProperty("Description", IsKey = false, IsRequired = true)] public String Description { get { return ((String)base["Description"]).Trim(); } }
    }

    public class TestOperationsSection : ConfigurationSection { [ConfigurationProperty("TestOperations")] public TestOperations TestOperations { get { return ((TestOperations)(base["TestOperations"])); } } }
    [ConfigurationCollection(typeof(TestOperation))]
    public class TestOperations : TestElements { public new const String PropertyName = "TestOperations"; }
    public class TestOperation : TestElement {
        [ConfigurationProperty("TestGroupIDs", IsKey = false, IsRequired = true)] public String TestGroupIDs { get { return ((String)base["TestGroupIDs"]).Trim(); } }
    }

    public class TestGroupsSection : ConfigurationSection { [ConfigurationProperty("TestGroups")] public TestGroups TestGroups { get { return ((TestGroups)(base["TestGroups"])); } } }
    [ConfigurationCollection(typeof(TestGroup))]
    public class TestGroups : TestElements { public new const String PropertyName = "TestGroups"; }
    public class TestGroup : TestElement {
        [ConfigurationProperty("TestMeasurementIDs", IsKey = false, IsRequired = true)] public String TestMeasurementIDs { get { return ((String)base["TestMeasurementIDs"]).Trim(); } }
    }

    public class TestMeasurementsSection : ConfigurationSection { [ConfigurationProperty("TestMeasurements")] public TestMeasurements TestMeasurements { get { return ((TestMeasurements)(base["TestMeasurements"])); } } }
    [ConfigurationCollection(typeof(TestMeasurement))]
    public class TestMeasurements : TestElements { public new const String PropertyName = "TestMeasurements"; }
    public class TestMeasurement : TestElement {
        [ConfigurationProperty("ClassName", IsKey = false, IsRequired = true)] public String ClassName { get { return ((String)base["ClassName"]).Trim(); } }
        [ConfigurationProperty("Arguments", IsKey = false, IsRequired = true)] public String Arguments { get { return ((String)base["Arguments"]).Trim(); } }
    }

    public class ConfigTests {
        public TestMeasurementsSection TestMeasurementsSection { get { return (TestMeasurementsSection)ConfigurationManager.GetSection("TestMeasurementsSection"); } }
        public TestMeasurements TestMeasurements { get { return this.TestMeasurementsSection.TestMeasurements; } }
        public IEnumerable<TestMeasurement> TestMeasurement { get { foreach (TestMeasurement tm in this.TestMeasurements) if (tm != null) yield return tm; } }
    }

    public class Group {
        public readonly String ID;
        public readonly String Revision;    
        public readonly String Description;
        public readonly String TestMeasurementIDs;

        private Group(String id, String revision, String description, String testMeasurementIDs) {
            this.ID = id;
            this.Revision = revision;
            this.Description = description;
            this.TestMeasurementIDs = testMeasurementIDs;
        }

        public static Dictionary<String, Group> Get() {
            TestGroupsSection testGroupSection = (TestGroupsSection)ConfigurationManager.GetSection("TestGroupsSection");
            TestGroups testGroups = testGroupSection.TestGroups;
            Dictionary<String, Group> dictionary = new Dictionary<String, Group>();
            foreach (TestGroup tg in testGroups) dictionary.Add(tg.ID, new Group(tg.ID, tg.Revision, tg.Description, tg.TestMeasurementIDs));
            return dictionary;
        }
    }
}
