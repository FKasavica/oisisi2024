using SSluzba.DAO;
using SSluzba.Models;
using SSluzba.Observer;
using System.Collections.Generic;
using System.Linq;

namespace SSluzba.Controllers
{
    public class SubjectController
    {
        private readonly SubjectDAO _subjectDAO = new();
        private readonly StudentSubjectController _studentSubjectController = new();
        private readonly ProfessorController _professorController = new();

        public SubjectController() { }

        public void Subscribe(IObserver observer)
        {
            _subjectDAO.Subscribe(observer);
        }

        public List<Subject> GetAllSubjects()
        {
            return _subjectDAO.GetAll();
        }

        public Subject GetSubjectById(int subjectId)
        {
            return _subjectDAO.GetAll().FirstOrDefault(s => s.Id == subjectId);
        }

        public void AddSubject(Subject subject)
        {
            _subjectDAO.Add(subject);
        }

        public void UpdateSubject(Subject subject)
        {
            _subjectDAO.Update(subject);
        }

        public void DeleteSubject(int subjectId)
        {
            var subject = _subjectDAO.GetAll().FirstOrDefault(s => s.Id == subjectId);
            if (subject != null)
            {
                _subjectDAO.Delete(subject.Id);
            }
        }

        // Fetch students who passed the subject
        public List<Student> GetStudentsWhoPassed(int subjectId)
        {
            var studentSubjects = _studentSubjectController.GetStudentsBySubjectId(subjectId)
                .Where(ss => ss.Passed).ToList();
            return studentSubjects.Select(ss => new StudentController().GetStudentById(ss.StudentId)).Where(s => s != null).ToList();
        }

        // Fetch students who did not pass the subject
        public List<Student> GetStudentsWhoFailed(int subjectId)
        {
            var studentSubjects = _studentSubjectController.GetStudentsBySubjectId(subjectId)
                .Where(ss => !ss.Passed).ToList();
            return studentSubjects.Select(ss => new StudentController().GetStudentById(ss.StudentId)).Where(s => s != null).ToList();
        }

        public List<dynamic> GetSubjectDetails()
        {
            var subjects = GetAllSubjects();
            var subjectDetails = new List<dynamic>();

            foreach (var subject in subjects)
            {
                // Retrieve professor details if available
                var professor = _professorController.GetProfessorById(subject.ProfessorId);
                string professorName = professor != null ? $"{professor.Name} {professor.Surname}" : "Professor not found";

                subjectDetails.Add(new
                {
                    subject.Id,
                    subject.Code,
                    subject.Name,
                    subject.Semester,
                    subject.StudyYear,
                    ProfessorName = professorName,
                    subject.EspbPoints
                });
            }

            return subjectDetails;
        }
    }
}
