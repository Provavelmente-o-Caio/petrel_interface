using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Slb.Ocean.Petrel.Commands;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.DomainObject;
using Slb.Ocean.Petrel.DomainObject.Well;
using System.IO;
using CsvHelper;
using System.Globalization;

namespace dlseis_well_tie_petrel
{
    class Export_Well_Data : SimpleCommandHandler
    {
        public static string ID = "dlseis_well_tie_petrel.Export_Well_Data";

        #region SimpleCommandHandler Members

        public override bool CanExecute(Slb.Ocean.Petrel.Contexts.Context context)
        {
            return true;
        }

        public override void Execute(Slb.Ocean.Petrel.Contexts.Context context)
        {
            // Log a message to the Petrel output window
            PetrelLogger.InfoOutputWindow(string.Format("{0} clicked", @"Export Well Data"));

            // Select the primary project
            Project project = PetrelProject.PrimaryProject;

            // Select the well root
            var wellroot = WellRoot.Get(project);

            // Select all wells (borehole collection)
            var wellCollection = wellroot.BoreholeCollection;

            // TODO: Define which well to choose dynamically
            // Initial idea is to create a window that lists the names of loaded wells in a dropdown
            // Select the well of type Borehole where the well name is equal to F02-1
            var wellname = "F02-1";
            var well = wellCollection.BoreholeCollections.ElementAt(0).Where(w => w.Name == wellname).Select(w => w).ElementAt(0);

            // Seleciona o log com nome igual a AI dentro do poço F02-1
            var log = well.Logs.WellLogs.Where(l => l.Name == "AI").Select(L => L).ElementAt(0);

            // Carrega Lista de MDs das amostras
            var logmds = log.Samples.Select(s => s.MD).ToList();

            // Carrega lista de valores das amostras
            var logsamples = log.Samples.Select(s => s.Value).ToList();
            logsamples = logsamples.Where(s => !float.IsNaN(s)).Select(s => s).ToList();

            // Caminho do arquivo CSV
            string localDataPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string writePath = String.Concat(localDataPath, "\\", wellname);
            ExportTrajectoryToTxt(writePath);
            PetrelLogger.InfoOutputWindow($"Export: Found {wellCollection.Count} boreholes");

        }

        private void ExportTrajectoryToTxt(string writePath)
        {
            string path = String.Concat(writePath, "\\", "trajectoryData.txt");
            PetrelLogger.InfoOutputWindow($"Trajectory data exported to: {path}");
        }

        private void ExportCheckshotToTxt(string writePath)
        {
            string path = String.Concat(writePath, "\\", "checkshot.txt");
            PetrelLogger.InfoOutputWindow($"Checkshot exported to: {path}");
        }

        private void ExportLogsToLas(string writePath)
        {
            string path = String.Concat(writePath, "\\", "logs.las");
            PetrelLogger.InfoOutputWindow($"Logs exported to: {path}");
        }

        private void ExportSeismicToSgy(string writePath)
        {
            string path = String.Concat(writePath, "\\", "seismic.sgy");
            PetrelLogger.InfoOutputWindow($"Seismic data exported to: {path}");
        }

        #endregion
    }
}