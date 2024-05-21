using System.Diagnostics;
using System.Runtime.CompilerServices;
using ViBe_SzL_CH;

namespace Test1.Cmd {
    public partial class Form1 : Form {
        private string inputboxtext = String.Empty;
        private IVibeObject? vibeObject;
        private readonly System.Windows.Forms.Timer timer = new();
        private readonly Stopwatch stopwatch = new();
        private Task ts = new(new Action(() => { }));

        public Form1()
        {
            InitializeComponent();
            timer.Interval = 1000;
            timer.Tick += Report;
            maskingBox.SelectedIndex = 0;
            bufferSlider.Minimum = Command_definition.Mruler[0] + 1;
            bufferSlider.Maximum = Command_definition.Mruler[1] - 1;
            bufferSlider.Value = 20;
            cardinalitySlider.Minimum = Command_definition.Min_cardinality_ruler[0] + 1;
            cardinalitySlider.Maximum = Command_definition.Min_cardinality_ruler[1] - 1;
            cardinalitySlider.Value = 3;
            radiusSlider.Minimum = Command_definition.Nruler[0] + 1;
            radiusSlider.Maximum = Command_definition.Nruler[1] - 1;
            radiusSlider.Value = 20;
            omegaSlider.Minimum = Command_definition.Omega_ruler[0] + 1;
            omegaSlider.Maximum = Command_definition.Omega_ruler[1] - 1;
            omegaSlider.Value = 32;
            reinitSlider.Minimum = (int)(Command_definition.Reinit_treshold_ruler[0] * 100) + 1;
            reinitSlider.Maximum = (int)(Command_definition.Reinit_treshold_ruler[1] * 100) - 1;
            reinitSlider.Value = 80;
            progressBar1.Maximum = 1000;
            progressBar1.ForeColor = Color.Orange;
            UpdateLabels();
        }

        private void cameraCheck_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox box = (CheckBox)sender;
            if (box.Checked) {
                inputBox.Enabled = false;
                inputboxtext = inputBox.Text;
                inputBox.Text = String.Empty;
                inputBoxButton.Enabled = false;
            }
            else {
                inputBox.Enabled = true;
                inputBox.Text = inputboxtext;
                inputBoxButton.Enabled = true;
            }
        }

