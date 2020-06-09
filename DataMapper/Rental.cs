using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMapper
{
    class Rental
    {
        public int ID { get; private set; }
        public int CopyID { get; private set; }
        public int ClientID { get; private set; }
        public DateTime DateOfRental { get; private set; }
        public DateTime DateOfReturn { get; private set; }
        public Rental (int id, int copyid, int clientid, DateTime dateofrental, DateTime dateofreturn)
        {
            ID = id;
            CopyID = copyid;
            ClientID = clientid;
            DateOfRental = dateofrental;
            DateOfReturn = dateofreturn;
        }
        public override string ToString()
        {
            return $"Rental {ID}: Copy of {CopyID} ordered by client {ClientID} rented from {DateOfRental} till {DateOfReturn}";
        }
    }
}
