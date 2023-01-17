using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TestLibrary.Config {
    public partial class GroupSelect : Form {
        public String GroupSelected;
        internal Dictionary<String, Group> _groups { get; private set; }
        private List<String> _keysRequired;
        private List<String> _keysNotRequired;

        public GroupSelect(Dictionary<String, Group> Groups) {
            this.InitializeComponent();
            this._groups = Groups;
            this._keysRequired = this._groups.Where(g => (g.Value.Required)).Select(g => g.Key).ToList();
            this._keysNotRequired = this._groups.Where(g => (!g.Value.Required)).Select(g => g.Key).ToList();
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
            this.ListGroups.Columns.Add("Summary");
        }

        private void FormRefresh() {
            if (this.radioButtonRequired.Checked) foreach (String key in this._keysRequired) this.ListGroups.Items.Add(new ListViewItem(new String[] { this._groups[key].ID, this._groups[key].Name }));
            else if (this.radioButtonNotRequired.Checked) foreach (String key in this._keysNotRequired) this.ListGroups.Items.Add(new ListViewItem(new String[] { this._groups[key].ID, this._groups[key].Name }));
            this.ListGroups.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent);
            this.ListGroups.Columns[1].Width = -2;
            // https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.columnheader.width?redirectedfrom=MSDN&view=windowsdesktop-7.0#System_Windows_Forms_ColumnHeader_Width
            this.ListGroups.ResetText();
            this.OK.Enabled = false;
        }

        private void OK_Click(object sender, EventArgs e) {
            if (this.ListGroups.SelectedItems.Count == 1) {
                this.GroupSelected = this.ListGroups.SelectedItems[0].Text;
                this.DialogResult = DialogResult.OK;
            }
        }

        public static String Get(Dictionary<String, Group> Groups) {
            GroupSelect gs = new GroupSelect(Groups);
            gs.ShowDialog(); // Waits until user clicks OK button.
            String g = gs.GroupSelected;
            gs.Dispose();
            return g;
        }

        private void ListGroups_SelectionChanged(Object sender, ListViewItemSelectionChangedEventArgs e) {
            this.OK.Enabled = true;
        }

        private void groupBoxRequired_CheckedChanged(Object sender, EventArgs e) {
            if (((RadioButton)sender).Checked) { // Do stuff only if the radio button is checked (or the action will run twice).
                this.ListViewRefresh();
                this.FormRefresh();
            }
        }
    }
}
