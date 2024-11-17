using System.Collections.Generic;
using System.Linq;
using SSluzba.Models;
using SSluzba.Observer;
using SSluzba.Repositories;

namespace SSluzba.DAO
{
    public class DepartmentDAO : ISubject
    {
        private List<IObserver> _observers;
        private List<Department> _departments;
        private DepartmentRepository _repository;

        public DepartmentDAO()
        {
            _repository = new DepartmentRepository();
            _departments = _repository.LoadDepartments();
            _observers = new List<IObserver>();
        }

        public void Add(Department department)
        {
            department.Id = GetNextId();
            _departments.Add(department);
            _repository.SaveDepartments(_departments);
            NotifyObservers();
        }

        public void Update(Department updatedDepartment)
        {
            var existingDepartment = _departments.FirstOrDefault(d => d.Id == updatedDepartment.Id);
            if (existingDepartment != null)
            {
                existingDepartment.DepartmentCode = updatedDepartment.DepartmentCode;
                existingDepartment.DepartmentName = updatedDepartment.DepartmentName;
                existingDepartment.HeadOfDepartmentId = updatedDepartment.HeadOfDepartmentId;
                existingDepartment.ProfessorIdList = updatedDepartment.ProfessorIdList;
                _repository.SaveDepartments(_departments);
                NotifyObservers();
            }
        }

        public void Remove(int id)
        {
            var department = _departments.FirstOrDefault(d => d.Id == id);
            if (department != null)
            {
                _departments.Remove(department);
                _repository.SaveDepartments(_departments);
                NotifyObservers();
            }
        }

        public List<Department> GetAll()
        {
            return _departments;
        }

        private int GetNextId()
        {
            return _departments.Count == 0 ? 1 : _departments.Max(d => d.Id) + 1;
        }

        public void Subscribe(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.Update();
            }
        }
    }
}
