using System;
using System.Collections.Generic;
using System.Configuration;

namespace ABT.TestSpace.AppConfig {
    public class GroupElement : ConfigurationElement {
        [ConfigurationProperty("ID", IsKey = true, IsRequired = true)] public String ID { get { return ((String)base["ID"]).Trim(); } }
        [ConfigurationProperty("Required", IsKey = false, IsRequired = true)] public Boolean Required { get { return (Boolean)base["Required"]; } }
        [ConfigurationProperty("Revision", IsKey = false, IsRequired = true)] public String Revision { get { return ((String)base["Revision"]).Trim(); } }
        [ConfigurationProperty("Description", IsKey = false, IsRequired = true)] public String Description { get { return ((String)base["Description"]).Trim(); } }
        [ConfigurationProperty("TestIDs", IsKey = false, IsRequired = true)] public String TestIDs { get { return ((String)base["TestIDs"]).Trim(); } }
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

    public class GroupsSection : ConfigurationSection {
        [ConfigurationProperty("GroupElements")] public GroupElements GroupElements { get { return ((GroupElements)(base["GroupElements"])); } }
    }

    //public class AppConfigGroups {
    //    public GroupsSection GroupsSection { get { return (GroupsSection)ConfigurationManager.GetSection("GroupsSection"); } }
    //    public GroupElements GroupElements { get { return this.GroupsSection.GroupElements; } }
    //    public IEnumerable<GroupElement> GroupElement { get { foreach (GroupElement ge in this.GroupElements) if (ge != null) yield return ge; } }
    //}

    // NOTE: Possibly desirable to implement a 3rd tier of ConfigurationCollections:
    //  - Currently is Groups to Tests.
    //  - 3rd tier would be Groups to SubGroups to Tests.
    //  - Would permit Groups to relate to SubGroups like 'Shorts', 'Opens', 'Analog to Digital", "In System-Programming", "Boundary Scan", etc.
    //  - Implementation would simply be another nested foreach loop like below:
    //      Dictionary<String, Group> dictionary = new Dictionary<String, Group>();
    //      foreach (GroupElement groupElement in groupElements) {
    //          foreach (SubGroupElement sge in groupElements) dictionary.Add(sge.ID, new Group(sge.ID, sge.Required, sge.Revision, sge.Summary, sge.Detail, sge.TestIDs));
    //      }
    //      return dictionary;
    //  - Possibly desirable if there are numerous Tests, too many to easily keep track of, and organizing into SubGroups would help.
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
            // TODO: Add verification of App.config's GroupsSection fields.
            GroupsSection groupElementsSection = (GroupsSection)ConfigurationManager.GetSection("GroupsSection");
            GroupElements groupElements = groupElementsSection.GroupElements;
            Dictionary<String, Group> dictionary = new Dictionary<String, Group>();
            foreach (GroupElement groupElement in groupElements) dictionary.Add(groupElement.ID, new Group(groupElement.ID, groupElement.Required, groupElement.Revision, groupElement.Description, groupElement.TestIDs));
            return dictionary;
        }
    }
}
