using System;
using System.Collections.Generic;
using System.Configuration;

namespace TestLibrary.Config {
    // NOTE: If TestLibrary transitions from current C# 7.3 to ≥ C# 8.0,add 'readonly'
    // modifier to all { get; private set; } fields in namespace TestLibrary.Config.
    // NOTE: Add verification of App.config key-value pairs & configuration collections into namespace TestLibrary.Config.
    // That is, check for errors in the .Get methods, rather than current checks in TestSupport.TestTasks.EvaluateTestResult().
    public class GroupElement : ConfigurationElement {
        [ConfigurationProperty("ID", IsKey = true, IsRequired = true)] public String ID { get { return (String)base["ID"]; } }
        [ConfigurationProperty("Required", IsKey = false, IsRequired = true)] public Boolean Required { get { return (Boolean)base["Required"]; } }
        [ConfigurationProperty("Revision", IsKey = false, IsRequired = true)] public String Revision { get { return (String)base["Revision"]; } }
        [ConfigurationProperty("Name", IsKey = false, IsRequired = true)] public String Name { get { return (String)base["Name"]; } }
        [ConfigurationProperty("Description", IsKey = false, IsRequired = true)] public String Description { get { return (String)base["Description"]; } }
        [ConfigurationProperty("TestIDs", IsKey = false, IsRequired = true)] public String TestIDs { get { return (String)base["TestIDs"]; } }
    }

    [ConfigurationCollection(typeof(GroupElement))]
    public class GroupElements : ConfigurationElementCollection {
        public const String PropertyName = "GroupElement";
        public GroupElement this[Int32 idx] { get { return (GroupElement)BaseGet(idx); } }
        public override ConfigurationElementCollectionType CollectionType { get { return ConfigurationElementCollectionType.BasicMapAlternate; } }
        protected override String ElementName { get { return PropertyName; } }
        protected override bool IsElementName(String elementName) { return elementName.Equals(PropertyName, StringComparison.InvariantCultureIgnoreCase); }
        public override bool IsReadOnly() { return false; }
        protected override ConfigurationElement CreateNewElement() { return new GroupElement(); }
        protected override object GetElementKey(ConfigurationElement element) { return ((GroupElement)(element)).ID; }
    }

    public class GroupElementsSection : ConfigurationSection {
        [ConfigurationProperty("GroupElements")] public GroupElements GroupElements { get { return ((GroupElements)(base["GroupElements"])); } }
    }

    public class ConfigGroups {
        public GroupElementsSection GroupElementsSection { get { return (GroupElementsSection)ConfigurationManager.GetSection("GroupElementsSection"); } }
        public GroupElements GroupElements { get { return this.GroupElementsSection.GroupElements; } }
        public IEnumerable<GroupElement> GroupElement { get { foreach (GroupElement ge in this.GroupElements) if (ge != null) yield return ge; } }
    }

    // NOTE: Possibly desirable to implement a 3rd tier of ConfigurationCollections:
    //  - Currently is Groups to Tests.
    //  - 3rd tier would be Groups to SubGroups to Tests.
    //  - Would permit Groups to relate to SubGroups like 'Shorts', 'Opens', 'Analog to Digital", "In System-Programming", "Boundary Scan", etc.
    //  - Implementation would simply be another nested foreach loop like below:
    //      Dictionary<String, Group> d = new Dictionary<String, Group>();
    //      foreach (GroupElement ge in e) {
    //          foreach (SubGroupElement sge in ge) d.Add(sge.ID, new Group(sge.ID, sge.Required, sge.Revision, sge.Summary, sge.Detail, sge.TestIDs));
    //      }
    //      return d;
    //  - Possibly desirable if there are numerous Tests, too many to easily keep track of, and organizing into SubGroups would help.
    public class Group {
        public String ID { get; private set; }
        public Boolean Required { get; private set; }
        public String Revision { get; private set; }
        public String Name { get; private set; }
        public String Description { get; private set; }
        public String TestIDs { get; private set; }

        private Group(String ID, Boolean Required, String Revision, String Name, String Description, String TestIDs) {
            this.ID = ID;
            this.Required = Required;
            this.Revision = Revision;
            this.Name = Name;
            this.Description = Description;
            this.TestIDs = TestIDs;
        }

        public static Dictionary<String, Group> Get() {
            GroupElementsSection s = (GroupElementsSection)ConfigurationManager.GetSection("GroupElementsSection");
            GroupElements e = s.GroupElements;
            Dictionary<String, Group> d = new Dictionary<String, Group>();
            foreach (GroupElement ge in e) d.Add(ge.ID, new Group(ge.ID, ge.Required, ge.Revision, ge.Name, ge.Description, ge.TestIDs));
            return d;
        }
    }
}
