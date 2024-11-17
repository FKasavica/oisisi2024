using System.Collections.Generic;
using System.Linq;
using SSluzba.Models;
using SSluzba.Repositories;

namespace SSluzba.DAO
{
    public class StudentSubjectDAO
    {
        private List<StudentSubject> _studentSubjects;
        private readonly StudentSubjectRepository _repository;

        public StudentSubjectDAO()
        {
            _repository = new StudentSubjectRepository();
            _studentSubjects = _repository.LoadStudentSubjects();
        }

        public void AddStudentSubject(StudentSubject studentSubject)
        {
            studentSubject.Id = GetNextId();
            _studentSubjects.Add(studentSubject);
            _repository.SaveStudentSubjects(_studentSubjects);
        }

        public void UpdateStudentSubject(StudentSubject studentSubject)
        {
            var existing = _studentSubjects.FirstOrDefault(ss => ss.Id == studentSubject.Id);
            if (existing != null)
            {
                existing.StudentId = studentSubject.StudentId;
                existing.SubjectId = studentSubject.SubjectId;
                existing.Passed = studentSubject.Passed;
                _repository.SaveStudentSubjects(_studentSubjects);
            }
        }

        public void DeleteStudentSubject(int id)
        {
            _studentSubjects.RemoveAll(ss => ss.Id == id);
            _repository.SaveStudentSubjects(_studentSubjects);
        }

        public List<StudentSubject> GetAll()
        {
            return _studentSubjects;
        }

        private int GetNextId()
        {
            return _studentSubjects.Count == 0 ? 1 : _studentSubjects.Max(ss => ss.Id) + 1;
        }
    }
}
