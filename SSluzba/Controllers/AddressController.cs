using SSluzba.DAO;
using SSluzba.Models;
using SSluzba.Observer;
using System.Collections.Generic;

namespace SSluzba.Controllers
{
    public class AddressController
    {
        private AddressDAO _addressDAO;

        public AddressController()
        {
            _addressDAO = new AddressDAO();
        }

        public void Subscribe(IObserver observer)
        {
            _addressDAO.Subscribe(observer);
        }

        public void AddAddress(string street, string number, string city, string country)
        {
            Address newAddress = new Address
            {
                Street = street,
                Number = number,
                City = city,
                Country = country
            };
            _addressDAO.Add(newAddress);
        }

        public void UpdateAddress(Address updatedAddress)
        {
            _addressDAO.Update(updatedAddress);
        }

        public void DeleteAddress(int id)
        {
            var address = _addressDAO.GetAll().FirstOrDefault(a => a.Id == id);
            if (address != null)
            {
                _addressDAO.Remove(address);
            }
        }

        public List<Address> GetAllAddresses()
        {
            return _addressDAO.GetAll();
        }

        public Address GetAddressById(int id)
        {
            return _addressDAO.GetAll().FirstOrDefault(address => address.Id == id);
        }
    }
}
