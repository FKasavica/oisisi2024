﻿using System;
using System.Collections.Generic;
using System.IO;
using SSluzba.Models;

namespace SSluzba.Repositories
{
    public class ExamGradeRepository
    {
        private readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Data", "exam_grades.csv");

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
