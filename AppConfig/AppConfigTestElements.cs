using System;
using System.Collections.Generic;
using System.Configuration;

namespace ABT.TestSpace.TestExec.AppConfig {
    public class TestOperationsSection : ConfigurationSection {
        public const String ClassName = nameof(TestOperationsSection);
        [ConfigurationProperty("TestOperations")] public TestOperations TestOperations { get { return ((TestOperations)(base["TestOperations"])); } }
    }
    [ConfigurationCollection(typeof(TestOperation))]
    public class TestOperations : ConfigurationElementCollection {
        public const String PropertyName = "TestOperation";
        public TestOperation this[Int32 idx] { get { return (TestOperation)this.BaseGet(idx); } }
        public override ConfigurationElementCollectionType CollectionType { get { return ConfigurationElementCollectionType.BasicMapAlternate; } }
        protected override String ElementName { get { return PropertyName; } }
        protected override Boolean IsElementName(String elementName) { return elementName.Equals(PropertyName, StringComparison.InvariantCultureIgnoreCase); }
        public override Boolean IsReadOnly() { return false; }
        protected override ConfigurationElement CreateNewElement() { return new TestOperation(); }
        protected override Object GetElementKey(ConfigurationElement element) { return ((TestOperation)(element)).ID; }
    }
    public class TestOperation : ConfigurationElement {
        [ConfigurationProperty("ID", IsKey = true, IsRequired = true)] public String ID { get { return ((String)base["ID"]).Trim(); } }
        [ConfigurationProperty("Revision", IsKey = false, IsRequired = true)] public String Revision { get { return ((String)base["Revision"]).Trim(); } }
        [ConfigurationProperty("Description", IsKey = false, IsRequired = true)] public String Description { get { return ((String)base["Description"]).Trim(); } }
        [ConfigurationProperty("TestGroupIDs", IsKey = false, IsRequired = true)] public String TestGroupIDs { get { return ((String)base["TestGroupIDs"]).Trim(); } }
    }

    public class TestGroupsSection : ConfigurationSection {
        public const String ClassName = nameof(TestGroupsSection);
        [ConfigurationProperty("TestGroups")] public TestGroups TestGroups { get { return ((TestGroups)(base["TestGroups"])); } }
    }
    [ConfigurationCollection(typeof(TestGroup))]
    public class TestGroups : ConfigurationElementCollection {
        public const String PropertyName = "TestGroup";
        public TestGroup this[Int32 idx] { get { return (TestGroup)this.BaseGet(idx); } }
        public override ConfigurationElementCollectionType CollectionType { get { return ConfigurationElementCollectionType.BasicMapAlternate; } }
        protected override String ElementName { get { return PropertyName; } }
        protected override Boolean IsElementName(String elementName) { return elementName.Equals(PropertyName, StringComparison.InvariantCultureIgnoreCase); }
        public override Boolean IsReadOnly() { return false; }
        protected override ConfigurationElement CreateNewElement() { return new TestGroup(); }
        protected override Object GetElementKey(ConfigurationElement element) { return ((TestGroup)(element)).ID; }
    }
    public class TestGroup : ConfigurationElement {
        [ConfigurationProperty("ID", IsKey = true, IsRequired = true)] public String ID { get { return ((String)base["ID"]).Trim(); } }
        [ConfigurationProperty("Revision", IsKey = false, IsRequired = true)] public String Revision { get { return ((String)base["Revision"]).Trim(); } }
        [ConfigurationProperty("Description", IsKey = false, IsRequired = true)] public String Description { get { return ((String)base["Description"]).Trim(); } }
        [ConfigurationProperty("Selectable", IsKey = false, IsRequired = true)] public Boolean Selectable { get { return ((Boolean)base["Selectable"]); } }
        [ConfigurationProperty("CancelOnFailure", IsKey = false, IsRequired = true)] public Boolean CancelOnFailure { get { return ((Boolean)base["CancelOnFailure"]); } }
        [ConfigurationProperty("TestMeasurementIDs", IsKey = false, IsRequired = true)] public String TestMeasurementIDs { get { return ((String)base["TestMeasurementIDs"]).Trim(); } }
    }

    public class TestMeasurementsSection : ConfigurationSection {
        public const String ClassName = nameof(TestMeasurementsSection);
        [ConfigurationProperty("TestMeasurements")] public TestMeasurements TestMeasurements { get { return ((TestMeasurements)(base["TestMeasurements"])); } }
    }
    [ConfigurationCollection(typeof(TestMeasurement))]
    public class TestMeasurements : ConfigurationElementCollection {
        public const String PropertyName = "TestMeasurement";
        public TestMeasurement this[Int32 idx] { get { return (TestMeasurement)this.BaseGet(idx); } }
        public override ConfigurationElementCollectionType CollectionType { get { return ConfigurationElementCollectionType.BasicMapAlternate; } }
        protected override String ElementName { get { return PropertyName; } }
        protected override Boolean IsElementName(String elementName) { return elementName.Equals(PropertyName, StringComparison.InvariantCultureIgnoreCase); }
        public override Boolean IsReadOnly() { return false; }
        protected override ConfigurationElement CreateNewElement() { return new TestMeasurement(); }
        protected override Object GetElementKey(ConfigurationElement element) { return ((TestMeasurement)(element)).ID; }
    }
    public class TestMeasurement : ConfigurationElement {
        [ConfigurationProperty("ID", IsKey = true, IsRequired = true)] public String ID { get { return ((String)base["ID"]).Trim(); } }
        [ConfigurationProperty("Revision", IsKey = false, IsRequired = true)] public String Revision { get { return ((String)base["Revision"]).Trim(); } }
        [ConfigurationProperty("Description", IsKey = false, IsRequired = true)] public String Description { get { return ((String)base["Description"]).Trim(); } }
        [ConfigurationProperty("ClassName", IsKey = false, IsRequired = true)] public String ClassName { get { return ((String)base["ClassName"]).Trim(); } }
        [ConfigurationProperty("CancelOnFailure", IsKey = false, IsRequired = true)] public Boolean CancelOnFailure { get { return ((Boolean)base["CancelOnFailure"]); } }
        [ConfigurationProperty("Arguments", IsKey = false, IsRequired = true)] public String Arguments { get { return ((String)base["Arguments"]).Trim(); } }
    }

