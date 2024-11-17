using SSluzba.DAO;
using SSluzba.Models;
using SSluzba.Observer;
using SSluzba.Views;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace SSluzba.Controllers
{
    public class StudentController
    {
        private readonly StudentDAO _studentDAO = new();
        private readonly AddressController _addressController = new();
        private readonly IndexController _indexController = new();
        private readonly ExamGradeController _examGradeController = new();

        public StudentController(){}

        public void Subscribe(IObserver observer)
        {
            _studentDAO.Subscribe(observer);
        }

        public void AddStudent(Student student)
        {
            _studentDAO.Add(student);
        }

        public void OpenUpdateStudentView(SSluzba.Models.Student student)
        {
            var updateStudentWindow = new UpdateStudentView(student);
            updateStudentWindow.ShowDialog();
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

        public List<dynamic> GetStudentDetails()
        {
            var students = GetAllStudents();
            var studentDetails = new List<dynamic>();

            foreach (var student in students)
            {
                var address = _addressController.GetAddressById(student.AddressId);
                string addressString = address != null ? address.ToString() : "Address not found";

                var index = _indexController.GetIndexById(student.IndexId);
                string indexString = index != null ? index.ToString() : "Index not found";

                var averageGrade = _examGradeController.GetAverageGrade(student.Id);

                studentDetails.Add(new
                {
                    student.Id,
                    student.Surname,
                    student.Name,
                    student.CurrentYear,
                    AverageGrade = averageGrade,
                    Index = indexString,
                    Address = addressString
                });
            }

            return studentDetails;
        }

        public List<ExamGrade> GetExamGradesForStudent(int studentId)
        {
            return _examGradeController.GetExamGradesForStudent(studentId);
        }

        public Student CreateNewStudent(
            string surname,
            string name,
            DateTime? dateOfBirth,
            string phoneNumber,
            string email,
            string majorCode,
            string enrollmentNumberText,
            string enrollmentYearText,
            string currentYearText,
            object statusInput,
            object addressComboBoxSelectedItem)
        {
            // Validacija unosa
            if (string.IsNullOrWhiteSpace(surname) ||
                string.IsNullOrWhiteSpace(name) ||
                dateOfBirth == null ||
                string.IsNullOrWhiteSpace(phoneNumber) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(majorCode) ||
                !int.TryParse(enrollmentNumberText, out int enrollmentNumber) ||
                !int.TryParse(enrollmentYearText, out int enrollmentYear) ||
                !int.TryParse(currentYearText, out int currentYear) ||
                statusInput == null ||
                addressComboBoxSelectedItem == null)
            {
                throw new ArgumentException("Please fill in all fields correctly.");
            }

            _indexController.AddIndex(majorCode, enrollmentNumber, enrollmentYear);

            var studentStatus = Status.Budget;
            if (statusInput is ComboBoxItem selectedItem)
            {
                string statusContent = selectedItem.Content.ToString();
                studentStatus = statusContent == "Budget" ? Status.Budget : Status.SelfFinanced;
            }
            else
            {
                throw new ArgumentException("Invalid status selected.");
            }
            

            int indexId = _indexController.GetIndexId(majorCode, enrollmentNumber, enrollmentYear);
            if (indexId == -1)
            {
                throw new Exception("Index could not be found or created.");
            }

            var selectedAddress = addressComboBoxSelectedItem as Address;
            if (selectedAddress == null)
            {
                throw new ArgumentException("Invalid address selected.");
            }

            var student = new Student
            {
                Surname = surname,
                Name = name,
                DateOfBirth = dateOfBirth.Value,
                AddressId = selectedAddress.Id,
                PhoneNumber = phoneNumber,
                Email = email,
                CurrentYear = currentYear,
                Status = studentStatus,
                IndexId = indexId
            };

            AddStudent(student);
            return student;
        }
    }
}
