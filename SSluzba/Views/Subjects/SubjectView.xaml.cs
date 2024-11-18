using SSluzba.Controllers;
using SSluzba.Models;
using SSluzba.Observer;
using SSluzba.Views.Subjects;
using System.Collections.ObjectModel;
using System.Windows;

namespace SSluzba.Views.Subject
{
    public partial class SubjectView : Window, IObserver
    {
        private readonly SubjectController _controller;
        private ObservableCollection<dynamic> _subjectDetails;
        private readonly StudentSubjectController _studentSubjectController = new();

        public SubjectView()
        {
            InitializeComponent();

            _controller = new SubjectController();
            _controller.Subscribe(this);

            _subjectDetails = new ObservableCollection<dynamic>();
            SubjectListView.ItemsSource = _subjectDetails;

            RefreshSubjectList();
        }

        public void Update()
        {
            RefreshSubjectList(); 
        }

        //TODO: popraviti refresh kada se update napravi
        private void RefreshSubjectList()
        {
            var currentDetails = _controller.GetSubjectDetails();

            // Remove elements no longer in the list
            for (int i = _subjectDetails.Count - 1; i >= 0; i--)
            {
                if (!currentDetails.Contains(_subjectDetails[i]))
                {
                    _subjectDetails.RemoveAt(i);
                }
            }

            // Add new or updated elements
            foreach (var detail in currentDetails)
            {
                if (!_subjectDetails.Contains(detail))
                {
                    _subjectDetails.Add(detail);
                }
            }
        }

        private void AddSubjectButton_Click(object sender, RoutedEventArgs e)
        {
            var addSubjectWindow = new AddSubjectView();
            if (addSubjectWindow.ShowDialog() == true)
            {
                var newSubject = addSubjectWindow.Subject;
                _controller.AddSubject(newSubject);
            }
        }

        private void UpdateSubjectButton_Click(object sender, RoutedEventArgs e)
        {
            if (SubjectListView.SelectedItem is not null)
            {
                var selectedSubject = SubjectListView.SelectedItem;
                int subjectId = (int)selectedSubject.GetType().GetProperty("Id").GetValue(selectedSubject);
                var subjectToUpdate = _controller.GetAllSubjects().FirstOrDefault(s => s.Id == subjectId);

                if (subjectToUpdate != null)
                {
                    _controller.OpenUpdateSubjectView(subjectToUpdate);
                    var updatedSubjectDetail = _controller.GetSubjectDetails().Find(s => s.Id == subjectId);
                    int index = _subjectDetails.IndexOf(selectedSubject);
                    if (index >= 0)
                    {
                        _subjectDetails[index] = updatedSubjectDetail;
                    }
                }
                else
                {
                    MessageBox.Show("Could not find the selected subject for updating.", "Update Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a subject to update.", "Update Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteSubjectButton_Click(object sender, RoutedEventArgs e)
        {
            if (SubjectListView.SelectedItem is not null)
            {
                var selectedSubject = SubjectListView.SelectedItem;
                int subjectId = (int)selectedSubject.GetType().GetProperty("Id").GetValue(selectedSubject);
                var subjectToDelete = _controller.GetAllSubjects().FirstOrDefault(s => s.Id == subjectId);

                if (subjectToDelete != null)
                {
                    _controller.DeleteSubject(subjectToDelete.Id);
                    _subjectDetails.Remove(selectedSubject); 
                }
                else
                {
                    MessageBox.Show("Could not find the selected subject for deletion.", "Delete Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a subject to delete.", "Delete Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SubjectListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (SubjectListView.SelectedItem is not null)
            {
                var selectedSubject = SubjectListView.SelectedItem;
                int subjectId = (int)selectedSubject.GetType().GetProperty("Id").GetValue(selectedSubject);

                var passedStudents = _controller.GetStudentsWhoPassed(subjectId);
                var failedStudents = _controller.GetStudentsWhoFailed(subjectId);

                PassedStudentsListView.ItemsSource = passedStudents;
                FailedStudentsListView.ItemsSource = failedStudents;
            }
        }

        private void MoveStudentToPassedButton_Click(object sender, RoutedEventArgs e)
        {
            if (SubjectListView.SelectedItem is null)
            {
                MessageBox.Show("Please select a subject first.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var selectedSubject = SubjectListView.SelectedItem;
            int subjectId = (int)selectedSubject.GetType().GetProperty("Id").GetValue(selectedSubject);

            if (FailedStudentsListView.SelectedItem is not null)
            {
                var selectedStudent = FailedStudentsListView.SelectedItem;
                int studentId = (int)selectedStudent.GetType().GetProperty("Id").GetValue(selectedStudent);

                _controller.MoveStudentToPassed(studentId, subjectId);
                RefreshSubjectList(); // Refresh to reflect changes
            }
            else
            {
                MessageBox.Show("Please select a student from the 'Failed' list to move.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MoveStudentToFailedButton_Click(object sender, RoutedEventArgs e)
        {
            if (SubjectListView.SelectedItem is null)
            {
                MessageBox.Show("Please select a subject first.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var selectedSubject = SubjectListView.SelectedItem;
            int subjectId = (int)selectedSubject.GetType().GetProperty("Id").GetValue(selectedSubject);

            if (PassedStudentsListView.SelectedItem is not null)
            {
                var selectedStudent = PassedStudentsListView.SelectedItem;
                int studentId = (int)selectedStudent.GetType().GetProperty("Id").GetValue(selectedStudent);

                _controller.MoveStudentToFailed(studentId, subjectId);
                RefreshSubjectList(); // Refresh to reflect changes
            }
            else
            {
                MessageBox.Show("Please select a student from the 'Passed' list to move.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddStudentToSubjectButton_Click(object sender, RoutedEventArgs e)
        {
            if (SubjectListView.SelectedItem is null)
            {
                MessageBox.Show("Please select a subject first.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Retrieve selected subject
            var selectedSubject = SubjectListView.SelectedItem;
            int subjectId = (int)selectedSubject.GetType().GetProperty("Id").GetValue(selectedSubject);

            // Prompt for a student ID or implement a way to get it
            string input = Microsoft.VisualBasic.Interaction.InputBox("Enter the ID of the student to add:", "Add Student", "0");
            if (int.TryParse(input, out int studentId) && studentId > 0)
            {
                _controller.AddStudentToSubject(studentId, subjectId);

                // Refresh both passed and failed student lists after the operation
                //RefreshStudentLists(subjectId);
            }
            else
            {
                MessageBox.Show("Invalid student ID entered.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void RemoveStudentFromSubjectButton_Click(object sender, RoutedEventArgs e)
        {
            if (SubjectListView.SelectedItem is null)
            {
                MessageBox.Show("Please select a subject first.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Retrieve selected subject
            var selectedSubject = SubjectListView.SelectedItem;
            int subjectId = (int)selectedSubject.GetType().GetProperty("Id").GetValue(selectedSubject);

            // Check which ListView the selected student belongs to
            if (PassedStudentsListView.SelectedItem is Models.Student selectedPassedStudent)
            {
                _controller.RemoveStudentFromSubject(selectedPassedStudent.Id, subjectId);
            }
            else if (FailedStudentsListView.SelectedItem is Models.Student selectedFailedStudent)
            {
                _controller.RemoveStudentFromSubject(selectedFailedStudent.Id, subjectId);
            }
            else
            {
                MessageBox.Show("Please select a student to remove.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Refresh both passed and failed student lists after the operation
           // RefreshStudentLists(subjectId);
        }


    }
}
