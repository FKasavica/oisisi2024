using SSluzba.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SSluzba.Views
{
    public partial class UpdateStudentView : Window
    {
        public Student Student { get; private set; }

        public UpdateStudentView(Student student)
        {
            InitializeComponent();

            // Popunjavanje polja sa podacima o studentu
            if (student != null)
            {
                SurnameInput.Text = student.Surname;
                NameInput.Text = student.Name;
                DateOfBirthInput.SelectedDate = student.DateOfBirth;
                PhoneNumberInput.Text = student.PhoneNumber;
                EmailInput.Text = student.Email;
                IndexIdInput.Text = student.IndexId.ToString();
                CurrentYearInput.Text = student.CurrentYear.ToString();
                StatusInput.SelectedItem = student.Status == Status.Budget ? StatusInput.Items[0] : StatusInput.Items[1];
                AverageGradeInput.Text = student.AverageGrade.ToString();
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            // Validacija unosa
            if (string.IsNullOrWhiteSpace(SurnameInput.Text) ||
                string.IsNullOrWhiteSpace(NameInput.Text) ||
                DateOfBirthInput.SelectedDate == null ||
                string.IsNullOrWhiteSpace(PhoneNumberInput.Text) ||
                string.IsNullOrWhiteSpace(EmailInput.Text) ||
                !int.TryParse(IndexIdInput.Text, out int indexId) ||
                !int.TryParse(CurrentYearInput.Text, out int currentYear) ||
                StatusInput.SelectedItem == null ||
                !double.TryParse(AverageGradeInput.Text, out double averageGrade))
            {
                MessageBox.Show("Please fill in all fields correctly.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Ažuriranje podataka o studentu
            Student = new Student
            {
                Id = indexId,
                Surname = SurnameInput.Text,
                Name = NameInput.Text,
                DateOfBirth = DateOfBirthInput.SelectedDate.Value,
                PhoneNumber = PhoneNumberInput.Text,
                Email = EmailInput.Text,
                IndexId = indexId,
                CurrentYear = currentYear,
                Status = (Status)Enum.Parse(typeof(Status), ((ComboBoxItem)StatusInput.SelectedItem).Content.ToString()),
                AverageGrade = averageGrade
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
