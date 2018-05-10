using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

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

        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private CancellationToken cancellationToken;
        private Task Logging;
        private Task WorkOfParking;
        private Task InteractionWithUser;

        private FileStream fileStream;
        private StreamReader streamReader;
        private StreamWriter streamWriter;

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
            //fileStream = new FileStream(@"..\Transactions.log", FileMode.Open, FileAccess.ReadWrite);
            //streamReader = new StreamReader(fileStream);
            //streamWriter = new StreamWriter(fileStream);
            Logging = new Task(() => 
            {
                TimerCallback callBack = new TimerCallback(OutputSumOfTransactionsToFile);
                Timer timer = new Timer(callBack, null, 0, 5000);
            });
            WorkOfParking = new Task(() =>
            {
                TimerCallback callBack = new TimerCallback(WithdrawMoneyForParkingPlace);
                Timer timer = new Timer(callBack, null, 0, Settings.TimeOut*1000);
            });
            InteractionWithUser = new Task(() =>
            {
                Menu.Run();
            });
            Logging.Start();
            WorkOfParking.Start();
            InteractionWithUser.Start();
        }
        public void Start()
        {
            if (isInitialized)
            {
                cancellationToken = cancellationTokenSource.Token;
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
                cancellationTokenSource.Cancel();
                Logging.Dispose();
                WorkOfParking.Dispose();
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
        private void WithdrawMoneyForParkingPlace(object obj)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                int valueForWithdraw = 0;
                foreach (var car in Cars)
                {
                    if (car.Balance >= Settings.PricesForParking[car.TypeOfCar])
                    {
                        valueForWithdraw = Settings.PricesForParking[car.TypeOfCar];
                    }
                    else
                    {
                        valueForWithdraw = Settings.PricesForParking[car.TypeOfCar] * Settings.Fine;
                    }
                    car.Withdraw(valueForWithdraw);
                    Transactions.Add(new Transaction(DateTime.Now, car.Id, valueForWithdraw));
                }
            }
            else
            {
                cancellationTokenSource.Cancel();
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
            if (!cancellationToken.IsCancellationRequested)
            {
                int sumOfTransactions = 0;
                foreach (var transaction in Transactions)
                {
                    sumOfTransactions += transaction.WithdrawnFunds;
                }



            }
            else
            {
                cancellationTokenSource.Cancel();
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
