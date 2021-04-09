# Mocking-with-Moq
Mocking with Moq

Otto’s Autos is a used car company in Rancho Cucamonga, CA.  You’re working on a project to help automate inventory at Otto’s used car lot.
You have been given a .cs file named CarsWithPizzazz to act as the software under test.  Your job is to test some methods that are intended to go against a database layer, but the database is not yet available.  Thus, you must prepare a mock to mimic the database access behaviors, and you must write unit tests to validate that each method that relies on data from the database works.

**Use these values to simulate the cars in the database:

****VIN	Year	Make	Model	Location on Lot

01xxxxxxxxxxxxxxx	2008	Cadillac	CTS-V	A5

02xxxxxxxxxxxxxxx	1964	Dodge	Dart	F3

03xxxxxxxxxxxxxxx	1963	Cadillac	Fleetwood	A23

04xxxxxxxxxxxxxxx	1995	Hummer	H1 (Gas)	C7

05xxxxxxxxxxxxxxx	1958	Triumph	TR3	A1

06xxxxxxxxxxxxxxx	1968	Triumph	TR5	A2



**The tests:
**
**Here are the tests:

•	**FindCar**

-	Return instance of Auto when requested car is found.

-	Throw VINNotFoundException when the requested car is not found.

•	**FindCarsByMake**

-	Return the correct number of instances (2) when looking for “Cadillac” as a car make.

-	Return zero when looking for “Audi.”

•	**AddCar**

-	Return properly updated collection when the add succeeds.  To ensure that the collection is properly updated requires two checks:
	The updated collection must have the correct count of items, and
	The last item in the collection must have the VIN associated with the auto to add.
 
-	Throw DuplicateVINException if there’s already a car on the lot with the new auto’s VIN.

-	Throw DuplicateLocationException if there’s already car at the same spot on the lot as the new car.

-	Throw InvalidVINException when the VIN is not exactly 17 characters long (no joke…that’s the actual required length of a Vehicle Identification Number.) 

•	**RemoveCar**

-	Return collection with requested car removed car if the car was initially on the lot.  

-	Throw VINNotFoundException when the car to be removed is not on the lot.
 

