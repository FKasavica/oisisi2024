using System.Collections.Generic;
using System.Linq;
using SSluzba.Models;
using SSluzba.Observer;
using SSluzba.Repositories;

namespace SSluzba.DAO
{
    public class SubjectDAO : ISubject
    {
        private List<IObserver> _observers;
        private List<Subject> _subjects;
        private SubjectRepository _repository;

        public SubjectDAO()
        {
            _repository = new SubjectRepository();
            _subjects = _repository.LoadSubjects();
            _observers = new List<IObserver>();
        }

        public void Add(Subject subject)
        {
            subject.Id = GetNextId();
            _subjects.Add(subject);
            _repository.SaveSubjects(_subjects);
            NotifyObservers();
        }

        public void Update(Subject updatedSubject)
        {
            var existingSubject = _subjects.FirstOrDefault(s => s.Id == updatedSubject.Id);
            if (existingSubject != null)
            {
                existingSubject.Code = updatedSubject.Code;
                existingSubject.Name = updatedSubject.Name;
                existingSubject.Semester = updatedSubject.Semester;
                existingSubject.StudyYear = updatedSubject.StudyYear;
                existingSubject.ProfessorId = updatedSubject.ProfessorId;
                existingSubject.EspbPoints = updatedSubject.EspbPoints;
                _repository.SaveSubjects(_subjects);
                NotifyObservers();
            }
        }

        public void Remove(int id)
        {
            var subject = _subjects.FirstOrDefault(s => s.Id == id);
            if (subject != null)
            {
                _subjects.Remove(subject);
                _repository.SaveSubjects(_subjects);
                NotifyObservers();
            }
        }

        public List<Subject> GetAll()
        {
            return _subjects;
        }

        private int GetNextId()
        {
            return _subjects.Count == 0 ? 1 : _subjects.Max(s => s.Id) + 1;
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
