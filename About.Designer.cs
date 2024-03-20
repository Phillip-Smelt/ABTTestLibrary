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
            this.Link.Font = new System.Drawing.Font("Consolas", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Link.Location = new System.Drawing.Point(16, 265);
            this.Link.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Link.Name = "Link";
            this.Link.Size = new System.Drawing.Size(90, 20);
            this.Link.TabIndex = 1;
            this.Link.TabStop = true;
            this.Link.Text = "linkAbout";
            this.Link.Click += new System.EventHandler(this.Link_Clicked);
            // 
            // OK
            // 
            this.OK.Location = new System.Drawing.Point(831, 256);
            this.OK.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(116, 34);
            this.OK.TabIndex = 2;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Clicked);
            // 
            // Label
            // 
            this.Label.AutoSize = true;
            this.Label.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label.Location = new System.Drawing.Point(16, 12);
            this.Label.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Label.Name = "Label";
            this.Label.Size = new System.Drawing.Size(120, 23);
            this.Label.TabIndex = 4;
            this.Label.Text = "labelAbout";
            // 
            // About
            // 
            this.AcceptButton = this.OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(965, 310);
            this.ControlBox = false;
            this.Controls.Add(this.Label);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.Link);
            this.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
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