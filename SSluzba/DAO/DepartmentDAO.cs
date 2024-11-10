using System;
using System.Collections.Generic;
using System.Linq;
using SSluzba.Models;

namespace SSluzba.DAO
{
    public class DepartmentDAO
    {
        private List<Department> _departments;

        public DepartmentDAO()
        {
            _departments = new List<Department>();
        }

        public void AddDepartment(Department department)
        {
            _departments.Add(department);
        }

        public void UpdateDepartment(Department department)
        {
            var existingDepartment = _departments.FirstOrDefault(d => d.Id == department.Id);
            if (existingDepartment != null)
            {
                existingDepartment.DepartmentCode = department.DepartmentCode;
                existingDepartment.DepartmentName = department.DepartmentName;
                existingDepartment.HeadOfDepartmentId = department.HeadOfDepartmentId;
                existingDepartment.ProfessorIdList = department.ProfessorIdList;
            }
        }

        public void RemoveDepartment(int id)
        {
            var department = _departments.FirstOrDefault(d => d.Id == id);
            if (department != null)
            {
                _departments.Remove(department);
            }
        }

        public List<Department> GetAllDepartments()
        {
            return _departments;
        }
    }
}