using System.Collections.Generic;
using SSluzba.DAO;
using SSluzba.Models;

namespace SSluzba.Controllers
{
    public class StudentSubjectController
    {
        private StudentSubjectDAO _studentSubjectDAO;

        public StudentSubjectController()
        {
            _studentSubjectDAO = new StudentSubjectDAO();
        }

        public List<StudentSubject> GetAllStudentSubjects()
        {
            return _studentSubjectDAO.GetAll();
        }

        public void AddStudentSubject(int studentId, int subjectId, bool passed)
        {
            StudentSubject newStudentSubject = new StudentSubject
            {
                StudentId = studentId,
                SubjectId = subjectId,
                Passed = passed
            };
            _studentSubjectDAO.AddStudentSubject(newStudentSubject);
        }

        public void UpdateStudentSubject(StudentSubject studentSubject)
        {
            _studentSubjectDAO.UpdateStudentSubject(studentSubject);
        }

        public void DeleteStudentSubject(int id)
        {
            _studentSubjectDAO.DeleteStudentSubject(id);
        }

        public List<StudentSubject> GetSubjectsForStudent(int studentId)
        {
            return _studentSubjectDAO.GetAll().Where(ss => ss.StudentId == studentId).ToList();
        }
    }
}
