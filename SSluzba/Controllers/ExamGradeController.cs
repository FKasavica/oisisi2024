﻿using SSluzba.DAO;
using SSluzba.Models;
using SSluzba.Observer;
using SSluzba.Views;
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

        public double GetAverageGrade(int studentId)
        {
            var grades = _examGradeDAO.GetGradesByStudentId(studentId);
            if (grades.Count == 0) return 0;

            return grades.Average(g => g.NumericGrade);
        }

        public List<ExamGrade> GetExamGradesForStudent(int studentId)
        {
            return _examGradeDAO.GetGradesByStudentId(studentId);
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

        // Dodavanje metode za otvaranje pogleda za dodavanje ocene
        public void OpenAddExamGradeView()
        {
            var addExamGradeWindow = new AddExamGradeView();
            if (addExamGradeWindow.ShowDialog() == true)
            {
                AddExamGrade(addExamGradeWindow.ExamGrade);
            }
        }

        // Dodavanje metode za otvaranje pogleda za ažuriranje ocene
        public void OpenUpdateExamGradeView(ExamGrade examGrade)
        {
            var updateExamGradeWindow = new UpdateExamGradeView(examGrade);
            if (updateExamGradeWindow.ShowDialog() == true)
            {
                UpdateExamGrade(updateExamGradeWindow.ExamGrade);
            }
        }
    }
}
