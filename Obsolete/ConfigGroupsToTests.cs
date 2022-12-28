using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace ABTTestLibrary.AppConfig {
    public class GroupsToTestsElement : ConfigurationElement {
        [ConfigurationProperty("IDGroup", IsKey = true, IsRequired = true)] public String IDGroup { get { return (String)base["IDGroup"]; } }
        [ConfigurationProperty("IDTests", IsKey = true, IsRequired = true)] public String IDTests { get { return (String)base["IDTests"]; } }
    }

    [ConfigurationCollection(typeof(GroupsToTestsElement))]
    public class GroupsToTestsElements : ConfigurationElementCollection {
        public const String PropertyName = "GroupsToTestsElement";
        public GroupsToTestsElement this[Int32 idx] { get { return (GroupsToTestsElement)BaseGet(idx); } }
        public override ConfigurationElementCollectionType CollectionType { get { return ConfigurationElementCollectionType.BasicMapAlternate; } }
        protected override String ElementName { get { return PropertyName; } }
        protected override bool IsElementName(String elementName) { return elementName.Equals(PropertyName, StringComparison.InvariantCultureIgnoreCase); }
        public override bool IsReadOnly() { return false; }
        protected override ConfigurationElement CreateNewElement() { return new GroupsToTestsElement(); }
        protected override object GetElementKey(ConfigurationElement element) { return (((GroupsToTestsElement)(element)).IDGroup, ((GroupsToTestsElement)(element)).IDTests); }
    }

    public class GroupsToTestsElementsSection : ConfigurationSection {
        [ConfigurationProperty("GroupsToTestsElements")] public GroupsToTestsElements GroupsToTestsElements { get { return ((GroupsToTestsElements)(base["GroupsToTestsElements"])); } }
    }

    public class ConfigGroupsToTests {
        public GroupsToTestsElementsSection GroupsToTestsElementsSection { get { return (GroupsToTestsElementsSection)ConfigurationManager.GetSection("GroupsToTestsElementsSection"); } }
        public GroupsToTestsElements GroupsToTestsElements { get { return this.GroupsToTestsElementsSection.GroupsToTestsElements; } }
        public IEnumerable<GroupsToTestsElement> GroupsToTestsElement { get { foreach (GroupsToTestsElement ge in this.GroupsToTestsElements) if (ge != null) yield return ge; } }
    }

    public class GroupsToTests {
        public String IDGroup { get; private set; }
        public String IDTests { get; private set; }
        public static Char testsSepChar = '|';

        private GroupsToTests(String IDGroup, String IDTests) {
            this.IDGroup = IDGroup;
            this.IDTests = IDTests;
        }

        public static Dictionary<String, String> Get() {
            GroupsToTestsElementsSection s = (GroupsToTestsElementsSection)ConfigurationManager.GetSection("GroupsToTestsElementsSection");
            GroupsToTestsElements e = s.GroupsToTestsElements;
            Dictionary<String, String> d = new Dictionary<String, String>();
            foreach (GroupsToTestsElement gtte in e) d.Add(gtte.IDGroup, gtte.IDTests);
            return d;
        }
    }
}
