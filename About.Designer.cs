namespace ABT.TestSpace {
    partial class About {
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.link = new System.Windows.Forms.LinkLabel();
            this.rtf = new System.Windows.Forms.RichTextBox();
            this.ok = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // link
            // 
            this.link.AutoSize = true;
            this.link.Location = new System.Drawing.Point(12, 185);
            this.link.Name = "link";
            this.link.Size = new System.Drawing.Size(62, 16);
            this.link.TabIndex = 1;
            this.link.TabStop = true;
            this.link.Text = "linkAbout";
            // 
            // rtf
            // 
            this.rtf.Location = new System.Drawing.Point(12, 12);
            this.rtf.Name = "rtf";
            this.rtf.ReadOnly = true;
            this.rtf.Size = new System.Drawing.Size(466, 156);
            this.rtf.TabIndex = 0;
            this.rtf.TabStop = false;
            this.rtf.Text = "";
            // 
            // ok
            // 
            this.ok.Location = new System.Drawing.Point(393, 178);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(85, 23);
            this.ok.TabIndex = 2;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            // 
            // About
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(493, 215);
            this.ControlBox = false;
            this.Controls.Add(this.ok);
            this.Controls.Add(this.rtf);
            this.Controls.Add(this.link);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "About";
            this.Text = "About";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel link;
        private System.Windows.Forms.RichTextBox rtf;
        private System.Windows.Forms.Button ok;
    }
}