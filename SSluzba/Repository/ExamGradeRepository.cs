using System;
using System.Collections.Generic;
using System.IO;
using SSluzba.Models;

namespace SSluzba.Repositories
{
    public class ExamGradeRepository
    {
        private readonly string FilePath = @"D:" + Path.DirectorySeparatorChar + "Github" + Path.DirectorySeparatorChar + "oisisi2024" + Path.DirectorySeparatorChar + "SSluzba" + Path.DirectorySeparatorChar + "Data" + Path.DirectorySeparatorChar + "examg_rades.csv";

        public List<ExamGrade> LoadExamGrades()
        {
            List<ExamGrade> examGrades = new List<ExamGrade>();
            if (File.Exists(FilePath))
            {
                foreach (var line in File.ReadLines(FilePath))
                {
                    var values = line.Split(',');
                    ExamGrade examGrade = new ExamGrade();
                    examGrade.FromCSV(values);
                    examGrades.Add(examGrade);
                }
            }
            return examGrades;
        }

        public void SaveExamGrades(List<ExamGrade> examGrades)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(FilePath))
                {
                    foreach (var examGrade in examGrades)
                    {
                        sw.WriteLine(string.Join(",", examGrade.ToCSV()));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving exam grades: {ex.Message}");
            }
        }
    }
}
