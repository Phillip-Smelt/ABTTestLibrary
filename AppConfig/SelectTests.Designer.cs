using System.ComponentModel;
using System.Windows.Forms;

namespace ABT.TestSpace.AppConfig {
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
            this.LabelSelections = new System.Windows.Forms.Label();
            this.OK = new System.Windows.Forms.Button();
            this.ListSelections = new System.Windows.Forms.ListView();
            this.radioButtonRequired = new System.Windows.Forms.RadioButton();
            this.radioButtonOptional = new System.Windows.Forms.RadioButton();
            this.selectionType = new System.Windows.Forms.GroupBox();
            this.selectionType.SuspendLayout();
            this.SuspendLayout();
            // 
            // LabelSelections
            // 
            this.LabelSelections.AutoSize = true;
            this.LabelSelections.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.LabelSelections.Location = new System.Drawing.Point(12, 6);
            this.LabelSelections.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LabelSelections.Name = "LabelSelections";
            this.LabelSelections.Size = new System.Drawing.Size(56, 13);
            this.LabelSelections.TabIndex = 0;
            this.LabelSelections.Text = "Selections";
            this.LabelSelections.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // OK
            // 
            this.OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK.Enabled = false;
            this.OK.Location = new System.Drawing.Point(310, 297);
            this.OK.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(58, 36);
            this.OK.TabIndex = 4;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // ListSelections
            // 
            this.ListSelections.FullRowSelect = true;
            this.ListSelections.GridLines = true;
            this.ListSelections.HideSelection = false;
            this.ListSelections.LabelWrap = false;
            this.ListSelections.Location = new System.Drawing.Point(10, 22);
            this.ListSelections.MultiSelect = false;
            this.ListSelections.Name = "ListSelections";
            this.ListSelections.ShowGroups = false;
            this.ListSelections.Size = new System.Drawing.Size(666, 254);
            this.ListSelections.TabIndex = 0;
            this.ListSelections.UseCompatibleStateImageBehavior = false;
            this.ListSelections.View = System.Windows.Forms.View.Details;
            this.ListSelections.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.ListGroups_SelectionChanged);
            // 
            // radioButtonRequired
            // 
            this.radioButtonRequired.AutoSize = true;
            this.radioButtonRequired.Location = new System.Drawing.Point(5, 15);
            this.radioButtonRequired.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.radioButtonRequired.Name = "radioButtonRequired";
            this.radioButtonRequired.Size = new System.Drawing.Size(68, 17);
            this.radioButtonRequired.TabIndex = 2;
            this.radioButtonRequired.TabStop = true;
            this.radioButtonRequired.Text = "Required";
            this.radioButtonRequired.UseVisualStyleBackColor = true;
            this.radioButtonRequired.CheckedChanged += new System.EventHandler(this.GroupBoxSelect_CheckedChanged);
            // 
            // radioButtonOptional
            // 
            this.radioButtonOptional.AutoSize = true;
            this.radioButtonOptional.Location = new System.Drawing.Point(4, 36);
            this.radioButtonOptional.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.radioButtonOptional.Name = "radioButtonOptional";
            this.radioButtonOptional.Size = new System.Drawing.Size(64, 17);
            this.radioButtonOptional.TabIndex = 3;
            this.radioButtonOptional.TabStop = true;
            this.radioButtonOptional.Text = "Optional";
            this.radioButtonOptional.UseVisualStyleBackColor = true;
            this.radioButtonOptional.CheckedChanged += new System.EventHandler(this.GroupBoxSelect_CheckedChanged);
            // 
            // selectionType
            // 
            this.selectionType.Controls.Add(this.radioButtonOptional);
            this.selectionType.Controls.Add(this.radioButtonRequired);
            this.selectionType.Location = new System.Drawing.Point(9, 281);
            this.selectionType.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.selectionType.Name = "selectionType";
            this.selectionType.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.selectionType.Size = new System.Drawing.Size(104, 57);
            this.selectionType.TabIndex = 1;
            this.selectionType.TabStop = false;
            // 
            // SelectTests
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 379);
            this.ControlBox = false;
            this.Controls.Add(this.selectionType);
            this.Controls.Add(this.ListSelections);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.LabelSelections);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectTests";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Tests";
            this.Load += new System.EventHandler(this.SelectTests_Load);
            this.selectionType.ResumeLayout(false);
            this.selectionType.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private Label LabelSelections;
        private Button OK;
        private ListView ListSelections;
        private RadioButton radioButtonRequired;
        private RadioButton radioButtonOptional;
        private GroupBox selectionType;
    }
}