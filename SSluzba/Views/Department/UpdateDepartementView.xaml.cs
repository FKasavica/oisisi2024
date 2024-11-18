using System;
using System.Windows;
using SSluzba.Models;

namespace SSluzba.Views
{
    public partial class UpdateDepartmentView : Window
    {
        public Department Department { get; private set; }

        public UpdateDepartmentView(Department department)
        {
            InitializeComponent();

            if (department != null)
            {
                Department = department;
                DepartmentCodeInput.Text = department.DepartmentCode;
                DepartmentCodeInput.IsReadOnly = false;
                DepartmentNameInput.Text = department.DepartmentName;
                HeadOfDepartmentIdInput.Text = department.HeadOfDepartmentId.ToString();
                HeadOfDepartmentIdInput.IsReadOnly = false;
                ProfessorIdListInput.Text = string.Join(", ", department.ProfessorIdList);
                ProfessorIdListInput.IsReadOnly = false;
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(DepartmentNameInput.Text))
                {
                    MessageBox.Show("Please fill in all fields correctly.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Department.DepartmentName = DepartmentNameInput.Text;

                var professorIdsInput = ProfessorIdListInput.Text;
                if (!string.IsNullOrWhiteSpace(professorIdsInput))
                {
                    Department.ProfessorIdList = professorIdsInput.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                                  .Select(id => int.TryParse(id.Trim(), out var idValue) ? idValue : 0)
                                                                  .Where(id => id != 0)
                                                                  .ToList();
                }

                Department.HeadOfDepartmentId = int.Parse(HeadOfDepartmentIdInput.Text);

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
