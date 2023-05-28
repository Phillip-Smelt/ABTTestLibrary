using System;
using System.Collections.Generic;
using System.Configuration;

namespace ABT.TestSpace.AppConfig {
    public class Node : ConfigurationElement {
        [ConfigurationProperty("ID", IsKey = true, IsRequired = true)] public String ID { get { return ((String)base["ID"]).Trim(); } }
        [ConfigurationProperty("Revision", IsKey = false, IsRequired = true)] public String Revision { get { return ((String)base["Revision"]).Trim(); } }
        [ConfigurationProperty("Description", IsKey = false, IsRequired = true)] public String Description { get { return ((String)base["Description"]).Trim(); } }
        [ConfigurationProperty("Nodes", IsKey = false, IsRequired = true)] public String Nodes { get { return ((String)base["Nodes"]).Trim(); } }
    }

    [ConfigurationCollection(typeof(Node))]
    public class Nodes : ConfigurationElementCollection {
        public const String PropertyName = "Node";
        public Node this[Int32 idx] { get { return (Node)this.BaseGet(idx); } }
        public override ConfigurationElementCollectionType CollectionType { get { return ConfigurationElementCollectionType.BasicMapAlternate; } }
        protected override String ElementName { get { return PropertyName; } }
        protected override Boolean IsElementName(String elementName) { return elementName.Equals(PropertyName, StringComparison.InvariantCultureIgnoreCase); }
        public override Boolean IsReadOnly() { return false; }
        protected override ConfigurationElement CreateNewElement() { return new Node(); }
        protected override Object GetElementKey(ConfigurationElement element) { return ((Node)(element)).ID; }
    }
    public class NodesSection : ConfigurationSection { [ConfigurationProperty("Nodes")] public Nodes Node { get { return ((Nodes)(base["Nodes"])); } } }

    public class TestProgram : Node { [ConfigurationProperty("TestGroups", IsKey = false, IsRequired = true)] public String TestGroups { get { return ((String)base["TestGroups"]).Trim(); } } }
    [ConfigurationCollection(typeof(Node))]
    public class TestPrograms : Nodes { }
    public class TestProgramsSection : Nodes { }

    public class TestGroup: Node { [ConfigurationProperty("Tests", IsKey = false, IsRequired = true)] public String Tests { get { return ((String)base["Tests"]).Trim(); } } }
    [ConfigurationCollection(typeof(Node))]
    public class TestGroups : Nodes { }
    public class TestGroupsSection : Nodes { }

    public class Test : Node { }
    [ConfigurationCollection(typeof(Node))]
    public class Tests : Nodes { }
    public class TestsSection : NodesSection { }

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
