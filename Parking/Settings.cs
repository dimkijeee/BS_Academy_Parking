using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking
{

    //Threat-safe class.
    public static class Settings
    {
        private static object locker = new object();
        private static bool isInitialized = false;  //This field means that we didn`t use method SetSettings, 
        //and all properties and fields initialized by default.

        private static int timeOut = 3;
        private static Dictionary<CarType, int> pricesForParking = new Dictionary<CarType, int>
        {
            { CarType.Bus, 5 },
            { CarType.Truck, 4 },
            { CarType.Passenger, 3 },
            { CarType.Motorcycle, 2 }
        };
        private static int parkingSpace = 30;
        private static double fine = 5;

        public static int TimeOut
        {
            get
            {
                lock (locker)
                {
                    return timeOut;
                }
            }
            private set
            {
                timeOut = value;
            }
        }
        public static Dictionary<CarType, int> PricesForParking
        {
            get
            {
                lock (locker)
                {
                    return pricesForParking;
                }
            }
            private set
            {
                pricesForParking = value;
            }
        }
        public static int ParkingSpace
        {
            get
            {
                lock (locker)
                {
                    return parkingSpace;
                }
            }
            private set
            {
                parkingSpace = value;
            }
        }
        public static double Fine
        {
            get
            {
                lock (locker)
                {
                    return fine;
                }
            }
            private set
            {
                fine = value;
            }
        }

        //This method works like constructor. We can use this method just once. 
        //If settings changed method returns true, else - false.
        public static bool SetSettings(int _timeOut, Dictionary<CarType, int> _pricesForParking,
            int _parkingSpace, double _fine)
        {
            //If method gets values 0 or null, fields set as default values for parking.
            if (isInitialized == false)
            {
                if (_timeOut != 0)
                {
                    timeOut = _timeOut;
                }
                if (_pricesForParking != null)
                {
                    pricesForParking = _pricesForParking;
                }
                if (_parkingSpace != 0)
                {
                    parkingSpace = _parkingSpace;
                }

                if (_fine != 0)
                {
                    fine = _fine;
                }
                isInitialized = true;
                return isInitialized;
            }
            else
            {
                return false;
            }
        }
        public static void Show()
        {
            Console.WriteLine("Settings:");
            Console.WriteLine($"Time out: {TimeOut}");
            Console.Write("Prices for parking: ");
            foreach(var a in PricesForParking)
            {
                Console.Write($"{a}. ");
            }
            Console.WriteLine("");
            Console.WriteLine($"Parking space: {ParkingSpace}");
            Console.WriteLine($"Fine: {Fine}");
        }
    }
}
