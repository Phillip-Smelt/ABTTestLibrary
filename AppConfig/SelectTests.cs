﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ABT.TestSpace.AppConfig {
    public partial class SelectTests : Form {
        public String Selection { get; private set; }
        internal readonly Dictionary<String, Operation> Operations;
        internal readonly Dictionary<String, Group> Groups;

        public SelectTests(Dictionary<String, Operation> testOperations, Dictionary<String, Group> testGroups) {
            this.InitializeComponent();
            this.Operations = testOperations;
            this.Groups = testGroups;
            this.ListSelections.MultiSelect = false;
            this.ListViewRefresh();
            this.FormRefresh();
        }

        private void ListViewRefresh() {
            this.ListSelections.Clear();
            this.ListSelections.View = View.Details;
            this.ListSelections.Columns.Add("ID");
            this.ListSelections.Columns.Add("Description");
            this.ListSelections.Columns.Add("IDs");
        }

        private void FormRefresh() {
            if (this.radioButtonTestOperations.Checked) 
                foreach (KeyValuePair<String, Operation> kvp in this.Operations) 
                    this.ListSelections.Items.Add(new ListViewItem(new String[] { kvp.Key, kvp.Value.Description, kvp.Value.TestGroupIDs }));
            else 
                foreach (KeyValuePair<String, Group> kvp in this.Groups)
                    this.ListSelections.Items.Add(new ListViewItem(new String[] { kvp.Key, kvp.Value.Description, kvp.Value.TestMeasurementIDs }));
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

        public static (Boolean IsOperation, String TestElementID) Get(Dictionary<String, Operation> testOperations, Dictionary<String, Group> testGroups) {
            SelectTests st = new SelectTests(testOperations, testGroups);
            st.ShowDialog(); // Waits until user clicks OK button.
            Boolean isOperation = st.radioButtonTestOperations.Checked;
            String testElementID = st.ListSelections.SelectedItems[0].Text;
            st.Dispose();
            return (isOperation, testElementID);
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
