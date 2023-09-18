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
            SerialNumberDisplay = new System.Windows.Forms.Label();
            ButtonOK = new System.Windows.Forms.Button();
            ButtonCancel = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // SerialNumberDisplay
            // 
            SerialNumberDisplay.AutoSize = true;
            SerialNumberDisplay.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            SerialNumberDisplay.Location = new System.Drawing.Point(58, 33);
            SerialNumberDisplay.Name = "SerialNumberDisplay";
            SerialNumberDisplay.Size = new System.Drawing.Size(339, 39);
            SerialNumberDisplay.TabIndex = 0;
            SerialNumberDisplay.Text = "SerialNumberDisplay";
            SerialNumberDisplay.Click += new System.EventHandler(SerialNumberDisplay_Click);
            // 
            // ButtonOK
            // 
            ButtonOK.BackColor = System.Drawing.Color.Green;
            ButtonOK.Location = new System.Drawing.Point(104, 105);
            ButtonOK.Name = "ButtonOK";
            ButtonOK.Size = new System.Drawing.Size(67, 49);
            ButtonOK.TabIndex = 1;
            ButtonOK.Text = "&OK";
            ButtonOK.UseVisualStyleBackColor = false;
            // 
            // ButtonCancel
            // 
            ButtonCancel.BackColor = System.Drawing.Color.Red;
            ButtonCancel.Location = new System.Drawing.Point(294, 106);
            ButtonCancel.Name = "ButtonCancel";
            ButtonCancel.Size = new System.Drawing.Size(67, 48);
            ButtonCancel.TabIndex = 2;
            ButtonCancel.Text = "&Cancel";
            ButtonCancel.UseVisualStyleBackColor = false;
            // 
            // SerialNumberControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(ButtonCancel);
            Controls.Add(ButtonOK);
            Controls.Add(SerialNumberDisplay);
            Name = "SerialNumberControl";
            Size = new System.Drawing.Size(457, 218);
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label SerialNumberDisplay;
        private System.Windows.Forms.Button ButtonOK;
        private System.Windows.Forms.Button ButtonCancel;
    }
}
