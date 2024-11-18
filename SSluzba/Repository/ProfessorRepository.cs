using System;
using System.Collections.Generic;
using System.IO;
using SSluzba.Models;

namespace SSluzba.Repositories
{
    public class ProfessorRepository
    {
        private readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Data", "professors.csv");

        public List<Professor> LoadProfessors()
        {
            List<Professor> professors = new List<Professor>();
            if (File.Exists(FilePath))
            {
                foreach (var line in File.ReadLines(FilePath))
                {
                    var values = line.Split(',');
                    Professor professor = new Professor();
                    professor.FromCSV(values);
                    professors.Add(professor);
                }
            }
            return professors;
        }

        public void SaveProfessors(List<Professor> professors)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(FilePath))
                {
                    foreach (var professor in professors)
                    {
                        sw.WriteLine(string.Join(",", professor.ToCSV()));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving professors: {ex.Message}");
            }
        }
    }
}
