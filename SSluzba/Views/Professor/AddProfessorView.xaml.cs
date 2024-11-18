using SSluzba.Controllers;
using SSluzba.Models;
using System;
using System.Collections.Generic;
using System.Windows;

namespace SSluzba.Views.Professor
{
    public partial class AddProfessorView : Window
    {
        public Models.Professor _professor { get; private set; }
        private ProfessorController _controller;
        private AddressController _addressController;

        public AddProfessorView()
        {
            InitializeComponent();
            _controller = new ProfessorController();
            _addressController = new AddressController();

            LoadAddresses();  // Load the addresses into the combo box
        }

        // Load all addresses into the combo box
        private void LoadAddresses()
        {
            var addresses = _addressController.GetAllAddresses();
            AddressComboBox.ItemsSource = addresses;
        }

        // Event handler for the "Add Professor" button click
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate inputs
            if (string.IsNullOrEmpty(SurnameInput.Text) ||
                string.IsNullOrEmpty(NameInput.Text) ||
                DateOfBirthInput.SelectedDate == null ||
                string.IsNullOrEmpty(PhoneNumberInput.Text) ||
                string.IsNullOrEmpty(EmailInput.Text) ||
                string.IsNullOrEmpty(PersonalIdNumberInput.Text) ||
                string.IsNullOrEmpty(TitleInput.Text) ||
                string.IsNullOrEmpty(YearsOfExperienceInput.Text) ||
                AddressComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all fields correctly.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // Create a new professor using the controller
                _professor = _controller.CreateNewProfessor(
                    SurnameInput.Text,
                    NameInput.Text,
                    DateOfBirthInput.SelectedDate.Value,
                    PhoneNumberInput.Text,
                    EmailInput.Text,
                    PersonalIdNumberInput.Text,
                    TitleInput.Text,
                    int.Parse(YearsOfExperienceInput.Text),
                    ((Address)AddressComboBox.SelectedItem).Id,
                    new List<SSluzba.Models.Subject>() // Assuming no subjects are selected by default
                );

                DialogResult = true;  // Indicate success
                Close();  // Close the window
            }
            catch (Exception ex)
            {
                // Show an error message if something goes wrong
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Event handler for the "Cancel" button click
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;  // Indicate that the user cancelled
            Close();  // Close the window
        }
    }
}
