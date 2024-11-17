using SSluzba.Views;
using SSluzba.Views.Student;
using SSluzba.Views.Subject;
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
            // Ovde kasnije možeš implementirati prozor za upravljanje profesorima
            MessageBox.Show("Manage Professors feature is not yet implemented.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ManageSubjectsButton_Click(object sender, RoutedEventArgs e)
        {
            SubjectView subjectView = new SubjectView();
            subjectView.Show();
        }
    }
}
