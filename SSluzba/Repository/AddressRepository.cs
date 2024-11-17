﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using SSluzba.Models;

namespace SSluzba.Repositories
{
    public class AddressRepository
    {
        //private readonly string FilePath = @"D:" + Path.DirectorySeparatorChar + "Github" + Path.DirectorySeparatorChar + "oisisi2024" + Path.DirectorySeparatorChar + "SSluzba" + Path.DirectorySeparatorChar + "Data" + Path.DirectorySeparatorChar + "addresses.csv";
        private readonly string FilePath = @"D:\Github\oisisi2024\SSluzba\Data\addresses.csv";


        public AddressRepository()
        {
        }

        public List<Address> LoadAddresses()
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

        public void SaveAddresses(List<Address> addresses)
        {
            using (StreamWriter sw = new StreamWriter(FilePath))
            {
                foreach (var address in addresses)
                {
                    sw.WriteLine(string.Join(",", address.ToCSV()));
                }
            }
        }
    }
}