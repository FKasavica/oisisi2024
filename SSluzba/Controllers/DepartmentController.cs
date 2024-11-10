using SSluzba.DAO;
using SSluzba.Models;

namespace SSluzba.Controllers
{
    public class DepartmentController
    {
        private DepartmentDAO _departmentDAO;

        public DepartmentController()
        {
            _departmentDAO = new DepartmentDAO();
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

            _departmentDAO.AddDepartment(newDepartment);
        }

        public void UpdateDepartment(int id, string departmentCode, string departmentName, int headOfDepartmentId, List<int> professorIdList)
        {
            var updatedDepartment = new Department
            {
                Id = id,
                DepartmentCode = departmentCode,
                DepartmentName = departmentName,
                HeadOfDepartmentId = headOfDepartmentId,
                ProfessorIdList = professorIdList
            };

            _departmentDAO.UpdateDepartment(updatedDepartment);
        }

        public void DeleteDepartment(int id)
        {
            _departmentDAO.RemoveDepartment(id);
        }

        public List<Department> GetAllDepartments()
        {
            return _departmentDAO.GetAllDepartments();
        }
    }
}
