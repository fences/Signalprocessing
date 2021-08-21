using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{
    public partial class SplitForm : Form
    {
        public SplitForm()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        public char[] SplitOption
        {
            get
            {
                char[] sp =  new char[] { '\n', '\r' };
                if ( radioButton1.Checked)
                    sp = new char[] { '\n','\r' };
                if (radioButton2.Checked)
                    sp = sp = new char[] { ',' };

                return sp;

            }
        }
    }
}
