using System.ComponentModel;
using System.Windows.Forms;

namespace ABT.TestSpace.TestExec.AppConfig {
    partial class SelectTests {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region
        private void InitializeComponent() {
            LabelSelections = new System.Windows.Forms.Label();
            OK = new System.Windows.Forms.Button();
            ListSelections = new System.Windows.Forms.ListView();
            radioButtonTestOperations = new System.Windows.Forms.RadioButton();
            radioButtonTestGroups = new System.Windows.Forms.RadioButton();
            selectionType = new System.Windows.Forms.GroupBox();
            selectionType.SuspendLayout();
            SuspendLayout();
            // 
            // LabelSelections
            // 
            LabelSelections.AutoSize = true;
            LabelSelections.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            LabelSelections.Location = new System.Drawing.Point(16, 7);
            LabelSelections.Name = "LabelSelections";
            LabelSelections.Size = new System.Drawing.Size(70, 16);
            LabelSelections.TabIndex = 0;
            LabelSelections.Text = "Selections";
            LabelSelections.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // OK
            // 
            OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            OK.Enabled = false;
            OK.Location = new System.Drawing.Point(413, 366);
            OK.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            OK.Name = "OK";
            OK.Size = new System.Drawing.Size(77, 44);
            OK.TabIndex = 4;
            OK.Text = "OK";
            OK.UseVisualStyleBackColor = true;
            OK.Click += new System.EventHandler(OK_Click);
            // 
            // ListSelections
            // 
            ListSelections.FullRowSelect = true;
            ListSelections.GridLines = true;
            ListSelections.HideSelection = false;
            ListSelections.LabelWrap = false;
            ListSelections.Location = new System.Drawing.Point(13, 27);
            ListSelections.Margin = new System.Windows.Forms.Padding(4);
            ListSelections.MultiSelect = false;
            ListSelections.Name = "ListSelections";
            ListSelections.ShowGroups = false;
            ListSelections.Size = new System.Drawing.Size(887, 312);
            ListSelections.TabIndex = 0;
            ListSelections.UseCompatibleStateImageBehavior = false;
            ListSelections.View = System.Windows.Forms.View.Details;
            ListSelections.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(List_SelectionChanged);
            // 
            // radioButtonTestOperations
            // 
            radioButtonTestOperations.AutoSize = true;
            radioButtonTestOperations.Location = new System.Drawing.Point(7, 18);
            radioButtonTestOperations.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            radioButtonTestOperations.Name = "radioButtonTestOperations";
            radioButtonTestOperations.Size = new System.Drawing.Size(327, 20);
            radioButtonTestOperations.TabIndex = 2;
            radioButtonTestOperations.TabStop = true;
            radioButtonTestOperations.Text = "FilePro Traveler Operations for Production Testing";
            radioButtonTestOperations.UseVisualStyleBackColor = true;
            radioButtonTestOperations.CheckedChanged += new System.EventHandler(GroupBoxSelect_CheckedChanged);
            // 
            // radioButtonTestGroups
            // 
            radioButtonTestGroups.AutoSize = true;
            radioButtonTestGroups.Location = new System.Drawing.Point(5, 44);
            radioButtonTestGroups.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            radioButtonTestGroups.Name = "radioButtonTestGroups";
            radioButtonTestGroups.Size = new System.Drawing.Size(225, 20);
            radioButtonTestGroups.TabIndex = 3;
            radioButtonTestGroups.TabStop = true;
            radioButtonTestGroups.Text = "Test Groups for Trouble-shooting";
            radioButtonTestGroups.UseVisualStyleBackColor = true;
            radioButtonTestGroups.CheckedChanged += new System.EventHandler(GroupBoxSelect_CheckedChanged);
            // 
            // selectionType
            // 
            selectionType.Controls.Add(radioButtonTestGroups);
            selectionType.Controls.Add(radioButtonTestOperations);
            selectionType.Location = new System.Drawing.Point(12, 346);
            selectionType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            selectionType.Name = "selectionType";
            selectionType.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            selectionType.Size = new System.Drawing.Size(355, 70);
            selectionType.TabIndex = 1;
            selectionType.TabStop = false;
            // 
            // SelectTests
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(912, 466);
            ControlBox = false;
            Controls.Add(selectionType);
            Controls.Add(ListSelections);
            Controls.Add(OK);
            Controls.Add(LabelSelections);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SelectTests";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Select Tests";
            Load += new System.EventHandler(SelectTests_Load);
            selectionType.ResumeLayout(false);
            selectionType.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }
        #endregion
        private Label LabelSelections;
        private Button OK;
        private ListView ListSelections;
        private RadioButton radioButtonTestOperations;
        private RadioButton radioButtonTestGroups;
        private GroupBox selectionType;
    }
}