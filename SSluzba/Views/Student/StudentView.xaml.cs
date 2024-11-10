using SSluzba.Controllers;
using SSluzba.Models;
using SSluzba.Observer;
using SSluzba.Views.Index;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SSluzba.Views
{
    public partial class StudentView : Window, IObserver
    {
        private StudentController _controller;
        private ExamGradeController _examGradeController;
        private IndexController _indexController;

        public StudentView() : base()
        {
            InitializeComponent();
            _controller = new StudentController();
            _controller.Subscribe(this);
            _examGradeController = new ExamGradeController();
            _indexController = new IndexController();
            RefreshStudentList();
        }

        public void Update()
        {
            RefreshStudentList();
        }

        private void RefreshStudentList()
        {
            StudentDataGrid.ItemsSource = null;
            StudentDataGrid.ItemsSource = _controller.GetAllStudents();
        }

        private void AddStudentButton_Click(object sender, RoutedEventArgs e)
        {
            // Otvaranje novog prozora za dodavanje studenta
            AddStudentView addStudentWindow = new AddStudentView();
            if (addStudentWindow.ShowDialog() == true)
            {
                // Ako je unos uspešan, dodaj studenta
                _controller.AddStudent(addStudentWindow.Student.Surname, addStudentWindow.Student.Name, addStudentWindow.Student.DateOfBirth, addStudentWindow.Student.PhoneNumber, addStudentWindow.Student.Email, addStudentWindow.Student.IndexId, addStudentWindow.Student.CurrentYear, addStudentWindow.Student.Status, addStudentWindow.Student.AverageGrade);
            }
        }

        private void UpdateStudentButton_Click(object sender, RoutedEventArgs e)
        {
            if (StudentDataGrid.SelectedItem is Student selectedStudent)
            {
                // Otvaranje prozora za ažuriranje studenta
                UpdateStudentView updateStudentWindow = new UpdateStudentView(selectedStudent);
                if (updateStudentWindow.ShowDialog() == true)
                {
                    // Ažuriraj studenta
                    _controller.UpdateStudent(updateStudentWindow.Student);
                }
            }
            else
            {
                MessageBox.Show("Please select a student to update.", "Update Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteStudentButton_Click(object sender, RoutedEventArgs e)
        {
            if (StudentDataGrid.SelectedItem is Student selectedStudent)
            {
                _controller.DeleteStudent(selectedStudent.Id);
            }
            else
            {
                MessageBox.Show("Please select a student to delete.", "Delete Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ManageExamGradesButton_Click(object sender, RoutedEventArgs e)
        {
            if (StudentDataGrid.SelectedItem is Student selectedStudent)
            {
                ExamGradesView examGradesView = new ExamGradesView(selectedStudent.Id);
                examGradesView.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select a student to manage exam grades.", "Manage Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ChangeIndexButton_Click(object sender, RoutedEventArgs e)
        {
            if (StudentDataGrid.SelectedItem is Student selectedStudent)
            {
                ChangeIndexView changeIndexWindow = new ChangeIndexView(selectedStudent, _indexController.GetAllIndices());
                if (changeIndexWindow.ShowDialog() == true)
                {
                    // Ažuriraj indeks studenta
                    selectedStudent.IndexId = changeIndexWindow.SelectedIndex.Id;
                    _controller.UpdateStudent(selectedStudent);
                }
            }
            else
            {
                MessageBox.Show("Please select a student to change the index.", "Change Index Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
