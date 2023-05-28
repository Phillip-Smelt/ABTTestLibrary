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

    public class TestOperationsSection : ConfigurationSection { [ConfigurationProperty("TestOperations")] public TestElements TestOperations { get { return ((TestElements)(base["TestOperations"])); } } }
    [ConfigurationCollection(typeof(TestElement))]
    public class TestOperations : TestElements { public new const String PropertyName = "TestOperations"; }
    public class TestOperation : TestElement {
        [ConfigurationProperty("TestGroups", IsKey = false, IsRequired = true)] public String TestGroups { get { return ((String)base["TestGroups"]).Trim(); } }
    }

    public class TestGroupsSection : ConfigurationSection { [ConfigurationProperty("TestOperations")] public TestElements TestOperations { get { return ((TestElements)(base["TestOperations"])); } } }
    [ConfigurationCollection(typeof(TestElement))]
    public class TestGroups : TestElements { public new const String PropertyName = "TestGroups"; }
    public class TestGroup : TestElement {
        [ConfigurationProperty("Tests", IsKey = false, IsRequired = true)] public String Tests { get { return ((String)base["Tests"]).Trim(); } }
    }

    public class Group {
        public readonly String ID;
        public readonly Boolean Required;
        public readonly String Revision;    
        public readonly String Description;
        public readonly String TestIDs;

        private Group(String id, Boolean required, String revision, String description, String testIDs) {
            this.ID = id;
            this.Required = required;
            this.Revision = revision;
            this.Description = description;
            this.TestIDs = testIDs;
        }

        public static Dictionary<String, Group> Get() {
            // TODO: Add verification of App.config's NodesMiddleSection fields.
            NodesMiddleSection nodesMiddle = NodesMiddleSection.NodeMiddle;
            Dictionary<String, Group> dictionary = new Dictionary<String, Group>();
            foreach (NodeMiddle nodeMiddle in nodesMiddle) dictionary.Add(nodeMiddle.ID, new Group(nodeMiddle.ID, nodeMiddle.Revision, nodeMiddle.Description, nodeMiddle.NodesBottom));
            return dictionary;
        }
    }
}
