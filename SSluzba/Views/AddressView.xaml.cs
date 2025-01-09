using SSluzba.Controllers;
using SSluzba.Models;
using SSluzba.Observer;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace SSluzba.Views
{
    public partial class AddressView : Window, IObserver
    {
        private readonly AddressController _addressController;
        public ObservableCollection<Address> Addresses { get; private set; }

        public AddressView()
        {
            InitializeComponent();
            _addressController = new AddressController();
            _addressController.Subscribe(this);

            LoadAddresses();
        }

        public void Update()
        {
            LoadAddresses();
        }

        private void LoadAddresses()
        {
            var addressList = _addressController.GetAllAddresses();
            Addresses = new ObservableCollection<Address>(addressList);
            AddressListView.ItemsSource = Addresses;
        }

        private void AddAddressButton_Click(object sender, RoutedEventArgs e)
        {
            // Example add logic; you may prompt for input values using a dialog or form
            _addressController.AddAddress("New Street", "0", "New City", "New Country");
            LoadAddresses(); // Refresh the list
        }

        private void UpdateAddressButton_Click(object sender, RoutedEventArgs e)
        {
            if (AddressListView.SelectedItem is Address selectedAddress)
            {
                // Example update logic - modify properties as needed or prompt for input
                selectedAddress.Street = "Updated Street";
                selectedAddress.Number = "Updated Number";
                selectedAddress.City = "Updated City";
                selectedAddress.Country = "Updated Country";

                _addressController.UpdateAddress(selectedAddress);
                LoadAddresses(); // Refresh the list
            }
            else
            {
                MessageBox.Show("Please select an address to update.", "Update Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteAddressButton_Click(object sender, RoutedEventArgs e)
        {
            if (AddressListView.SelectedItem is Address selectedAddress)
            {
                _addressController.DeleteAddress(selectedAddress.Id);
                LoadAddresses(); // Refresh the list
            }
            else
            {
                MessageBox.Show("Please select an address to delete.", "Delete Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
