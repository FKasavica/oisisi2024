using SSluzba.DAO;
using SSluzba.Models;
using SSluzba.Observer;

namespace SSluzba.Controllers
{
    public class DepartmentController
    {
        private DepartmentDAO _departmentDAO;

        public DepartmentController()
        {
            _departmentDAO = new DepartmentDAO();
        }

        public void Subscribe(IObserver observer)
        {
            _departmentDAO.Subscribe(observer);
        }

        public void AddDepartment(string departmentCode, string departmentName, int headOfDepartmentId, List<int> professorIdList)
        {
            var newDepartment = new Department
            {
                DepartmentCode = departmentCode,
                DepartmentName = departmentName,
                HeadOfDepartmentId = headOfDepartmentId,
                ProfessorIdList = professorIdList
            };

            _departmentDAO.Add(newDepartment);
        }

        public void UpdateDepartment(Department updatedDepartment)
        {
            _departmentDAO.Update(updatedDepartment);
        }

        public void DeleteDepartment(int id)
        {
            _departmentDAO.Remove(id);
        }

        public List<Department> GetAllDepartments()
        {
            return _departmentDAO.GetAll();
        }
    }
}
