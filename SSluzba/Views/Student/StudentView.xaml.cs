using SSluzba.Controllers;
using SSluzba.Observer;
using System.Collections.ObjectModel;
using System.Windows;

namespace SSluzba.Views.Student
{
    public partial class StudentView : Window, IObserver
    {
        private readonly StudentController _controller;
        private ObservableCollection<dynamic> _studentDetails;

        public StudentView() : base()
        {
            InitializeComponent();

            _controller = new StudentController();
            _controller.Subscribe(this);

            _studentDetails = new ObservableCollection<dynamic>();
            StudentListView.ItemsSource = _studentDetails;

            RefreshStudentList();
        }

        public void Update()
        {
            // Umesto korišćenja `Clear()`, osvežite promene
            RefreshStudentList(); // ili koristite metode za pojedinačne izmene ako je moguće
        }

        private void RefreshStudentList()
        {
            var currentDetails = _controller.GetStudentDetails();

            // Uklanjanje elemenata koji više ne postoje
            for (int i = _studentDetails.Count - 1; i >= 0; i--)
            {
                if (!currentDetails.Contains(_studentDetails[i]))
                {
                    _studentDetails.RemoveAt(i);
                }
            }

            // Dodavanje novih ili ažuriranih elemenata
            foreach (var detail in currentDetails)
            {
                if (!_studentDetails.Contains(detail))
                {
                    _studentDetails.Add(detail);
                }
            }
        }

        private void AddStudentButton_Click(object sender, RoutedEventArgs e)
        {
            var addStudentWindow = new AddStudentView();
            if (addStudentWindow.ShowDialog() == true)
            {
                var newStudent = addStudentWindow._student;
                _controller.AddStudent(newStudent);
                //_studentDetails.Add(_controller.GetStudentDetails().Find(s => s.Id == newStudent.Id));
            }
        }

        private void UpdateStudentButton_Click(object sender, RoutedEventArgs e)
        {
            if (StudentListView.SelectedItem is not null)
            {
                var selectedStudent = StudentListView.SelectedItem;
                int studentId = (int)selectedStudent.GetType().GetProperty("Id").GetValue(selectedStudent);
                var studentToUpdate = _controller.GetAllStudents().FirstOrDefault(s => s.Id == studentId);

                if (studentToUpdate != null)
                {
                    _controller.OpenUpdateStudentView(studentToUpdate);
                    // Osveži izmenjeni element nakon ažuriranja
                    var updatedStudentDetail = _controller.GetStudentDetails().Find(s => s.Id == studentId);
                    int index = _studentDetails.IndexOf(selectedStudent);
                    if (index >= 0)
                    {
                        _studentDetails[index] = updatedStudentDetail;
                    }
                }
                else
                {
                    MessageBox.Show("Could not find the selected student for updating.", "Update Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a student to update.", "Update Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteStudentButton_Click(object sender, RoutedEventArgs e)
        {
            if (StudentListView.SelectedItem is not null)
            {
                var selectedStudent = StudentListView.SelectedItem;
                int studentId = (int)selectedStudent.GetType().GetProperty("Id").GetValue(selectedStudent);
                var studentToDelete = _controller.GetAllStudents().FirstOrDefault(s => s.Id == studentId);

                if (studentToDelete != null)
                {
                    _controller.DeleteStudent(studentToDelete.Id);
                    _studentDetails.Remove(selectedStudent); // Uklanjanje iz kolekcije
                }
                else
                {
                    MessageBox.Show("Could not find the selected student for deletion.", "Delete Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a student to delete.", "Delete Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ManageExamGradesButton_Click(object sender, RoutedEventArgs e)
        {
            if (StudentListView.SelectedItem is not null)
            {
                var selectedStudent = StudentListView.SelectedItem;
                int studentId = (int)selectedStudent.GetType().GetProperty("Id").GetValue(selectedStudent);
                var studentToManage = _controller.GetAllStudents().FirstOrDefault(s => s.Id == studentId);

                if (studentToManage != null)
                {
                    var examGradesView = new ExamGradesView(_controller.GetExamGradesForStudent(studentToManage.Id));
                    examGradesView.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Could not find the selected student to manage exam grades.", "Manage Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a student to manage exam grades.", "Manage Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
