using System;
using System.Collections.Generic;
using System.Linq;
using SSluzba.DAO;
using SSluzba.DAO.SSluzba.DAO;
using SSluzba.Models;
using SSluzba.Observer;

namespace SSluzba.Controllers
{
    public class StudentController
    {
        private StudentDAO _studentDAO;

        public StudentController()
        {
            _studentDAO = new StudentDAO();
        }

        public void Subscribe(IObserver observer)
        {
            _studentDAO.Subscribe(observer);
        }

        public void AddStudent(string surname, string name, DateTime dateOfBirth, string phoneNumber, string email, int indexId, int currentYear, Status status, double averageGrade)
        {
            var newStudent = new Student
            {
                Surname = surname,
                Name = name,
                DateOfBirth = dateOfBirth,
                PhoneNumber = phoneNumber,
                Email = email,
                IndexId = indexId,
                CurrentYear = currentYear,
                Status = status,
                AverageGrade = averageGrade
            };

            _studentDAO.Add(newStudent);
        }

        public void UpdateStudent(Student updatedStudent)
        {
            _studentDAO.Update(updatedStudent);
        }

        public void DeleteStudent(int id)
        {
            var student = _studentDAO.GetAll().FirstOrDefault(s => s.Id == id);
            if (student != null)
            {
                _studentDAO.Remove(student);
            }
        }

        public List<Student> GetAllStudents()
        {
            return _studentDAO.GetAll();
        }
    }
}