using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking
{
    public enum CarType { Passenger, Truck, Bus, Motorcycle }

    public class Car
    {
        //When we create a new instance of this class, IdGenerator generate unique id.
        private static int IdGenerator { get; set; }

        public int Id { get; } 
        public int Balance { get; set; }
        public CarType TypeOfCar { get; }
        public Car(int balance = 0, CarType typeOfCar = CarType.Passenger)
        {
            Id = ++IdGenerator;
            Balance = balance;
            TypeOfCar = typeOfCar;
        }
        public void Put(int cash)
        {
            Balance += cash;
        }
        public void Withdraw(int cash)
        {
            Balance -= cash;
        }
        public void Show()
        {
            Console.WriteLine($"Type of car: {TypeOfCar}, Id: {Id}, Balance: {Balance}.");
        }
    }
}
