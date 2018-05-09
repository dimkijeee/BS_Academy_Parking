using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Parking
{
    public class Parking
    {
        public List<Car> Cars { get; set; }
        public List<Transaction> Transactions { get; set; }
        public int Balance { get; private set; }

        private static readonly Lazy<Parking> lazy = new Lazy<Parking>(() => new Parking());
        public static Parking Instance { get { return lazy.Value; } }

        private static bool isInitialized = false;
        private bool isRunning = false;
        Task Logging;
        Task WorkOfParking;

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

        private void Work()
        {
            Logging = new Task(() => 
            {
                TimerCallback callBack = new TimerCallback(OutputSumOfTransactionsToFile);
                Timer timer = new Timer(callBack, null, 0, 10000);
            });
            WorkOfParking = new Task(() =>
            {
                TimerCallback callBack = new TimerCallback(WithdrawMoneyForParkingPlace);
                Timer timer = new Timer(callBack, null, 0, Settings.TimeOut*1000);
            });
            Logging.Start();
            WorkOfParking.Start();
            //Task InteractionWithUser = new Task(() =>
            //{
            //    Menu.Run();
            //});

        }
        public void Start()
        {
            if (isInitialized)
            {
                isRunning = true;
                Work();
            }
            else
            {
                throw new InvalidOperationException("You cant start working! Parking is not initialized!");
            }
        }
        public void End()
        {
            if (isInitialized)
            {
                isRunning = false;
            }
            else
            {
                throw new InvalidOperationException("You cant stop working! Parking is not initialized!");
            }
        }
        public bool AddCar(Car car)
        {
            if (car.Balance > 0)
            {
                Cars.Add(car);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool AddCar(int balance, CarType carType)
        {
            if (balance > 0)
            {
                Cars.Add(new Car(balance, carType));
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool TakeCar(int id)
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
            if(toRemove!=null && toRemove.Balance>=0)
            {
                Cars.Remove(toRemove);
                return true;
            }
            else
            {
                return false;
            }
        }
        //Refills balance for car with Id - id. If succesfull - returns true, else - false.
        public bool RefillBalance(int id, int sum)
        {
            foreach(Car car in Cars)
            {
                if(car.Id==id)
                {
                    car.Put(sum);
                    return true;
                }
            }
            return false;
        }
        public void WithdrawMoneyForParkingPlace(object obj)
        {
            foreach (var car in Cars)
            {
                if (car.Balance >= Settings.PricesForParking[car.TypeOfCar])
                {
                    car.Withdraw(Settings.PricesForParking[car.TypeOfCar]);
                }
                else
                {
                    car.Withdraw(Settings.PricesForParking[car.TypeOfCar] * Settings.Fine);
                }
            }
            if(isRunning==false)
            {
                WorkOfParking.Wait();
            }
        }
        public int GetEarnedMoneyAtTheLastMinute()
        {
            int sum = 0;
            foreach(var transaction in Transactions)
            {
                sum += transaction.WithdrawnFunds;
            }
            return sum;
        }
        public int CurrentBalance() => Balance;
        public int CountOfFreePlaces() => Settings.ParkingSpace - Cars.Count;
        public int CountOfOccupiedPlaces() => Cars.Count;

        private void OutputSumOfTransactionsToFile(object obj)
        {
            Console.WriteLine("Temporary method.");
            if(isRunning==false)
            {
                Logging.Wait();
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
        public void ShowTransactionsAtTheLastMinute()
        {
            
        }
        public void ShowAllTransactions()
        {

        }
    }
}
