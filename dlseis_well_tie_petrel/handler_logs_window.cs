using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.DomainObject.Well;
using Slb.Ocean.Petrel.DomainObject;

namespace dlseis_well_tie_petrel
{
    public partial class handler_logs_window : Form
    {
        private Dictionary<string, string> SelectedLogs = new Dictionary<string, string>();
        private List<string> logs = new List<string>();
        private int log_range;
        private int startRange;
        private int endRange;
        private string lasUnit;
        private Borehole well;
        private int SCROLL_SIZE = 2000;
        // Campos novos na classe
        private Panel chartPanel;
        private List<Chart> logCharts = new List<Chart>();
        private bool isSyncing = false;
        private const int CHART_WIDTH = 180;
        public handler_logs_window(string wellname)
        {
            InitializeComponent();

            List<string> units = new List<string> { "m/s", "us/ft", "us/m", "s/m", "ft/s", "km/s" };

            // Selecting the well root
            Project project = PetrelProject.PrimaryProject;
            var wellroot = WellRoot.Get(project);
            var wellColection = wellroot.BoreholeCollection;

            well = wellColection.BoreholeCollections.ElementAt(0).Where(w => w.Name == wellname).Select(w => w).ElementAt(0);

            var lognames = well.Logs.WellLogs.Select(w => w.Name).ToArray();
            log_range = well.Logs.WellLogs.Select(w => w.SampleCount).ElementAt(0);

            logs.Clear();
            logs = lognames.ToList();

            comboBoxVP.Items.AddRange(lognames);
            comboBoxVS.Items.AddRange(lognames);
            comboBoxRho.Items.AddRange(lognames);
            comboBoxGR.Items.AddRange(lognames);
            comboBoxCali.Items.AddRange(lognames);
            comboBoxUnit.DataSource = units;

            unitTextBox_range_start.Value = log_range;
            unitTextBox_range_end.Value = 0;

            configureChartLogs();

            comboBoxVP.SelectedIndexChanged += UpdateLogPreview;
            comboBoxVS.SelectedIndexChanged += UpdateLogPreview;
            comboBoxRho.SelectedIndexChanged += UpdateLogPreview;
            comboBoxGR.SelectedIndexChanged += UpdateLogPreview;
            comboBoxCali.SelectedIndexChanged += UpdateLogPreview;
            unitTextBox_range_start.ValueChanged += UpdateLogPreview;
            unitTextBox_range_end.ValueChanged += UpdateLogPreview;
        }

        private void configureChartLogs()
        {
            // Esconde o chartLogs original e usa um painel dinâmico no lugar
            chartLogs.Visible = false;

            chartPanel = new Panel
            {
                AutoScroll = true,
                BackColor = System.Drawing.Color.WhiteSmoke,
                Bounds = chartLogs.Bounds,
                Anchor = chartLogs.Anchor
            };

            // Substitui o chartLogs no layout
            chartLogs.Parent.Controls.Add(chartPanel);
        }

        private void UpdateLogPreview(object sender, EventArgs e)
        {
            // Limpar charts anteriores
            foreach (var c in logCharts)
            {
                c.AxisScrollBarClicked -= OnScrollBarClicked;
                chartPanel.Controls.Remove(c);
                c.Dispose();
            }
            logCharts.Clear();

            int start = (int)unitTextBox_range_start.Value;
            int end = (int)unitTextBox_range_end.Value;

            // Definir quais logs estão selecionados
            var selected = new List<(string logName, string label, System.Drawing.Color color)>();
            if (!string.IsNullOrEmpty(comboBoxVP.Text)) selected.Add((comboBoxVP.Text, "VP", System.Drawing.Color.Blue));
            if (!string.IsNullOrEmpty(comboBoxVS.Text)) selected.Add((comboBoxVS.Text, "VS", System.Drawing.Color.Red));
            if (!string.IsNullOrEmpty(comboBoxRho.Text)) selected.Add((comboBoxRho.Text, "Rho", System.Drawing.Color.Green));
            if (!string.IsNullOrEmpty(comboBoxGR.Text)) selected.Add((comboBoxGR.Text, "GR", System.Drawing.Color.Orange));
            if (!string.IsNullOrEmpty(comboBoxCali.Text)) selected.Add((comboBoxCali.Text, "Cali", System.Drawing.Color.Purple));

            if (selected.Count == 0) return;

            int panelH = chartPanel.ClientSize.Height;
            int totalW = selected.Count * CHART_WIDTH;
            // chartPanel.AutoScrollMinSize = new System.Drawing.Size(totalW, 0);

            for (int idx = 0; idx < selected.Count; idx++)
            {
                var (logName, label, color) = selected[idx];
                bool isFirst = idx == 0;

                var chart = CreateLogChart(logName, label, color, start, end, showYAxis: isFirst);
                chart.Bounds = new System.Drawing.Rectangle(idx * CHART_WIDTH, 0, CHART_WIDTH, panelH);
                chart.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;

                // Sincronização via evento de scroll
                chart.AxisScrollBarClicked += OnScrollBarClicked;
                chart.GetToolTipText += OnChartGetToolTip;

                // Capturar scroll via MouseWheel
                chart.MouseWheel += OnChartMouseWheel;

                chartPanel.Controls.Add(chart);
                logCharts.Add(chart);
            }
        }

