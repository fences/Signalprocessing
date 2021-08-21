
using MathNet.Filtering;
using MathNet.Filtering.FIR;
using MathNet.Filtering.IIR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using ZedGraph;

namespace WindowsFormsApplication3
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private GraphPane pane1;
        private GraphPane pane2;
        private GraphPane pane3;

        private void Form1_Load(object sender, EventArgs e)
        {


            pane1 = zedGraphControl1.GraphPane;
            pane2 = zedGraphControl2.GraphPane;
            pane3 = zedGraphControl3.GraphPane;

            pane1.Title.FontSpec.Size = 10.0f;
            pane2.Title.FontSpec.Size = 10.0f;
            pane3.Title.FontSpec.Size = 10.0f;

            pane1.YAxis.Title.FontSpec.Size = 10.0f;
            pane1.XAxis.Title.FontSpec.Size = 10.0f;
            pane2.YAxis.Title.FontSpec.Size = 10.0f;
            pane2.XAxis.Title.FontSpec.Size = 10.0f;
            pane3.YAxis.Title.FontSpec.Size = 10.0f;
            pane3.XAxis.Title.FontSpec.Size = 10.0f;

            pane1.YAxis.Scale.FontSpec.Size = 10.0f;
            pane1.XAxis.Scale.FontSpec.Size = 10.0f;
            pane2.YAxis.Scale.FontSpec.Size = 10.0f;
            pane2.XAxis.Scale.FontSpec.Size = 10.0f;
            pane3.YAxis.Scale.FontSpec.Size = 10.0f;
            pane3.XAxis.Scale.FontSpec.Size = 10.0f;


            pane1.Title.Text = "Signal/Filter";
            pane2.Title.Text = "Signal FFT";
            pane3.Title.Text = "FilteredSignal FFT";

            pane1.YAxis.Title.Text = "Amplitude";
            pane1.XAxis.Title.Text = "Time Domain";

            pane2.YAxis.Title.Text = "Amplitude";
            pane2.XAxis.Title.Text = "Frequency Domain";

            pane3.YAxis.Title.Text = "Amplitude";
            pane3.XAxis.Title.Text = "Frequency Domain";



            pane1.XAxis.Scale.FontSpec.IsAntiAlias = true;
            pane1.YAxis.Scale.FontSpec.IsAntiAlias = true;
            pane1.XAxis.MajorGrid.IsVisible = true;
            pane1.XAxis.MinorGrid.IsVisible = true;
            pane1.XAxis.MajorGrid.Color = Color.Gray;
            pane1.XAxis.MinorGrid.Color = Color.LightGray;
            pane1.XAxis.MinorGrid.DashOff = 1.0f;
            pane1.XAxis.MajorGrid.DashOff = 1.0f;
            pane1.YAxis.MajorGrid.IsVisible = true;
            pane1.YAxis.MinorGrid.IsVisible = true;
            pane1.YAxis.MajorGrid.Color = Color.Gray;
            pane1.YAxis.MinorGrid.Color = Color.LightGray;
            pane1.YAxis.MinorGrid.DashOff = 1.0f;
            pane1.YAxis.MajorGrid.DashOff = 1.0f;
            zedGraphControl1.IsShowPointValues = true;

            pane2.XAxis.Scale.FontSpec.IsAntiAlias = true;
            pane2.YAxis.Scale.FontSpec.IsAntiAlias = true;
            pane2.XAxis.MajorGrid.IsVisible = true;
            pane2.XAxis.MinorGrid.IsVisible = true;
            pane2.XAxis.MajorGrid.Color = Color.Gray;
            pane2.XAxis.MinorGrid.Color = Color.LightGray;
            pane2.XAxis.MinorGrid.DashOff = 1.0f;
            pane2.XAxis.MajorGrid.DashOff = 1.0f;
            pane2.YAxis.MajorGrid.IsVisible = true;
            pane2.YAxis.MinorGrid.IsVisible = true;
            pane2.YAxis.MajorGrid.Color = Color.Gray;
            pane2.YAxis.MinorGrid.Color = Color.LightGray;
            pane2.YAxis.MinorGrid.DashOff = 1.0f;
            pane2.YAxis.MajorGrid.DashOff = 1.0f;
            zedGraphControl2.IsShowPointValues = true;


            pane3.XAxis.Scale.FontSpec.IsAntiAlias = true;
            pane3.YAxis.Scale.FontSpec.IsAntiAlias = true;
            pane3.XAxis.MajorGrid.IsVisible = true;
            pane3.XAxis.MinorGrid.IsVisible = true;
            pane3.XAxis.MajorGrid.Color = Color.Gray;
            pane3.XAxis.MinorGrid.Color = Color.LightGray;
            pane3.XAxis.MinorGrid.DashOff = 1.0f;
            pane3.XAxis.MajorGrid.DashOff = 1.0f;
            pane3.YAxis.MajorGrid.IsVisible = true;
            pane3.YAxis.MinorGrid.IsVisible = true;
            pane3.YAxis.MajorGrid.Color = Color.Gray;
            pane3.YAxis.MinorGrid.Color = Color.LightGray;
            pane3.YAxis.MinorGrid.DashOff = 1.0f;
            pane3.YAxis.MajorGrid.DashOff = 1.0f;
            zedGraphControl3.IsShowPointValues = true;



            cbFilter.SelectedIndex = 0;


        }

        private double[] Y;
        private void openDataToolStripMenuItem_Click(object sender, EventArgs e)
        {

            SplitForm form = new SplitForm();
            if (form.ShowDialog() == DialogResult.OK)
            {

                openFileDialog1.FileName = "";
                openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    List<double> li = ReadFile(openFileDialog1.FileName, form.SplitOption);
                    Y = li.ToArray();
                    Analysis(Y);

                }
            }
        }


        private void Analysis(double[] data)
        {
            try
            {


                double fs = Convert.ToDouble(txtSampleRate.Text); //sampling rate

                if (chRemoveDC.Checked)
                {
                    var avg = data.Average();
                    for (int i = 0; i < data.Length; i++)
                    {
                        data[i] = data[i] - avg;
                    }

                }
                var y = data.ToArray();
                double[] filter = null;


                if (cbFilter.SelectedIndex == 0)
                {
                    double fc = Convert.ToDouble(txtFC1.Text); //cutoff frequency
                    var lowpass = OnlineFirFilter.CreateLowpass(ImpulseResponse.Finite, fs, fc);
                    filter = lowpass.ProcessSamples(y);
                }

                if (cbFilter.SelectedIndex == 1)
                {
                    double fc = Convert.ToDouble(txtFC1.Text); //cutoff frequency
                    var highpass = OnlineFirFilter.CreateHighpass(ImpulseResponse.Finite, fs, fc);
                    filter = highpass.ProcessSamples(y);
                }

                if (cbFilter.SelectedIndex == 2)
                {
                    //narrow bandpass filter
                    double fc1 = Convert.ToDouble(txtFC1.Text); //low cutoff frequency
                    double fc2 = Convert.ToDouble(txtFC2.Text); //high cutoff frequency
                    var bandpassnarrow =
                        OnlineFirFilter.CreateBandpass(ImpulseResponse.Finite, fs, fc1, fc2);
                    filter = bandpassnarrow.ProcessSamples(y);
                }

                if (filter != null)
                {

                    zedGraphControl1.GraphPane.CurveList.Clear();
                    zedGraphControl2.GraphPane.CurveList.Clear();
                    zedGraphControl3.GraphPane.CurveList.Clear();

                    PointPairList signal = new PointPairList();
                    PointPairList filterData = new PointPairList();
                    PointPairList fftData = new PointPairList();
                    PointPairList filterfftData = new PointPairList();

                    for (int i = 0; i < y.Length; i++)
                    {

                        signal.Add(new PointPair(i, y[i]));
                        filterData.Add(i, filter[i] * Convert.ToDouble(txtAmp.Text));
                    }

                    Complex[] buffer = Frequency(filter);
                    double[] fft = new double[buffer.Length];
                    for (int i = 0; i < y.Length / 2; i++)
                    {
                        fft[i] = buffer[i].Magnitude;
                        double cf = (double)((i * 1.0) / (1.0 * y.Length));
                        double f = fs * cf;
                        filterfftData.Add(f, fft[i]);
                    }

                    buffer = Frequency(y);
                    fft = new double[buffer.Length];
                    for (int i = 0; i < y.Length / 2; i++)
                    {
                        fft[i] = buffer[i].Magnitude;
                        double cf = (double)((i * 1.0) / (1.0 * y.Length));
                        double f = fs * cf;
                        fftData.Add(f, fft[i]);
                    }


                    LineItem line1 = pane1.AddCurve("Signal", signal, Color.Red, SymbolType.None);
                    pane1.AddCurve("Filter", filterData, Color.Blue, SymbolType.None);
                    LineItem line2 = pane2.AddCurve("Signal FFT", fftData, Color.Green, SymbolType.None);
                    LineItem line3 = pane3.AddCurve("Filtered Signal FFT", filterfftData, Color.Orange, SymbolType.None);


                    line1.Line.IsSmooth = true;
                    line1.Line.SmoothTension = 0.1F;
                    line2.Line.IsSmooth = true;
                    line2.Line.SmoothTension = 0.1F;
                    line3.Line.IsSmooth = true;
                    line3.Line.SmoothTension = 0.1F;
                    line2.Line.Width = 2.0f;
                    line3.Line.Width = 2.0f;
                    line1.Line.Width = 1.0f;

                    pane1.AxisChange();
                    pane2.AxisChange();
                    pane3.AxisChange();


                    zedGraphControl1.Refresh();
                    zedGraphControl2.Refresh();
                    zedGraphControl3.Refresh();
                }
            }
            catch { }


        }




        private Complex[] Frequency(double[] data)
        {
            
            int N = data.Length;
            Complex[] buffer = new Complex[N];

                for (int i = 0; i < N; i++)
                {
                    System.Numerics.Complex tmp = 
                    new System.Numerics.Complex(data[i], 0);
                    buffer[i] = tmp;
                }

            MathNet.Numerics.IntegralTransforms.Fourier.Forward(buffer,
                MathNet.Numerics.IntegralTransforms.FourierOptions.Matlab);
            return buffer;
        }

        private static List<double> ReadFile(string FileName,char[] seps)
        {
            try
            {
                if (!File.Exists(FileName))
                {
                    Console.WriteLine("File not found");
                    return null;
                }

                using (StreamReader sr = new StreamReader(FileName, Encoding.Default))
                {
                    string content = sr.ReadToEnd();
                    List<string> list =
                        content.Split(seps, StringSplitOptions.RemoveEmptyEntries).Where(c => c.Length > 1).ToList();
                    return list.Select(x => double.Parse(x)).ToList();
                }
            }
            catch
            {
                return null;
            }
        }

        private void btnAnalysis_Click(object sender, EventArgs e)
        {
            Analysis(Y);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void pieChart1_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }

        private void cartesianChart1_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }

        private void generateSampleSignalToolStripMenuItem_Click(object sender, EventArgs e)
        {

            SignalGenerator form = new SignalGenerator();
            if ( form.ShowDialog() == DialogResult.OK)
            {
                zedGraphControl1.GraphPane.CurveList.Clear();
                zedGraphControl2.GraphPane.CurveList.Clear();
                zedGraphControl3.GraphPane.CurveList.Clear();

                //signal + noise
                double fs = form.SampleRate; //sampling rate
                txtSampleRate.Text = fs.ToString();

                double fw = form.SignalFrequency; //signal frequency
                double fn = form.NoiseFrequency; //noise frequency
                double n = 20; //number of periods to show
                double A = form.SignalAmp; //signal amplitude
                double N = form.NoiseAmp; //noise amplitude
                int size = (int)(n * fs / fw); //sample size


                lblParam.Text = "SampleRate:" + fs.ToString() + "Hz" + Environment.NewLine +
                    "Signal Frequency:" + fw.ToString() + "Hz" + Environment.NewLine +
                    "Signal Amplitude" + A.ToString() + " " + Environment.NewLine +
                    "Noise Frequency:" + fn.ToString() + "Hz" + Environment.NewLine +
                    "Noise Amplitude:" + N.ToString();


                var t = Enumerable.Range(1, size).Select(p => p * 1 / fs).ToArray();
                Y = t.Select(p => (A * Math.Sin(2 * Math.PI * fw * p)) + 
                (N * Math.Sin(2 * Math.PI * fn * p))).ToArray(); //Original

                Analysis(Y);

            }




        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ( cbFilter.SelectedIndex == 0)
            {
                label5.Visible = false;
                label4.Visible = false;
                txtFC2.Visible = false;
                label2.Text = "FC:";
                

            }
            if (cbFilter.SelectedIndex == 1)
            {
                label5.Visible = false;
                label4.Visible = false;
                txtFC2.Visible = false;
                label2.Text = "FC:";

            }
            if (cbFilter.SelectedIndex == 2)
            {
                label5.Visible = true;
                label4.Visible = true;
                txtFC2.Visible = true;
                label2.Text = "FC1:";
            }

        }
    }
}
