using SSluzba.DAO;
using SSluzba.Models;
using SSluzba.Observer;
using SSluzba.Views;

namespace SSluzba.Controllers
{
    public class StudentController
    {
        private StudentDAO _studentDAO;

        public StudentController()
        {
            _studentDAO = new StudentDAO();
        }

        public void Subscribe(IObserver observer)
        {
            _studentDAO.Subscribe(observer);
        }

        // Ova metoda pokreće AddStudentView i delegira dodavanje novog studenta
        public void OpenAddStudentView()
        {
            var addStudentWindow = new AddStudentView();
        }

        public void AddStudent(Student student)
        {
            _studentDAO.Add(student);
        }

        // Ova metoda pokreće UpdateStudentView i delegira ažuriranje studenta
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
    }
}
