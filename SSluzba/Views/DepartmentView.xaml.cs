using System;
using System.Collections.Generic;
using System.Windows;
using SSluzba.Controllers;
using SSluzba.Models;

namespace SSluzba.Views
{
    public partial class DepartmentView : Window
    {
        private DepartmentController _controller;

        public DepartmentView()
        {
            InitializeComponent();
            _controller = new DepartmentController();
            RefreshDepartmentList();
        }

        private void RefreshDepartmentList()
        {
            DepartmentDataGrid.ItemsSource = null;
            DepartmentDataGrid.ItemsSource = _controller.GetAllDepartments();
        }

        private void AddDepartmentButton_Click(object sender, RoutedEventArgs e)
        {
            AddDepartmentView addDepartmentWindow = new AddDepartmentView();
            if (addDepartmentWindow.ShowDialog() == true)
            {
                _controller.AddDepartment(addDepartmentWindow.Department.DepartmentCode, addDepartmentWindow.Department.DepartmentName, addDepartmentWindow.Department.HeadOfDepartmentId, addDepartmentWindow.Department.ProfessorIdList);
                RefreshDepartmentList();
            }
        }

        private void UpdateDepartmentButton_Click(object sender, RoutedEventArgs e)
        {
            if (DepartmentDataGrid.SelectedItem is Department selectedDepartment)
            {
                UpdateDepartmentView updateDepartmentWindow = new UpdateDepartmentView(selectedDepartment);
                if (updateDepartmentWindow.ShowDialog() == true)
                {
                    _controller.UpdateDepartment(updateDepartmentWindow.Department.Id, updateDepartmentWindow.Department.DepartmentCode, updateDepartmentWindow.Department.DepartmentName, updateDepartmentWindow.Department.HeadOfDepartmentId, updateDepartmentWindow.Department.ProfessorIdList);
                    RefreshDepartmentList();
                }
            }
            else
            {
                MessageBox.Show("Please select a department to update.", "Update Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteDepartmentButton_Click(object sender, RoutedEventArgs e)
        {
            if (DepartmentDataGrid.SelectedItem is Department selectedDepartment)
            {
                _controller.DeleteDepartment(selectedDepartment.Id);
                RefreshDepartmentList();
            }
            else
            {
                MessageBox.Show("Please select a department to delete.", "Delete Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
