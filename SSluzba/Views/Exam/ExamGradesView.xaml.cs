using SSluzba.Controllers;
using SSluzba.Models;
using SSluzba.Observer;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SSluzba.Views
{
    public partial class ExamGradesView : Window, IObserver
    {
        private ExamGradeController _examGradeController;

        public ExamGradesView(List<ExamGrade> examGrades) : base()
        {
            InitializeComponent();
            _examGradeController = new ExamGradeController();
            _examGradeController.Subscribe(this);
            RefreshExamGradesList(examGrades);
        }

        public void Update()
        {
            RefreshExamGradesList(_examGradeController.GetAllExamGrades());
        }

        private void RefreshExamGradesList(List<ExamGrade> examGrades)
        {
            ExamGradesDataGrid.ItemsSource = null;
            ExamGradesDataGrid.ItemsSource = examGrades;
        }

        private void AddExamGradeButton_Click(object sender, RoutedEventArgs e)
        {
            _examGradeController.OpenAddExamGradeView();
        }

        private void UpdateExamGradeButton_Click(object sender, RoutedEventArgs e)
        {
            if (ExamGradesDataGrid.SelectedItem is ExamGrade selectedExamGrade)
            {
                _examGradeController.OpenUpdateExamGradeView(selectedExamGrade);
            }
            else
            {
                MessageBox.Show("Please select an exam grade to update.", "Update Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteExamGradeButton_Click(object sender, RoutedEventArgs e)
        {
            if (ExamGradesDataGrid.SelectedItem is ExamGrade selectedExamGrade)
            {
                _examGradeController.DeleteExamGrade(selectedExamGrade.Id);
            }
            else
            {
                MessageBox.Show("Please select an exam grade to delete.", "Delete Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
