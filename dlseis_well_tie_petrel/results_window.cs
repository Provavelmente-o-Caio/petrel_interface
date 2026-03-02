using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Newtonsoft.Json.Linq;

namespace dlseis_well_tie_petrel
{
    public partial class results_window : Form
    {
        private JObject results;

        private TabPage tabWavelet;
        private TabPage tabTieWindow;
        private TabPage tabTDTable;
        private TabPage tabWarping;

        private Label labelGOM;
        private Label labelLag;

        public results_window(string resultsPath)
        {
            InitializeComponent();

            results = JObject.Parse(File.ReadAllText(resultsPath));

            BuildLayout();
            LoadAllPlots();
        }

        private void BuildLayout()
        {
            this.Text = "Well Tie - Results";
            this.Size = new Size(1280, 800);
            this.MinimumSize = new Size(900, 600);

            var KpiPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 48,
                BackColor = Color.FromArgb(30, 30, 40),
                Padding = new Padding(16, 0, 16, 0)
            };

            labelGOM = new Label
            {
                ForeColor = Color.FromArgb(100, 220, 100),
                Font = new Font("Consolas", 11, FontStyle.Bold),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft,
                Location = new Point(16, 12)
            };

            labelLag = new Label
            {
                ForeColor = Color.FromArgb(100, 180, 255),
                Font = new Font("Consolas", 11, FontStyle.Bold),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft,
                Location = new Point(340, 12)
            };

            var btnExportTD = new Button
            {
                Text = "Export T-D Table",
                Width = 130,
                Height = 28,
                Location = new Point(600, 10),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(26, 77, 122),
                ForeColor = Color.White,
                Font = new Font("Consolas", 8, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnExportTD.FlatAppearance.BorderSize = 0;
            btnExportTD.Click += BtnExportTD_Click;

            KpiPanel.Controls.Add(btnExportTD);
            KpiPanel.Controls.Add(labelGOM);
            KpiPanel.Controls.Add(labelLag);

            var tabs = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 9)
            };

            tabWavelet = new TabPage("Wavelet");
            tabTieWindow = new TabPage("Tie Window");
            tabTDTable = new TabPage("T-D Table");
            tabWarping = new TabPage("Warping");

            tabs.TabPages.Add(tabWavelet);
            tabs.TabPages.Add(tabTieWindow);
            tabs.TabPages.Add(tabTDTable);
            tabs.TabPages.Add(tabWarping);

            this.Controls.Add(tabs);
            this.Controls.Add(KpiPanel);
        }

        private void LoadAllPlots()
        {
            try
            {
                // KPIs
                double gom = (double)results["optimization"]["means"]["goodness_of_match"];
                double shift = (double)results["optimization"]["best_parameters"]["table_t_shift"];
                labelGOM.Text = $"Goodness of Match:  {gom:F4}";
                labelLag.Text = $"Best shift:  {shift:F4} s";
            }
            catch (Exception ex) { MessageBox.Show("KPI: " + ex.Message); return; }

            try { PlotWavelet(); }
            catch (Exception ex) { MessageBox.Show("Wavelet: " + ex.Message); return; }

            try { PlotTieWindow(); }
            catch (Exception ex) { MessageBox.Show("TieWindow: " + ex.Message); return; }

            try { PlotTDTable(); }
            catch (Exception ex) { MessageBox.Show("TDTable: " + ex.Message); return; }

            try { PlotWarping(); }
            catch (Exception ex) { MessageBox.Show("Warping: " + ex.Message); return; }
        }

