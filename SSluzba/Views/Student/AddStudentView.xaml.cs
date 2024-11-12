using SSluzba.Controllers;
using SSluzba.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SSluzba.Views
{
    public partial class AddStudentView : Window
    {
        public Models.Student _student { get; private set; }
        private StudentController _controller;
        private ExamGradeController _examGradeController;
        private IndexController _indexController;
        private AddressController _addressController;

        public AddStudentView()
        {
            InitializeComponent();

            _controller = new StudentController();
            _examGradeController = new ExamGradeController();
            _indexController = new IndexController();
            _addressController = new AddressController();

            LoadAddresses();
        }

        private void LoadAddresses()
        {
            var addresses = _addressController.GetAllAddresses();
            AddressComboBox.ItemsSource = addresses;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            // Validacija unosa
            if (string.IsNullOrWhiteSpace(SurnameInput.Text) ||
                string.IsNullOrWhiteSpace(NameInput.Text) ||
                DateOfBirthInput.SelectedDate == null ||
                string.IsNullOrWhiteSpace(PhoneNumberInput.Text) ||
                string.IsNullOrWhiteSpace(EmailInput.Text) ||
                string.IsNullOrWhiteSpace(MajorCodeInput.Text) ||
                !int.TryParse(EnrollmentNumberInput.Text, out int enrollmentNumber) ||
                !int.TryParse(EnrollmentYearInput.Text, out int enrollmentYear) ||
                !int.TryParse(CurrentYearInput.Text, out int currentYear) ||
                StatusInput.SelectedItem == null ||
                AddressComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all fields correctly.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Generisanje indeksa
            string majorCode = MajorCodeInput.Text;
            string generatedIndex = $"{majorCode}{enrollmentNumber}/{enrollmentYear}";

            // Kreiranje novog indeksa
            _indexController.AddIndex(majorCode, enrollmentNumber, enrollmentYear);

            // Kreiranje novog studenta na osnovu unosa
            Status studentStatus;
            if (!Enum.TryParse(((ComboBoxItem)StatusInput.SelectedItem).Content.ToString(), out studentStatus))
            {
                MessageBox.Show("Invalid status selected.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _student = new Models.Student
            {
                Surname = SurnameInput.Text,
                Name = NameInput.Text,
                DateOfBirth = DateOfBirthInput.SelectedDate.Value,
                AddressId = ((Address)AddressComboBox.SelectedItem).Id, // Postavljanje ID-a adrese
                PhoneNumber = PhoneNumberInput.Text,
                Email = EmailInput.Text,
                CurrentYear = currentYear,
                Status = studentStatus,
                IndexId = _indexController.GetIndexId(majorCode, enrollmentNumber, enrollmentYear)
            };

            _controller.AddStudent(_student);

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
