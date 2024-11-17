using SSluzba.Controllers;
using System.Collections.ObjectModel;
using System.Windows;

namespace SSluzba.Views.Subject
{
    public partial class SubjectView : Window
    {
        private readonly SubjectController _controller;
        private ObservableCollection<dynamic> _subjectDetails;

        public SubjectView()
        {
            InitializeComponent();
            _controller = new SubjectController();

            _subjectDetails = new ObservableCollection<dynamic>(_controller.GetAllSubjects());
            SubjectListView.ItemsSource = _subjectDetails;
        }
    }
}
