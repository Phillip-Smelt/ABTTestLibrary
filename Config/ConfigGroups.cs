using System;
using System.Collections.Generic;
using System.Configuration;

namespace ABTTestLibrary.Config {
    public class GroupElement : ConfigurationElement {
        [ConfigurationProperty("ID", IsKey = true, IsRequired = true)] public String ID { get { return (String)base["ID"]; } }
        [ConfigurationProperty("Required", IsKey = false, IsRequired = true)] public Boolean Required { get { return (Boolean)base["Required"]; } }
        [ConfigurationProperty("Summary", IsKey = false, IsRequired = true)] public String Summary { get { return (String)base["Summary"]; } }
        [ConfigurationProperty("Detail", IsKey = false, IsRequired = false)] public String Detail { get { return (String)base["Detail"]; } }
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

    public class Group {
        public String ID { get; private set; }
        public Boolean Required { get; private set; }
        public String Summary { get; private set; }
        public String Detail { get; private set; }
        public String TestIDs { get; private set; }

        private Group(String ID, Boolean Required, String Summary, String Detail, String TestIDs) {
            this.ID = ID;
            this.Required = Required;
            this.Summary = Summary;
            this.Detail = Detail;
            this.TestIDs = TestIDs;
        }

        public static Dictionary<String, Group> Get() {
            GroupElementsSection s = (GroupElementsSection)ConfigurationManager.GetSection("GroupElementsSection");
            GroupElements e = s.GroupElements;
            Dictionary<String, Group> d = new Dictionary<String, Group>();
            foreach (GroupElement ge in e) d.Add(ge.ID, new Group(ge.ID, ge.Required, ge.Summary, ge.Detail, ge.TestIDs));
            return d;
        }
    }
}
