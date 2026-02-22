using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;

namespace dlseis_well_tie_petrel
{
    class Utils
    {
        public static List<string> GetWellNames()
        {
            List<string> wellNames = new List<string>();

            // Get the root of all wells
            return wellNames;
        }

        public static string[] GetColumnsFromFile(string filePath, int lineToRead)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found.", filePath);
            }

            string extension = Path.GetExtension(filePath).ToLower();

            if (extension == ".las")
            {
                return GetColumnsFromLasFile(filePath, lineToRead);
            }

            // Processamento normal
            return GetColumnsFromTextFile(filePath, lineToRead);
        }

        private static string[] GetColumnsFromLasFile(string filePath, int lineToRead)
        {
            var lines = File.ReadAllLines(filePath);
            var columns = new List<string>();
            bool inCurveSelection = false;

            foreach (var line in lines)
            {
                var trimmedLine = line.Trim();

                if (trimmedLine.StartsWith("~C") || trimmedLine.StartsWith("~CURVE"))
                {
                    inCurveSelection = true;
                    continue;
                }

                // fim da leitura de headers
                if (inCurveSelection && trimmedLine.StartsWith("~"))
                {
                    break;
                }

                if (inCurveSelection && !string.IsNullOrEmpty(trimmedLine) && !trimmedLine.StartsWith("#"))
                {
                    var parts = trimmedLine.Split(new[] { '.', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length > 0)
                    {
                        string curveName = parts[0].Trim();

                        if (!string.IsNullOrWhiteSpace(curveName))
                        {
                            columns.Add(curveName);
                        }
                    }
                }
            }

            if (columns.Count == 0)
            {
                throw new Exception("No curves found in LAS file.");
            }

            return columns.ToArray();
        }

        private static string[] GetColumnsFromTextFile(string filePath, int lineToRead)
        {
            var lines = File.ReadAllLines(filePath);

            if (lines.Length < 2)
            {
                throw new Exception("Not enouth lines in the file.");
            }

            var columnOccurrences = new Dictionary<string, int>();
            var columns = lines[lineToRead]
                .Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(col => System.Text.RegularExpressions.Regex.Replace(col, @"\([^)]*\)", ""))
                .Select(col => col.Trim())
                .Where(col => col != "#" && col != "")
                .Select(col =>
                {
                    if (!columnOccurrences.ContainsKey(col))
                    {
                        columnOccurrences[col] = 1;
                        return col;
                    }
                    else
                    {
                        columnOccurrences[col]++;
                        return $"{col}{columnOccurrences[col]}";
                    }
                })
                .ToArray();

            return columns;
        }

        public static string WriteJson<T>(T exportData, string runURI, string write_path)
        {
            String exportJson = JsonConvert.SerializeObject(exportData, Formatting.Indented);
            String path_Json = Path.Combine(runURI, write_path);
            File.WriteAllText(path_Json, exportJson);

            return path_Json;
        }
    }
}
