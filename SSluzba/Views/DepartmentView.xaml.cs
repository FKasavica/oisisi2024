using System;
using System.Collections.Generic;
using System.Windows;
using SSluzba.Controllers;
using SSluzba.Models;
using SSluzba.Observer;

namespace SSluzba.Views
{
    public partial class DepartmentView : Window, IObserver
    {
        private DepartmentController _controller;

        public DepartmentView()
        {
            InitializeComponent();
            _controller = new DepartmentController();
            _controller.Subscribe(this);
            RefreshDepartmentList();
        }

        public void Update()
        {
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
            }
        }

        private void UpdateDepartmentButton_Click(object sender, RoutedEventArgs e)
        {
            if (DepartmentDataGrid.SelectedItem is Department selectedDepartment)
            {
                UpdateDepartmentView updateDepartmentWindow = new UpdateDepartmentView(selectedDepartment);
                if (updateDepartmentWindow.ShowDialog() == true)
                {
                    _controller.UpdateDepartment(updateDepartmentWindow.Department);
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
            }
            else
            {
                MessageBox.Show("Please select a department to delete.", "Delete Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}