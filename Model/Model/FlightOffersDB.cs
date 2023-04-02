using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Model
{
    public class FlightOffersDB
    {
        protected FlightOffersDB() { }

        public FlightOffersDB(string departs_airlines, int departs_duration, string arrives_airlines, float price, string currency, 
            //List<DateTime> dep_Departures, List<DateTime> dep_Arrivals, List<DateTime> arr_Departures, List<DateTime> arr_Arrivals,
            int dep_Stops, int arr_Stops)
        {
            Departs_airlines = departs_airlines;
            Departs_duration = departs_duration;
            Arrives_airlines = arrives_airlines;
            Price = price;
            Currency = currency;
            //flToDep = dep_Departures;
            //flToArr = dep_Arrivals;
            //flBackDep = arr_Departures;
            //flBackArr = arr_Arrivals;
            Dep_Stops = dep_Stops;
            Arr_Stops = arr_Stops;
        }

        public int Id { get; private set; }

        public string Departs_airlines { get; private set; } = string.Empty;

        public int Departs_duration { get; private set; }

        public string Arrives_airlines { get; private set; } = string.Empty;

        public int Arrives_duration { get; private set; }

        public float Price { get; private set; }

        public string Currency { get; private set; } = string.Empty;

        //public List<DateTime> Dep_Departures { get; private set; } = new List<DateTime> ();

        ////public List<DateTime> Dep_Arrivals { get; private set; } = default!;

        ////public List<DateTime> Arr_Departures { get; private set; } = default!;

        ////public List<DateTime> Arr_Arrivals { get; private set; } = default!;

        public int Dep_Stops { get; private set; }

        public int Arr_Stops { get; private set; }

        //private List<FlightToDep> flToDep;

        //public IReadOnlyList<FlightToDep> flightToDeps => flToDep;

        //private List<FlightToArr> flToArr;

        //public IReadOnlyList<FlightToArr> flightToArrs => flToArr;

        //private List<FlightBackDep> flBackDep;

        //public IReadOnlyList<FlightBackDep> flightBackDeps => flBackDep;

        //private List<FlightBackArr> flBackArr;

        //public IReadOnlyList<FlightBackArr> flightBackArrs => flBackArr;
    }
}
