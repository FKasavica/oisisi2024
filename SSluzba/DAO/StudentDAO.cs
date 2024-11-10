using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSluzba.DAO
{
    using global::SSluzba.Models;
    using global::SSluzba.Observer;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    namespace SSluzba.DAO
    {
        public class StudentDAO
        {
            private List<Student> _students;

            public StudentDAO()
            {
                _students = new List<Student>();
            }

            public int NextId()
            {
                return _students.Count == 0 ? 1 : _students.Max(s => s.Id) + 1;
            }

            public void Add(Student student)
            {
                student.Id = NextId();
                _students.Add(student);
                NotifyObservers();
            }

            public void Remove(Student student)
            {
                _students.Remove(student);
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
                    NotifyObservers();
                }
            }

            public List<Student> GetAll()
            {
                return _students;
            }

            private List<IObserver> _observers = new List<IObserver>();

            public void Subscribe(IObserver observer)
            {
                _observers.Add(observer);
            }

            public void Unsubscribe(IObserver observer)
            {
                _observers.Remove(observer);
            }

            private void NotifyObservers()
            {
                foreach (var observer in _observers)
                {
                    observer.Update();
                }
            }
        }
    }

}
