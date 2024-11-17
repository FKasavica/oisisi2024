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
        private AddressController _addressController;

        public AddStudentView()
        {
            InitializeComponent();

            _controller = new StudentController();
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
            // Kreiranje novog studenta preko kontrolera
            try
            {
                _student = _controller.CreateNewStudent(
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
