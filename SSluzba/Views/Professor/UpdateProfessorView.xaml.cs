﻿using SSluzba.Controllers;
using SSluzba.Models;
using System;
using System.Windows;

namespace SSluzba.Views.Professor
{
    public partial class UpdateProfessorView : Window
    {
        private ProfessorController _controller;
        public Models.Professor Professor { get; private set; }

        public UpdateProfessorView(int professorId)
        {
            InitializeComponent();
            _controller = new ProfessorController();

            Professor = _controller.GetProfessorById(professorId);

            if (Professor != null)
            {
                SurnameInput.Text = Professor.Surname;
                NameInput.Text = Professor.Name;
                DateOfBirthInput.SelectedDate = Professor.DateOfBirth;
                PhoneNumberInput.Text = Professor.PhoneNumber;
                EmailInput.Text = Professor.Email;
                TitleInput.Text = Professor.Title;
                YearsOfExperienceInput.Text = Professor.YearsOfExperience.ToString();
            }
            else
            {
                MessageBox.Show("Professor not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Professor = _controller.UpdateProfessor(
                    Professor.Id,
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

                MessageBox.Show("Professor updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
