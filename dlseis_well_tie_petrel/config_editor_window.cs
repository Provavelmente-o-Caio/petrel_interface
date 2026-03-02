using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace dlseis_well_tie_petrel
{
    public partial class config_editor_window : Form
    {
        private string _configPath;
        private JObject _config;

        // SearchSpace
        private NumericUpDown numMedianLenMin, numMedianLenMax;
        private NumericUpDown numMedianThMin, numMedianThMax;
        private NumericUpDown numStdMin, numStdMax;
        private NumericUpDown numShiftMin, numShiftMax;

        // SearchParams
        private NumericUpDown numSearchIters;
        private NumericUpDown numSimilarityStd;

        // WaveletScaling
        private NumericUpDown numWavMinScale, numWavMaxScale;
        private NumericUpDown numWavIters;

        // StretchAndSqueeze
        private NumericUpDown numWindowLength;
        private NumericUpDown numMaxLag;

        public config_editor_window(string configPath)
        {
            InitializeComponent();
            _configPath = configPath;
            _config = JObject.Parse(File.ReadAllText(configPath));

            this.Text = "Well Tie — Configuration Editor";
            this.Size = new System.Drawing.Size(620, 580);
            this.MinimumSize = new System.Drawing.Size(580, 520);
            this.StartPosition = FormStartPosition.CenterParent;

            BuildUI();
            LoadValues();
        }

        private void BuildUI()
        {
            var tabs = new TabControl { Dock = DockStyle.Fill };

            tabs.TabPages.Add(BuildSearchSpacePage());
            tabs.TabPages.Add(BuildSearchParamsPage());
            tabs.TabPages.Add(BuildWaveletPage());
            tabs.TabPages.Add(BuildStretchPage());

            // Botões no fundo
            var btnPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 48,
                Padding = new Padding(8)
            };

            var btnSave = new Button
            {
                Text = "Save",
                Width = 100,
                Height = 32,
                Location = new System.Drawing.Point(8, 8),
                BackColor = System.Drawing.Color.FromArgb(45, 106, 45),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSave.Click += BtnSave_Click;

            var btnReset = new Button
            {
                Text = "Reset Defaults",
                Width = 120,
                Height = 32,
                Location = new System.Drawing.Point(116, 8),
                FlatStyle = FlatStyle.Flat
            };
            btnReset.Click += BtnReset_Click;

            var btnCancel = new Button
            {
                Text = "Cancel",
                Width = 100,
                Height = 32,
                Location = new System.Drawing.Point(244, 8),
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.Click += (s, e) => this.Close();

            btnPanel.Controls.Add(btnSave);
            btnPanel.Controls.Add(btnReset);
            btnPanel.Controls.Add(btnCancel);

            this.Controls.Add(tabs);
            this.Controls.Add(btnPanel);
        }

        private TabPage BuildSearchSpacePage()
        {
            var page = new TabPage("Search Space");
            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                Padding = new Padding(12)
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));

            AddHeader(layout, "Parameter", "Min", "Max");

            numMedianLenMin = MakeInt(11, 1, 99);
            numMedianLenMax = MakeInt(63, 1, 99);
            AddRow(layout, "Median Filter Length", numMedianLenMin, numMedianLenMax,
                   "Odd values only — range of median filter window size");

            numMedianThMin = MakeDecimal(0.1m, 0.01m, 0m, 20m);
            numMedianThMax = MakeDecimal(5.5m, 0.1m, 0m, 20m);
            AddRow(layout, "Median Threshold", numMedianThMin, numMedianThMax,
                   "Controls aggressiveness of outlier removal");

            numStdMin = MakeDecimal(0.5m, 0.1m, 0m, 20m);
            numStdMax = MakeDecimal(5.5m, 0.1m, 0m, 20m);
            AddRow(layout, "Log Std Dev", numStdMin, numStdMax,
                   "Standard deviation smoothing range");

            numShiftMin = MakeDecimal(-0.012m, 0.001m, -1m, 0m, 4);
            numShiftMax = MakeDecimal(0.012m, 0.001m, 0m, 1m, 4);
            AddRow(layout, "T-D Shift [s]", numShiftMin, numShiftMax,
                   "Time shift bounds for T-D table adjustment");

            page.Controls.Add(layout);
            return page;
        }

        private TabPage BuildSearchParamsPage()
        {
            var page = new TabPage("Search Params");
            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                Padding = new Padding(12)
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));

            AddHeader2(layout, "Parameter", "Value");

            numSearchIters = MakeInt(80, 1, 500);
            numSimilarityStd = MakeDecimal(0.02m, 0.001m, 0m, 1m, 3);

            AddRow2(layout, "Num Iterations", numSearchIters,
                    "Number of Bayesian optimization iterations (more = slower but better)");
            AddRow2(layout, "Similarity Std", numSimilarityStd,
                    "Noise level assumed in the objective function");

            page.Controls.Add(layout);
            return page;
        }

        private TabPage BuildWaveletPage()
        {
            var page = new TabPage("Wavelet Scaling");
            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                Padding = new Padding(12)
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));

            AddHeader2(layout, "Parameter", "Value");

            numWavMinScale = MakeInt(50000, 1000, 1000000);
            numWavMaxScale = MakeInt(500000, 1000, 9999999);
            numWavIters = MakeInt(60, 1, 500);

            AddRow2(layout, "Min Scale", numWavMinScale,
                    "Minimum wavelet amplitude scaling factor");
            AddRow2(layout, "Max Scale", numWavMaxScale,
                    "Maximum wavelet amplitude scaling factor");
            AddRow2(layout, "Num Iterations", numWavIters,
                    "Iterations for wavelet scaling optimization");

            page.Controls.Add(layout);
            return page;
        }

        private TabPage BuildStretchPage()
        {
            var page = new TabPage("Stretch && Squeeze");
            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                Padding = new Padding(12)
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));

            AddHeader2(layout, "Parameter", "Value");

            numWindowLength = MakeDecimal(0.060m, 0.001m, 0.010m, 1m, 3);
            numMaxLag = MakeDecimal(0.010m, 0.001m, 0.001m, 1m, 3);

            AddRow2(layout, "Window Length [s]", numWindowLength,
                    "DTW warping window length in seconds");
            AddRow2(layout, "Max Lag [s]", numMaxLag,
                    "Maximum allowable time lag in seconds");

            page.Controls.Add(layout);
            return page;
        }

        // ── Helpers de criação de controles ──────────────────────────────────

        private NumericUpDown MakeInt(int val, int step, int max, int min = 0)
        {
            return new NumericUpDown
            {
                Minimum = min,
                Maximum = max,
                Value = val,
                Increment = step,
                DecimalPlaces = 0,
                Dock = DockStyle.Fill
            };
        }

        private NumericUpDown MakeDecimal(decimal val, decimal step,
                                          decimal min, decimal max, int places = 2)
        {
            return new NumericUpDown
            {
                Minimum = min,
                Maximum = max,
                Value = val,
                Increment = step,
                DecimalPlaces = places,
                Dock = DockStyle.Fill
            };
        }

        private void AddHeader(TableLayoutPanel layout,
                                string col1, string col2, string col3)
        {
            var font = new System.Drawing.Font("Consolas", 8, System.Drawing.FontStyle.Bold);
            layout.Controls.Add(MakeLabel(col1, font));
            layout.Controls.Add(MakeLabel(col2, font));
            layout.Controls.Add(MakeLabel(col3, font));
        }

        private void AddHeader2(TableLayoutPanel layout, string col1, string col2)
        {
            var font = new System.Drawing.Font("Consolas", 8, System.Drawing.FontStyle.Bold);
            layout.Controls.Add(MakeLabel(col1, font));
            layout.Controls.Add(MakeLabel(col2, font));
        }

        private void AddRow(TableLayoutPanel layout, string name,
                             NumericUpDown ctrlMin, NumericUpDown ctrlMax, string tooltip)
        {
            var tip = new ToolTip();
            var lbl = MakeLabel(name);
            tip.SetToolTip(lbl, tooltip);
            layout.Controls.Add(lbl);
            layout.Controls.Add(ctrlMin);
            layout.Controls.Add(ctrlMax);
        }

        private void AddRow2(TableLayoutPanel layout, string name,
                              NumericUpDown ctrl, string tooltip)
        {
            var tip = new ToolTip();
            var lbl = MakeLabel(name);
            tip.SetToolTip(lbl, tooltip);
            layout.Controls.Add(lbl);
            layout.Controls.Add(ctrl);
        }

        private Label MakeLabel(string text,
                                 System.Drawing.Font font = null)
        {
            return new Label
            {
                Text = text,
                Font = font ?? new System.Drawing.Font("Consolas", 8),
                Dock = DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                Padding = new Padding(0, 4, 0, 4)
            };
        }

        // ── Load / Save ───────────────────────────────────────────────────────

        private void LoadValues()
        {
            var ss = _config["SearchSpace"];
            var sp = _config["SearchParams"];
            var ws = _config["WaveletScaling"];
            var sas = _config["StretchAndSqueeze"];

            // SearchSpace — com defaults caso não exista no JSON
            numMedianLenMin.Value = ss?["median_length_min"] != null ? (int)ss["median_length_min"] : 11;
            numMedianLenMax.Value = ss?["median_length_max"] != null ? (int)ss["median_length_max"] : 63;
            numMedianThMin.Value = ss?["median_th_min"] != null ? (decimal)(double)ss["median_th_min"] : 0.1m;
            numMedianThMax.Value = ss?["median_th_max"] != null ? (decimal)(double)ss["median_th_max"] : 5.5m;
            numStdMin.Value = ss?["std_min"] != null ? (decimal)(double)ss["std_min"] : 0.5m;
            numStdMax.Value = ss?["std_max"] != null ? (decimal)(double)ss["std_max"] : 5.5m;
            numShiftMin.Value = ss?["table_t_shift_min"] != null ? (decimal)(double)ss["table_t_shift_min"] : -0.012m;
            numShiftMax.Value = ss?["table_t_shift_max"] != null ? (decimal)(double)ss["table_t_shift_max"] : 0.012m;

            // SearchParams
            numSearchIters.Value = sp?["num_iters"] != null ? (int)sp["num_iters"] : 80;
            numSimilarityStd.Value = sp?["similarity_std"] != null ? (decimal)(double)sp["similarity_std"] : 0.02m;

            // WaveletScaling
            numWavMinScale.Value = ws?["wavelet_min_scale"] != null ? (int)ws["wavelet_min_scale"] : 50000;
            numWavMaxScale.Value = ws?["wavelet_max_scale"] != null ? (int)ws["wavelet_max_scale"] : 500000;
            numWavIters.Value = ws?["num_iters"] != null ? (int)ws["num_iters"] : 60;

            // StretchAndSqueeze
            numWindowLength.Value = sas?["window_length"] != null ? (decimal)(double)sas["window_length"] : 0.060m;
            numMaxLag.Value = sas?["max_lag"] != null ? (decimal)(double)sas["max_lag"] : 0.010m;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (numMedianLenMin.Value >= numMedianLenMax.Value ||
                numMedianThMin.Value >= numMedianThMax.Value ||
                numStdMin.Value >= numStdMax.Value ||
                numShiftMin.Value >= numShiftMax.Value ||
                numWavMinScale.Value >= numWavMaxScale.Value)
            {
                MessageBox.Show(
                    "Min values must be less than Max values.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // Garantir que as seções existem no JSON antes de escrever
            if (_config["SearchSpace"] == null)
                _config["SearchSpace"] = new JObject();
            if (_config["SearchParams"] == null)
                _config["SearchParams"] = new JObject();
            if (_config["WaveletScaling"] == null)
                _config["WaveletScaling"] = new JObject();
            if (_config["StretchAndSqueeze"] == null)
                _config["StretchAndSqueeze"] = new JObject();

            _config["SearchSpace"]["median_length_min"] = (int)numMedianLenMin.Value;
            _config["SearchSpace"]["median_length_max"] = (int)numMedianLenMax.Value;
            _config["SearchSpace"]["median_th_min"] = (double)numMedianThMin.Value;
            _config["SearchSpace"]["median_th_max"] = (double)numMedianThMax.Value;
            _config["SearchSpace"]["std_min"] = (double)numStdMin.Value;
            _config["SearchSpace"]["std_max"] = (double)numStdMax.Value;
            _config["SearchSpace"]["table_t_shift_min"] = (double)numShiftMin.Value;
            _config["SearchSpace"]["table_t_shift_max"] = (double)numShiftMax.Value;

            _config["SearchParams"]["num_iters"] = (int)numSearchIters.Value;
            _config["SearchParams"]["similarity_std"] = (double)numSimilarityStd.Value;

            _config["WaveletScaling"]["wavelet_min_scale"] = (int)numWavMinScale.Value;
            _config["WaveletScaling"]["wavelet_max_scale"] = (int)numWavMaxScale.Value;
            _config["WaveletScaling"]["num_iters"] = (int)numWavIters.Value;

            _config["StretchAndSqueeze"]["window_length"] = (double)numWindowLength.Value;
            _config["StretchAndSqueeze"]["max_lag"] = (double)numMaxLag.Value;

            File.WriteAllText(_configPath, _config.ToString());

            MessageBox.Show("Configuration saved.", "Saved",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Reset all values to defaults?", "Reset",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            numMedianLenMin.Value = 11; numMedianLenMax.Value = 63;
            numMedianThMin.Value = 0.1m; numMedianThMax.Value = 5.5m;
            numStdMin.Value = 0.5m; numStdMax.Value = 5.5m;
            numShiftMin.Value = -0.012m; numShiftMax.Value = 0.012m;
            numSearchIters.Value = 80;
            numSimilarityStd.Value = 0.02m;
            numWavMinScale.Value = 50000; numWavMaxScale.Value = 500000;
            numWavIters.Value = 60;
            numWindowLength.Value = 0.060m;
            numMaxLag.Value = 0.010m;
        }
    }
}
