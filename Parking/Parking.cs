using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking
{
    public class Parking
    {
        public List<Car> Cars { get; set; }
        public List<Transaction> Transactions { get; set; }
        public int Balance { get; set; }

        private static readonly Lazy<Parking> lazy = new Lazy<Parking>(() => new Parking());
        public static Parking Instance { get { return lazy.Value; } }
        private static bool isInitialized = false;

        //Initialize object (Made for convenience). This method using when we creating Parking.
        public static bool Initialzie()
        {
            if (isInitialized == false)
            {
                Instance.Equals(new object());
                return true;
            }
            else
            {
                return false;
            }
        }
        private Parking()
        {
            isInitialized = true;
            Cars = new List<Car>(Settings.ParkingSpace);
            Transactions = new List<Transaction>();
            Balance = 0;
            Console.WriteLine("Parking created succesfull.");
            Settings.Show();
        }
        public void AddCar(Car car)
        {
            Cars.Add(car);
        }
        public void AddCar(int balance, CarType carType)
        {
            Car toAdd = new Car(balance, carType);
            Cars.Add(toAdd);
        }
        public void RemoveCar(int id)
        {
            Car toRemove = null;
            foreach(var car in Cars)
            {
                if(car.Id==id)
                {
                    toRemove = car;
                    break;
                }
            }
            if(toRemove!=null)
            {
                Cars.Remove(toRemove);
            }
        }
        public void Show()
        {
            Console.WriteLine("\n-----Parking-----\n");
            Console.WriteLine("Cars:");
            foreach (var car in Cars)
            {
                Console.Write(".\t");
                car.Show();
            }
            Console.WriteLine("");
            Console.WriteLine("Transactions:");
            foreach(var transaction in Transactions)
            {
                Console.Write(".\t");
                transaction.Show();
            }
            Console.WriteLine("");
            Console.WriteLine($"Balance: {Balance}.\n");
        }
        public void ShowSettings()
        {
            Settings.Show();
        }
    }
}
