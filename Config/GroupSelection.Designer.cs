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
            this.radioButtonRequired = new System.Windows.Forms.RadioButton();
            this.radioButtonNotRequired = new System.Windows.Forms.RadioButton();
            this.groupBoxRequired = new System.Windows.Forms.GroupBox();
            this.groupBoxRequired.SuspendLayout();
            this.SuspendLayout();
            // 
            // LabelGroups
            // 
            this.LabelGroups.AutoSize = true;
            this.LabelGroups.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.LabelGroups.Location = new System.Drawing.Point(16, 7);
            this.LabelGroups.Name = "LabelGroups";
            this.LabelGroups.Size = new System.Drawing.Size(81, 16);
            this.LabelGroups.TabIndex = 0;
            this.LabelGroups.Text = "Test Groups";
            this.LabelGroups.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // OK
            // 
            this.OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK.Enabled = false;
            this.OK.Location = new System.Drawing.Point(225, 367);
            this.OK.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(77, 44);
            this.OK.TabIndex = 4;
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
            this.ListGroups.Location = new System.Drawing.Point(13, 27);
            this.ListGroups.Margin = new System.Windows.Forms.Padding(4);
            this.ListGroups.MultiSelect = false;
            this.ListGroups.Name = "ListGroups";
            this.ListGroups.ShowGroups = false;
            this.ListGroups.Size = new System.Drawing.Size(487, 312);
            this.ListGroups.TabIndex = 0;
            this.ListGroups.UseCompatibleStateImageBehavior = false;
            this.ListGroups.View = System.Windows.Forms.View.Details;
            this.ListGroups.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.ListGroups_SelectionChanged);
            // 
            // radioButtonRequired
            // 
            this.radioButtonRequired.AutoSize = true;
            this.radioButtonRequired.Location = new System.Drawing.Point(7, 18);
            this.radioButtonRequired.Name = "radioButtonRequired";
            this.radioButtonRequired.Size = new System.Drawing.Size(84, 20);
            this.radioButtonRequired.TabIndex = 2;
            this.radioButtonRequired.TabStop = true;
            this.radioButtonRequired.Text = "Required";
            this.radioButtonRequired.UseVisualStyleBackColor = true;
            this.radioButtonRequired.CheckedChanged += new System.EventHandler(this.GroupBoxRequired_CheckedChanged);
            // 
            // radioButtonNotRequired
            // 
            this.radioButtonNotRequired.AutoSize = true;
            this.radioButtonNotRequired.Location = new System.Drawing.Point(6, 44);
            this.radioButtonNotRequired.Name = "radioButtonNotRequired";
            this.radioButtonNotRequired.Size = new System.Drawing.Size(108, 20);
            this.radioButtonNotRequired.TabIndex = 3;
            this.radioButtonNotRequired.TabStop = true;
            this.radioButtonNotRequired.Text = "Not Required";
            this.radioButtonNotRequired.UseVisualStyleBackColor = true;
            this.radioButtonNotRequired.CheckedChanged += new System.EventHandler(this.GroupBoxRequired_CheckedChanged);
            // 
            // groupBoxRequired
            // 
            this.groupBoxRequired.Controls.Add(this.radioButtonNotRequired);
            this.groupBoxRequired.Controls.Add(this.radioButtonRequired);
            this.groupBoxRequired.Location = new System.Drawing.Point(12, 346);
            this.groupBoxRequired.Name = "groupBoxRequired";
            this.groupBoxRequired.Size = new System.Drawing.Size(138, 70);
            this.groupBoxRequired.TabIndex = 1;
            this.groupBoxRequired.TabStop = false;
            // 
            // GroupSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(525, 457);
            this.ControlBox = false;
            this.Controls.Add(this.groupBoxRequired);
            this.Controls.Add(this.ListGroups);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.LabelGroups);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GroupSelect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Choose Test Group";
            this.groupBoxRequired.ResumeLayout(false);
            this.groupBoxRequired.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private Label LabelGroups;
        private Button OK;
        private ListView ListGroups;
        private RadioButton radioButtonRequired;
        private RadioButton radioButtonNotRequired;
        private GroupBox groupBoxRequired;
    }
}