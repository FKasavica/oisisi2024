using SSluzba.Controllers;
using SSluzba.Observer;
using System.Collections.ObjectModel;
using System.Windows;

namespace SSluzba.Views.Subject
{
    public partial class SubjectView : Window, IObserver
    {
        private readonly SubjectController _controller;
        private ObservableCollection<dynamic> _subjectDetails;

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
            // Refresh or update changes
            RefreshSubjectList(); // This method handles the update logic for the ObservableCollection
        }

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
            //var addSubjectWindow = new AddSubjectView();
            //if (addSubjectWindow.ShowDialog() == true)
            //{
            //    var newSubject = addSubjectWindow.Subject;
            //    _controller.AddSubject(newSubject);
            //    // Optionally add directly if desired
            //    //_subjectDetails.Add(_controller.GetSubjectDetails().Find(s => s.Id == newSubject.Id));
            //}
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
                    //_controller.OpenUpdateSubjectView(subjectToUpdate);
                    // Refresh the updated element after modification
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
                    _subjectDetails.Remove(selectedSubject); // Remove from the collection
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

                // Use your controller logic to fetch the list of students who passed/failed
                var passedStudents = _controller.GetStudentsWhoPassed(subjectId);
                var failedStudents = _controller.GetStudentsWhoFailed(subjectId);

                // Update the ListViews for passed and failed students accordingly
                PassedStudentsListView.ItemsSource = passedStudents;
                FailedStudentsListView.ItemsSource = failedStudents;
            }
        }

    }
}
