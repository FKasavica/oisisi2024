using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public enum Status
{
    Budget,
    SelfFinanced
}

namespace SSluzba.Models
{
    public class Student : INotifyPropertyChanged
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

        private int _addressId;
        public int AddressId
        {
            get => _addressId;
            set
            {
                if (value != _addressId)
                {
                    _addressId = value;
                    OnPropertyChanged();
                }
            }
        }

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

        private int _indexId;
        public int IndexId
        {
            get => _indexId;
            set
            {
                if (value != _indexId)
                {
                    _indexId = value;
                    OnPropertyChanged();
                }
            }
        }

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


        public Student() { }

        public Student(int id, string surname, string name, DateTime dateOfBirth, int addressId, string phoneNumber, string email, int indexId, int currentYear, Status status)
        {
            Id = id;
            Surname = surname;
            Name = name;
            DateOfBirth = dateOfBirth;
            AddressId = addressId;
            PhoneNumber = phoneNumber;
            Email = email;
            IndexId = indexId;
            CurrentYear = currentYear;
            Status = status;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Surname,
                Name,
                DateOfBirth.ToString("yyyy-MM-dd"),
                AddressId.ToString(),
                PhoneNumber,
                Email,
                IndexId.ToString(),
                CurrentYear.ToString(),
                Status.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Surname = values[1];
            Name = values[2];
            DateOfBirth = DateTime.ParseExact(values[3], "yyyy-MM-dd", null);
            AddressId = int.Parse(values[4]);
            PhoneNumber = values[5];
            Email = values[6];
            IndexId = int.Parse(values[7]);
            CurrentYear = int.Parse(values[8]);
            Status = values[9] == "B" ? Status.Budget : Status.SelfFinanced;

        }

        //public override string ToString()
        //{
        //    return $"ID: {Id}, Name: {Name} {Surname}, Date of Birth: {DateOfBirth:yyyy-MM-dd}, AddressId: {AddressId}, " +
        //           $"Phone: {PhoneNumber}, Email: {Email}, IndexId: {IndexId}, Current Year: {CurrentYear}, " +
        //           $"Status: {Status}, Average Grade: {AverageGrade:F2}";
        //}

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
