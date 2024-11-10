using SSluzba.DAO;
using SSluzba.Models;
using SSluzba.Observer;
using System.Collections.Generic;

namespace SSluzba.Controllers
{
    public class ExamGradeController
    {
        private ExamGradeDAO _examGradeDAO;

        public ExamGradeController()
        {
            _examGradeDAO = new ExamGradeDAO();
        }

        public void Subscribe(IObserver observer)
        {
            _examGradeDAO.Subscribe(observer);
        }

        public List<ExamGrade> GetAllExamGrades()
        {
            return _examGradeDAO.GetAll();
        }

        public List<ExamGrade> GetExamGradesForStudent(int studentId)
        {
            return _examGradeDAO.GetAll().FindAll(g => g.StudentId == studentId);
        }

        public void AddExamGrade(ExamGrade examGrade)
        {
            _examGradeDAO.Add(examGrade);
        }

        public void UpdateExamGrade(ExamGrade updatedGrade)
        {
            _examGradeDAO.Update(updatedGrade);
        }

        public void DeleteExamGrade(int id)
        {
            _examGradeDAO.Delete(id);
        }
    }
}
