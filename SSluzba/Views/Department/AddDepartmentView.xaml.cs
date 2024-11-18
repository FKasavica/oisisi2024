using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using SSluzba.Models;
using SSluzba.Controllers;

namespace SSluzba.Views
{
    public partial class AddDepartmentView : Window
    {
        public Department Department { get; private set; }

        public AddDepartmentView()
        {
            InitializeComponent();
            PopulateReadOnlyFields();
        }

        private void PopulateReadOnlyFields()
        {
            HeadOfDepartmentIdInput.IsReadOnly = false;
            ProfessorIdListInput.IsReadOnly = false;
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
            if (string.IsNullOrWhiteSpace(DepartmentNameInput.Text) || string.IsNullOrWhiteSpace(DepartmentCodeInput.Text))
            {
                MessageBox.Show("Please fill in all fields correctly.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int newId = GetNextDepartmentId();

            int headOfDepartmentId;
            if (!int.TryParse(HeadOfDepartmentIdInput.Text, out headOfDepartmentId))
            {
                MessageBox.Show("Invalid Head of Department ID.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            List<int> professorIds = new List<int>();
            if (!string.IsNullOrWhiteSpace(ProfessorIdListInput.Text))
            {
                var professorIdStrings = ProfessorIdListInput.Text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var idStr in professorIdStrings)
                {
                    if (int.TryParse(idStr.Trim(), out var professorId))
                    {
                        professorIds.Add(professorId);
                    }
                    else
                    {
                        MessageBox.Show($"Invalid Professor ID: {idStr}", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
            }

            Department = new Department
            {
                Id = newId,
                DepartmentCode = DepartmentCodeInput.Text,
                DepartmentName = DepartmentNameInput.Text,
                HeadOfDepartmentId = headOfDepartmentId,
                ProfessorIdList = professorIds
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
