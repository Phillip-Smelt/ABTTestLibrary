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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
            this.Link = new System.Windows.Forms.LinkLabel();
            this.OK = new System.Windows.Forms.Button();
            this.Label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Link
            // 
            this.Link.AutoSize = true;
            this.Link.Location = new System.Drawing.Point(12, 185);
            this.Link.Name = "Link";
            this.Link.Size = new System.Drawing.Size(62, 16);
            this.Link.TabIndex = 1;
            this.Link.TabStop = true;
            this.Link.Text = "linkAbout";
            this.Link.Click += new System.EventHandler(this.Link_Clicked);
            // 
            // OK
            // 
            this.OK.Location = new System.Drawing.Point(605, 178);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(85, 23);
            this.OK.TabIndex = 2;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Clicked);
            // 
            // Label
            // 
            this.Label.AutoSize = true;
            this.Label.Location = new System.Drawing.Point(12, 9);
            this.Label.Name = "Label";
            this.Label.Size = new System.Drawing.Size(72, 16);
            this.Label.TabIndex = 4;
            this.Label.Text = "labelAbout";
            // 
            // About
            // 
            this.AcceptButton = this.OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(702, 215);
            this.ControlBox = false;
            this.Controls.Add(this.Label);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.Link);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "About";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel Link;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Label Label;
    }
}