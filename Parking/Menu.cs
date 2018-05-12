using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace Parking
{
    public static class Menu
    {
        private static int timeOut = 3;
        private static Dictionary<CarType, int> pricesForParking = new Dictionary<CarType, int>
            {
                { CarType.Bus, 2 },
                { CarType.Truck, 5 },
                { CarType.Passenger, 3 },
                { CarType.Motorcycle, 1 }
            };
        private static int parkingSpace = 30;
        private static int fine = 5;
        //Initialize settings and create parking.
        public static void Start()
        {
            bool actions = true;
            int action = 0;
            bool choise = true;
            Console.WriteLine("Hi! It`s program which simulate work of parking... To open main menu, press any button:");
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine("To start work with parking, please initialize Settings and Parking. Press any key, to start...");
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine("-----Initializing settings-----");
            Console.WriteLine("Make choise to set settings. If you do not choose some field it will accept the default value.");
            while (actions)
            {
                Console.WriteLine("1) Set timeout.");
                Console.WriteLine("2) Set prices for parking.");
                Console.WriteLine("3) Set parking space.");
                Console.WriteLine("4) Set fine.");
                Console.WriteLine("5) Show default values.");
                Console.WriteLine("6) Complete the settings.\n");
                Console.WriteLine("Enter your choise: ");
                action = InputValue();
                Console.Clear();
                switch(action)
                {
                    case 1:
                        Console.WriteLine("Enter timeout:");
                        timeOut = InputValue();
                        Console.WriteLine("Done!");
                        Console.ReadKey();
                        break;
                    case 2:
                        Console.WriteLine("Enter prices for parking through spaces (Bus, Truck, Passenger, Motorcycle)");
                        while (choise)
                        {
                            string[] data = Console.ReadLine().Split();
                            try
                            {
                                pricesForParking[CarType.Bus] = int.Parse(data[0]);
                                pricesForParking[CarType.Truck] = int.Parse(data[1]);
                                pricesForParking[CarType.Passenger] = int.Parse(data[2]);
                                pricesForParking[CarType.Motorcycle] = int.Parse(data[3]);
                                choise = false;
                            }
                            catch
                            {
                                Console.WriteLine("Error! Can`t read your choise! Please, try again!");
                            }
                        }
                        Console.WriteLine("Done!");
                        Console.ReadKey();
                        break;
                    case 3:
                        Console.WriteLine("Enter parking space:");
                        parkingSpace = InputValue();
                        Console.WriteLine("Done!");
                        Console.ReadKey();
                        break;
                    case 4:
                        Console.WriteLine("Enter fine:");
                        fine = InputValue();
                        Console.WriteLine("Done!");
                        Console.ReadKey();
                        break;
                    case 5:
                        Settings.Show();
                        Console.ReadKey();
                        break;
                    case 6:
                        Settings.SetSettings(timeOut, pricesForParking, parkingSpace, fine);
                        Console.WriteLine("Settings sets succesfull!");
                        Console.ReadKey();
                        actions = false;
                        break;
                }
                Console.Clear();
            }
            Parking.Initialzie();
            Console.WriteLine("To start work with parking press any button...");
            Console.ReadKey();
            Console.Clear();
            Parking.Instance.Start();
        }

        //Use for reading integer value by Console
        private static int InputValue()
        {
            bool choise = true;
            int value = 0;
            while (choise)
            {
                try
                {
                    value = int.Parse(Console.ReadLine());
                    choise = false;
                }
                catch
                {
                    Console.WriteLine("Error! Can`t read your choise! Please, try again!");
                }
            }
            return value;
        }

        //User-interface.
        public static void Run()
        {
            bool actions = true;
            int action = 0;
            while (actions)
            {
                Console.Clear();
                Console.WriteLine("Parking:");
                Console.WriteLine("1) Add car.");
                Console.WriteLine("2) Remove car.");
                Console.WriteLine("3) Refill balance of car.");
                Console.WriteLine("4) Show parking income for the last minute.");
                Console.WriteLine("5) Show Total parking income.");
                Console.WriteLine("6) Show history of transactions for the last minute.");
                Console.WriteLine("7) Show history of transactions for all time.");
                Console.WriteLine("8) Show free places.");
                Console.WriteLine("9) Show occupied places.");
                Console.WriteLine("10) Show parking settings.");
                Console.WriteLine("11) Show parking info.");
                Console.WriteLine("12) Exit.\n");
                Console.WriteLine("Enter your action:");
                action = InputValue();
                switch (action)
                {
                    case 1:
                        Console.WriteLine("Enter balance and number type of car through spaces (Bus - 0, Truck - 1, Passenger - 2, Motorcycle - 3)");
                        string[] data = Console.ReadLine().Split();
                        try
                        {
                            bool isSuccesfullAdded = Parking.Instance.AddCar(int.Parse(data[0]), (CarType)int.Parse(data[1]));
                            if (isSuccesfullAdded)
                            {
                                Console.WriteLine($"Car added succesfull! Id of this car - {Parking.Instance.Cars.Last().Id}");
                            }
                            else
                            {
                                if(int.Parse(data[0])<=0)
                                {
                                    Console.WriteLine("Sorry, cant add car to parking! Your balance is negative!");
                                }
                                else
                                {
                                    Console.WriteLine("Sorry, cant add car to parking! The parking hasn`t free places!");
                                }
                            }
                            Console.ReadKey();
                        }
                        catch
                        {
                            Console.WriteLine("Error! Invalid input data! Moving to main menu...");
                            Console.ReadKey();
                        }
                        break;
                    case 2:
                        Console.WriteLine("Enter id of car:");
                        int id = InputValue();
                        ResultsOfTheFunction isSuccesfull = Parking.Instance.TakeCar(id);
                        if(isSuccesfull == ResultsOfTheFunction.Succesfull)
                        {
                            Console.WriteLine("Car took succesfull!");
                        }
                        else
                        {
                            if (isSuccesfull == ResultsOfTheFunction.Unsuccesfull_NegativeBalance)
                            {
                                Console.WriteLine("Sorry, but you cant take your car! Please, fill your balance!");
                            }
                            else
                            {
                                Console.WriteLine("The car is absent in the parking.");
                            }
                        }
                        Console.ReadKey();
                        break;
                    case 3:
                        Console.WriteLine("Enter id of car and sum of money through spaces:");
                        string[] putData = Console.ReadLine().Split();
                        try
                        {
                            Parking.Instance.RefillBalance(int.Parse(putData[0]), int.Parse(putData[1]));
                            Console.WriteLine("Refilling is succesfull!");
                            Console.ReadLine();
                        }
                        catch
                        {
                            Console.WriteLine("Error! Invalid input data! Moving to main menu...");
                            Console.ReadKey();
                        }
                        break;
                    case 4:
                        Console.WriteLine($"Income for the last minute: {Parking.Instance.GetEarnedMoneyAtTheLastMinute()}");
                        Console.ReadKey();
                        break;
                    case 5:
                        Console.WriteLine($"Total parking income: {Parking.Instance.CurrentBalance()}");
                        Console.ReadKey();
                        break;
                    case 6:
                        foreach (var transaction in Parking.Instance.Transactions)
                        {
                            transaction.Show();
                        }
                        Console.ReadKey();
                        break;
                    case 7:
                        Console.WriteLine("All transactions:\n");
                        using (var streamReader = new StreamReader(Settings.pathToFile))
                        {
                            string transaction = streamReader.ReadLine();
                            while (transaction != null)
                            {
                                Console.WriteLine($"{transaction}");
                                transaction = streamReader.ReadLine();
                            }
                        }
                        Console.ReadKey();
                        break;
                    case 8:
                        Console.WriteLine($"Free places - {Parking.Instance.CountOfFreePlaces()}");
                        Console.ReadKey();
                        break;
                    case 9:
                        Console.WriteLine($"Occupied places - {Parking.Instance.CountOfOccupiedPlaces()}");
                        Console.ReadKey();
                        break;
                    case 10:
                        Settings.Show();
                        Console.ReadKey();
                        break;
                    case 11:
                        Console.WriteLine("\n-----Parking-----\n");
                        Console.WriteLine("Cars:");
                        foreach (var car in Parking.Instance.Cars)
                        {
                            Console.Write(".\t");
                            car.Show();
                        }
                        Console.WriteLine("");
                        Console.ReadKey();
                        break;
                    case 12:
                        Parking.Instance.End();
                        Console.WriteLine("Good bye!");
                        return;
                }
            }
        }
    }
}
