using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Parking
{
    class Program
    {
        static void Main(string[] args)
        {
            Settings.SetSettings(10, null, 0, 0);
            Parking.Initialzie();
            Parking.Instance.Start();
            Parking.Instance.AddCar(40, CarType.Passenger);
            Parking.Instance.AddCar(25, CarType.Truck);
            Parking.Instance.AddCar(10, CarType.Bus);
            Parking.Instance.Show();
            System.Threading.Thread.Sleep(10000);
            Parking.Instance.Show();
            Parking.Instance.End();
            Console.ReadKey();
        }

    }
}
