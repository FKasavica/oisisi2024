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
    public class ExamGrade : INotifyPropertyChanged
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

        private double _numericGrade;
        public double NumericGrade
        {
            get => _numericGrade;
            set
            {
                if (value >= 6 && value <= 10)
                {
                    if (value != _numericGrade)
                    {
                        _numericGrade = value;
                        OnPropertyChanged();
                    }
                }
                else
                {
                    throw new ArgumentException("Grade must be between 6 and 10.");
                }
            }
        }

        private DateTime _examDate;
        public DateTime ExamDate
        {
            get => _examDate;
            set
            {
                if (value != _examDate)
                {
                    _examDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public ExamGrade() { }

        public ExamGrade(int id, int studentId, int subjectId, double numericGrade, DateTime examDate)
        {
            Id = id;
            StudentId = studentId;
            SubjectId = subjectId;
            NumericGrade = numericGrade;
            ExamDate = examDate;
        }

        public string[] ToCSV()
        {
            return new string[]
            {
                Id.ToString(),
                StudentId.ToString(),
                SubjectId.ToString(),
                NumericGrade.ToString(),
                ExamDate.ToString("yyyy-MM-dd")
            };
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            StudentId = int.Parse(values[1]);
            SubjectId = int.Parse(values[2]);
            NumericGrade = double.Parse(values[3]);
            ExamDate = DateTime.ParseExact(values[4], "yyyy-MM-dd", null);
        }

        public override string ToString()
        {
            return $"ID: {Id}, Student ID: {StudentId}, Subject ID: {SubjectId}, Grade: {NumericGrade}, Exam Date: {ExamDate:yyyy-MM-dd}";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
