using System;
using System.Collections.Generic;
using System.IO;
using SSluzba.Models;
using SSluzba.Observer;

namespace SSluzba.DAO
{
    public class ExamGradeDAO : ISubject
    {
        private const string FilePath = "../../Data/exam_grades.csv";
        private List<ExamGrade> _examGrades;
        private List<IObserver> _observers;

        public ExamGradeDAO()
        {
            _examGrades = LoadExamGrades();
            _observers = new List<IObserver>();
        }

        public void Add(ExamGrade examGrade)
        {
            examGrade.Id = GetNextId();
            _examGrades.Add(examGrade);
            SaveExamGrades();
            NotifyObservers();
        }

        public void Update(ExamGrade updatedGrade)
        {
            var existingGrade = _examGrades.Find(g => g.Id == updatedGrade.Id);
            if (existingGrade != null)
            {
                existingGrade.StudentId = updatedGrade.StudentId;
                existingGrade.SubjectId = updatedGrade.SubjectId;
                existingGrade.NumericGrade = updatedGrade.NumericGrade;
                existingGrade.ExamDate = updatedGrade.ExamDate;
                SaveExamGrades();
                NotifyObservers();
            }
        }

        public void Delete(int id)
        {
            _examGrades.RemoveAll(g => g.Id == id);
            SaveExamGrades();
            NotifyObservers();
        }

        public List<ExamGrade> GetAll()
        {
            return _examGrades;
        }

        private List<ExamGrade> LoadExamGrades()
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

        private int GetNextId()
        {
            return _examGrades.Count == 0 ? 1 : _examGrades[^1].Id + 1;
        }

        private void SaveExamGrades()
        {
            using (var sw = new StreamWriter(FilePath))
            {
                foreach (var grade in _examGrades)
                {
                    sw.WriteLine(string.Join(",", grade.ToCSV()));
                }
            }
        }

        public void Subscribe(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.Update();
            }
        }
    }
}
