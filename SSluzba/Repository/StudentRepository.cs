using System;
using System.Collections.Generic;
using System.IO;
using SSluzba.Models;

namespace SSluzba.Repositories
{
    public class StudentRepository
    {
        private readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Data", "students.csv");

        public StudentRepository() { }

        public List<Student> LoadStudents()
        {
            List<Student> students = new List<Student>();
            if (File.Exists(FilePath))
            {
                foreach (var line in File.ReadLines(FilePath))
                {
                    var values = line.Split(',');
                    Student student = new Student();
                    student.FromCSV(values);
                    students.Add(student);
                }
            }
            return students;
        }

        public void SaveStudents(List<Student> students)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(FilePath))
                {
                    foreach (var student in students)
                    {
                        sw.WriteLine(string.Join(",", student.ToCSV()));
                    }
                }
            }
            catch (Exception ex)
            {
                // Prikaz poruke za grešku ili logovanje greške
                Console.WriteLine($"Error saving students: {ex.Message}");
            }
        }

    }
}
