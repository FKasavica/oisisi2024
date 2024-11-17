using System;
using System.Collections.Generic;
using System.IO;
using SSluzba.Models;

namespace SSluzba.Repositories
{
    public class IndexRepository
    {
        //private readonly string FilePath = @"D:" + Path.DirectorySeparatorChar + "Github" + Path.DirectorySeparatorChar + "oisisi2024" + Path.DirectorySeparatorChar + "SSluzba" + Path.DirectorySeparatorChar + "Data" + Path.DirectorySeparatorChar + "indexes.csv";
        private readonly string FilePath = @"D:\Github\oisisi2024\SSluzba\Data\indexes.csv";

        public IndexRepository()
        {
            //FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "indexes.csv");
        }

        public List<Models.Index> LoadIndices()
        {
            List<Models.Index> indices = new List<Models.Index>();
            if (File.Exists(FilePath))
            {
                foreach (var line in File.ReadLines(FilePath))
                {
                    var values = line.Split(',');
                    Models.Index index = new Models.Index();
                    index.FromCSV(values);
                    indices.Add(index);
                }
            }
            return indices;
        }

        public void SaveIndices(List<Models.Index> indices)
        {
            using (StreamWriter sw = new StreamWriter(FilePath))
            {
                foreach (var index in indices)
                {
                    sw.WriteLine(string.Join(",", index.ToCSV()));
                }
            }
        }
    }
}
