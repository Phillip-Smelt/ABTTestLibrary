using System.ComponentModel;
using System.Windows.Forms;

namespace TestLibrary.Config {
    partial class GroupSelect {
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
            this.LabelGroups = new System.Windows.Forms.Label();
            this.OK = new System.Windows.Forms.Button();
            this.ListGroups = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // LabelGroups
            // 
            this.LabelGroups.AutoSize = true;
            this.LabelGroups.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.LabelGroups.Location = new System.Drawing.Point(16, 13);
            this.LabelGroups.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LabelGroups.Name = "LabelGroups";
            this.LabelGroups.Size = new System.Drawing.Size(65, 13);
            this.LabelGroups.TabIndex = 0;
            this.LabelGroups.Text = "Test Groups";
            this.LabelGroups.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // OK
            // 
            this.OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK.Enabled = false;
            this.OK.Location = new System.Drawing.Point(169, 298);
            this.OK.Margin = new System.Windows.Forms.Padding(2);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(58, 36);
            this.OK.TabIndex = 1;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // ListGroups
            // 
            this.ListGroups.FullRowSelect = true;
            this.ListGroups.GridLines = true;
            this.ListGroups.HideSelection = false;
            this.ListGroups.LabelWrap = false;
            this.ListGroups.Location = new System.Drawing.Point(12, 39);
            this.ListGroups.MultiSelect = false;
            this.ListGroups.Name = "ListGroups";
            this.ListGroups.ShowGroups = false;
            this.ListGroups.Size = new System.Drawing.Size(366, 254);
            this.ListGroups.TabIndex = 0;
            this.ListGroups.UseCompatibleStateImageBehavior = false;
            this.ListGroups.View = System.Windows.Forms.View.Details;
            this.ListGroups.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.ListGroups_SelectionChanged);
            // 
            // GroupSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 339);
            this.ControlBox = false;
            this.Controls.Add(this.ListGroups);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.LabelGroups);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GroupSelect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Choose Test Group";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private Label LabelGroups;
        private Button OK;
        private ListView ListGroups;
    }
}