        private Chart CreateLogChart(string logName, string label, System.Drawing.Color color,
                                      int start, int end, bool showYAxis)
        {
            var wellLog = well.Logs.WellLogs.FirstOrDefault(w => w.Name == logName);

            var chart = new Chart
            {
                BackColor = System.Drawing.Color.White,
                BorderlineColor = System.Drawing.Color.LightGray,
                BorderlineWidth = 1,
                BorderlineDashStyle = ChartDashStyle.Solid
            };

            var area = new ChartArea("area");
            area.BackColor = System.Drawing.Color.WhiteSmoke;

            // Eixo X — valores do log
            area.AxisX.TitleFont = new System.Drawing.Font("Consolas", 8, System.Drawing.FontStyle.Bold);
            area.AxisX.TitleForeColor = color;
            area.AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            area.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            area.AxisX.LabelStyle.Font = new System.Drawing.Font("Consolas", 7);
            area.AxisX.LabelStyle.Format = "0.000";
            area.AxisX.LabelStyle.Angle = -45;

            // Eixo Y — profundidade (invertido)
            area.AxisY.IsReversed = true;
            area.AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            area.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            area.AxisY.LabelStyle.Font = new System.Drawing.Font("Consolas", 7);
            area.AxisY.LabelStyle.Format = "0";
            area.AxisY.LabelStyle.Enabled = showYAxis;
            area.AxisY.Minimum = end;
            area.AxisY.Maximum = start;

            // Scroll no eixo Y
            area.AxisY.ScaleView.Size = SCROLL_SIZE;
            area.AxisY.ScrollBar.Enabled = true;
            area.AxisY.ScrollBar.IsPositionedInside = true;
            area.AxisY.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            area.AxisY.ScrollBar.LineColor = System.Drawing.Color.Gray;
            area.AxisY.ScrollBar.Size = 12;

            // Zoom
            area.CursorX.IsUserEnabled = true;
            area.CursorX.IsUserSelectionEnabled = true;
            area.AxisX.ScaleView.Zoomable = true;
            area.CursorY.IsUserEnabled = true;
            area.CursorY.IsUserSelectionEnabled = true;
            area.AxisY.ScaleView.Zoomable = true;

            chart.ChartAreas.Add(area);

            // Reset zoom com botão direito
            chart.MouseClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    area.AxisX.ScaleView.ZoomReset();
                    SyncScroll(area.AxisY.ScaleView.Position);
                }
            };

            // Título no topo
            chart.Titles.Add(new Title(label, Docking.Top,
                new System.Drawing.Font("Consolas", 9, System.Drawing.FontStyle.Bold), color));

            // Série
            if (wellLog != null)
            {
                var series = new Series(label)
                {
                    ChartType = SeriesChartType.Line,
                    BorderWidth = 2,
                    Color = color,
                    MarkerStyle = MarkerStyle.None
                };

                int count = start - end;
                int step = Math.Max(1, count / 1000);

                for (int i = 0; i < count; i += step)
                {
                    int idx = end + i;
                    if (idx >= wellLog.SampleCount) break;
                    float v = (float)wellLog[idx].Value;
                    if (!float.IsNaN(v) && !float.IsInfinity(v))
                        series.Points.AddXY(v, idx);
                }

                chart.Series.Add(series);
            }

