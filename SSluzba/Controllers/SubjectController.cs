using SSluzba.DAO;
using SSluzba.Models;
using SSluzba.Observer;
using SSluzba.Views.Subjects;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace SSluzba.Controllers
{
    public class SubjectController
    {
        private readonly SubjectDAO _subjectDAO = new();
        private readonly StudentSubjectController _studentSubjectController = new();
        private readonly ProfessorController _professorController = new();

        public SubjectController() { }

        public (Subject subject, List<Professor> professors) GetSubjectDataForUpdate(int subjectId)
        {
            var subject = _subjectDAO.GetAll().FirstOrDefault(s => s.Id == subjectId);
            if (subject == null)
            {
                throw new ArgumentException("Subject not found.");
            }

            var professors = _professorController.GetAllProfessors();
            return (subject, professors);
        }

        public Subject UpdateExistingSubject(
            int subjectId,
            string code,
            string name,
            Semester semester,
            string studyYearText,
            int professorId,
            string espbPointsText)
        {
            if (string.IsNullOrWhiteSpace(code) ||
                string.IsNullOrWhiteSpace(name) ||
                !int.TryParse(studyYearText, out int studyYear) ||
                !int.TryParse(espbPointsText, out int espbPoints))
            {
                throw new ArgumentException("Please fill in all fields correctly.");
            }

            var existingSubject = _subjectDAO.GetAll().FirstOrDefault(s => s.Id == subjectId);
            if (existingSubject == null)
            {
                throw new ArgumentException("Subject not found.");
            }

            var updatedSubject = new Subject
            {
                Id = subjectId,
                Code = code,
                Name = name,
                Semester = semester,
                StudyYear = studyYear,
                ProfessorId = professorId,
                EspbPoints = espbPoints
            };

            _subjectDAO.Update(updatedSubject);
            return updatedSubject;
        }
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

        public List<Subject> GetSubjectsByProfessorId(int professorId)
        {
            return _subjectDAO.GetAll().FindAll(subject => subject.ProfessorId == professorId);
        }

        public List<Student> GetStudentsWhoPassed(int subjectId)
        {
            var studentSubjects = _studentSubjectController.GetStudentsBySubjectId(subjectId)
                .Where(ss => ss.Passed).ToList();
            return studentSubjects.Select(ss => new StudentController().GetStudentById(ss.StudentId)).Where(s => s != null).ToList();
        }

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

        public Subject CreateNewSubject(
            string code,
            string name,
            object semesterInput,
            string studyYearText,
            int professorId,
            string espbPointsText)
        {
            if (string.IsNullOrWhiteSpace(code) ||
                string.IsNullOrWhiteSpace(name) ||
                semesterInput == null ||
                !int.TryParse(studyYearText, out int studyYear) ||
                !int.TryParse(espbPointsText, out int espbPoints))
            {
                throw new ArgumentException("Please fill in all fields correctly.");
            }

            var semester = Semester.Winter; // Default value
            if (semesterInput is ComboBoxItem selectedItem)
            {
                Enum.TryParse(selectedItem.Content.ToString(), out semester);
            }

            var newSubject = new Subject
            {
                Code = code,
                Name = name,
                Semester = semester,
                StudyYear = studyYear,
                ProfessorId = professorId,
                EspbPoints = espbPoints
            };

            AddSubject(newSubject);
            return newSubject;
        }

        public void OpenUpdateSubjectView(Subject subject)
        {
            var updateSubjectWindow = new UpdateSubjectView(subject.Id);
            updateSubjectWindow.ShowDialog();
        }

        public void MoveStudentToPassed(int studentId, int subjectId)
        {
            var studentSubjects = _studentSubjectController.GetStudentsBySubjectId(subjectId)
                .Where(ss => ss.StudentId == studentId).ToList();

            foreach (var studentSubject in studentSubjects)
            {
                studentSubject.Passed = true;
                _studentSubjectController.UpdateStudentSubject(studentSubject);
            }
        }

        public void MoveStudentToFailed(int studentId, int subjectId)
        {
            var studentSubjects = _studentSubjectController.GetStudentsBySubjectId(subjectId)
                .Where(ss => ss.StudentId == studentId).ToList();

            foreach (var studentSubject in studentSubjects)
            {
                studentSubject.Passed = false;
                _studentSubjectController.UpdateStudentSubject(studentSubject);
            }
        }

        public void AddStudentToSubject(int studentId, int subjectId)
        {
            var studentSubject = new StudentSubject
            {
                StudentId = studentId,
                SubjectId = subjectId,
                Passed = false // Default to 'failed' status initially
            };
            _studentSubjectController.AddStudentSubject(studentSubject);
        }

        public void RemoveStudentFromSubject(int studentId, int subjectId)
        {
            var studentSubjects = _studentSubjectController.GetStudentsBySubjectId(subjectId)
                .Where(ss => ss.StudentId == studentId).ToList();

            foreach (var studentSubject in studentSubjects)
            {
                _studentSubjectController.DeleteStudentSubject(studentSubject.Id);
            }
        }


    }
}
