using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SSluzba.Models
{
    public class Address : INotifyPropertyChanged
    {
        public int Id { get; set; }

        private string _street;
        public string Street
        {
            get => _street;
            set
            {
                if (value != _street)
                {
                    _street = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _number;
        public string Number
        {
            get => _number;
            set
            {
                if (value != _number)
                {
                    _number = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _city;
        public string City
        {
            get => _city;
            set
            {
                if (value != _city)
                {
                    _city = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _country;
        public string Country
        {
            get => _country;
            set
            {
                if (value != _country)
                {
                    _country = value;
                    OnPropertyChanged();
                }
            }
        }

        public Address() { }

        public Address(int id, string street, string number, string city, string country)
        {
            Id = id;
            Street = street;
            Number = number;
            City = city;
            Country = country;
        }

        public string[] ToCSV()
        {
            return new string[]
            {
                Id.ToString(),
                Street,
                Number,
                City,
                Country
            };
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Street = values[1];
            Number = values[2];
            City = values[3];
            Country = values[4];
        }

        public override string ToString()
        {
            return $"{Street} {Number}, {City}, {Country}";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}