using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ABTTestLibrary.Config {
    public partial class GroupSelect : Form {
        public String GroupSelected;
        public GroupSelect(Dictionary<String, Group> Groups) {
            InitializeComponent();
            this.ListGroups.View = View.Details;
            this.ListGroups.Columns.Add("ID");
            this.ListGroups.Columns.Add("Required?");
            this.ListGroups.Columns.Add("Summary");
            String[] item = new String[3];
            foreach (KeyValuePair<String, Group> kvp in Groups) {
                item[0] = kvp.Value.ID;
                item[1] = kvp.Value.Required.ToString();
                item[2] = kvp.Value.Summary;
                this.ListGroups.Items.Add(new ListViewItem(item));
            }
            this.ListGroups.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent);
            this.ListGroups.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.HeaderSize);
            this.ListGroups.Columns[2].Width = -2;
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
    }
}
