using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TestLibrary.AppConfig {
    public partial class GroupSelect : Form {
        public String GroupSelected;
        internal Dictionary<String, Group> Groups { get; private set; }
        private readonly List<String> _keysRequired;
        private readonly List<String> _keysNotRequired;

        public GroupSelect(Dictionary<String, Group> groups) {
            this.InitializeComponent();
            this.Groups = groups;
            this._keysRequired = this.Groups.Where(g => (g.Value.Required)).Select(g => g.Key).ToList();
            this._keysNotRequired = this.Groups.Where(g => (!g.Value.Required)).Select(g => g.Key).ToList();
            this.ListGroups.MultiSelect = false;

            this.ListViewRefresh();
            this.radioButtonRequired.Enabled = (this._keysRequired.Count > 0);
            this.radioButtonNotRequired.Enabled = (this._keysNotRequired.Count > 0);
            this.FormRefresh();
        }

        private void ListViewRefresh() {
            this.ListGroups.Clear();
            this.ListGroups.View = View.Details;
            this.ListGroups.Columns.Add("ID");
            this.ListGroups.Columns.Add("Description");
        }

        private void FormRefresh() {
            if (this.radioButtonRequired.Checked) foreach (String key in this._keysRequired) this.ListGroups.Items.Add(new ListViewItem(new String[] { this.Groups[key].ID, this.Groups[key].Description }));
            else if (this.radioButtonNotRequired.Checked) foreach (String key in this._keysNotRequired) this.ListGroups.Items.Add(new ListViewItem(new String[] { this.Groups[key].ID, this.Groups[key].Description }));
            this.ListGroups.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent);
            this.ListGroups.Columns[1].Width = -2;
            // https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.columnheader.width?redirectedfrom=MSDN&view=windowsdesktop-7.0#System_Windows_Forms_ColumnHeader_Width
            this.ListGroups.ResetText();
            this.OK.Enabled = false;
        }

        private void OK_Click(Object sender, EventArgs e) {
            if (this.ListGroups.SelectedItems.Count == 1) {
                this.GroupSelected = this.ListGroups.SelectedItems[0].Text;
                this.DialogResult = DialogResult.OK;
            }
        }

        public static String Get(Dictionary<String, Group> groups) {
            GroupSelect gs = new GroupSelect(groups);
            gs.ShowDialog(); // Waits until user clicks OK button.
            String g = gs.GroupSelected;
            gs.Dispose();
            return g;
        }

        private void ListGroups_SelectionChanged(Object sender, ListViewItemSelectionChangedEventArgs e) {
            this.OK.Enabled = true;
        }

        private void GroupBoxRequired_CheckedChanged(Object sender, EventArgs e) {
            if (((RadioButton)sender).Checked) { // Do stuff only if the radio button is checked (or the action will run twice).
                this.ListViewRefresh();
                this.FormRefresh();
            }
        }
    }
}
