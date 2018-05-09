using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Parking
{
    class Program
    {
        static void Main(string[] args)
        {
            Settings.SetSettings(5, null, 20, 4);
            Parking.Initialzie();
            Console.ReadKey();
            Parking.Instance.AddCar(100, CarType.Bus);
            Parking.Instance.AddCar(300, CarType.Motorcycle);
            Parking.Instance.AddCar(200, CarType.Passenger);
            Parking.Instance.Show();
            Console.ReadKey();
            Parking.Instance.RemoveCar(2);
            Parking.Instance.RemoveCar(3);
            Parking.Instance.Show();
            Console.ReadKey();
        }
    }
}