        private void inputBoxButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new() {
                Filter = "Videó fájlok|*.mov;*.mp4;*.mkv;*.avi;*.wmv|Minden fájl|*.*",
                FilterIndex = 0,
                AutoUpgradeEnabled = true,
                CheckFileExists = true,
                DereferenceLinks = true,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Title = "Forrás fájl kiválasztása..."
            };
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK) {
                inputBox.Text = dialog.FileName;
            }
        }

        private void outputBoxButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new() {
                Filter = "Matroska Videó fájl|*.mkv",
                AddExtension = true,
                AutoUpgradeEnabled = true,
                CheckPathExists = true,
                DefaultExt = "mkv",
                DereferenceLinks = true,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                OverwritePrompt = true,
                Title = "Kimeneti fájl kiválasztása..."
            };
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK) {
                outputBox.Text = dialog.FileName;
            }
        }

        private void Slider_ValueChanged(object sender, EventArgs e)
        {
            UpdateLabels();
            cardinalitySlider.Maximum = bufferSlider.Value - 5;
        }

        private void UpdateLabels()
        {
            bufferNum.Text = bufferSlider.Value.ToString();
            cardinalityNum.Text = cardinalitySlider.Value.ToString();
            omegaNum.Text = omegaSlider.Value.ToString();
            radiusNum.Text = radiusSlider.Value.ToString();
            reinitNum.Text = reinitSlider.Value.ToString();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            if ((cameraCheck.Checked && (outputBox.Text != string.Empty)) || ((outputBox.Text != string.Empty) && (inputBox.Text != string.Empty))) {
                if (!(Directory.Exists(Path.GetDirectoryName(outputBox.Text)) && Path.GetExtension(outputBox.Text) == ".mkv")) {
                    ErrorMessage();
                    return;
                }
                if (!cameraCheck.Checked) {
                    if (!File.Exists(inputBox.Text)) {
                        ErrorMessage();
                        return;
                    }
                }
                DisableControls();
                if (cameraCheck.Checked) {
                    vibeObject = new Camera_ViBe_Object(outputBox.Text);
                }
                else {
                    vibeObject = new ViBe_Object(inputBox.Text, outputBox.Text);
                }
                vibeObject.N = bufferSlider.Value;
                vibeObject.Radius = radiusSlider.Value;
                vibeObject.Omega = omegaSlider.Value;
                vibeObject.Min_Cardinality = cardinalitySlider.Value;
                vibeObject.Reinit_Treshold = (float)(reinitSlider.Value / 100.0f);
                vibeObject.Masking = (byte)maskingBox.SelectedIndex;
                stopwatch.Start();
                timer.Start();
                ts = new(new Action(() => {
                    vibeObject.Start();
                }));

                ts.Start();

                while (!ts.IsCompleted) {
                    Application.DoEvents();
                }

                if (vibeObject.Interrupt) {
                    MessageBox.Show("A feldolgozási folyamatot félbeszakította a felhasználó!", "Megszakítás", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                vibeObject.Dispose();
                timer.Stop();
                stopwatch.Stop();
                stopwatch.Reset();
                EnableControls();
                progressBar1.Value = 0;
                progressBar1.Refresh();
                MessageBox.Show("A folyamat befejezõdött!", "Sikeres mûvelet", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else {
                ErrorMessage();
            }
        }

        private static void ErrorMessage()
        {
            MessageBox.Show("Valamelyik fájl útvonal nem felel meg a követelményeknek! Ellenõrizze hogy a megadott útvonalak helyesek-e, esetleg hogy a megadott forrásfájl létezik-e!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void DisableControls()
        {
            inputBox.Enabled = false;
            outputBox.Enabled = false;
            inputBoxButton.Enabled = false;
            outputBoxButton.Enabled = false;
            cameraCheck.Enabled = false;
            bufferSlider.Enabled = false;
            cardinalitySlider.Enabled = false;
            omegaSlider.Enabled = false;
            radiusSlider.Enabled = false;
            reinitSlider.Enabled = false;
            maskingBox.Enabled = false;
            startButton.Enabled = false;
            this.Refresh();
        }

        private void EnableControls()
        {
            inputBox.Enabled = true;
            outputBox.Enabled = true;
            inputBoxButton.Enabled = true;
            outputBoxButton.Enabled = true;
            cameraCheck.Enabled = true;
            bufferSlider.Enabled = true;
            cardinalitySlider.Enabled = true;
            omegaSlider.Enabled = true;
            radiusSlider.Enabled = true;
            reinitSlider.Enabled = true;
            maskingBox.Enabled = true;
            startButton.Enabled = true;
            this.Refresh();
        }

        private void Report(object? sender, EventArgs e)
        {
            if (vibeObject != null && vibeObject is not Camera_ViBe_Object) {
                decimal fps = Math.Round(vibeObject.Completed_frames / (decimal)this.stopwatch.ElapsedMilliseconds * 1000, 2);
                int rtime, rsec;
                try {
                    rtime = (int)((vibeObject.Capture_frame_count - vibeObject.Completed_frames) / fps);
                    rsec = rtime - (rtime / 60) * 60;
                    rtime /= 60;
                }
                catch (DivideByZeroException) {
                    rtime = int.MaxValue;
                    rsec = int.MaxValue;
                }
                int percentage = (int)Math.Round(vibeObject.Completed_frames / (vibeObject.Capture_frame_count / 1000.0));
                remtimeLabel.Text = rtime.ToString() + " : " + rsec.ToString("00");
                progressBar1.Value = percentage;
                this.Refresh();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ts.Status.Equals(TaskStatus.Running) && vibeObject != null) {
                vibeObject.ForceInterrupt();
            }
        }
    }
}
