using SSluzba.Controllers;
using SSluzba.Models;
using SSluzba.Observer;
using SSluzba.Views.Index;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SSluzba.Views.Student
{
    public partial class StudentView : Window, IObserver
    {
        private StudentController _controller;
        private ExamGradeController _examGradeController;
        private IndexController _indexController;
        private AddressController _addressController;

        public StudentView() : base()
        {
            InitializeComponent();

            _controller = new StudentController();
            _examGradeController = new ExamGradeController();
            _indexController = new IndexController();
            _addressController = new AddressController();

            _controller.Subscribe(this);
            _indexController.Subscribe(this);
            _examGradeController.Subscribe(this);
            _addressController.Subscribe(this);

            RefreshStudentList();
        }


        public void Update()
        {
            RefreshStudentList();
        }

private void RefreshStudentList()
        {
            var students = _controller.GetAllStudents();
            var studentDetails = new List<dynamic>();

            foreach (var student in students)
            {
                var address = _addressController.GetAddressById(student.AddressId)?.ToString() ?? "N/A";
                var index = _indexController.GetIndexById(student.IndexId)?.ToString() ?? "N/A";
                var averageGrade = _examGradeController.GetAverageGrade(student.Id);

                studentDetails.Add(new
                {
                    student.Id,
                    student.Surname,
                    student.Name,
                    student.CurrentYear,
                    AverageGrade = averageGrade,
                    Index = index,
                    Address = address
                });
            }

            StudentDataGrid.ItemsSource = null;
            StudentDataGrid.ItemsSource = studentDetails;
        }

        private void AddStudentButton_Click(object sender, RoutedEventArgs e)
        {
            var addStudentWindow = new AddStudentView();
            addStudentWindow.ShowDialog();
        }

        private void UpdateStudentButton_Click(object sender, RoutedEventArgs e)
        {
            if (StudentDataGrid.SelectedItem is Models.Student selectedStudent)
            {
                _controller.OpenUpdateStudentView(selectedStudent);
            }
            else
            {
                MessageBox.Show("Please select a student to update.", "Update Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteStudentButton_Click(object sender, RoutedEventArgs e)
        {
            if (StudentDataGrid.SelectedItem is Models.Student selectedStudent)
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
            if (StudentDataGrid.SelectedItem is Models.Student selectedStudent)
            {
                ExamGradesView examGradesView = new ExamGradesView(_examGradeController.GetExamGradesForStudent(selectedStudent.Id));
                examGradesView.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select a student to manage exam grades.", "Manage Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
