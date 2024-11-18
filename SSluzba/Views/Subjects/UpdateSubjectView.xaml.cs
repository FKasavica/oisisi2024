using SSluzba.Controllers;
using SSluzba.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SSluzba.Views.Subjects
{
    public partial class UpdateSubjectView : Window
    {
        public Models.Subject Subject { get; private set; }
        private readonly SubjectController _controller;
        private readonly ProfessorController _professorController;

        public UpdateSubjectView(int subjectId)
        {
            InitializeComponent();
            _controller = new SubjectController();
            _professorController = new ProfessorController();

            // Fetch data from controller
            var (subject, professors) = _controller.GetSubjectDataForUpdate(subjectId);
            Subject = subject;

            // Set values to UI elements
            CodeInput.Text = Subject.Code;
            NameInput.Text = Subject.Name;
            SemesterInput.SelectedItem = SemesterInput.Items
                .Cast<ComboBoxItem>()
                .FirstOrDefault(item => item.Content.ToString() == Subject.Semester.ToString());

            StudyYearInput.Text = Subject.StudyYear.ToString();
            EspbPointsInput.Text = Subject.EspbPoints.ToString();

            // Set professor data
            ProfessorComboBox.ItemsSource = professors;
            ProfessorComboBox.DisplayMemberPath = "Name"; // Assuming "Name" property exists in Professor
            ProfessorComboBox.SelectedValuePath = "Id";
            ProfessorComboBox.SelectedValue = Subject.ProfessorId;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedProfessor = ProfessorComboBox.SelectedItem as Professor;
                var semester = SemesterInput.SelectedItem is ComboBoxItem selectedItem
                    ? (Semester)Enum.Parse(typeof(Semester), selectedItem.Content.ToString())
                    : Subject.Semester;

                Subject = _controller.UpdateExistingSubject(
                    Subject.Id,
                    CodeInput.Text,
                    NameInput.Text,
                    semester,
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
