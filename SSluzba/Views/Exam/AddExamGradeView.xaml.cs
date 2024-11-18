using System;
using System.Windows;
using SSluzba.Models;
using SSluzba.Controllers;

namespace SSluzba.Views
{
    public partial class AddExamGradeView : Window
    {
        public ExamGrade ExamGrade { get; private set; }
        private readonly ExamGradeController _examGradeController;

        public AddExamGradeView(int studentId, int subjectId)
        {
            InitializeComponent();

            // Inicijalizacija kontrolera
            _examGradeController = new ExamGradeController();

            // Postavi početne vrednosti
            StudentIdInput.Text = studentId.ToString();
            SubjectIdInput.Text = subjectId.ToString();

            // Zaključaj polja za uređivanje ID-ova jer su već definisani
            StudentIdInput.IsEnabled = false;
            SubjectIdInput.IsEnabled = false;

            // Inicijalizuj praznu ocenu
            ExamGrade = new ExamGrade
            {
                StudentId = studentId,
                SubjectId = subjectId
            };
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            // Validacija unosa za ocenu i datum
            if (!double.TryParse(NumericGradeInput.Text, out double numericGrade) ||
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

            // Ažuriranje ocene
            ExamGrade.NumericGrade = numericGrade;
            ExamGrade.ExamDate = ExamDateInput.SelectedDate.Value;

            // Koristi kontroler za dodavanje ocene u skladište (DAO)
            _examGradeController.AddExamGrade(ExamGrade);

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
