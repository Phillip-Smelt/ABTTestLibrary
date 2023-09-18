namespace ABT.TestSpace.Logging {
    partial class SerialNumberControl {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.SerialNumberDisplay = new System.Windows.Forms.Label();
            this.ButtonOK = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SerialNumberDisplay
            // 
            this.SerialNumberDisplay.AutoSize = true;
            this.SerialNumberDisplay.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SerialNumberDisplay.Location = new System.Drawing.Point(58, 33);
            this.SerialNumberDisplay.Name = "SerialNumberDisplay";
            this.SerialNumberDisplay.Size = new System.Drawing.Size(339, 39);
            this.SerialNumberDisplay.TabIndex = 0;
            this.SerialNumberDisplay.Text = "SerialNumberDisplay";
            this.SerialNumberDisplay.Click += new System.EventHandler(this.SerialNumberDisplay_Click);
            // 
            // ButtonOK
            // 
            this.ButtonOK.BackColor = System.Drawing.Color.Green;
            this.ButtonOK.Location = new System.Drawing.Point(104, 105);
            this.ButtonOK.Name = "ButtonOK";
            this.ButtonOK.Size = new System.Drawing.Size(67, 49);
            this.ButtonOK.TabIndex = 1;
            this.ButtonOK.Text = "&OK";
            this.ButtonOK.UseVisualStyleBackColor = false;
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.BackColor = System.Drawing.Color.Red;
            this.ButtonCancel.Location = new System.Drawing.Point(294, 106);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(67, 48);
            this.ButtonCancel.TabIndex = 2;
            this.ButtonCancel.Text = "&Cancel";
            this.ButtonCancel.UseVisualStyleBackColor = false;
            // 
            // SerialNumberControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ButtonOK);
            this.Controls.Add(this.SerialNumberDisplay);
            this.Name = "SerialNumberControl";
            this.Size = new System.Drawing.Size(457, 218);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label SerialNumberDisplay;
        private System.Windows.Forms.Button ButtonOK;
        private System.Windows.Forms.Button ButtonCancel;
    }
}