        private void PlotWavelet()
        {
            var w = results["wavelet"];
            var basis = w["basis"].ToObject<double[]>();
            var values = w["values"].ToObject<double[]>();
            var freq = w["frequency"].ToObject<double[]>();
            var ampl = w["amplitude"].ToObject<double[]>();
            var phase = w["phase"].ToObject<double[]>();

            // Normalizar amplitude
            double maxAmpl = ampl.Max();
            ampl = ampl.Select(a => a / maxAmpl).ToArray();

            // Layout: wavelet em cima (full width), espectro e fase embaixo
            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 2,
                BackColor = Color.White
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 55));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 45));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            // --- Chart 1: Wavelet no tempo ---
            var chartWav = new Chart { Dock = DockStyle.Fill };
            var areaWav = new ChartArea("wav");
            areaWav.AxisX.Title = "Time [s]";
            areaWav.AxisX.TitleFont = new Font("Consolas", 8);
            areaWav.AxisY.Title = "Amplitude";
            areaWav.AxisY.TitleFont = new Font("Consolas", 8);
            areaWav.AxisX.MajorGrid.LineColor = Color.LightGray;
            areaWav.AxisY.MajorGrid.LineColor = Color.LightGray;
            areaWav.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            areaWav.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            areaWav.BackColor = Color.White;
            areaWav.AxisX.Minimum = basis[0];
            areaWav.AxisX.Maximum = basis[basis.Length - 1];
            areaWav.AxisX.LabelStyle.Format = "0.000";
            chartWav.ChartAreas.Add(areaWav);

            // Linha zero
            var sZero = new Series("Zero")
            {
                ChartType = SeriesChartType.Line,
                Color = Color.Black,
                BorderWidth = 1,
                IsVisibleInLegend = false
            };
            sZero.Points.AddXY(basis[0], 0);
            sZero.Points.AddXY(basis[basis.Length - 1], 0);
            chartWav.Series.Add(sZero);

            // Fill positivo (azul) e negativo (vermelho)
            var sPos = new Series("Positive")
            {
                ChartType = SeriesChartType.Area,
                Color = Color.FromArgb(200, 30, 80, 220),
                IsVisibleInLegend = false
            };
            var sNeg = new Series("Negative")
            {
                ChartType = SeriesChartType.Area,
                Color = Color.FromArgb(200, 220, 40, 40),
                IsVisibleInLegend = false
            };

            for (int i = 0; i < basis.Length; i++)
            {
                double v = values[i];
                sPos.Points.AddXY(basis[i], v >= 0 ? v : 0);
                sNeg.Points.AddXY(basis[i], v < 0 ? v : 0);
            }
            chartWav.Series.Add(sPos);
            chartWav.Series.Add(sNeg);

            var titleWav = new Title("Wavelet", Docking.Top,
                new Font("Consolas", 9, FontStyle.Bold), Color.Black);
            chartWav.Titles.Add(titleWav);

            // --- Chart 2: Espectro de amplitude ---
            var chartAmp = new Chart { Dock = DockStyle.Fill };
            var areaAmp = new ChartArea("amp");
            areaAmp.AxisX.Title = "Frequency [Hz]";
            areaAmp.AxisX.TitleFont = new Font("Consolas", 8);
            areaAmp.AxisY.Title = "Normalized Amplitude";
            areaAmp.AxisY.TitleFont = new Font("Consolas", 8);
            areaAmp.AxisX.MajorGrid.LineColor = Color.LightGray;
            areaAmp.AxisY.MajorGrid.LineColor = Color.LightGray;
            areaAmp.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            areaAmp.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            areaAmp.BackColor = Color.White;
            areaAmp.AxisX.Minimum = 0;
            chartAmp.ChartAreas.Add(areaAmp);

            var sAmp = new Series("Amplitude")
            {
                ChartType = SeriesChartType.Line,
                Color = Color.SteelBlue,
                BorderWidth = 2
            };
            for (int i = 0; i < freq.Length; i++)
                sAmp.Points.AddXY(freq[i], ampl[i]);
            chartAmp.Series.Add(sAmp);

            var titleAmp = new Title("Amplitude Spectrum", Docking.Top,
                new Font("Consolas", 9, FontStyle.Bold), Color.Black);
            chartAmp.Titles.Add(titleAmp);

            // --- Chart 3: Fase ---
            var chartPhase = new Chart { Dock = DockStyle.Fill };
            var areaPhase = new ChartArea("phase");
            areaPhase.AxisX.Title = "Frequency [Hz]";
            areaPhase.AxisX.TitleFont = new Font("Consolas", 8);
            areaPhase.AxisY.Title = "Phase [°]";
            areaPhase.AxisY.TitleFont = new Font("Consolas", 8);
            areaPhase.AxisX.MajorGrid.LineColor = Color.LightGray;
            areaPhase.AxisY.MajorGrid.LineColor = Color.LightGray;
            areaPhase.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            areaPhase.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            areaPhase.BackColor = Color.White;
            areaPhase.AxisX.Minimum = 0;
            areaPhase.AxisY.Minimum = -100;
            areaPhase.AxisY.Maximum = 100;
            chartPhase.ChartAreas.Add(areaPhase);

            // Linha zero tracejada
            var sPhaseZero = new Series("Zero")
            {
                ChartType = SeriesChartType.Line,
                Color = Color.Gray,
                BorderDashStyle = ChartDashStyle.Dash,
                BorderWidth = 1,
                IsVisibleInLegend = false
            };
            for (int i = 0; i < freq.Length; i++)
                sPhaseZero.Points.AddXY(freq[i], 0);
            chartPhase.Series.Add(sPhaseZero);

            // Pontos de fase como cruz (+)
            var sPhase = new Series("Phase")
            {
                ChartType = SeriesChartType.Point,
                Color = Color.SteelBlue,
                MarkerStyle = MarkerStyle.Cross,
                MarkerSize = 5
            };
            for (int i = 0; i < freq.Length; i++)
                sPhase.Points.AddXY(freq[i], phase[i]);
            chartPhase.Series.Add(sPhase);

            var titlePhase = new Title("Phase Spectrum", Docking.Top,
                new Font("Consolas", 9, FontStyle.Bold), Color.Black);
            chartPhase.Titles.Add(titlePhase);

            // Montar layout
            layout.Controls.Add(chartWav, 0, 0);
            layout.SetColumnSpan(chartWav, 2);
            layout.Controls.Add(chartAmp, 0, 1);
            layout.Controls.Add(chartPhase, 1, 1);

            tabWavelet.Controls.Clear();
            tabWavelet.Controls.Add(layout);
        }

        private Chart CreateBaseChart(string xTitle, string yTitle, bool invertY = false)
        {
            var chart = new Chart { Dock = DockStyle.Fill };
            var area = new ChartArea("area");
            area.AxisX.Title = xTitle;
            area.AxisX.TitleFont = new Font("Consolas", 8);
            area.AxisY.Title = yTitle;
            area.AxisY.TitleFont = new Font("Consolas", 8);
            area.AxisX.MajorGrid.LineColor = Color.LightGray;
            area.AxisY.MajorGrid.LineColor = Color.LightGray;
            area.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            area.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            area.AxisX.LabelStyle.Format = "0.000";
            area.AxisY.IsReversed = invertY;
            area.BackColor = Color.White;
            chart.ChartAreas.Add(area);
            return chart;
        }

        private Chart CreateWiggleChart(string title, double[] twt, double[] values, int repeatN)
        {
            var chart = CreateBaseChart(title, "", invertY: true);
            var area = chart.ChartAreas[0];
            area.AxisY.LabelStyle.Enabled = false;
            area.AxisX.LabelStyle.Format = "0.000";
            area.AxisY.Minimum = twt[0];
            area.AxisY.Maximum = twt[twt.Length - 1];

            double maxVal = values.Select(Math.Abs).Max();
            if (maxVal == 0) maxVal = 1;
            double scale = 1.0 / maxVal;

            for (int rep = 0; rep < repeatN; rep++)
            {
                double offset = rep * 2.0;

                var sLine = new Series($"line_{rep}")
                {
                    ChartType = SeriesChartType.Line,
                    Color = Color.Black,
                    BorderWidth = 1,
                    IsVisibleInLegend = false
                };

                for (int i = 0; i < twt.Length; i++)
                {
                    double v = values[i] * scale;
                    sLine.Points.AddXY(offset + v, twt[i]);
                }

                chart.Series.Add(sLine);
            }

    chart.Titles.Add(new Title(title, Docking.Top,
                new Font("Consolas", 8, FontStyle.Bold), Color.Black));
            if (twt.Length > 0)
            {
                area.AxisY.Minimum = twt[0];
                area.AxisY.Maximum = twt[twt.Length - 1];
            }
            return chart;
        }

        private void PlotTieWindow()
        {
            var tw = results["tie_window"];
            var twt = tw["time_twt"].ToObject<double[]>();
            var ai = tw["ai"].ToObject<double[]>();
            var r0 = tw["r0"].ToObject<double[]>();
            var syn = tw["synthetic_seismic"].ToObject<double[]>();
            var real_s = tw["real_seismic"].ToObject<double[]>();
            var dxcorrNode = results["dxcorr"];
            var dxTimeTwt = dxcorrNode["basis"].ToObject<double[]>();
            var dxLags = dxcorrNode["lags_basis"].ToObject<double[]>();
            var dxRaw = dxcorrNode["values"].ToObject<double[][]>();

            int twtLen = twt.Length;
            int aiLen = Math.Min(twtLen, ai.Length);
            int r0Len = Math.Min(twtLen, r0.Length);
            int synLen = Math.Min(twtLen, syn.Length);
            int realLen = Math.Min(twtLen, real_s.Length);
            int resLen = Math.Min(synLen, realLen);

            var res = Enumerable.Range(0, resLen)
                                .Select(i => real_s[i] - syn[i])
                                .ToArray();

            var synTrimmed = syn.Take(synLen).ToArray();
            var realTrimmed = real_s.Take(realLen).ToArray();
            var twtSyn = twt.Take(synLen).ToArray();
            var twtReal = twt.Take(realLen).ToArray();
            var twtRes = twt.Take(resLen).ToArray();

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 1,
                ColumnCount = 6,
                BackColor = Color.White
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            float[] widths = { 10f, 10f, 22f, 22f, 18f, 18f };
            foreach (var w in widths)
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, w));

            // 1. AI
            var chartAI = CreateBaseChart("AI", "TWT [s]", invertY: true);
            chartAI.ChartAreas[0].AxisX.LabelStyle.Format = "0";
            chartAI.ChartAreas[0].AxisY.LabelStyle.Format = "0.000";
            chartAI.ChartAreas[0].AxisY.Minimum = twt[0];
            chartAI.ChartAreas[0].AxisY.Maximum = twt[twtLen - 1];
            var sAI = new Series("AI")
            {
                ChartType = SeriesChartType.Line,
                Color = Color.SteelBlue,
                BorderWidth = 2,
                IsVisibleInLegend = false
            };
            for (int i = 0; i < aiLen; i++)
                sAI.Points.AddXY(ai[i], twt[i]);
            chartAI.Series.Add(sAI);
            chartAI.Titles.Add(new Title("AI", Docking.Top,
                new Font("Consolas", 8, FontStyle.Bold), Color.Black));

            // 2. R0
            var chartR0 = CreateBaseChart("R0", "", invertY: true);
            chartR0.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
            chartR0.ChartAreas[0].AxisX.LabelStyle.Format = "0.00";
            chartR0.ChartAreas[0].AxisY.Minimum = twt[0];
            chartR0.ChartAreas[0].AxisY.Maximum = twt[twtLen - 1];
            var sR0 = new Series("R0")
            {
                ChartType = SeriesChartType.Line,
                Color = Color.Red,
                BorderWidth = 1,
                IsVisibleInLegend = false
            };
            for (int i = 0; i < r0Len; i++)
                sR0.Points.AddXY(r0[i], twt[i]);
            chartR0.Series.Add(sR0);
            chartR0.Titles.Add(new Title("R0", Docking.Top,
                new Font("Consolas", 8, FontStyle.Bold), Color.Black));

            // 3. Synthetic (wiggle x5)
            var chartSyn = CreateWiggleChart("Synthetic Seismic", twtSyn, synTrimmed, 5);

            // 4. Real (wiggle x5)
            var chartReal = CreateWiggleChart("Real Seismic", twtReal, realTrimmed, 5);

            // 5. Residual (wiggle x1)
            var chartRes = CreateWiggleChart("Residual", twtRes, res, 1);

            // 6. Dynamic XCorr heatmap
            int nT = Math.Min(dxTimeTwt.Length, dxRaw.Length);
            int nL = dxLags.Length;
            var dxValues = new double[nT, nL];
            for (int i = 0; i < nT; i++)
            {
                int rowLen = Math.Min(nL, dxRaw[i].Length);
                for (int j = 0; j < rowLen; j++)
                    dxValues[i, j] = dxRaw[i][j];
            }

            var chartCorr = CreateHeatmapChart(dxValues, dxTimeTwt, dxLags);

            // Montar layout
            layout.Controls.Add(chartAI, 0, 0);
            layout.Controls.Add(chartR0, 1, 0);
            layout.Controls.Add(chartSyn, 2, 0);
            layout.Controls.Add(chartReal, 3, 0);
            layout.Controls.Add(chartRes, 4, 0);
            layout.Controls.Add(chartCorr, 5, 0);

            tabTieWindow.Controls.Clear();
            tabTieWindow.Controls.Add(layout);
        }

        private void PlotTDTable()
        {
            var td = results["td_table"];
            var orig = td["original"];
            var mod = td["modified"];
            var origTwt = orig["twt"].ToObject<double[]>();
            var origTvd = orig["tvdss"].ToObject<double[]>();
            var modTwt = mod["twt"].ToObject<double[]>();
            var modTvd = mod["tvdss"].ToObject<double[]>();

            var chart = CreateBaseChart("TVDSS (MSL) [m]", "TWT [s]", invertY: false);
            var area = chart.ChartAreas[0];
            area.AxisX.LabelStyle.Format = "0";
            area.AxisY.LabelStyle.Format = "0.000";
            area.AxisY.IsReversed = true;  // TWT cresce para baixo
            area.AxisX.Minimum = origTvd[0];
            area.AxisX.Maximum = origTvd[origTvd.Length - 1];
            area.AxisY.Minimum = origTwt[0];
            area.AxisY.Maximum = origTwt[origTwt.Length - 1];

            // Original
            var sOrig = new Series("Original")
            {
                ChartType = SeriesChartType.Line,
                Color = Color.SteelBlue,
                BorderWidth = 2
            };
            for (int i = 0; i < origTwt.Length; i++)
                sOrig.Points.AddXY(origTvd[i], origTwt[i]);
            chart.Series.Add(sOrig);

            // Modificada
            var sMod = new Series("Modified")
            {
                ChartType = SeriesChartType.Line,
                Color = Color.Orange,
                BorderWidth = 2,
                BorderDashStyle = ChartDashStyle.Dash
            };
            for (int i = 0; i < modTwt.Length; i++)
                sMod.Points.AddXY(modTvd[i], modTwt[i]);
            chart.Series.Add(sMod);

            var legend = new Legend("main");
            legend.Docking = Docking.Top;
            legend.Alignment = System.Drawing.StringAlignment.Center;
            legend.Font = new Font("Consolas", 8);
            chart.Legends.Add(legend);

            chart.Titles.Add(new Title("Time-Depth Table", Docking.Top,
                new Font("Consolas", 10, FontStyle.Bold), Color.Black));

            tabTDTable.Controls.Clear();
            tabTDTable.Controls.Add(chart);
        }

        private void PlotWarping()
        {
            var wp = results["warping_path"];
            var twt = wp["time_twt"].ToObject<double[]>();
            var dlags = wp["dlags"].ToObject<double[]>();
            var syn = results["tie_window"]["synthetic_seismic"].ToObject<double[]>();
            var real_s = results["tie_window"]["real_seismic"].ToObject<double[]>();

            int len = Math.Min(Math.Min(twt.Length, dlags.Length),
                   Math.Min(syn.Length, real_s.Length));

            double maxSyn = syn.Take(len).Select(Math.Abs).Max();
            double maxReal = real_s.Take(len).Select(Math.Abs).Max();
            double scale = 1.0;

            var chart = CreateBaseChart("TWT [s]", "", invertY: false);
            var area = chart.ChartAreas[0];
            area.AxisX.LabelStyle.Format = "0.000";
            area.AxisY.LabelStyle.Enabled = false;

            // Real trace (topo, +scale)
            var sReal = new Series("Real")
            {
                ChartType = SeriesChartType.Line,
                Color = Color.RoyalBlue,
                BorderWidth = 2,
                IsVisibleInLegend = false
            };
            for (int i = 0; i < len; i++)
                sReal.Points.AddXY(twt[i], (real_s[i] / maxReal) + scale);
            chart.Series.Add(sReal);

            // Synthetic trace (base, -scale)
            var sSyn = new Series("Synthetic")
            {
                ChartType = SeriesChartType.Line,
                Color = Color.RoyalBlue,
                BorderWidth = 2,
                IsVisibleInLegend = false
            };
            for (int i = 0; i < len; i++)
                sSyn.Points.AddXY(twt[i], (syn[i] / maxSyn) - scale);
            chart.Series.Add(sSyn);

            // Linhas de conexão laranja (~150 linhas para performance)
            double dt = len > 1 ? (twt[1] - twt[0]) : 0.002;
            int step = Math.Max(1, len / 150);

            for (int i = 0; i < len; i += step)
            {
                double lag = dlags[i];
                int j = i + (int)Math.Round(lag / dt);
                if (j < 0 || j >= len) continue;

                double yTop = (real_s[i] / maxReal) + scale;
                double yBot = (syn[j] / maxSyn) - scale;

                var sLine = new Series($"conn_{i}")
                {
                    ChartType = SeriesChartType.Line,
                    Color = Color.FromArgb(120, Color.Orange),
                    BorderWidth = 1,
                    IsVisibleInLegend = false
                };
                sLine.Points.AddXY(twt[i], yTop);
                sLine.Points.AddXY(twt[j], yBot);
                chart.Series.Add(sLine);
            }

            chart.Titles.Add(new Title("Warping Path", Docking.Top,
                new Font("Consolas", 10, FontStyle.Bold), Color.Black));

            tabWarping.Controls.Clear();
            tabWarping.Controls.Add(chart);
        }

        private Chart CreateHeatmapChart(double[,] values, double[] timeTwt, double[] lags)
        {
            var chart = new Chart { Dock = DockStyle.Fill };
            var area = new ChartArea("area");
            area.AxisX.Title = "Lag [s]";
            area.AxisX.TitleFont = new Font("Consolas", 8);
            area.AxisY.IsReversed = true;
            area.AxisY.LabelStyle.Enabled = false;
            area.AxisX.LabelStyle.Format = "0.00";
            area.AxisX.Minimum = lags[0];
            area.AxisX.Maximum = lags[lags.Length - 1];
            area.AxisY.Minimum = timeTwt[0];
            area.AxisY.Maximum = timeTwt[timeTwt.Length - 1];
            area.AxisX.MajorGrid.LineColor = Color.FromArgb(40, 255, 255, 255);
            area.AxisY.MajorGrid.LineColor = Color.FromArgb(40, 255, 255, 255);
            area.BackColor = Color.Beige;
            chart.BorderlineWidth = 0;
            chart.ChartAreas.Add(area);

            // Cada linha de tempo é uma série de pontos coloridos por valor
            int nTime = timeTwt.Length;
            int nLags = lags.Length;

            double vMin = -1.0;
            double vMax = 1.0;

            // Passo de tempo para calcular altura de cada célula
            double dtStep = nTime > 1 ? (timeTwt[nTime - 1] - timeTwt[0]) / nTime : 0.002;

            // Agrupamos em N faixas para não criar 278 séries — usa 1 série por linha
            for (int ti = 0; ti < nTime; ti++)
            {
                var s = new Series($"row_{ti}")
                {
                    ChartType = SeriesChartType.Point,
                    MarkerStyle = MarkerStyle.Square,
                    MarkerSize = 8,
                    IsVisibleInLegend = false
                };

                for (int li = 0; li < nLags; li++)
                {
                    double v = values[ti, li];
                    double t = (v - vMin) / (vMax - vMin); // 0..1
                    t = Math.Max(0, Math.Min(1, t));
                    Color color = InterpolateRdBu(t);

                    var pt = new DataPoint(lags[li], timeTwt[ti]);
                    pt.Color = color;
                    s.Points.Add(pt);
                }

                chart.Series.Add(s);
            }

            // Linha zero
            var sZero = new Series("zero")
            {
                ChartType = SeriesChartType.Line,
                Color = Color.Black,
                BorderWidth = 1,
                BorderDashStyle = ChartDashStyle.Dash,
                IsVisibleInLegend = false
            };
            sZero.Points.AddXY(0, timeTwt[0]);
            sZero.Points.AddXY(0, timeTwt[nTime - 1]);
            chart.Series.Add(sZero);

            chart.Titles.Add(new Title("Dynamic XCorr", Docking.Top,
                new Font("Consolas", 8, FontStyle.Bold), Color.Black));

            return chart;
        }

        // Colormap RdBu: azul (negativo) → branco (zero) → vermelho (positivo)
        private Color InterpolateRdBu(double t)
        {
            // t = 0 → azul escuro, t = 0.5 → branco, t = 1 → vermelho escuro
            if (t < 0.5)
            {
                double s = t / 0.5; // 0..1
                int r = (int)(33 + s * (255 - 33));
                int g = (int)(102 + s * (255 - 102));
                int b = (int)(172 + s * (255 - 172));
                return Color.FromArgb(r, g, b);
            }
            else
            {
                double s = (t - 0.5) / 0.5; // 0..1
                int r = (int)(255 + s * (178 - 255));
                int g = (int)(255 + s * (24 - 255));
                int b = (int)(255 + s * (43 - 255));
                return Color.FromArgb(r, g, b);
            }
        }

        private void BtnExportTD_Click(object sender, EventArgs e)
        {
            try
            {
                var mod = results["td_table"]["modified"];
                var twt = mod["twt"].ToObject<double[]>();
                var tvdss = mod["tvdss"].ToObject<double[]>();

                int len = Math.Min(twt.Length, tvdss.Length);

                using (var dialog = new SaveFileDialog
                {
                    Title = "Export Modified T-D Table",
                    Filter = "LAS File (*.las)|*.las",
                    DefaultExt = "las",
                    FileName = "td_table_modified.las"
                })
                {
                    if (dialog.ShowDialog() != DialogResult.OK) return;

                    using (var writer = new StreamWriter(dialog.FileName))
                    {
                        // LAS header
                        writer.WriteLine("~VERSION INFORMATION");
                        writer.WriteLine(" VERS.                 2.0   : CWLS LOG ASCII STANDARD - VERSION 2.0");
                        writer.WriteLine(" WRAP.                  NO   : ONE LINE PER DEPTH STEP");
                        writer.WriteLine();
                        writer.WriteLine("~WELL INFORMATION");
                        writer.WriteLine($" DATE.                       : {DateTime.Now:yyyy-MM-dd}");
                        writer.WriteLine(" WELL.                       : Modified T-D Table");
                        writer.WriteLine();
                        writer.WriteLine("~CURVE INFORMATION");
                        writer.WriteLine(" TVDSS.  M               : Depth below mean sea level");
                        writer.WriteLine(" TWT  .  S               : Two-way travel time");
                        writer.WriteLine();
                        writer.WriteLine("~A  TVDSS          TWT");

                        for (int i = 0; i < len; i++)
                            writer.WriteLine($"  {tvdss[i],12:F4}  {twt[i],12:F6}");
                    }

                    MessageBox.Show(
                        $"T-D Table exported successfully.\n{dialog.FileName}",
                        "Export Complete",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Export failed:\n" + ex.Message,
                    "Export Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
