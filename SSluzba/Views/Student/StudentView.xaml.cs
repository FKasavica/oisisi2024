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
            StudentListView.ItemsSource = _studentDetails; // Prilagođeno za ListView

            RefreshStudentList();
        }

        public void Update()
        {
            RefreshStudentList();
        }

        private void RefreshStudentList()
        {
            _studentDetails.Clear();
            var studentDetails = _controller.GetStudentDetails();

            foreach (var detail in studentDetails)
            {
                _studentDetails.Add(detail);
            }
        }

        private void AddStudentButton_Click(object sender, RoutedEventArgs e)
        {
            var addStudentWindow = new AddStudentView();
            addStudentWindow.ShowDialog();
        }

        private void UpdateStudentButton_Click(object sender, RoutedEventArgs e)
        {
            if (StudentListView.SelectedItem is not null)
            {
                // Pretpostavljamo da je `SelectedItem` dinamički objekt sa ID svojstvom
                var selectedStudent = StudentListView.SelectedItem;

                // Ako imaš potrebu da koristiš `Models.Student`, možeš ovde da pozoveš podatke iz baze ili kontrolera
                int studentId = (int)selectedStudent.GetType().GetProperty("Id").GetValue(selectedStudent);
                var studentToUpdate = _controller.GetAllStudents().FirstOrDefault(s => s.Id == studentId);

                if (studentToUpdate != null)
                {
                    _controller.OpenUpdateStudentView(studentToUpdate);
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
                    ExamGradesView examGradesView = new ExamGradesView(_controller.GetExamGradesForStudent(studentToManage.Id));
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