    public class ConfigTests {
        public TestMeasurementsSection TestMeasurementsSection { get { return (TestMeasurementsSection)ConfigurationManager.GetSection(TestMeasurementsSection.ClassName); } }
        public TestMeasurements TestMeasurements { get { return this.TestMeasurementsSection.TestMeasurements; } }
        public IEnumerable<TestMeasurement> TestMeasurement { get { foreach (TestMeasurement tm in this.TestMeasurements) if (tm != null) yield return tm; } }
    }

    public class Operation {
        public readonly String ID;
        public readonly String Revision;
        public readonly String Description;
        public readonly String TestGroupIDs;

        private Operation(String id, String revision, String description, String testGroupIDs) {
            this.ID = id;
            this.Revision = revision;
            this.Description = description;
            this.TestGroupIDs = testGroupIDs;
        }

        public static Dictionary<String, Operation> Get() {
            TestOperationsSection testOperationsSection = (TestOperationsSection)ConfigurationManager.GetSection(TestOperationsSection.ClassName);
            TestOperations testOperations = testOperationsSection.TestOperations;
            Dictionary<String, Operation> dictionary = new Dictionary<String, Operation>();
            foreach (TestOperation to in testOperations) dictionary.Add(to.ID, new Operation(to.ID, to.Revision, to.Description, to.TestGroupIDs));
            return dictionary;
        }

        public static Operation Get(String TestOperationID) { return Get()[TestOperationID]; }
    }

    public class Group {
        public readonly String ID;
        public readonly String Revision;    
        public readonly String Description;
        public readonly Boolean Selectable;
        public readonly Boolean CancelOnFailure;
        public readonly String TestMeasurementIDs;

        private Group(String id, String revision, String description, Boolean selectable, Boolean cancelOnFailure, String testMeasurementIDs) {
            this.ID = id;
            this.Revision = revision;
            this.Description = description;
            this.Selectable = selectable;
            this.CancelOnFailure = cancelOnFailure;
            this.TestMeasurementIDs = testMeasurementIDs;
        }

        public static Dictionary<String, Group> Get() {
            TestGroupsSection testGroupSection = (TestGroupsSection)ConfigurationManager.GetSection(TestGroupsSection.ClassName);
            TestGroups testGroups = testGroupSection.TestGroups;
            Dictionary<String, Group> dictionary = new Dictionary<String, Group>();
            foreach (TestGroup tg in testGroups) dictionary.Add(tg.ID, new Group(tg.ID, tg.Revision, tg.Description, tg.Selectable, tg.CancelOnFailure, tg.TestMeasurementIDs));
            return dictionary;
        }

        public static Group Get(String TestGroupID) { return Get()[TestGroupID]; }
    }

    public class Measurement {
        public readonly String ID;
        public readonly String Revision;
        public readonly String Description;
        public readonly String ClassName;
        public readonly Object ClassObject;
        public readonly Boolean CancelOnFailure;
        public String GroupID { get; set; } = String.Empty; // Determined pre-test.
        public String Value { get; set; } = String.Empty; // Determined during test.
        public String Result { get; set; } = EventCodes.UNSET; // Determined post-test.
        public String Message { get; set; } = String.Empty; // Determined during test.

        private Measurement(String id, String revision, String description, String className, Boolean cancelOnFailure, String arguments) {
            this.ID = id;
            this.Revision = revision;
            this.Description = description;
            this.ClassName = className;
            this.ClassObject = Activator.CreateInstance(Type.GetType(this.GetType().Namespace + "." + this.ClassName), new Object[] { this.ID, arguments });
            this.CancelOnFailure = cancelOnFailure;
            if (String.Equals(this.ClassName, MeasurementNumeric.ClassName)) this.Value = Double.NaN.ToString();
        }

        public static Dictionary<String, Measurement> Get() {
            TestMeasurementsSection testMeasurementsSection = (TestMeasurementsSection)ConfigurationManager.GetSection(TestMeasurementsSection.ClassName);
            TestMeasurements testMeasurements = testMeasurementsSection.TestMeasurements;
            Dictionary<String, Measurement> dictionary = new Dictionary<String, Measurement>();
            foreach (TestMeasurement tm in testMeasurements) { dictionary.Add(tm.ID, new Measurement(tm.ID, tm.Revision, tm.Description, tm.ClassName, tm.CancelOnFailure, tm.Arguments)); }
            return dictionary;
        }

        public static Measurement Get(String TestMeasurementID) { return Get()[TestMeasurementID]; }
    }
}
