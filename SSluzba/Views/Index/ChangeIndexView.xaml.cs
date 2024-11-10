using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using SSluzba.Models;

namespace SSluzba.Views.Index
{
    public partial class ChangeIndexView : Window
    {
        private List<Models.Index> _allIndices;
        public Models.Index SelectedIndex { get; private set; }
        private Student _student;

        public ChangeIndexView(Student student, List<Models.Index> allIndices)
        {
            InitializeComponent();
            _student = student;
            _allIndices = allIndices;

            // Popuni polja sa podacima o studentu i trenutnom indeksu
            StudentInfo.Text = $"{student.Name} {student.Surname}";
            CurrentIndex.Text = student.IndexId.ToString();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            // Validacija unosa
            if (string.IsNullOrWhiteSpace(NewIndexInput.Text) || !int.TryParse(NewIndexInput.Text, out int newIndexId))
            {
                MessageBox.Show("Please enter a valid index number.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Proveri da li već postoji student sa ovim indeksom
            if (_allIndices.Any(index => index.Id == newIndexId))
            {
                MessageBox.Show("The entered index is already assigned to another student.", "Models.Index Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Postavi novi indeks
            SelectedIndex = new Models.Index { Id = newIndexId, MajorCode = _student.IndexId.ToString() }; // Assume MajorCode is derived for demo purposes
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