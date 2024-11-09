using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public enum Status
{
    Budget = 'B',
    SelfFinanced = 'S'
}

namespace SSluzba
{
    public class Student: INotifyPropertyChanged
    {
        public int Id { get; set; }

        private string _surname;
        public string Surname
        {
            get => _surname;
            set
            {
                if (value != _surname)
                {
                    _surname = value;
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

        private DateTime _dateOfBirth;
        public DateTime DateOfBirth
        {
            get => _dateOfBirth;
            set
            {
                if (value != _dateOfBirth)
                {
                    _dateOfBirth = value;
                    OnPropertyChanged();
                }
            }
        }

        //Address, class needed

        private string _phoneNumber;
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (value != _phoneNumber)
                {
                    _phoneNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                if (value != _email)
                {
                    _email = value;
                    OnPropertyChanged();
                }
            }
        }

        //Index, class needed

        private int _currentYear;
        public int CurrentYear
        {
            get => _currentYear;
            set
            {
                if (value != _currentYear)
                {
                    _currentYear = value;
                    OnPropertyChanged();
                }
            }
        }

        private Status _status;
        public Status Status
        {
            get => _status;
            set
            {
                if (value != _status)
                {
                    _status = value;
                    OnPropertyChanged();
                }
            }
        }

        private double _averageGrade;
        public double AverageGrade
        {
            get => _averageGrade;
            set
            {
                if (value != _averageGrade)
                {
                    _averageGrade = value;
                    OnPropertyChanged();
                }
            }
        }

        //PassedSubjects, class needed

        //FailedSubjects, class needed

        public Student() { }

        public Student(int id, string surname, string name, DateTime dateOfBirth, string phoneNumber, string email, int currentYear, Status status, double averageGrade)
        {
            Id = id;
            Surname = surname;
            Name = name;
            DateOfBirth = dateOfBirth;
            PhoneNumber = phoneNumber;
            Email = email;
            CurrentYear = currentYear;
            Status = status;
            AverageGrade = averageGrade;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
            Id.ToString(),
            Surname,
            Name,
            DateOfBirth.ToString("yyyy-MM-dd"),
            PhoneNumber,
            Email,
            CurrentYear.ToString(),
            Status.ToString(),
            AverageGrade.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Surname = values[1];
            Name = values[2];
            DateOfBirth = DateTime.ParseExact(values[3], "yyyy-MM-dd", null);
            PhoneNumber = values[4];
            Email = values[5];
            CurrentYear = int.Parse(values[6]);
            Status = (Status)Enum.Parse(typeof(Status), values[7]);
            AverageGrade = double.Parse(values[8]);
        }

        public override string ToString()
        {
            return $"ID: {Id}, Name: {Name} {Surname}, Date of Birth: {DateOfBirth:yyyy-MM-dd}, " +
                   $"Phone: {PhoneNumber}, Email: {Email}, Current Year: {CurrentYear}, " +
                   $"Status: {Status}, Average Grade: {AverageGrade:F2}";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}