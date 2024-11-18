using System;
using System.Collections.Generic;
using System.Linq;
using SSluzba.DAO;
using SSluzba.Models;
using SSluzba.Observer;
using SSluzba.Repositories;

namespace SSluzba.Controllers
{
    public class ProfessorController
    {
        private readonly ProfessorDAO _professorDAO;
        private readonly AddressController _addressController;
        //private readonly SubjectController _subjectController;

        public ProfessorController()
        {
            _professorDAO = new ProfessorDAO();
            _addressController = new AddressController();
            //_subjectController = new SubjectController();
        }

        // Get a professor by ID
        public Professor GetProfessorById(int professorId)
        {
            return _professorDAO.GetAll().FirstOrDefault(p => p.Id == professorId);
        }

        // Get all professors
        public List<Professor> GetAllProfessors()
        {
            return _professorDAO.GetAll();
        }

        // Create a new professor
        public Professor CreateNewProfessor(
            string surname,
            string name,
            DateTime dateOfBirth,
            string phoneNumber,
            string email,
            string personalIdNumber,
            string title,
            int yearsOfExperience,
            int addressId,
            List<Subject> subjects)
        {
            if (string.IsNullOrWhiteSpace(surname) || string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(phoneNumber) ||
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(personalIdNumber) || string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Please fill in all fields correctly.");
            }

            var newProfessor = new Professor
            {
                Surname = surname,
                Name = name,
                DateOfBirth = dateOfBirth,
                PhoneNumber = phoneNumber,
                Email = email,
                PersonalIdNumber = personalIdNumber,
                Title = title,
                YearsOfExperience = yearsOfExperience,
                AddressId = addressId,
                Subjects = subjects ?? new List<Subject>()
            };

            _professorDAO.Add(newProfessor);
            return newProfessor;
        }

        // Update an existing professor
        public Professor UpdateProfessor(
            int professorId,
            string surname,
            string name,
            DateTime dateOfBirth,
            string phoneNumber,
            string email,
            string personalIdNumber,
            string title,
            int yearsOfExperience,
            int addressId,
            List<Subject> subjects)
        {
            var existingProfessor = GetProfessorById(professorId);
            if (existingProfessor == null)
            {
                throw new ArgumentException("Professor not found.");
            }

            existingProfessor.Surname = surname;
            existingProfessor.Name = name;
            existingProfessor.DateOfBirth = dateOfBirth;
            existingProfessor.PhoneNumber = phoneNumber;
            existingProfessor.Email = email;
            existingProfessor.PersonalIdNumber = personalIdNumber;
            existingProfessor.Title = title;
            existingProfessor.YearsOfExperience = yearsOfExperience;
            existingProfessor.AddressId = addressId;
            existingProfessor.Subjects = subjects;

            _professorDAO.Update(existingProfessor);
            return existingProfessor;
        }

        // Delete a professor
        public void DeleteProfessor(int professorId)
        {
            var professorToDelete = GetProfessorById(professorId);
            if (professorToDelete != null)
            {
                _professorDAO.Remove(professorToDelete);
            }
        }

        // Get professor details (with related address and subjects)
        public dynamic GetProfessorDetails(int professorId)
        {
            var professor = GetProfessorById(professorId);
            if (professor == null)
            {
                throw new ArgumentException("Professor not found.");
            }

            var address = _addressController.GetAddressById(professor.AddressId);
            string addressString = address != null ? address.ToString() : "No address found";

            //var subjects = _subjectController.GetSubjectsByProfessorId(professor.Id);
            //string subjectsString = subjects != null ? string.Join(", ", subjects.Select(s => s.Name)) : "No subjects assigned";

            return new
            {
                professor.Id,
                professor.Name,
                professor.Surname,
                professor.Title,
                professor.YearsOfExperience,
                Address = addressString
                //Subjects
            };
        }

        // Subscribe an observer
        public void Subscribe(IObserver observer)
        {
            _professorDAO.Subscribe(observer);
        }

        // Unsubscribe an observer
        public void Unsubscribe(IObserver observer)
        {
            _professorDAO.Unsubscribe(observer);
        }
    }
}
