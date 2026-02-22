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
        private Chart logPreviewChart;
        private int SCROLL_SIZE = 2000; 
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
            chartLogs.Series.Clear();
            chartLogs.ChartAreas.Clear();
            chartLogs.Legends.Clear();
            chartLogs.Titles.Clear();

            ChartArea chartArea = new ChartArea("MainArea");
            chartArea.AxisX.Title = "Log Value";
            chartArea.AxisX.TitleFont = new System.Drawing.Font("Arial", 9, System.Drawing.FontStyle.Bold);
            chartArea.AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chartArea.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;

            chartArea.AxisY.Title = "Sample Index";
            chartArea.AxisY.TitleFont = new System.Drawing.Font("Arial", 9, System.Drawing.FontStyle.Bold);
            chartArea.AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chartArea.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            chartArea.AxisY.LabelStyle.Format = "0";
            chartArea.AxisY.IsReversed = true; // Profundidade crescente de cima para baixo

            // scroll
            chartArea.AxisY.ScaleView.Size = SCROLL_SIZE;
            chartArea.AxisY.ScrollBar.Enabled = true;
            chartArea.AxisY.ScrollBar.IsPositionedInside = true;
            chartArea.AxisY.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            chartArea.AxisY.ScrollBar.LineColor = System.Drawing.Color.Gray;
            chartArea.AxisY.ScrollBar.Size = 15;

            chartArea.BackColor = System.Drawing.Color.WhiteSmoke;
            chartLogs.ChartAreas.Add(chartArea);

            Legend legend = new Legend("MainLegend");
            legend.Docking = Docking.Top;
            legend.Alignment = System.Drawing.StringAlignment.Center;
            legend.Font = new System.Drawing.Font("Arial", 8);
            chartLogs.Legends.Add(legend);

            Title title = new Title("Log Preview", Docking.Top,
                new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold),
                System.Drawing.Color.Black);
            chartLogs.Titles.Add(title);

            chartArea.CursorX.IsUserEnabled = true;
            chartArea.CursorX.IsUserSelectionEnabled = true;
            chartArea.AxisX.ScaleView.Zoomable = true;

            chartArea.CursorY.IsUserEnabled = true;
            chartArea.CursorY.IsUserSelectionEnabled = true;
            chartArea.AxisY.ScaleView.Zoomable = true;

            chartLogs.MouseClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    chartArea.AxisX.ScaleView.ZoomReset();
                    chartArea.AxisY.ScaleView.ZoomReset();
                }
            };
        }

        private void UpdateLogPreview(object send, EventArgs e)
        {
            chartLogs.Series.Clear();

            int start = (int)unitTextBox_range_start.Value;
            int end = (int)unitTextBox_range_end.Value;

            // Desenhar cada log selecionado
            if (!string.IsNullOrEmpty(comboBoxVP.Text))
                DrawLogTrace(comboBoxVP.Text, "VP", start, end, System.Drawing.Color.Blue);

            if (!string.IsNullOrEmpty(comboBoxVS.Text))
                DrawLogTrace(comboBoxVS.Text, "VS", start, end, System.Drawing.Color.Red);

            if (!string.IsNullOrEmpty(comboBoxRho.Text))
                DrawLogTrace(comboBoxRho.Text, "Rho", start, end, System.Drawing.Color.Green);

            if (!string.IsNullOrEmpty(comboBoxGR.Text))
                DrawLogTrace(comboBoxGR.Text, "GR", start, end, System.Drawing.Color.Orange);

            if (!string.IsNullOrEmpty(comboBoxCali.Text))
                DrawLogTrace(comboBoxCali.Text, "Cali", start, end, System.Drawing.Color.Purple);

            var area = chartLogs.ChartAreas["MainArea"];
            area.AxisY.ScaleView.Zoom(start, start + SCROLL_SIZE);

            // Atualizar título com informações
            if (chartLogs.Titles.Count > 0)
            {
                chartLogs.Titles[0].Text = $"Log Preview (Range: {end} to {start})";
            }
        }

        private void DrawLogTrace(string logName, string seriesName, int startIdx, int endIdx, System.Drawing.Color color)
        {
            try
            {
                var wellLog = well.Logs.WellLogs.FirstOrDefault(w => w.Name == logName);
                if (wellLog == null) return;

                // Validar range
                if (startIdx < endIdx || startIdx > wellLog.SampleCount || endIdx < 0)
                    return;

                // Criar nova série
                Series series = new Series(seriesName);
                series.ChartType = SeriesChartType.Line;
                series.BorderWidth = 2;
                series.Color = color;
                series.MarkerStyle = MarkerStyle.None; // Sem marcadores para melhor performance
                series.LegendText = $"{seriesName} ({logName})";

                // Extrair valores do log no range especificado
                int count = startIdx - endIdx;
                if (count <= 0) return;

                // Adicionar pontos à série
                int step = Math.Max(1, count / 1000); // Limitar a 1000 pontos para performance

                for (int i = 0; i < count; i += step)
                {
                    int idx = endIdx + i;
                    if (idx >= wellLog.SampleCount) break;

                    float value = (float)wellLog[idx].Value;

                    // Verificar valores inválidos (NaN, Infinity)
                    if (!float.IsNaN(value) && !float.IsInfinity(value))
                    {
                        series.Points.AddXY(value, idx);
                    }
                }

                // Adicionar último ponto se não foi incluído
                if ((count - 1) % step != 0)
                {
                    int lastIdx = endIdx + count - 1;
                    if (lastIdx < wellLog.SampleCount)
                    {
                        float lastValue = (float)wellLog[lastIdx].Value;
                        if (!float.IsNaN(lastValue) && !float.IsInfinity(lastValue))
                        {
                            series.Points.AddXY(lastValue, lastIdx);
                        }
                    }
                }

                // Adicionar série ao gráfico
                chartLogs.Series.Add(series);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao desenhar {seriesName}: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
