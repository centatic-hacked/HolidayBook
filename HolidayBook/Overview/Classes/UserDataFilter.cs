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
            MainViewModel.PR = this;
        }

        public MainViewModel MW { get; private set; }


        public HolidayBookContext DB { get; set; }

        private List<FlightOffersDB> flights;

        public List<FlightOffersDB> Flights
        {
            get { return flights; }
            set
            {
                flights = value;
                Airlines.Clear();
                if (flights.Count() != 0)
                {
                    LowestPrice = Flights.Min(flightDat => flightDat.Price);
                    OneLowestPrice = calculateLowesPriceFromOne();
                    MinimalTime = lowestHours();
                    MaximalTime = highestHours();
                    AmountOfOneFlights = avalFlights();
                    calculateAirlinesWithAvalaibility();
                    MW.MinimalPriceAll = "";
                    MW.MinimalPriceMax1 = "";
                }
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

        public IDictionary<int, int> startTimes { get; private set; }
    }
}
