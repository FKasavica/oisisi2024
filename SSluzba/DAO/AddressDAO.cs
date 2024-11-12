using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SSluzba.Models;
using SSluzba.Observer;

namespace SSluzba.DAO
{
    public class AddressDAO : ISubject
    {
        private readonly string FilePath = "data" + Path.DirectorySeparatorChar + "addresses.csv";
        private List<Address> _addresses;
        private List<IObserver> _observers;

        public AddressDAO()
        {
            _addresses = LoadAddresses();
            _observers = new List<IObserver>();
        }

        private List<Address> LoadAddresses()
        {
            List<Address> addresses = new List<Address>();
            if (File.Exists(FilePath))
            {
                foreach (var line in File.ReadLines(FilePath))
                {
                    var values = line.Split(',');
                    Address address = new Address();
                    address.FromCSV(values);
                    addresses.Add(address);
                }
            }
            return addresses;
        }

        private void SaveAddresses()
        {
            using (StreamWriter sw = new StreamWriter(FilePath))
            {
                foreach (var address in _addresses)
                {
                    sw.WriteLine(string.Join(",", address.ToCSV()));
                }
            }
        }

        public int NextId()
        {
            return _addresses.Count == 0 ? 1 : _addresses.Max(a => a.Id) + 1;
        }

        public void Add(Address address)
        {
            address.Id = NextId();
            _addresses.Add(address);
            SaveAddresses();
            NotifyObservers();
        }

        public void Remove(Address address)
        {
            _addresses.Remove(address);
            SaveAddresses();
            NotifyObservers();
        }

        public void Update(Address updatedAddress)
        {
            var existingAddress = _addresses.FirstOrDefault(a => a.Id == updatedAddress.Id);
            if (existingAddress != null)
            {
                existingAddress.Street = updatedAddress.Street;
                existingAddress.Number = updatedAddress.Number;
                existingAddress.City = updatedAddress.City;
                existingAddress.Country = updatedAddress.Country;
                SaveAddresses();
                NotifyObservers();
            }
        }

        public List<Address> GetAll()
        {
            return _addresses;
        }

        public void Subscribe(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.Update();
            }
        }
    }
}
