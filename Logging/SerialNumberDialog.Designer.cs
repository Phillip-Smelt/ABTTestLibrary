namespace ABT.TestSpace.TestExec.Logging {
    partial class SerialNumberDialog {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent() {
            this.BarCodeText = new System.Windows.Forms.TextBox();
            this.Cancel = new System.Windows.Forms.Button();
            this.OK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BarCodeText
            // 
            this.BarCodeText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BarCodeText.Enabled = false;
            this.BarCodeText.Location = new System.Drawing.Point(51, 15);
            this.BarCodeText.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BarCodeText.MaxLength = 11;
            this.BarCodeText.Name = "BarCodeText";
            this.BarCodeText.Size = new System.Drawing.Size(313, 22);
            this.BarCodeText.TabIndex = 0;
            this.BarCodeText.TabStop = false;
            this.BarCodeText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.BarCodeText.WordWrap = false;
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel.BackColor = System.Drawing.Color.Red;
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(265, 59);
            this.Cancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(100, 28);
            this.Cancel.TabIndex = 2;
            this.Cancel.Text = "&Cancel";
            this.Cancel.UseVisualStyleBackColor = false;
            this.Cancel.Click += new System.EventHandler(Cancel_Clicked);
            // 
            // OK
            // 
            this.OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.OK.BackColor = System.Drawing.Color.Green;
            this.OK.Location = new System.Drawing.Point(51, 59);
            this.OK.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(100, 28);
            this.OK.TabIndex = 21;
            this.OK.Text = "&OK";
            this.OK.UseVisualStyleBackColor = false;
            this.OK.Click += new System.EventHandler(OK_Clicked);
            // 
            // SerialNumberDialog
            // 
            this.AcceptButton = this.OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(420, 102);
            this.ControlBox = false;
            this.Controls.Add(this.OK);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.BarCodeText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SerialNumberDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Scan ABT Serial Number";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox BarCodeText;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button OK;
    }
}

