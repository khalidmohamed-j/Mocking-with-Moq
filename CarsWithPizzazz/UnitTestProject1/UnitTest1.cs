using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using Moq;
using SUT = CarsWithPizzazz;


namespace UnitTestProject1
{
    class SetupMockLot
    {
        //LoadMockLot is used to fake out the LoadLot method in the IAutoDBAccess interface
        //LoadLot itself is called by each of the methods under test
        public static List<SUT.Auto> LoadMockLot()
        {
            var listOfCars = new List<SUT.Auto>
            {
                new SUT.Auto
                {
                    VehicleIdentificationNumber = "01xxxxxxxxxxxxxxx",
                    Year = "2008",
                    Make = "Cadillac",
                    Model = "CTS-V",
                    LocationOnLot = "A5"

                },

                new SUT.Auto
                {
                    VehicleIdentificationNumber = "02xxxxxxxxxxxxxxx",
                    Year = "1964",
                    Make = "Dodge",
                    Model = "Dart",
                    LocationOnLot = "F3"

                },

                new SUT.Auto
                {
                    VehicleIdentificationNumber = "03xxxxxxxxxxxxxxx",
                    Year = "1963",
                    Make = "Cadillac",
                    Model = "Fleetwood",
                    LocationOnLot = "A23"

                },

                new SUT.Auto
                {
                    VehicleIdentificationNumber = "04xxxxxxxxxxxxxxx",
                    Year = "1995",
                    Make = "Hummer",
                    Model = "H1 Gas",
                    LocationOnLot = "C7"

                },

                new SUT.Auto
                {
                    VehicleIdentificationNumber = "05xxxxxxxxxxxxxxx",
                    Year = "1958",
                    Make = "Triumph",
                    Model = "TR3",
                    LocationOnLot = "A1"

                },

                new SUT.Auto
                {
                    VehicleIdentificationNumber = "06xxxxxxxxxxxxxxx",
                    Year = "1968",
                    Make = "Triumph",
                    Model = "TR5",
                    LocationOnLot = "A2"

                }
            };

            return listOfCars;
        }

    }


    [TestClass]
    public class FindCar_Should
    {


        [TestMethod]
        public void ThrowVINNotFoundException_WhenNoCarWithRequestedVIN()
        {
            //Arrange
            SetupMockLot sl = new SetupMockLot();
            Mock<SUT.IAutoDBAccess> mockLot = new Mock<SUT.IAutoDBAccess>();
            mockLot.Setup(x => x.LoadLot()).Returns(SetupMockLot.LoadMockLot());

            SUT.AutoControl carController = new SUT.AutoControl(mockLot.Object);

            //Act
            Action act = () => carController.FindCar("This car does not exist on the lot.");

            //Assert
            act.Should().Throw<SUT.VINNotFoundException>();

        }

        [TestMethod]
        public void ReturnCar_WhenVINisFound()
        {
            //Arrange

            Mock<SUT.IAutoDBAccess> mockLot = new Mock<SUT.IAutoDBAccess>();
            mockLot.Setup(x => x.LoadLot()).Returns(SetupMockLot.LoadMockLot());
            SUT.AutoControl carController = new SUT.AutoControl(mockLot.Object);
            string expectedLocation = "A23";

            //Act
            SUT.Auto result = carController.FindCar("03xxxxxxxxxxxxxxx");

            //Assert
            result.LocationOnLot.Should().Be(expectedLocation);

        }
    }

    [TestClass]
    public class FindCarsByMake_Should
    {
        [TestMethod]
        public void ReturnTwo_WhenCadillacRequested()
        {
            //Arrange

            Mock<SUT.IAutoDBAccess> mockLot = new Mock<SUT.IAutoDBAccess>();
            mockLot.Setup(x => x.LoadLot()).Returns(SetupMockLot.LoadMockLot());
            var carController = new SUT.AutoControl(mockLot.Object);
            int expectedCount = 2;

            //Act
            List<SUT.Auto> result = carController.FindCarsByMake("Cadillac");

            //Assert
            result.Count.Should().Be(expectedCount);
        }

        [TestMethod]
        public void ReturnZero_WhenAudiRequested()
        {
            //Arrange

            Mock<SUT.IAutoDBAccess> mockLot = new Mock<SUT.IAutoDBAccess>();
            mockLot.Setup(x => x.LoadLot()).Returns(SetupMockLot.LoadMockLot());
            var carController = new SUT.AutoControl(mockLot.Object);
            int expectedCount = 0;


            //Act
            List<SUT.Auto> result = carController.FindCarsByMake("Audi");

            //Assert
            result.Count.Should().Be(expectedCount);
        }

    }

