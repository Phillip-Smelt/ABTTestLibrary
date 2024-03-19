using System;
using System.Windows.Forms;

namespace ABT.TestSpace {
    public partial class About : Form {
        public About(String Title, String RTF, String Link) {
            InitializeComponent();
            this.Text = Title;
            this.RTF.Text = RTF;
            this.Link.Text = Link;
        }

        private void OK_Clicked(Object sender, EventArgs e) {
            this.Close();
        }

        private void Link_Clicked(Object sender, EventArgs e) {
            System.Diagnostics.Process.Start(this.Link.Text);
            this.Link.LinkVisited = true;
        }
    }
}
