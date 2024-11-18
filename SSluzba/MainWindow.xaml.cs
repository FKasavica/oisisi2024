using SSluzba.Views;
using SSluzba.Views.Student;
using SSluzba.Views.Subject;
//using SSluzba.Views.Department;
using SSluzba.Views.Professor;
using System;
using System.Windows;

namespace SSluzba
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ManageStudentsButton_Click(object sender, RoutedEventArgs e)
        {
            StudentView studentView = new StudentView();
            studentView.Show();
        }

        private void ManageProfessorsButton_Click(object sender, RoutedEventArgs e)
        {
            ProfessorView professorView = new ProfessorView();
            professorView.Show();
        }

        private void ManageSubjectsButton_Click(object sender, RoutedEventArgs e)
        {
            SubjectView subjectView = new SubjectView();
            subjectView.Show();
        }
    }
}
