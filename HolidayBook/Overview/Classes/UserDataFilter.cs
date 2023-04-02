using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HolidayBook.ViewModels;
using Model.Infrastructure;
using Model.Model;
using Nancy.Extensions;
using SQLitePCL;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace HolidayBook.Overview.Classes
{
    public partial class PreparedDataFilter
    {
        public PreparedDataFilter(HolidayBookContext db, MainViewModel mw)
        {
            MW = mw;
            DB = db;
            Flights = DB.Flights.ToList();
            LowestPrice = Flights.Min(flightDat => flightDat.Price);
            MinimalTime = lowestHours();
            MaximalTime = highestHours();
            AmountOfOneFlights = avalFlights();
            calculateAirlinesWithAvalaibility();
        }

        public MainViewModel MW { get; private set; }


        public HolidayBookContext DB { get; set; }

        private List<FlightOffersDB> flights;

        public List<FlightOffersDB> Flights
        {
            get { return flights; }
            set { 
                flights = value; 
                LowestPrice = Flights.Min(flightDat => flightDat.Price);
                OneLowestPrice = calculateLowesPriceFromOne();
                MinimalTime = lowestHours();
                MaximalTime = highestHours();
                AmountOfOneFlights = avalFlights();
                calculateAirlinesWithAvalaibility();
                MainViewModel.PR = this;
                MW.MinimalPriceAll = "";
                MW.MinimalPriceMax1 = "";
                List<ExcludeAirlinesList> ls = new List<ExcludeAirlinesList>();
                foreach(string airline in Airlines.Keys)
                {
                   ls.Add(new ExcludeAirlinesList(airline, Airlines[airline]));
                }
                MW.AirlinesList = ls;
            }
        }


        public float LowestPrice { get; private set; }

        public float OneLowestPrice { get; private set; }

        public int MinimalTime { get; private set; }

        public int MaximalTime { get; private set; }

        public int AmountOfOneFlights { get; private set; }

        public Dictionary<string, int> Airlines { get; private set; } = new();

        public int dep_wishedDep { get; private set; }

        public int dep_wishedArr { get; private set; }

        public int arr_wishedDep { get; private set; }

        public int arr_wishedDArr { get; private set; }

        public IDictionary<int, int> startTimes {get; private set; }

        public void AllFlights(bool option)
        {
            if(option == true)
            {
                var allFlights = DB.Flights.ToList();
                IEnumerable<FlightOffersDB> newList;
                newList = allFlights.Where(flightDat => Airlines.ContainsKey(flightDat.Arrives_airlines) 
                && Airlines.ContainsKey(flightDat.Departs_airlines) &&  flightDat.Arrives_duration < MaximalTime && flightDat.Departs_duration < MaximalTime);
                for(int i = 0; i < startTimes.Count; i++)
                {
                    switch(i)
                    {
                        case 0:
                              newList = getNumberOfFlightsFromArrInTime(startTimes[0], newList);
                            break;
                        case 1:
                            newList = getNumberOfFlightsToArrInTime(startTimes[1], newList);
                            break;
                        case 2:
                            newList = getNumberOfFlightsFromDepInTime(startTimes[2], newList);
                            break;
                        case 3:
                            newList = getNumberOfFlightsToDepInTime(startTimes[3], newList);
                            break;
                    }
                }
            }
        }
    }
}
