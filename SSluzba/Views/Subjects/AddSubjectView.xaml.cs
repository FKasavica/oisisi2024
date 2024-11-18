using SSluzba.Controllers;
using SSluzba.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SSluzba.Views.Subjects
{
    public partial class AddSubjectView : Window
    {
        public Models.Subject Subject { get; private set; }
        private readonly SubjectController _controller;
        private readonly ProfessorController _professorController;

        public AddSubjectView()
        {
            InitializeComponent();
            _controller = new SubjectController();
            _professorController = new ProfessorController();

            LoadProfessors();
        }

        private void LoadProfessors()
        {
            var professors = _professorController.GetAllProfessors();
            ProfessorComboBox.ItemsSource = professors;
            ProfessorComboBox.DisplayMemberPath = "Name"; // Assuming "Name" property exists
            ProfessorComboBox.SelectedValuePath = "Id";
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedProfessor = ProfessorComboBox.SelectedItem as Models.Professor;

                Subject = _controller.CreateNewSubject(
                    CodeInput.Text,
                    NameInput.Text,
                    SemesterInput.SelectedItem,
                    StudyYearInput.Text,
                    selectedProfessor?.Id ?? 0,
                    EspbPointsInput.Text
                );

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
