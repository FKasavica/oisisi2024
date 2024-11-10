using SSluzba.Controllers;
using SSluzba.Models;
using SSluzba.Observer;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SSluzba.Views
{
    public partial class StudentView : Window, IObserver
    {
        private StudentController _controller;

        public StudentView() : base()
        {
            InitializeComponent();
            _controller.Subscribe(this);
            _controller = new StudentController();
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
                AddStudentView updateStudentWindow = new AddStudentView(selectedStudent);
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
    }
}
