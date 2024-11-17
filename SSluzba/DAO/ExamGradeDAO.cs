using System.Collections.Generic;
using System.Linq;
using SSluzba.Models;
using SSluzba.Observer;
using SSluzba.Repositories;

namespace SSluzba.DAO
{
    public class ExamGradeDAO : ISubject
    {
        private List<ExamGrade> _examGrades;
        private List<IObserver> _observers;
        private ExamGradeRepository _repository;

        public ExamGradeDAO()
        {
            _repository = new ExamGradeRepository();
            _examGrades = _repository.LoadExamGrades();
            _observers = new List<IObserver>();
        }

        public void Add(ExamGrade examGrade)
        {
            examGrade.Id = GetNextId();
            _examGrades.Add(examGrade);
            _repository.SaveExamGrades(_examGrades);
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
                _repository.SaveExamGrades(_examGrades);
                NotifyObservers();
            }
        }

        public void Delete(int id)
        {
            _examGrades.RemoveAll(g => g.Id == id);
            _repository.SaveExamGrades(_examGrades);
            NotifyObservers();
        }

        public List<ExamGrade> GetAll()
        {
            return _examGrades;
        }

        public List<ExamGrade> GetGradesByStudentId(int studentId)
        {
            return _examGrades.Where(g => g.StudentId == studentId).ToList();
        }

        private int GetNextId()
        {
            return _examGrades.Count == 0 ? 1 : _examGrades[^1].Id + 1;
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
