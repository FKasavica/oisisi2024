using SSluzba.Controllers;
using SSluzba.Observer;
using System.Collections.ObjectModel;
using System.Windows;

namespace SSluzba.Views.Student
{
    public partial class StudentView : Window, IObserver
    {
        private readonly StudentController _controller = new();
        private ObservableCollection<dynamic> _studentDetails = new();
        private readonly StudentSubjectController _studentSubjectController = new();
        private readonly SubjectController _subjectController = new();
        private readonly ExamGradeController _examGradeController = new();

        private ObservableCollection<dynamic> _passedSubjects = new();
        private ObservableCollection<dynamic> _failedSubjects = new();


        public StudentView() : base()
        {
            InitializeComponent();

            _controller.Subscribe(this);

            StudentListView.ItemsSource = _studentDetails;
            PassedSubjectsListView.ItemsSource = _passedSubjects;
            FailedSubjectsListView.ItemsSource = _failedSubjects;

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

        private void StudentListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (StudentListView.SelectedItem is not null)
            {
                var selectedStudent = StudentListView.SelectedItem;
                int studentId = (int)selectedStudent.GetType().GetProperty("Id").GetValue(selectedStudent);

                var passedSubjects = _studentSubjectController.GetSubjectsByStudentId(studentId)
                    .Where(ss => ss.Passed)
                    .Select(ss =>
                    {
                        var subject = _subjectController.GetSubjectById(ss.SubjectId);
                        var examGrade = _examGradeController.GetExamGradeByStudentAndSubject(studentId, ss.SubjectId);
                        if (examGrade == null)
                        {
                            MessageBox.Show($"No exam grade found for Student ID: {studentId}, Subject ID: {ss.SubjectId}", "Debug Info", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        return new
                        {
                            subject.Id,
                            subject.Code,
                            subject.Name,
                            Grade = examGrade?.NumericGrade.ToString("0.00")
                        };
                    })
                    .ToList();

                var failedSubjects = _studentSubjectController.GetSubjectsByStudentId(studentId)
                    .Where(ss => !ss.Passed)
                    .Select(ss =>
                    {
                        var subject = _subjectController.GetSubjectById(ss.SubjectId);
                        return new
                        {
                            subject.Id,
                            subject.Code,
                            subject.Name
                        };
                    })
                    .ToList();

                PassedSubjectsListView.ItemsSource = passedSubjects;
                FailedSubjectsListView.ItemsSource = failedSubjects;
            }
        }

        private void RemoveGradeButton_Click(object sender, RoutedEventArgs e)
        {
            if (PassedSubjectsListView.SelectedItem is not null)
            {
                var selectedSubject = PassedSubjectsListView.SelectedItem;
                int subjectId = (int)selectedSubject.GetType().GetProperty("Id").GetValue(selectedSubject);
                int studentId = GetSelectedStudentId();

                var examGrade = _examGradeController.GetExamGradeByStudentAndSubject(studentId, subjectId);
                if (examGrade != null)
                {
                    _examGradeController.DeleteExamGrade(examGrade.Id);
                    RefreshSubjectListsForStudent(studentId);
                }
                else
                {
                    MessageBox.Show("No grade found to remove.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a subject from passed subjects.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ChangeGradeButton_Click(object sender, RoutedEventArgs e)
        {
            if (PassedSubjectsListView.SelectedItem is not null)
            {
                var selectedSubject = PassedSubjectsListView.SelectedItem;
                int subjectId = (int)selectedSubject.GetType().GetProperty("Id").GetValue(selectedSubject);
                int studentId = GetSelectedStudentId();

                var examGrade = _examGradeController.GetExamGradeByStudentAndSubject(studentId, subjectId);
                if (examGrade != null)
                {
                    _examGradeController.OpenUpdateExamGradeView(examGrade);
                    RefreshSubjectListsForStudent(studentId);
                }
                else
                {
                    MessageBox.Show("No grade found to change.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a subject from passed subjects.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddGradeButton_Click(object sender, RoutedEventArgs e)
        {
            if (FailedSubjectsListView.SelectedItem is not null)
            {
                var selectedSubject = FailedSubjectsListView.SelectedItem;
                int subjectId = (int)selectedSubject.GetType().GetProperty("Id").GetValue(selectedSubject);
                int studentId = GetSelectedStudentId();

                _examGradeController.OpenAddExamGradeView(studentId, subjectId);
                RefreshSubjectListsForStudent(studentId);
            }
            else
            {
                MessageBox.Show("Please select a subject from failed subjects.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int GetSelectedStudentId()
        {
            if (StudentListView.SelectedItem is not null)
            {
                var selectedStudent = StudentListView.SelectedItem;
                return (int)selectedStudent.GetType().GetProperty("Id").GetValue(selectedStudent);
            }
            return -1; // Invalid ID if no student is selected
        }

        private void RefreshSubjectListsForStudent(int studentId)
        {
            // Logic to refresh passed and failed subject lists for the selected student
            StudentListView_SelectionChanged(null, null);
        }

    }
}