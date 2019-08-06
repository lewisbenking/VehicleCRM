using System;
using System.Collections.Generic;

namespace VehicleCRM
{
    class Customer
    {
        public int CustomerId { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Dictionary<string, Vehicle> Vehicles { get; set; }

        public Customer(int customerId, string forename, string surname, DateTime dateOfBirth)
        {
            CustomerId = customerId;
            Forename = forename;
            Surname = surname;
            DateOfBirth = dateOfBirth;
        }
    }
}