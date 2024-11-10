using SSluzba.DAO;
using SSluzba.Models;
using SSluzba.Observer;

namespace SSluzba.Controllers
{
    public class ProfessorController
    {
        private ProfessorDAO _professorDAO;

        public ProfessorController()
        {
            _professorDAO = new ProfessorDAO();
        }

        public void Subscribe(IObserver observer)
        {
            _professorDAO.Subscribe(observer);
        }

        public void AddProfessor(string surname, string name, DateTime dateOfBirth, string phoneNumber, string email, string personalIdNumber, string title, int yearsOfExperience, List<Subject> subjects)
        {
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
                Subjects = subjects
            };

            _professorDAO.Add(newProfessor);
        }

        public void UpdateProfessor(Professor updatedProfessor)
        {
            _professorDAO.Update(updatedProfessor);
        }

        public void DeleteProfessor(int id)
        {
            var professor = _professorDAO.GetAll().FirstOrDefault(p => p.Id == id);
            if (professor != null)
            {
                _professorDAO.Remove(professor);
            }
        }

        public List<Professor> GetAllProfessors()
        {
            return _professorDAO.GetAll();
        }
    }
}
