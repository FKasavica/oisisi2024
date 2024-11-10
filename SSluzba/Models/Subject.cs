using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public enum Semester
{
    Winter,
    Summer
}

namespace SSluzba.Models
{
    public class Subject : INotifyPropertyChanged
    {
        public int Id { get; set; }

        private string _code;
        public string Code
        {
            get => _code;
            set
            {
                if (value != _code)
                {
                    _code = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        private Semester _semester;
        public Semester Semester
        {
            get => _semester;
            set
            {
                if (value != _semester)
                {
                    _semester = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _studyYear;
        public int StudyYear
        {
            get => _studyYear;
            set
            {
                if (value != _studyYear)
                {
                    _studyYear = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _professorId;
        public int ProfessorId
        {
            get => _professorId;
            set
            {
                if (value != _professorId)
                {
                    _professorId = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _espbPoints;
        public int EspbPoints
        {
            get => _espbPoints;
            set
            {
                if (value != _espbPoints)
                {
                    _espbPoints = value;
                    OnPropertyChanged();
                }
            }
        }

        private List<Student> _passedStudents = new List<Student>();
        public List<Student> PassedStudents
        {
            get => _passedStudents;
            set
            {
                if (value != _passedStudents)
                {
                    _passedStudents = value;
                    OnPropertyChanged();
                }
            }
        }

        private List<Student> _failedStudents = new List<Student>();
        public List<Student> FailedStudents
        {
            get => _failedStudents;
            set
            {
                if (value != _failedStudents)
                {
                    _failedStudents = value;
                    OnPropertyChanged();
                }
            }
        }

        public Subject() { }

        public Subject(int id, string code, string name, Semester semester, int studyYear, int professorId, int espbPoints, List<Student> passedStudents, List<Student> failedStudents)
        {
            Id = id;
            Code = code;
            Name = name;
            Semester = semester;
            StudyYear = studyYear;
            ProfessorId = professorId;
            EspbPoints = espbPoints;
            PassedStudents = passedStudents;
            FailedStudents = failedStudents;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Code,
                Name,
                Semester.ToString(),
                StudyYear.ToString(),
                ProfessorId.ToString(),
                EspbPoints.ToString(),
                string.Join(", ", PassedStudents),
                string.Join(", ", FailedStudents)
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Code = values[1];
            Name = values[2];
            Semester = (Semester)Enum.Parse(typeof(Semester), values[3]);
            StudyYear = int.Parse(values[4]);
            ProfessorId = int.Parse(values[5]);
            EspbPoints = int.Parse(values[6]);
            PassedStudents = new List<Student>();
            FailedStudents = new List<Student>();
        }

        public override string ToString()
        {
            return $"ID: {Id}, Code: {Code}, Name: {Name}, Semester: {Semester}, Study Year: {StudyYear}, " +
                   $"Professor ID: {ProfessorId}, ESPB Points: {EspbPoints}, " +
                   $"Passed Students: {PassedStudents.Count}, Failed Students: {FailedStudents.Count}";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
