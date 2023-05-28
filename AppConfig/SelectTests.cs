using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ABT.TestSpace.AppConfig {
    public partial class SelectTests : Form {
        public String Selection { get; private set; }
        internal readonly Dictionary<String, Group> Groups;
        private readonly List<String> _keysRequired;
        private readonly List<String> _keysOptional;

        public SelectTests(Dictionary<String, Group> groups) {
            this.InitializeComponent();
            this.Groups = groups;
            this._keysRequired = this.Groups.Where(g => (g.Value.Required)).Select(g => g.Key).ToList();
            this._keysOptional = this.Groups.Where(g => (!g.Value.Required)).Select(g => g.Key).ToList();
            this.ListSelections.MultiSelect = false;

            this.ListViewRefresh();
            this.radioButtonRequired.Enabled = (this._keysRequired.Count > 0);
            this.radioButtonOptional.Enabled = (this._keysOptional.Count > 0);
            this.FormRefresh();
        }

        private void ListViewRefresh() {
            this.ListSelections.Clear();
            this.ListSelections.View = View.Details;
            this.ListSelections.Columns.Add("ID");
            this.ListSelections.Columns.Add("Description");
        }

        private void FormRefresh() {
            if (this.radioButtonRequired.Checked) foreach (String key in this._keysRequired) this.ListSelections.Items.Add(new ListViewItem(new String[] { this.Groups[key].ID, this.Groups[key].Description }));
            else if (this.radioButtonOptional.Checked) foreach (String key in this._keysOptional) this.ListSelections.Items.Add(new ListViewItem(new String[] { this.Groups[key].ID, this.Groups[key].Description }));
            this.ListSelections.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent);
            this.ListSelections.Columns[1].Width = -2;
            // https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.columnheader.width?redirectedfrom=MSDN&view=windowsdesktop-7.0#System_Windows_Forms_ColumnHeader_Width
            this.ListSelections.ResetText();
            this.OK.Enabled = false;
        }

        private void OK_Click(Object sender, EventArgs e) {
            if (this.ListSelections.SelectedItems.Count == 1) {
                this.Selection = this.ListSelections.SelectedItems[0].Text;
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

        private void GroupBoxSelect_CheckedChanged(Object sender, EventArgs e) {
            if (((RadioButton)sender).Checked) { // Do stuff only if the radio button is checked (or the action will run twice).
                this.ListViewRefresh();
                this.FormRefresh();
            }
        }

        private void SelectTests_Load(Object sender, EventArgs e) {

        }
    }
}
