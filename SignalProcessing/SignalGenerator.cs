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
    public partial class SignalGenerator : Form
    {
        public SignalGenerator()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        public double SampleRate
        {
            get { return Convert.ToDouble(txtSampleRate.Text); }
        }
        public double SignalFrequency
        {
            get { return Convert.ToDouble(txtSignalFreq.Text); }
        }
        public double SignalAmp
        {
            get { return Convert.ToDouble(txtSignalAmp.Text); }
        }
        public double NoiseFrequency
        {
            get { return Convert.ToDouble(txtNoiseFreq.Text); }
        }
        public double NoiseAmp
        {
            get { return Convert.ToDouble(txtNoiseAmp.Text); }
        }
    }
}
