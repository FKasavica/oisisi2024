using System;
using System.Collections.Generic;
using System.Windows;
using SSluzba.Controllers;
using SSluzba.Models;
using SSluzba.Observer;

namespace SSluzba.Views
{
    public partial class ExamGradesView : Window, IObserver
    {
        private ExamGradeController _controller;
        private int _studentId;

        public ExamGradesView(int studentId)
        {
            InitializeComponent();
            _controller = new ExamGradeController();
            _studentId = studentId;
            _controller.Subscribe(this);
            RefreshExamGradesList();
        }

        public void Update()
        {
            RefreshExamGradesList();
        }

        private void RefreshExamGradesList()
        {
            ExamGradesDataGrid.ItemsSource = null;
            ExamGradesDataGrid.ItemsSource = _controller.GetExamGradesForStudent(_studentId);
        }

        private void AddExamGradeButton_Click(object sender, RoutedEventArgs e)
        {
            AddExamGradeView addExamGradeWindow = new AddExamGradeView();
            if (addExamGradeWindow.ShowDialog() == true)
            {
                _controller.AddExamGrade(addExamGradeWindow.ExamGrade);
            }
        }

        private void UpdateExamGradeButton_Click(object sender, RoutedEventArgs e)
        {
            if (ExamGradesDataGrid.SelectedItem is ExamGrade selectedExamGrade)
            {
                UpdateExamGradeView updateExamGradeWindow = new UpdateExamGradeView(selectedExamGrade);
                if (updateExamGradeWindow.ShowDialog() == true)
                {
                    _controller.UpdateExamGrade(updateExamGradeWindow.ExamGrade);
                }
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
                _controller.DeleteExamGrade(selectedExamGrade.Id);
            }
            else
            {
                MessageBox.Show("Please select an exam grade to delete.", "Delete Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
