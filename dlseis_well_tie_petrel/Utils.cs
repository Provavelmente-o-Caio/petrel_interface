using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.DomainObject.Well;
using System.Globalization;
using System.IO;

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

            var lines = File.ReadAllLines(filePath);

            if (lines.Length < 2)
            {
                throw new Exception("Not enouth lines in the file.");
            }

            var columns = lines[lineToRead]
                .Split((char[])null, StringSplitOptions.RemoveEmptyEntries)
                .Select(col => System.Text.RegularExpressions.Regex.Replace(col, @"\([^)]*\)", "")) // Remove content inside (...)
                .Select(col => col.Trim())
                .Where(col => col != "#" && col != "")
                .Distinct()
                .ToArray();

            return columns;
        }
    }
}
