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

            LoadAddresses();
        }

        private void LoadAddresses()
        {
            var addresses = _addressController.GetAllAddresses();
            AddressComboBox.ItemsSource = addresses;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
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
                    new List<SSluzba.Models.Subject>()
                );

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
