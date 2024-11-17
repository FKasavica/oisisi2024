using System.Collections.Generic;
using System.Linq;
using SSluzba.Models;
using SSluzba.Observer;
using SSluzba.Repositories;

namespace SSluzba.DAO
{
    public class StudentSubjectDAO : ISubject
    {
        private List<StudentSubject> _studentSubjects;
        private readonly StudentSubjectRepository _repository;
        private readonly List<IObserver> _observers; // List to manage observers

        public StudentSubjectDAO()
        {
            _repository = new StudentSubjectRepository();
            _studentSubjects = _repository.LoadStudentSubjects();
            _observers = new List<IObserver>(); // Initialize the observers list
        }

        public void AddStudentSubject(StudentSubject studentSubject)
        {
            studentSubject.Id = GetNextId();
            _studentSubjects.Add(studentSubject);
            _repository.SaveStudentSubjects(_studentSubjects);
            NotifyObservers(); // Notify observers of the change
        }

        public void UpdateStudentSubject(StudentSubject studentSubject)
        {
            var existing = _studentSubjects.FirstOrDefault(ss => ss.Id == studentSubject.Id);
            if (existing != null)
            {
                existing.StudentId = studentSubject.StudentId;
                existing.SubjectId = studentSubject.SubjectId;
                existing.Passed = studentSubject.Passed;
                _repository.SaveStudentSubjects(_studentSubjects);
                NotifyObservers(); // Notify observers of the change
            }
        }

        public void DeleteStudentSubject(int id)
        {
            _studentSubjects.RemoveAll(ss => ss.Id == id);
            _repository.SaveStudentSubjects(_studentSubjects);
            NotifyObservers(); // Notify observers of the change
        }

        public List<StudentSubject> GetAll()
        {
            return _studentSubjects;
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

        private int GetNextId()
        {
            return _studentSubjects.Count == 0 ? 1 : _studentSubjects.Max(ss => ss.Id) + 1;
        }
    }
}
