using SSluzba.DAO;
using SSluzba.Models;
using SSluzba.Observer;
using System.Collections.Generic;
using System.Linq;

namespace SSluzba.Controllers
{
    public class StudentSubjectController
    {
        private readonly StudentSubjectDAO _studentSubjectDAO = new();

        public StudentSubjectController()
        {
            
        }
        public void Subscribe(IObserver observer)
        {
            _studentSubjectDAO.Subscribe(observer);
        }

        public List<StudentSubject> GetAllStudentSubjects()
        {
            return _studentSubjectDAO.GetAll();
        }

        public List<StudentSubject> GetSubjectsByStudentId(int studentId)
        {
            return _studentSubjectDAO.GetAll().Where(ss => ss.StudentId == studentId).ToList();
        }

        public List<StudentSubject> GetStudentsBySubjectId(int subjectId)
        {
            return _studentSubjectDAO.GetAll().Where(ss => ss.SubjectId == subjectId).ToList();
        }

        public void AddStudentSubject(StudentSubject studentSubject)
        {
            _studentSubjectDAO.AddStudentSubject(studentSubject);
        }

        public void UpdateStudentSubject(StudentSubject studentSubject)
        {
            _studentSubjectDAO.UpdateStudentSubject(studentSubject);
        }

        public void DeleteStudentSubject(int id)
        {
            _studentSubjectDAO.DeleteStudentSubject(id);
        }
    }
}
