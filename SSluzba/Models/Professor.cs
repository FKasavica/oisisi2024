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
    public class Professor : INotifyPropertyChanged
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

        private string _personalIdNumber;
        public string PersonalIdNumber
        {
            get => _personalIdNumber;
            set
            {
                if (value != _personalIdNumber)
                {
                    _personalIdNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                if (value != _title)
                {
                    _title = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _yearsOfExperience;
        public int YearsOfExperience
        {
            get => _yearsOfExperience;
            set
            {
                if (value != _yearsOfExperience)
                {
                    _yearsOfExperience = value;
                    OnPropertyChanged();
                }
            }
        }

        private List<Subject> _subjects = new List<Subject>();
        public List<Subject> Subjects
        {
            get => _subjects;
            set
            {
                if (value != _subjects)
                {
                    _subjects = value;
                    OnPropertyChanged();
                }
            }
        }

        public Professor() 
        {
            _subjects = new List<Subject>();
        }

        public Professor(int id, string surname, string name, DateTime dateOfBirth, string phoneNumber, string email, string personalIdNumber, string title, int yearsOfExperience, List<Subject> subjects, int addressId)
        {
            Id = id;
            Surname = surname;
            Name = name;
            DateOfBirth = dateOfBirth;
            PhoneNumber = phoneNumber;
            Email = email;
            PersonalIdNumber = personalIdNumber;
            Title = title;
            YearsOfExperience = yearsOfExperience;
            Subjects = subjects;
            AddressId = addressId;
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
                PersonalIdNumber,
                Title,
                YearsOfExperience.ToString(),
                string.Join(", ", Subjects.Select(s => s.Id)),
                AddressId.ToString()
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
            PersonalIdNumber = values[6];
            Title = values[7];
            YearsOfExperience = int.Parse(values[8]);
            AddressId = int.Parse(values[9]);

            //var subjectIds = values[9].Split(',').Select(int.Parse).ToList();
            //Subjects = FetchSubjectsByIds(subjectIds);    Enable when service for this is implemented
        }

        public override string ToString()
        {
            return $"ID: {Id}, Name: {Name} {Surname}, Date of Birth: {DateOfBirth:yyyy-MM-dd}, " +
                   $"Phone: {PhoneNumber}, Email: {Email}, Personal ID: {PersonalIdNumber}, " +
                   $"Title: {Title}, Experience: {YearsOfExperience} years, " +
                   $"Subjects: {string.Join(", ", Subjects.Select(s => s.Name))}, " +
                   $"AddressId: {AddressId}";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
