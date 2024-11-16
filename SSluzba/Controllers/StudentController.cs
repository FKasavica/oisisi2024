using SSluzba.DAO;
using SSluzba.Models;
using SSluzba.Observer;
using SSluzba.Views;

namespace SSluzba.Controllers
{
    public class StudentController
    {
        private readonly StudentDAO _studentDAO = new();
        private readonly AddressController _addressController = new();
        private readonly IndexController _indexController = new();
        private readonly ExamGradeController _examGradeController = new();

        public StudentController()
        {

        }

        public void Subscribe(IObserver observer)
        {
            _studentDAO.Subscribe(observer);
        }

        public void AddStudent(Student student)
        {
            _studentDAO.Add(student);
        }

        public void OpenUpdateStudentView(SSluzba.Models.Student student)
        {
            var updateStudentWindow = new UpdateStudentView(student);
            updateStudentWindow.ShowDialog();
        }

        public void UpdateStudent(Student updatedStudent)
        {
            _studentDAO.Update(updatedStudent);
        }

        public void DeleteStudent(int id)
        {
            var student = _studentDAO.GetAll().FirstOrDefault(s => s.Id == id);
            if (student != null)
            {
                _studentDAO.Remove(student);
            }
        }

        public List<Student> GetAllStudents()
        {
            return _studentDAO.GetAll();
        }

        public List<dynamic> GetStudentDetails()
        {
            var students = GetAllStudents();
            var studentDetails = new List<dynamic>();

            foreach (var student in students)
            {
                var address = _addressController.GetAddressById(student.AddressId)?.ToString() ?? "N/A";
                var index = _indexController.GetIndexById(student.IndexId)?.ToString() ?? "N/A";
                var averageGrade = _examGradeController.GetAverageGrade(student.Id);

                studentDetails.Add(new
                {
                    student.Id,
                    student.Surname,
                    student.Name,
                    student.CurrentYear,
                    AverageGrade = averageGrade,
                    Index = index,
                    Address = address
                });
            }

            return studentDetails;
        }

        public List<ExamGrade> GetExamGradesForStudent(int studentId)
        {
            return _examGradeController.GetExamGradesForStudent(studentId);
        }
    }
}
