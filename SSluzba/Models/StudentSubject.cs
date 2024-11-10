using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SSluzba.Models
{
    public class StudentSubject : INotifyPropertyChanged
    {
        public int Id { get; set; }

        private int _studentId;
        public int StudentId
        {
            get => _studentId;
            set
            {
                if (value != _studentId)
                {
                    _studentId = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _subjectId;
        public int SubjectId
        {
            get => _subjectId;
            set
            {
                if (value != _subjectId)
                {
                    _subjectId = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _passed;
        public bool Passed
        {
            get => _passed;
            set
            {
                if (value != _passed)
                {
                    _passed = value;
                    OnPropertyChanged();
                }
            }
        }

        public StudentSubject() { }

        public StudentSubject(int id, int studentId, int subjectId, bool passed)
        {
            Id = id;
            StudentId = studentId;
            SubjectId = subjectId;
            Passed = passed;
        }

        public string[] ToCSV()
        {
            return new string[]
            {
                Id.ToString(),
                StudentId.ToString(),
                SubjectId.ToString(),
                Passed.ToString()
            };
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            StudentId = int.Parse(values[1]);
            SubjectId = int.Parse(values[2]);
            Passed = bool.Parse(values[3]);
        }

        public override string ToString()
        {
            return $"Id: {Id}, Student {StudentId} has {(Passed ? "passed" : "failed")} Subject {SubjectId}.";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
