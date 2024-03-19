using System;
using System.Windows.Forms;

namespace ABT.TestSpace {
    public partial class About : Form {
        public About(String Title, String RTF, String Link) {
            InitializeComponent();
            this.Text = Title;
            this.rtf.Text = RTF;
            this.link.Text = Link;
        }
    }
}
