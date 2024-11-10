using System;
using System.Windows;
using SSluzba.Models;

namespace SSluzba.Views
{
    public partial class UpdateExamGradeView : Window
    {
        public ExamGrade ExamGrade { get; private set; }

        public UpdateExamGradeView(ExamGrade examGrade)
        {
            InitializeComponent();

            // Popunjavanje polja sa podacima o oceni
            if (examGrade != null)
            {
                ExamGrade = examGrade;
                GradeIdInput.Text = examGrade.Id.ToString();
                StudentIdInput.Text = examGrade.StudentId.ToString();
                SubjectIdInput.Text = examGrade.SubjectId.ToString();
                NumericGradeInput.Text = examGrade.NumericGrade.ToString();
                ExamDateInput.SelectedDate = examGrade.ExamDate;
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            // Validacija unosa
            if (!double.TryParse(NumericGradeInput.Text, out double numericGrade) || numericGrade < 6 || numericGrade > 10)
            {
                MessageBox.Show("Please enter a valid grade between 6 and 10.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (ExamDateInput.SelectedDate == null)
            {
                MessageBox.Show("Please select a valid exam date.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Ažuriranje podataka o oceni
            ExamGrade.NumericGrade = numericGrade;
            ExamGrade.ExamDate = ExamDateInput.SelectedDate.Value;

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