using System;
using System.Collections.Generic;

namespace CarsWithPizzazz
{
    public interface IAutoDBAccess
    {
        //LoadLot accesses the database and returns a collection of Auto objects
        List<Auto> LoadLot();

        //SaveLot writes results to the database following a change to the collection of Auto objects
        bool SaveLot(List<Auto> lot);
    }

    public class CarCollection : IAutoDBAccess
    {
        public List<Auto> LoadLot()
        {
            throw new NotImplementedException();
        }

        public bool SaveLot(List<Auto> lot)
        {
            throw new NotImplementedException();
        }

    }

    public class Auto
    {
        public string VehicleIdentificationNumber { get; set; }
        public string Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string LocationOnLot { get; set; }
    }


    public class AutoControl
    {
        IAutoDBAccess inventory;

        public AutoControl(IAutoDBAccess inventory)
        {
            this.inventory = inventory;
        }

        public Auto FindCar(string vehicleIdentificationNumber)
        {
            // When implemented, this method will return information about a car from the database based on its VIN.
            // If there is no such VIN in the database, throws a VINNotFoundException
            var cars = this.inventory.LoadLot();

            int sub = 0;
            while (sub < cars.Count &&
                cars[sub].VehicleIdentificationNumber != vehicleIdentificationNumber)
            {
                sub++;
            }
            if (sub >= cars.Count)
            {
                throw new VINNotFoundException($"VIN {vehicleIdentificationNumber} not in database");
            }
            else
            {
                return cars[sub];
            }
        }

        public List<Auto> FindCarsByMake(string make)
        {
            List<Auto> result = new List<Auto>();
            var cars = this.inventory.LoadLot();
            foreach (Auto car in cars)
            {
                if (make == car.Make)
                {
                    result.Add(car);
                }
            }
            return result;
        }

        public List<Auto> AddCar(Auto newCarOnTheLot)
        {
            //AddCar() adds a new car to the database.  If it fails, it raises one of these custom exceptions:
            //- Wrong length VIN.  VIN must be 17 characters string. (In reality, it's a little more complex than that.)
            //- Duplicate VIN.  VINs are unique, like a GUID for cars.
            //- Duplicate location.  Each location on the lot can accommodate one car, so you can only have one car per.
            //- Failure writing the result to the database, indicated by SaveLot() not returning true.

            var cars = this.inventory.LoadLot();
            if (newCarOnTheLot.VehicleIdentificationNumber.Length != 17) throw new InvalidVINException();
            foreach (Auto car in cars)
            {
                if (newCarOnTheLot.VehicleIdentificationNumber == car.VehicleIdentificationNumber)
                {
                    throw new DuplicateVINException();
                }
            }

            foreach (Auto car in cars)
            {
                if (newCarOnTheLot.LocationOnLot == car.LocationOnLot)
                {
                    throw new DuplicateLocationException();
                }
            }

            cars.Add(newCarOnTheLot);

            if (!this.inventory.SaveLot(cars)) throw new SaveLotFailureException();

            return cars;
        }

        public List<Auto> RemoveCar(string vehicleIdentificationNumber)
        {
            //When a car is sold (or stolen or scrapped), it is removed from the lot.  RemoveCar() deletes the car from the database.
            //- If there is no car with a matching VIN, RemoveCar throws a VINNotFound exception.
            //- If the SaveLot() method returns false, indicating an error writing the result to the database, RemoveCar throws a
            //  SaveLotFailureException exception.

            var cars = this.inventory.LoadLot();
            int sub = 0;
            while (sub < cars.Count &&
                cars[sub].VehicleIdentificationNumber != vehicleIdentificationNumber)
            {
                sub++;
            }
            if (sub >= cars.Count)
            {
                throw new VINNotFoundException($"VIN {vehicleIdentificationNumber} not in database");
            }
            else
            {
                cars.Remove(cars[sub]);
            }

            if (!this.inventory.SaveLot(cars)) throw new SaveLotFailureException();

            return cars;
        }
    }

    public class VINNotFoundException : Exception
    {
        public VINNotFoundException()
        {
        }

        public VINNotFoundException(string message) : base(message)
        {
        }
    }



    public class InvalidVINException : Exception
    {
        public InvalidVINException()
        {
        }

        public InvalidVINException(string message) : base(message)
        {
        }
    }

    public class DuplicateVINException : Exception
    {
        public DuplicateVINException()
        {
        }

        public DuplicateVINException(string message) : base(message)
        {
        }
    }

    public class DuplicateLocationException : Exception
    {
        public DuplicateLocationException()
        {
        }

        public DuplicateLocationException(string message) : base(message)
        {
        }
    }

    public class SaveLotFailureException : Exception
    {
        public SaveLotFailureException()
        {
        }
        public SaveLotFailureException(string message) : base(message)
        {
        }
    }
}



