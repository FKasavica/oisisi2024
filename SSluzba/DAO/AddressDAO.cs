using System.Collections.Generic;
using System.Linq;
using SSluzba.Models;
using SSluzba.Observer;
using SSluzba.Repositories; // Pretpostavka da postoji AddressRepository

namespace SSluzba.DAO
{
    public class AddressDAO : ISubject
    {
        private AddressRepository _repository;
        private List<Address> _addresses;
        private List<IObserver> _observers;

        public AddressDAO()
        {
            _repository = new AddressRepository();
            _addresses = _repository.LoadAddresses(); // Učitavanje podataka iz repository-a
            _observers = new List<IObserver>();
        }

        private void SaveAddresses()
        {
            _repository.SaveAddresses(_addresses); // Čuvanje podataka putem repository-a
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
