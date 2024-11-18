using SSluzba.DAO;
using SSluzba.Models;
using SSluzba.Observer;
using SSluzba.Views;
using System;
using System.Collections.Generic;
using System.Windows;
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

        public (Student student, string majorCode, int enrollmentNumber, int enrollmentYear, List<Address> allAddresses, Address selectedAddress, string status) GetStudentDataForUpdate(int studentId)
        {
            var student = _studentDAO.GetAll().FirstOrDefault(s => s.Id == studentId);
            if (student == null)
            {
                throw new ArgumentException("Student not found.");
            }

            // Retrieve index data
            var index = _indexController.GetIndexById(student.IndexId);
            string majorCode = index?.MajorCode ?? string.Empty;
            int enrollmentNumber = index?.EnrollmentNumber ?? 0;
            int enrollmentYear = index?.EnrollmentYear ?? 0;

            // Retrieve address data
            var addresses = _addressController.GetAllAddresses();
            var selectedAddress = addresses.FirstOrDefault(a => a.Id == student.AddressId);

            // Determine status as string
            string status = student.Status == Status.Budget ? "Budget" : "SelfFinanced";

            return (student, majorCode, enrollmentNumber, enrollmentYear, addresses, selectedAddress, status);
        }
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
            var updateStudentWindow = new UpdateStudentView(student.Id);
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

                var averageGrade = Math.Round(_examGradeController.GetAverageGrade(student.Id), 2);


                studentDetails.Add(new
                {
                    student.Id,
                    student.Surname,
                    student.Name,
                    student.CurrentYear,
                    AverageGrade = averageGrade,
                    Index = indexString,
                    Address = addressString,
                    student.Status
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

        public Student UpdateExistingStudent(
            int studentId,
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
            // Validate inputs
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

            // Determine student status
            Status studentStatus = Status.Budget;
            if (statusInput is ComboBoxItem selectedItem)
            {
                string statusContent = selectedItem.Content.ToString();
                studentStatus = statusContent == "Budget" ? Status.Budget : Status.SelfFinanced;
            }

            // Get selected address
            if (!(addressComboBoxSelectedItem is Address selectedAddress))
            {
                throw new ArgumentException("Invalid address selected.");
            }

            // Retrieve the student using the studentId
            var existingStudent = _studentDAO.GetAll().FirstOrDefault(s => s.Id == studentId);
            if (existingStudent == null)
            {
                throw new ArgumentException("Student not found.");
            }

            // Retrieve and update index using IndexController based on existing student's IndexId
            var existingIndex = _indexController.GetIndexById(existingStudent.IndexId);
            if (existingIndex != null)
            {
                existingIndex.MajorCode = majorCode;
                existingIndex.EnrollmentNumber = enrollmentNumber;
                existingIndex.EnrollmentYear = enrollmentYear;
                _indexController.UpdateIndex(existingIndex);
            }
            else
            {
                // If the index does not exist, create a new one
                _indexController.AddIndex(majorCode, enrollmentNumber, enrollmentYear);
            }

            // Create updated student object
            try
            {
                var updatedStudent = new Student
                {
                    Id = studentId,
                    Surname = surname,
                    Name = name,
                    DateOfBirth = dateOfBirth.Value,
                    AddressId = selectedAddress.Id,
                    PhoneNumber = phoneNumber,
                    Email = email,
                    CurrentYear = currentYear,
                    Status = studentStatus,
                    IndexId = existingIndex?.Id ?? _indexController.GetIndexId(majorCode, enrollmentNumber, enrollmentYear)
                };

                _studentDAO.Update(updatedStudent);
                return updatedStudent;
            }
            catch (Exception ex)
            {
                throw new Exception($"Greška prilikom kreiranja ili ažuriranja studenta: {ex.Message}", ex);
            }
        }

        public Student GetStudentById(int studentId)
        {
            return _studentDAO.GetAll().FirstOrDefault(s => s.Id == studentId);
        }
    }
}
