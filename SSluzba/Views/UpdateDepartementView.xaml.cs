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

            // Popunjavanje polja sa podacima o odeljenju
            if (department != null)
            {
                Department = department;
                DepartmentCodeInput.Text = department.DepartmentCode;
                DepartmentCodeInput.IsReadOnly = true;
                DepartmentNameInput.Text = department.DepartmentName;
                HeadOfDepartmentIdInput.Text = department.HeadOfDepartmentId.ToString();
                HeadOfDepartmentIdInput.IsReadOnly = true;
                ProfessorIdListInput.Text = string.Join(", ", department.ProfessorIdList);
                ProfessorIdListInput.IsReadOnly = true;
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            // Validacija unosa
            if (string.IsNullOrWhiteSpace(DepartmentNameInput.Text))
            {
                MessageBox.Show("Please fill in all fields correctly.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Ažuriranje podataka o odeljenju
            Department.DepartmentName = DepartmentNameInput.Text;

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
