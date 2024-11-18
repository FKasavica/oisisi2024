using SSluzba.Controllers;
using SSluzba.Observer;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace SSluzba.Views.Professor
{
    public partial class ProfessorView : Window, IObserver
    {
        private readonly ProfessorController _controller = new();
        private ObservableCollection<dynamic> _professorDetails = new();
        private readonly SubjectController _subjectController = new();

        private ObservableCollection<dynamic> _assignedSubjects = new();


        public ProfessorView() : base()
        {
            InitializeComponent();

            _controller.Subscribe(this);

            ProfessorListView.ItemsSource = _professorDetails;
            AssignedSubjectsListView.ItemsSource = _assignedSubjects;

            RefreshProfessorList();
        }

        public void Update()
        {
            // Refresh the professor list when an update occurs
            RefreshProfessorList();
        }

        private void RefreshProfessorList()
        {
            var currentDetails = _controller.GetAllProfessors();

            // Remove items that no longer exist
            for (int i = _professorDetails.Count - 1; i >= 0; i--)
            {
                if (!currentDetails.Contains(_professorDetails[i]))
                {
                    _professorDetails.RemoveAt(i);
                }
            }

            // Add new or updated items
            foreach (var detail in currentDetails)
            {
                if (!_professorDetails.Contains(detail))
                {
                    _professorDetails.Add(detail);
                }
            }
        }

        private void AddProfessorButton_Click(object sender, RoutedEventArgs e)
        {
            var addProfessorWindow = new AddProfessorView();
            if (addProfessorWindow.ShowDialog() == true)
            {
                var newProfessor = addProfessorWindow._professor;
                _controller.AddProfessor(newProfessor);
            }
        }

        private void UpdateProfessorButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProfessorListView.SelectedItem is not null)
            {
                var selectedProfessor = ProfessorListView.SelectedItem;
                int professorId = (int)selectedProfessor.GetType().GetProperty("Id").GetValue(selectedProfessor);
                var professorToUpdate = _controller.GetProfessorById(professorId);

                if (professorToUpdate != null)
                {
                    _controller.OpenUpdateProfessorView(professorToUpdate);
                    // Refresh the updated professor
                    var updatedProfessorDetail = _controller.GetProfessorDetails(professorId);
                    int index = _professorDetails.IndexOf(selectedProfessor);
                    if (index >= 0)
                    {
                        _professorDetails[index] = updatedProfessorDetail;
                    }
                }
                else
                {
                    MessageBox.Show("Could not find the selected professor for updating.", "Update Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a professor to update.", "Update Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteProfessorButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProfessorListView.SelectedItem is not null)
            {
                var selectedProfessor = ProfessorListView.SelectedItem;
                int professorId = (int)selectedProfessor.GetType().GetProperty("Id").GetValue(selectedProfessor);
                var professorToDelete = _controller.GetProfessorById(professorId);

                if (professorToDelete != null)
                {
                    _controller.DeleteProfessor(professorToDelete.Id);
                    _professorDetails.Remove(selectedProfessor); // Remove from collection
                }
                else
                {
                    MessageBox.Show("Could not find the selected professor for deletion.", "Delete Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a professor to delete.", "Delete Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ProfessorListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ProfessorListView.SelectedItem is not null)
            {
                var selectedProfessor = ProfessorListView.SelectedItem;
                int professorId = (int)selectedProfessor.GetType().GetProperty("Id").GetValue(selectedProfessor);

                var assignedSubjects = _subjectController.GetSubjectsByProfessorId(professorId)
                    .Select(subject => new
                    {
                        subject.Id,
                        subject.Code,
                        subject.Name
                    })
                    .ToList();

                AssignedSubjectsListView.ItemsSource = assignedSubjects;
            }
        }
    }
}
