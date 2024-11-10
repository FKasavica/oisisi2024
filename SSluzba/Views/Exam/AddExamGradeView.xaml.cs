using System;
using System.Windows;
using SSluzba.Models;

namespace SSluzba.Views
{
    public partial class AddExamGradeView : Window
    {
        public ExamGrade ExamGrade { get; private set; }

        public AddExamGradeView()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            // Validacija unosa
            if (!int.TryParse(StudentIdInput.Text, out int studentId) ||
                !int.TryParse(SubjectIdInput.Text, out int subjectId) ||
                !double.TryParse(NumericGradeInput.Text, out double numericGrade) ||
                ExamDateInput.SelectedDate == null)
            {
                MessageBox.Show("Please fill in all fields correctly.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (numericGrade < 6 || numericGrade > 10)
            {
                MessageBox.Show("Grade must be between 6 and 10.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Kreiranje nove ocene
            ExamGrade = new ExamGrade
            {
                StudentId = studentId,
                SubjectId = subjectId,
                NumericGrade = numericGrade,
                ExamDate = ExamDateInput.SelectedDate.Value
            };

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
