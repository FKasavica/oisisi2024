using SSluzba.Models;
using SSluzba.Observer;
using SSluzba.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace SSluzba.DAO
{
    public class StudentDAO : ISubject
    {
        private List<Student> _students;
        private List<IObserver> _observers;
        private StudentRepository _repository;

        public StudentDAO()
        {
            _repository = new StudentRepository();
            _students = _repository.LoadStudents();
            _observers = new List<IObserver>();
        }

        public int NextId()
        {
            return _students.Count == 0 ? 1 : _students.Max(s => s.Id) + 1;
        }

        public void Add(Student student)
        {
            student.Id = NextId();
            _students.Add(student);
            _repository.SaveStudents(_students);
            NotifyObservers();
        }

        public void Remove(Student student)
        {
            _students.Remove(student);
            _repository.SaveStudents(_students);
            NotifyObservers();
        }

        public void Update(Student updatedStudent)
        {
            var existingStudent = _students.FirstOrDefault(s => s.Id == updatedStudent.Id);
            if (existingStudent != null)
            {
                existingStudent.Surname = updatedStudent.Surname;
                existingStudent.Name = updatedStudent.Name;
                existingStudent.DateOfBirth = updatedStudent.DateOfBirth;
                existingStudent.PhoneNumber = updatedStudent.PhoneNumber;
                existingStudent.Email = updatedStudent.Email;
                existingStudent.IndexId = updatedStudent.IndexId;
                existingStudent.CurrentYear = updatedStudent.CurrentYear;
                existingStudent.Status = updatedStudent.Status;
                existingStudent.AverageGrade = updatedStudent.AverageGrade;
                _repository.SaveStudents(_students);
                NotifyObservers();
            }
        }

        public List<Student> GetAll()
        {
            return _students;
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
