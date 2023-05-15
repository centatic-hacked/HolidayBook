using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolidayBook.Overview.Classes
{
    public class FlightOfferDetails
    {
        public FlightOfferDetails(string departs_airlines, int departs_duration, string arrives_airlines, int arrives_duration, float price, string currency, Dictionary<DateTime, string> dep_Departures, Dictionary<DateTime, string> dep_Arrivals, Dictionary<DateTime, string> arr_Departures, Dictionary<DateTime, string> arr_Arrivals, int dep_Stops, int arr_Stops)
        {
            Departs_airlines = departs_airlines;
            Departs_duration = departs_duration;
            Arrives_airlines = arrives_airlines;
            Arrives_duration = arrives_duration;
            Price = price;
            Currency = currency;
            Dep_Departures = dep_Departures;
            Dep_Arrivals = dep_Arrivals;
            Arr_Departures = arr_Departures;
            Arr_Arrivals = arr_Arrivals;
            Dep_Stops = dep_Stops;
            Arr_Stops = arr_Stops;
        }

        public string Departs_airlines { get; private set; }

        public int Departs_duration { get; private set; }

        public string Arrives_airlines { get; private set; }

        public int Arrives_duration { get; private set; }

        public float Price { get; private set; }

        public string Currency { get; private set; }

        public Dictionary<DateTime, string> Dep_Departures { get; private set; }

        public Dictionary<DateTime, string> Dep_Arrivals { get; private set; }

        public Dictionary<DateTime, string> Arr_Departures { get; private set; }

        public Dictionary<DateTime, string> Arr_Arrivals { get; private set; }

        public int Dep_Stops { get; private set; }

        public int Arr_Stops { get; private set; }

    }
}
