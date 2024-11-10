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
    public class Index
    {
        public int Id { get; set; }

        private string _majorCode;
        public string MajorCode
        {
            get => _majorCode;
            set
            {
                if (value != _majorCode)
                {
                    _majorCode = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _enrollmentNumber;
        public int EnrollmentNumber
        {
            get => _enrollmentNumber;
            set
            {
                if (value != _enrollmentNumber)
                {
                    _enrollmentNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _enrollmentYear;
        public int EnrollmentYear
        {
            get => _enrollmentYear;
            set
            {
                if (value != _enrollmentYear)
                {
                    _enrollmentYear = value;
                    OnPropertyChanged();
                }
            }
        }

        public Index() { }

        public Index(int id, string majorCode, int enrollmentNumber, int enrollmentYear)
        {
            Id = id;
            MajorCode = majorCode;
            EnrollmentNumber = enrollmentNumber;
            EnrollmentYear = enrollmentYear;
        }

        public string[] ToCSV()
        {
            return new string[]
            {
                Id.ToString(),
                MajorCode,
                EnrollmentNumber.ToString(),
                EnrollmentYear.ToString()
            };
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            MajorCode = values[1];
            EnrollmentNumber = int.Parse(values[2]);
            EnrollmentYear = int.Parse(values[3]);
        }

        public override string ToString()
        {
            return $"ID: {Id}, Major Code: {MajorCode}, Enrollment Number: {EnrollmentNumber}, Enrollment Year: {EnrollmentYear}";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
