using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace Parking
{
    public enum ResultsOfTheFunction { Succesfull, Unsuccesfull_NotFound, Unsuccesfull_NegativeBalance}

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
        private static object locker = new object();

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
            Settings.Show();
        }

        private void Work()
        {
            Logging = new Task(() => 
            {
                TimerCallback callBack = new TimerCallback(OutputSumOfTransactionsToFile);
                Timer timer = new Timer(callBack, null, 0, 60000);
            });
            WorkOfParking = new Task(() =>
            {
                TimerCallback callBack = new TimerCallback(WithdrawMoneyForParkingPlace);
                Timer timer = new Timer(callBack, null, 0, Settings.TimeOut*1000);
            });
            Logging.Start();
            WorkOfParking.Start();
        }

        //Get started with parking. 
        public void Start()
        {
            if (isInitialized)
            {
                cancellationToken = cancellationTokenSource.Token;
                using (var fileStream = new FileStream(Settings.pathToFile, FileMode.Create))
                {
                    //fileStream created for clearing file "Transactions.log" before start of program.
                    fileStream.Close();
                }
                //It set up multi-threading.
                Work();
            }
            else
            {
                throw new InvalidOperationException("You cant start working! Parking is not initialized!");
            }
        }
        //Finish working with parking
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
            if (car.Balance > 0 && CountOfFreePlaces()!=0)
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
            if (balance > 0 && CountOfFreePlaces()!=0)
            {
                Cars.Add(new Car(balance, carType));
                return true;
            }
            else
            {
                return false;
            }
        }
        public ResultsOfTheFunction TakeCar(int id)
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
            if(toRemove==null)
            {
                return ResultsOfTheFunction.Unsuccesfull_NotFound;
            }
            else
            {
                if(toRemove.Balance >= 0)
                {
                    Cars.Remove(toRemove);
                    return ResultsOfTheFunction.Succesfull;
                }
                else
                {
                    return ResultsOfTheFunction.Unsuccesfull_NegativeBalance;
                }
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

        //Two multi-thread methods. They work in different streams.
        private void WithdrawMoneyForParkingPlace(object obj)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                Logging.Wait();
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
                    Balance += valueForWithdraw;
                }
            }
            else
            {
                cancellationTokenSource.Cancel();
            }
        }
        private void OutputSumOfTransactionsToFile(object obj)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
            WorkOfParking.Wait();
            int sumOfTransactions = 0;
                foreach (var transaction in Transactions)
                {
                sumOfTransactions += transaction.WithdrawnFunds;
                }
                using (var streamWriter = new StreamWriter(Settings.pathToFile, true))
                {
                    streamWriter.WriteLine($"{DateTime.Now} / Sum of transactions: {sumOfTransactions}");
                    streamWriter.Close();
                }
                Transactions.Clear();
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
    }
}
