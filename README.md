## BS_Academy_Parking
*Project for BS-Academy*

**A little documentation to my project.**

Program has all functionality that was needed in task.

**Class car.**
Class Car has neccesary properties and methods. In file Car.cs declared enum CarType.
**Class Transaction.**
Class Transaction is simple class which save information about transaction.
**Class Settings.**
Class Settings contains the most neccesary information about parking. 
It`s thread-safe static class (using construction lock).
Settings has method SetSettings(...) to set up info about parking. In code described how it works.
**Class Parking.**
Class Parking use programing-pattern singleton. Work of parking is multi-thread. It contains 
fields, properties and methods which are neccesary in task. Before first use we need to initialize
parking by using special method - Initialize. During execution parking use 3 threads - Logging(Work with file "Transactions.log"), 
WorkOfParking(Withdraw money for parking), Main thread - for interaction with user.
**Class Menu.**
Class Menu created as user-interface. Encapsulates work of parking.