            // Posição inicial do scroll
            area.AxisY.ScaleView.Zoom(end, end + SCROLL_SIZE);

            return chart;
        }

        private void OnScrollBarClicked(object sender, ScrollBarEventArgs e)
        {
            if (isSyncing) return;
            var chart = sender as Chart;
            if (chart == null) return;
            double pos = chart.ChartAreas[0].AxisY.ScaleView.Position;
            SyncScroll(pos);
        }

        private void OnChartMouseWheel(object sender, MouseEventArgs e)
        {
            if (isSyncing) return;
            var chart = sender as Chart;
            if (chart == null) return;

            var area = chart.ChartAreas[0];
            double pos = area.AxisY.ScaleView.Position;
            double size = area.AxisY.ScaleView.Size;
            double delta = size * 0.1 * (e.Delta > 0 ? -1 : 1);
            double newPos = Math.Max((int)unitTextBox_range_end.Value, pos + delta);

            SyncScroll(newPos);
        }

        private void SyncScroll(double position)
        {
            isSyncing = true;
            foreach (var c in logCharts)
            {
                var area = c.ChartAreas[0];
                double size = area.AxisY.ScaleView.Size;
                double max = area.AxisY.Maximum - size;
                double pos = Math.Max(area.AxisY.Minimum, Math.Min(position, max));
                area.AxisY.ScaleView.Zoom(pos, pos + size);
            }
            isSyncing = false;
        }

        private void OnChartGetToolTip(object sender, ToolTipEventArgs e)
        {
            if (e.HitTestResult.ChartElementType == ChartElementType.DataPoint)
            {
                var pt = e.HitTestResult.Series.Points[e.HitTestResult.PointIndex];
                e.Text = $"Idx: {pt.YValues[0]:F0}\nVal: {pt.XValue:F2}";
            }
        }

        private bool ValidateRange(int startRange, int endRange, int maxRange)
        {
            if (startRange < endRange)
            {
                MessageBox.Show(
                    "Start range must be greater than or equal to end range.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            if (startRange < 0 || endRange < 0)
            {
                MessageBox.Show(
                    "Range values must be non-negative.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            if (startRange > maxRange || endRange > maxRange)
            {
                MessageBox.Show(
                    $"Range values must not exceed {maxRange}.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void buttonHandlerLogs_Click(object sender, EventArgs e)
        {
            SelectedLogs.Clear();
            this.DialogResult = DialogResult.None;

            string selectedVP = comboBoxVP.Text;
            string selectedVS = comboBoxVS.Text;
            string selectedRho = comboBoxRho.Text;
            string selectedGR = comboBoxGR.Text;
            string selectedCali = comboBoxCali.Text;
            startRange = ((int)unitTextBox_range_start.Value);
            endRange = ((int)unitTextBox_range_end.Value);
            lasUnit = comboBoxUnit.Text;

            if (!string.IsNullOrEmpty(selectedVP)  && !string.IsNullOrEmpty(selectedVS) && !string.IsNullOrEmpty(selectedRho) && startRange >= 0 && endRange >= 0)
            {
                if (ValidateRange(startRange, endRange, log_range))
                {
                    // selecionar as janelas
                    SelectedLogs.Add("VP", selectedVP);
                    SelectedLogs.Add("VS", selectedVS);
                    SelectedLogs.Add("Rho", selectedRho);

                    if (!string.IsNullOrEmpty(selectedGR))
                    {
                        SelectedLogs.Add("GR", selectedGR);
                    }
                    if (!string.IsNullOrEmpty(selectedCali))
                    {
                        SelectedLogs.Add("Cali", selectedCali);
                    }
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                } else
                {
                    MessageBox.Show("Os indices selecionados para a borda dos logs é inválido");
                };
            }
            else
            {
                MessageBox.Show("Selecione todos os campos antes de avançar para o próximo ponto");
                return;
            }
        }

        public Dictionary<string, string> getSelectedLogs()
        {
            return SelectedLogs;
        }

        public List<string> getLogs()
        {
            return logs;
        }

        public string getStartRange()
        {
            return startRange.ToString();
        }

        public string getEndRange()
        {
            return endRange.ToString();
        }

        public string getLasUnit()
        {
            return lasUnit;
        }
    }
}
