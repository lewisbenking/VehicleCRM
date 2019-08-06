using System;

namespace VehicleCRM
{
    // Base class
    class Vehicle
    {
        public int VehicleId { get; set; }
        public Customer Customer { get; set; }
        // Wasn't sure how to make the VehicleType immutable, I think it's by putting readonly there
        public readonly string VehicleType;
        public string RegistrationNumber { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public int EngineSize { get; set; }
        public DateTime RegistrationDate { get; set; }

        public Vehicle(int vehicleId, string vehicleType, string registrationNumber, string manufacturer, string model, int engineSize, DateTime registrationDate, Customer customer)
        {
            VehicleId = vehicleId;
            VehicleType = vehicleType;
            RegistrationNumber = registrationNumber;
            Manufacturer = manufacturer;
            Model = model;
            EngineSize = engineSize;
            RegistrationDate = registrationDate;
            Customer = customer;
        }
    }

    // Derived Car and Motorcycle classes
    class Car : Vehicle
    {
        public string InteriorColour { get; set; }

        public Car(int vehicleId, string vehicleType, string registrationNumber, string manufacturer, string model, int engineSize, DateTime registrationDate, string interiorColour, Customer customer) : base(vehicleId, vehicleType, registrationNumber, manufacturer, model, engineSize, registrationDate, customer)
        {
            InteriorColour = interiorColour;
        }
    }

    class Motorcycle : Vehicle
    {
        public string HasHelmetStorage { get; set; }

        public Motorcycle(int vehicleId, string vehicleType, string registrationNumber, string manufacturer, string model, int engineSize, DateTime registrationDate, string hasHelmetStorage, Customer customer) : base(vehicleId, vehicleType, registrationNumber, manufacturer, model, engineSize, registrationDate, customer)
        {
            HasHelmetStorage = hasHelmetStorage;
        }
    }
}