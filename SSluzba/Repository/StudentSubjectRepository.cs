using System;
using System.Collections.Generic;
using System.IO;
using SSluzba.Models;

namespace SSluzba.Repositories
{
    public class StudentSubjectRepository
    {
        private readonly string FilePath = @"D:" + Path.DirectorySeparatorChar + "Github" + Path.DirectorySeparatorChar + "oisisi2024" + Path.DirectorySeparatorChar + "SSluzba" + Path.DirectorySeparatorChar + "Data" + Path.DirectorySeparatorChar + "student_subject.csv";

        public List<StudentSubject> LoadStudentSubjects()
        {
            List<StudentSubject> studentSubjects = new List<StudentSubject>();
            if (File.Exists(FilePath))
            {
                foreach (var line in File.ReadLines(FilePath))
                {
                    var values = line.Split(',');
                    StudentSubject studentSubject = new StudentSubject();
                    studentSubject.FromCSV(values);
                    studentSubjects.Add(studentSubject);
                }
            }
            return studentSubjects;
        }

        public void SaveStudentSubjects(List<StudentSubject> studentSubjects)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(FilePath))
                {
                    foreach (var studentSubject in studentSubjects)
                    {
                        sw.WriteLine(string.Join(",", studentSubject.ToCSV()));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving student subjects: {ex.Message}");
            }
        }

        public int GetNextId(List<StudentSubject> studentSubjects)
        {
            return studentSubjects.Count == 0 ? 1 : studentSubjects.Max(s => s.Id) + 1;
        }

        public void AddStudentSubject(StudentSubject studentSubject, List<StudentSubject> studentSubjects)
        {
            studentSubject.Id = GetNextId(studentSubjects);
            studentSubjects.Add(studentSubject);
            SaveStudentSubjects(studentSubjects);
        }

        public void UpdateStudentSubject(StudentSubject updatedStudentSubject, List<StudentSubject> studentSubjects)
        {
            var existingStudentSubject = studentSubjects.Find(s => s.Id == updatedStudentSubject.Id);
            if (existingStudentSubject != null)
            {
                existingStudentSubject.StudentId = updatedStudentSubject.StudentId;
                existingStudentSubject.SubjectId = updatedStudentSubject.SubjectId;
                existingStudentSubject.Passed = updatedStudentSubject.Passed;
                SaveStudentSubjects(studentSubjects);
            }
        }

        public void RemoveStudentSubject(int id, List<StudentSubject> studentSubjects)
        {
            var studentSubject = studentSubjects.Find(s => s.Id == id);
            if (studentSubject != null)
            {
                studentSubjects.Remove(studentSubject);
                SaveStudentSubjects(studentSubjects);
            }
        }
    }
}
