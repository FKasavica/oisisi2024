using System;
using System.Collections.Generic;
using System.IO;
using SSluzba.Models;

namespace SSluzba.Repositories
{
    public class SubjectRepository
    {
        private readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Data", "subjects.csv");

        public List<Subject> LoadSubjects()
        {
            List<Subject> subjects = new List<Subject>();
            if (File.Exists(FilePath))
            {
                foreach (var line in File.ReadLines(FilePath))
                {
                    var values = line.Split(',');
                    Subject subject = new Subject();
                    subject.FromCSV(values);
                    subjects.Add(subject);
                }
            }
            return subjects;
        }

        public void SaveSubjects(List<Subject> subjects)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(FilePath))
                {
                    foreach (var subject in subjects)
                    {
                        sw.WriteLine(string.Join(",", subject.ToCSV()));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving subjects: {ex.Message}");
            }
        }
    }
}
