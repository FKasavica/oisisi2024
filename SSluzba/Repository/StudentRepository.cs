using System;
using System.Collections.Generic;
using System.IO;
using SSluzba.Models;

namespace SSluzba.Repositories
{
    public class StudentRepository
    {
        private readonly string FilePath;

        public StudentRepository()
        {
            FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "students.csv");
        }

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
            using (StreamWriter sw = new StreamWriter(FilePath))
            {
                foreach (var student in students)
                {
                    sw.WriteLine(string.Join(",", student.ToCSV()));
                }
            }
        }
    }
}
