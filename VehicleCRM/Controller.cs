using System;
using System.Collections.Generic;
using System.IO;

namespace VehicleCRM
{
    class Controller
    {
        private readonly Customer customer;
        private readonly Car car;
        private readonly Motorcycle motorcycle;
        private static Dictionary<int, Customer> customers;
        private static Dictionary<string, Vehicle> vehicles;
        private static string filePath, dataToWriteToFile;

        public Controller()
        {
            string csvFile = @"..\..\CustomerInformation.csv";
            string[,] values = LoadCSVData(csvFile);
            int rowCount = values.GetUpperBound(0) + 1;
            customers = new Dictionary<int, Customer>();
            vehicles = new Dictionary<string, Vehicle>();

            //Read the data and add to List 
            for (int row = 1; row < rowCount; row++)
            {
                // Prevents creating duplicate customers
                if (!customers.ContainsKey(int.Parse(values[row, 0])))
                {
                    customer = new Customer(int.Parse(values[row, 0]), values[row, 1], values[row, 2], DateTime.Parse(values[row, 3]))
                    {
                        Vehicles = new Dictionary<string, Vehicle>()
                    };
                    customers.Add(int.Parse(values[row, 0]), customer);
                }

                // Validate the vehicle id is an integer (so last row in document doesn't create an empty vehicle)
                if (int.TryParse(values[row, 4], out int vehicleId))
                {
                    // Creates a car/motorbike depending on column 12
                    if (values[row, 12].Trim() == "Car")
                    {
                        car = new Car(vehicleId, values[row, 12], values[row, 5], values[row, 6], values[row, 7], int.Parse(values[row, 8]), DateTime.Parse(values[row, 9]), values[row, 10], customer);
                        vehicles.Add(values[row, 5], car);
                        customer.Vehicles.Add(values[row, 5], car);
                    }
                    else
                    {
                        motorcycle = new Motorcycle(vehicleId, values[row, 12], values[row, 5], values[row, 6], values[row, 7], int.Parse(values[row, 8]), DateTime.Parse(values[row, 9]), values[row, 11], customer);
                        vehicles.Add(values[row, 5], motorcycle);
                        customer.Vehicles.Add(values[row, 5], motorcycle);
                    }
                }
            }
        }

        private static string[,] LoadCSVData(string filename)
        {
            // Read the file, remove new line chars, split into array of lines, get row/column count
            string fileContents = File.ReadAllText(filename);
            fileContents = fileContents.Replace('\n', '\r');
            string[] lines = fileContents.Split(new char[] { '\r' }, StringSplitOptions.RemoveEmptyEntries);
            int rowCount = lines.Length;
            int columnCount = lines[0].Split(',').Length;

            // Allocate values at each row/column to an array of values
            string[,] values = new string[rowCount, columnCount];
            for (int row = 0; row < rowCount; row++)
            {
                string[] rowLine = lines[row].Split(',');
                for (int column = 0; column < columnCount; column++)
                {
                    values[row, column] = rowLine[column];
                }
            }
            return values;
        }

        public void HandleUserInput(int userInput)
        {
            filePath = $"./{DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}-Report{userInput}.csv";
            if (userInput == 1)
            {
                CustomersAndTheirVehiclesReport();
            }
            else if (userInput == 2)
            {
                CustomerAgeReport();
            }
            else if (userInput == 3 || userInput == 4)
            {
                VehicleReport(userInput);
            }
        }

        public static void CustomersAndTheirVehiclesReport()
        {
            dataToWriteToFile = "CustomerId,Forename,Surname,DateOfBirth,VehicleId,RegistrationNumber,Manufacturer,Model,EngineSize,RegistationDate,InteriorColour,HasHelmetStorage,VehicleType";
            foreach (KeyValuePair<int, Customer> customer in customers)
            {
                // If customer has no vehicles, just write customer data otherwise write customer and vehicle data
                if (customer.Value.Vehicles.Count == 0)
                {
                    dataToWriteToFile += $"\n{customer.Key},{customer.Value.Forename},{customer.Value.Surname},{customer.Value.DateOfBirth},,,,,,,,,";
                }
                else
                {
                    foreach (KeyValuePair<string, Vehicle> vehicle in customer.Value.Vehicles)
                    {
                        // Gets interior colour if it's a car or helmet storage if it's a motorcyle
                        string interiorColour = (vehicles[vehicle.Value.RegistrationNumber] as Car)?.InteriorColour;
                        string hasHelmetStorage = (vehicles[vehicle.Value.RegistrationNumber] as Motorcycle)?.HasHelmetStorage;
                        dataToWriteToFile += $"\n{customer.Key},{customer.Value.Forename},{customer.Value.Surname},{customer.Value.DateOfBirth},{vehicle.Value.VehicleId},{vehicle.Value.RegistrationNumber},{vehicle.Value.Manufacturer},{vehicle.Value.Model},{vehicle.Value.EngineSize},{vehicle.Value.RegistrationDate},{interiorColour},{hasHelmetStorage},{vehicle.Value.VehicleType}";
                    }
                }
            }
            Console.WriteLine(dataToWriteToFile);
            SaveResultsToFile();
        }

        private static void CustomerAgeReport()
        {
            dataToWriteToFile = "CustomerId,Forename,Surname,DateOfBirth";
            foreach (KeyValuePair<int, Customer> customer in customers)
            {
                DateTime dateOfBirth = customer.Value.DateOfBirth;
                int age = DateTime.Now.Year - dateOfBirth.Year;
                // Gets exact age, then checks if the age is between 20 and 30
                if (DateTime.Now.Month < dateOfBirth.Month || (DateTime.Now.Month == dateOfBirth.Month && DateTime.Now.Day < dateOfBirth.Day))
                {
                    age--;
                }
                if (age > 20 && age < 30)
                {
                    dataToWriteToFile += $"\n{customer.Key},{customer.Value.Forename},{customer.Value.Surname},{customer.Value.DateOfBirth}";
                }
            }
            Console.WriteLine(dataToWriteToFile);
            SaveResultsToFile();
        }

        private static void VehicleReport(int userInput)
        {
            dataToWriteToFile = "VehicleId,RegistrationNumber,Manufacturer,Model,EngineSize,RegistationDate,InteriorColour,HasHelmetStorage,VehicleType";
            foreach (KeyValuePair<string, Vehicle> vehicle in vehicles)
            {
                if ((userInput == 3 && vehicle.Value.RegistrationDate < DateTime.Parse("2010-01-01")) || (userInput == 4 && vehicle.Value.EngineSize > 1100))
                {
                    // Gets interior colour if it's a car or helmet storage if it's a motorcyle
                    string interiorColour = (vehicles[vehicle.Value.RegistrationNumber] as Car)?.InteriorColour;
                    string hasHelmetStorage = (vehicles[vehicle.Value.RegistrationNumber] as Motorcycle)?.HasHelmetStorage;
                    dataToWriteToFile += $"\n{vehicle.Value.VehicleId},{vehicle.Value.RegistrationNumber},{vehicle.Value.Manufacturer},{vehicle.Value.Model},{vehicle.Value.EngineSize},{vehicle.Value.RegistrationDate},{interiorColour},{hasHelmetStorage},{vehicle.Value.VehicleType}";
                }
            }
            Console.WriteLine(dataToWriteToFile);
            SaveResultsToFile();
        }

        private static void SaveResultsToFile()
        {
            using (StreamWriter file = File.AppendText(filePath))
            {
                file.Write(dataToWriteToFile);
                file.Close();
                file.Dispose();
                Console.WriteLine($"File saved to {filePath}\n");
            }
        }
    }
}