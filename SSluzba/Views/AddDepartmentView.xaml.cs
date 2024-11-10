using System;
using System.Windows;
using SSluzba.Models;
using SSluzba.Controllers;
using System.Collections.Generic;

namespace SSluzba.Views
{
    public partial class AddDepartmentView : Window
    {
        public Department Department { get; private set; }

        public AddDepartmentView() : base()
        {
            InitializeComponent();
            PopulateReadOnlyFields();
        }

        private void PopulateReadOnlyFields()
        {
            DepartmentController controller = new DepartmentController();
            List<Department> allDepartments = controller.GetAllDepartments();
            int maxId = allDepartments.Count > 0 ? allDepartments[^1].Id : 0;
            HeadOfDepartmentIdInput.Text = "Auto-Generated";
            HeadOfDepartmentIdInput.IsReadOnly = true;
            ProfessorIdListInput.Text = "Auto-Generated";
            ProfessorIdListInput.IsReadOnly = true;
        }

        private int GetNextDepartmentId()
        {
            DepartmentController controller = new DepartmentController();
            List<Department> allDepartments = controller.GetAllDepartments();
            int maxId = allDepartments.Count > 0 ? allDepartments[^1].Id : 0;
            return maxId + 1;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            // Validacija unosa
            if (string.IsNullOrWhiteSpace(DepartmentNameInput.Text) || string.IsNullOrWhiteSpace(DepartmentCodeInput.Text))
            {
                MessageBox.Show("Please fill in all fields correctly.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Automatsko generisanje ID-a za novo odeljenje
            int newId = GetNextDepartmentId();

            // Kreiranje novog odeljenja
            Department = new Department
            {
                Id = newId,
                DepartmentCode = DepartmentCodeInput.Text,
                DepartmentName = DepartmentNameInput.Text
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
