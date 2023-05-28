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
            this.LabelGroups = new System.Windows.Forms.Label();
            this.OK = new System.Windows.Forms.Button();
            this.ListGroups = new System.Windows.Forms.ListView();
            this.radioButtonRequired = new System.Windows.Forms.RadioButton();
            this.radioButtonOptional = new System.Windows.Forms.RadioButton();
            this.groupBoxSelect = new System.Windows.Forms.GroupBox();
            this.groupBoxSelect.SuspendLayout();
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
            this.OK.Location = new System.Drawing.Point(414, 366);
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
            this.ListGroups.Size = new System.Drawing.Size(886, 312);
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
            this.radioButtonRequired.CheckedChanged += new System.EventHandler(this.GroupBoxSelect_CheckedChanged);
            // 
            // radioButtonOptional
            // 
            this.radioButtonOptional.AutoSize = true;
            this.radioButtonOptional.Location = new System.Drawing.Point(6, 44);
            this.radioButtonOptional.Name = "radioButtonOptional";
            this.radioButtonOptional.Size = new System.Drawing.Size(78, 20);
            this.radioButtonOptional.TabIndex = 3;
            this.radioButtonOptional.TabStop = true;
            this.radioButtonOptional.Text = "Optional";
            this.radioButtonOptional.UseVisualStyleBackColor = true;
            this.radioButtonOptional.CheckedChanged += new System.EventHandler(this.GroupBoxSelect_CheckedChanged);
            // 
            // groupBoxSelect
            // 
            this.groupBoxSelect.Controls.Add(this.radioButtonOptional);
            this.groupBoxSelect.Controls.Add(this.radioButtonRequired);
            this.groupBoxSelect.Location = new System.Drawing.Point(12, 346);
            this.groupBoxSelect.Name = "groupBoxSelect";
            this.groupBoxSelect.Size = new System.Drawing.Size(138, 70);
            this.groupBoxSelect.TabIndex = 1;
            this.groupBoxSelect.TabStop = false;
            // 
            // GroupSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(912, 466);
            this.ControlBox = false;
            this.Controls.Add(this.groupBoxSelect);
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
            this.groupBoxSelect.ResumeLayout(false);
            this.groupBoxSelect.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private Label LabelGroups;
        private Button OK;
        private ListView ListGroups;
        private RadioButton radioButtonRequired;
        private RadioButton radioButtonOptional;
        private GroupBox groupBoxSelect;
    }
}