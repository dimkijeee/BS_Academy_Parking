using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Parking
{
    public class Transaction
    {
        public DateTime TimeOfTransaction { get; set; }
        public int CarId { get; set; }
        public int WithdrawnFunds { get; set; }
        public Transaction(DateTime timeOfTransaction, int carId, int withdrawnFunds)
        {
            TimeOfTransaction = timeOfTransaction;
            CarId = carId;
            WithdrawnFunds = withdrawnFunds;
        }
        public void Show()
        {
            Console.WriteLine($"\tTransaction: {TimeOfTransaction} / Car: {CarId} / Withdrawn funds: {WithdrawnFunds}");
        }
    }
}
