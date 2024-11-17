using SSluzba.Controllers;
using SSluzba.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SSluzba.Views
{
    public partial class UpdateStudentView : Window
    {
        public Models.Student Student { get; private set; }
        private StudentController _controller = new();
        private AddressController _addressController = new();
        private IndexController _indexController = new();

        public UpdateStudentView(int studentId)
        {
            InitializeComponent();

            // Preuzmi podatke iz kontrolera
            var (student, majorCode, enrollmentNumber, enrollmentYear, allAddresses, selectedAddress, status) = _controller.GetStudentDataForUpdate(studentId);
            Student = student;

            // Postavi vrednosti u UI elemente
            SurnameInput.Text = Student.Surname;
            NameInput.Text = Student.Name;
            DateOfBirthInput.SelectedDate = Student.DateOfBirth;
            PhoneNumberInput.Text = Student.PhoneNumber;
            EmailInput.Text = Student.Email;
            CurrentYearInput.Text = Student.CurrentYear.ToString();

            // Postavi status
            StatusInput.SelectedItem = StatusInput.Items
                .Cast<ComboBoxItem>()
                .FirstOrDefault(item => item.Content.ToString() == status);

            // Postavi adresu
            AddressComboBox.ItemsSource = allAddresses;
            AddressComboBox.SelectedItem = selectedAddress;

            // Postavi vrednosti za indeks
            MajorCodeInput.Text = majorCode;
            EnrollmentNumberInput.Text = enrollmentNumber.ToString();
            EnrollmentYearInput.Text = enrollmentYear.ToString();
        }


        private void LoadAddresses()
        {
            var addresses = _addressController.GetAllAddresses();
            AddressComboBox.ItemsSource = addresses;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Create updated student through controller logic
                Student = _controller.UpdateExistingStudent(
                    Student.Id,
                    SurnameInput.Text,
                    NameInput.Text,
                    DateOfBirthInput.SelectedDate,
                    PhoneNumberInput.Text,
                    EmailInput.Text,
                    MajorCodeInput.Text,
                    EnrollmentNumberInput.Text,
                    EnrollmentYearInput.Text,
                    CurrentYearInput.Text,
                    StatusInput.SelectedItem,
                    AddressComboBox.SelectedItem
                );

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                MessageBox.Show("Prslo");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