    [TestClass]
    public class AddCar_Should
    {
        [TestMethod]
        public void ReturnProperlyUpdatedCollection_WhenAddSucceeds()
        {
            //Arrange

            List<SUT.Auto> result = new List<SUT.Auto>();
            Mock<SUT.IAutoDBAccess> mockLot = new Mock<SUT.IAutoDBAccess>();
            mockLot.Setup(x => x.LoadLot()).Returns(SetupMockLot.LoadMockLot());
            mockLot.Setup(x => x.SaveLot(null)).Returns(true);  //Causes SaveLot to throw, as does leaving the SaveLot setup out entirely
            mockLot.Setup(x => x.SaveLot(It.IsAny<List<SUT.Auto>>())).Returns(true);  //Causes SaveLot to return true regardless of the input

            int expectedCount = 7;
            string expectedVIN = "07xxxxxxxxxxxxxxx";

            var carController = new SUT.AutoControl(mockLot.Object);

            //Act
            result = carController.AddCar(new SUT.Auto
            {
                VehicleIdentificationNumber = "07xxxxxxxxxxxxxxx",
                Year = "2000",
                Make = "Dodge",
                Model = "Ram",
                LocationOnLot = "A7"

            });

            //Assert
            Assert.AreEqual(expectedCount, result.Count);
            Assert.AreEqual(expectedVIN, result[expectedCount - 1].VehicleIdentificationNumber);
        }

        [TestMethod]
        public void ThrowDuplicateVINException_WhenDupVINFound()
        {
            //Arrange

            List<SUT.Auto> result = new List<SUT.Auto>();
            Mock<SUT.IAutoDBAccess> mockLot = new Mock<SUT.IAutoDBAccess>();
            mockLot.Setup(x => x.LoadLot()).Returns(SetupMockLot.LoadMockLot());

            var carController = new SUT.AutoControl(mockLot.Object);

            //Act
            Action act = () => carController.AddCar(new SUT.Auto
            {
                VehicleIdentificationNumber = "04xxxxxxxxxxxxxxx",
                Year = "1995",
                Make = "Hummer",
                Model = "H1 Gas",
                LocationOnLot = "C7"

            });

            //Assert
            act.Should().Throw<SUT.DuplicateVINException>();

        }

        [TestMethod]
        public void ThrowDuplicateLocationException_WhenLocationIsOccupied()
        {
            //Arrange

            List<SUT.Auto> result = new List<SUT.Auto>();
            Mock<SUT.IAutoDBAccess> mockLot = new Mock<SUT.IAutoDBAccess>();
            mockLot.Setup(x => x.LoadLot()).Returns(SetupMockLot.LoadMockLot());

            var carController = new SUT.AutoControl(mockLot.Object);

            //Act
            Action act = () => carController.AddCar(new SUT.Auto
            {
                VehicleIdentificationNumber = "99xxxxxxxxxxxxxxx",
                Year = "1964",
                Make = "Sunbeam",
                Model = "Alpine",
                LocationOnLot = "C7"

            });

            //Assert
            act.Should().Throw<SUT.DuplicateLocationException>();

        }

        [TestMethod]
        public void ThrowInvalidVINException_WhenWrongLengthVIN()
        {
            //Arrange

            List<SUT.Auto> result = new List<SUT.Auto>();
            Mock<SUT.IAutoDBAccess> mockLot = new Mock<SUT.IAutoDBAccess>();
            mockLot.Setup(x => x.LoadLot()).Returns(SetupMockLot.LoadMockLot());

            var carController = new SUT.AutoControl(mockLot.Object);

            //Act
            Action act = () => carController.AddCar(new SUT.Auto
            {
                VehicleIdentificationNumber = "99",
                Year = "1964",
                Make = "Sunbeam",
                Model = "Alpine",
                LocationOnLot = "B7"

            });

            //Assert
            act.Should().Throw<SUT.InvalidVINException>();
        }
    }

    [TestClass]
    public class RemoveCar_Should
    {
        [TestMethod]
        public void ReturnCollectionWithoutRemovedCar_WhenRemoveSuccessful()
        {
            //Arrange

            List<SUT.Auto> result = new List<SUT.Auto>();
            Mock<SUT.IAutoDBAccess> mockLot = new Mock<SUT.IAutoDBAccess>();
            mockLot.Setup(x => x.LoadLot()).Returns(SetupMockLot.LoadMockLot());
            mockLot.Setup(x => x.SaveLot(It.IsAny<List<SUT.Auto>>())).Returns(true);  //Causes SaveLot to return true regardless of the input

            string vinToRemove = "02xxxxxxxxxxxxxxx";

            var carController = new SUT.AutoControl(mockLot.Object);

            //Act
            result = carController.RemoveCar(vinToRemove);

            //Assert
            Action act = () => carController.FindCar(vinToRemove);  //Must trust that FindCar works :-)
            act.Should().Throw<SUT.VINNotFoundException>();
        }

        [TestMethod]
        public void ThrowVINNotFound_WhenVINNotInCollection()
        {
            //Arrange

            List<SUT.Auto> result = new List<SUT.Auto>();
            Mock<SUT.IAutoDBAccess> mockLot = new Mock<SUT.IAutoDBAccess>();
            mockLot.Setup(x => x.LoadLot()).Returns(SetupMockLot.LoadMockLot());
            string vinToRemove = "99xxxxxxxxxxxxxxx";

            var carController = new SUT.AutoControl(mockLot.Object);

            //Act
            Action act = () => carController.RemoveCar(vinToRemove);

            //Assert
            act.Should().Throw<SUT.VINNotFoundException>();
        }
    }
}


