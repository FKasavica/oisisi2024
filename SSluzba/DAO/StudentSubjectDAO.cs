using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SSluzba.Models;

namespace SSluzba.DAO
{
    public class StudentSubjectDAO
    {
        private readonly string FilePath = "data" + Path.DirectorySeparatorChar + "studentsubjects.csv";
        private List<StudentSubject> _studentSubjects;

        public StudentSubjectDAO()
        {
            _studentSubjects = LoadStudentSubjects();
        }

        private List<StudentSubject> LoadStudentSubjects()
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

        public void SaveStudentSubjects()
        {
            using (StreamWriter sw = new StreamWriter(FilePath))
            {
                foreach (var studentSubject in _studentSubjects)
                {
                    sw.WriteLine(string.Join(",", studentSubject.ToCSV()));
                }
            }
        }

        public void AddStudentSubject(StudentSubject studentSubject)
        {
            studentSubject.Id = GetNextId();
            _studentSubjects.Add(studentSubject);
            SaveStudentSubjects();
        }

        public void UpdateStudentSubject(StudentSubject studentSubject)
        {
            var existing = _studentSubjects.FirstOrDefault(ss => ss.Id == studentSubject.Id);
            if (existing != null)
            {
                existing.StudentId = studentSubject.StudentId;
                existing.SubjectId = studentSubject.SubjectId;
                existing.Passed = studentSubject.Passed;
                SaveStudentSubjects();
            }
        }

        public void DeleteStudentSubject(int id)
        {
            _studentSubjects.RemoveAll(ss => ss.Id == id);
            SaveStudentSubjects();
        }

        public List<StudentSubject> GetAll()
        {
            return _studentSubjects;
        }

        private int GetNextId()
        {
            return _studentSubjects.Count == 0 ? 1 : _studentSubjects.Max(ss => ss.Id) + 1;
        }
    }
}
