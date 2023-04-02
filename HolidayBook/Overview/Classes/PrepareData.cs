using Bogus.DataSets;
using Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace HolidayBook.Overview.Classes
{
    partial class PreparedDataFilter
    {

        private void calculateAirlinesWithAvalaibility()
        {
            foreach (var flight in Flights)
            {
                bool arrMore = false;
                bool depMore = false;
                if (flight.Arrives_airlines.Contains(","))
                {
                    arrMore = true;
                    foreach (string name in flight.Arrives_airlines.Split(','))
                    {
                        string scName = name.Trim();
                        createListOfAirlines(scName);
                    }
                }
                if (flight.Departs_airlines.Contains(","))
                {
                    depMore = true;
                    foreach (string name in flight.Arrives_airlines.Split(','))
                    {
                        string scName = name.Trim();
                        createListOfAirlines(scName);
                    }
                }
                if (arrMore ^ depMore)
                {
                    switch (arrMore)
                    {
                        case true:
                            createListOfAirlines(flight.Departs_airlines);
                            break;
                        case false:
                            createListOfAirlines(flight.Arrives_airlines);
                            break;
                    }
                }
                else if (!(arrMore && depMore))
                {
                    createListOfAirlines(flight.Departs_airlines);
                    createListOfAirlines(flight.Arrives_airlines);
                }
            }
        }

        private void createListOfAirlines(string name)
        {
            if (!Airlines.ContainsKey(name))
            {
                Airlines.Add(name, Flights.Where(flightDat => flightDat.Arrives_airlines.Contains(name) || flightDat.Departs_airlines.Contains(name)).Count());
            }
        }

        private float calculateLowesPriceFromOne()
        {
            List<FlightOffersDB> fl = Flights.Where(flightDat => flightDat.Arr_Stops <= 1 && flightDat.Dep_Stops <= 1).ToList();
            if (fl.Count() == 0)
            {
                return 0;
            }
            return fl.Min(flightDat => flightDat.Price);
        }

        private int lowestHours()
        {
            return Math.Max(Flights.Min(flightDat => flightDat.Departs_duration), Flights.Min(flightDat => flightDat.Arrives_duration));
        }

        private int highestHours()
        {
            return Math.Max(Flights.Max(flightDat => flightDat.Departs_duration), Flights.Max(flightDat => flightDat.Arrives_duration));
        }

        private int avalFlights()
        {
            return Flights.Where(flightDat => flightDat.Arr_Stops <= 1 && flightDat.Dep_Stops <= 1).Count();
        }


        public IEnumerable<FlightOffersDB> getNumberOfFlightsFromArrInTime(int timeBegin, IEnumerable<FlightOffersDB> filteredList)
        {
            IEnumerable<FlightOffersDB> query = default!;
            timeBegin = timeBegin / 6;
            if (timeBegin == 4)
            {
                timeBegin = 0;
            }
            switch (timeBegin)
            {
                case 0:
                    query = (from Flight in filteredList
                             join date in DB.FlightsBackArr on new
                             {
                                 Flight.Id,
                                 TrueData = true
                             }
                             equals new
                             {
                                 Id = date.FlightOfferNavigationId,
                                 TrueData = (0 < int.Parse(date.Dt.ToString("H")) && int.Parse(date.Dt.ToString("H")) < 6) ? true : false
                             }
                             select Flight).ToList();
                    return query;
                case 1:
                    query = (from Flight in filteredList
                             join date in DB.FlightsBackArr on new
                             {
                                 Flight.Id,
                                 TrueData = true
                             }
                             equals new
                             {
                                 Id = date.FlightOfferNavigationId,
                                 TrueData = (6 < int.Parse(date.Dt.ToString("H")) && int.Parse(date.Dt.ToString("H")) < 12) ? true : false
                             }
                             select Flight).ToList();
                    return query;
                case 2:
                    query = (from Flight in filteredList
                             join date in DB.FlightsBackArr on new
                             {
                                 Flight.Id,
                                 TrueData = true
                             }
                             equals new
                             {
                                 Id = date.FlightOfferNavigationId,
                                 TrueData = (12 < int.Parse(date.Dt.ToString("H")) && int.Parse(date.Dt.ToString("H")) < 18) ? true : false
                             }
                             select Flight).ToList();
                    return query;
                case 3:
                    query = (from Flight in filteredList
                             join date in DB.FlightsBackArr on new
                             {
                                 Flight.Id,
                                 TrueData = true
                             }
                             equals new
                             {
                                 Id = date.FlightOfferNavigationId,
                                 TrueData = (18 < int.Parse(date.Dt.ToString("H")) && int.Parse(date.Dt.ToString("H")) < 24) ? true : false
                             }
                             select Flight).ToList();
                    return query;
            }
            return default!;
        }

        public IEnumerable<FlightOffersDB> getNumberOfFlightsToDepInTime(int timeBegin, IEnumerable<FlightOffersDB> filteredList)
        {
            IEnumerable<FlightOffersDB> query = default!;
            timeBegin = timeBegin / 6;
            if (timeBegin == 4)
            {
                timeBegin = 0;
            }
            switch (timeBegin)
            {
                case 0:
                    query = (from Flight in filteredList
                             join date in DB.FlightsToDep on new
                             {
                                 Flight.Id,
                                 TrueData = true
                             }
                             equals new
                             {
                                 Id = date.FlightOfferNavigationId,
                                 TrueData = (0 < int.Parse(date.Dt.ToString("H")) && int.Parse(date.Dt.ToString("H")) < 6) ? true : false
                             }
                             select Flight).ToList();
                    return query;
                case 1:
                    query = (from Flight in filteredList
                             join date in DB.FlightsToDep on new
                             {
                                 Flight.Id,
                                 TrueData = true
                             }
                             equals new
                             {
                                 Id = date.FlightOfferNavigationId,
                                 TrueData = (6 < int.Parse(date.Dt.ToString("H")) && int.Parse(date.Dt.ToString("H")) < 12) ? true : false
                             }
                             select Flight).ToList();
                    return query;
                case 2:
                    query = (from Flight in filteredList
                             join date in DB.FlightsToDep on new
                             {
                                 Flight.Id,
                                 TrueData = true
                             }
                             equals new
                             {
                                 Id = date.FlightOfferNavigationId,
                                 TrueData = (12 < int.Parse(date.Dt.ToString("H")) && int.Parse(date.Dt.ToString("H")) < 18) ? true : false
                             }
                             select Flight).ToList();
                    return query;
                case 3:
                    query = (from Flight in filteredList
                             join date in DB.FlightsToDep on new
                             {
                                 Flight.Id,
                                 TrueData = true
                             }
                             equals new
                             {
                                 Id = date.FlightOfferNavigationId,
                                 TrueData = (18 < int.Parse(date.Dt.ToString("H")) && int.Parse(date.Dt.ToString("H")) < 24) ? true : false
                             }
                             select Flight).ToList();
                    return query;
            }
            return default!;
        }

        public IEnumerable<FlightOffersDB> getNumberOfFlightsFromDepInTime(int timeBegin, IEnumerable<FlightOffersDB> filteredList)
        {
            IEnumerable<FlightOffersDB> query = default!;
            timeBegin = timeBegin / 6;
            if (timeBegin == 4)
            {
                timeBegin = 0;
            }
            switch (timeBegin)
            {
                case 0:
                    query = (from Flight in filteredList
                             join date in DB.FlightsBackDep on new
                             {
                                 Flight.Id,
                                 TrueData = true
                             }
                             equals new
                             {
                                 Id = date.FlightOfferNavigationId,
                                 TrueData = (0 < int.Parse(date.Dt.ToString("H")) && int.Parse(date.Dt.ToString("H")) < 6) ? true : false
                             }
                             select Flight).ToList();
                    return query;
                case 1:
                    query = (from Flight in filteredList
                             join date in DB.FlightsBackDep on new
                             {
                                 Flight.Id,
                                 TrueData = true
                             }
                             equals new
                             {
                                 Id = date.FlightOfferNavigationId,
                                 TrueData = (6 < int.Parse(date.Dt.ToString("H")) && int.Parse(date.Dt.ToString("H")) < 12) ? true : false
                             }
                             select Flight).ToList();
                    return query;
                case 2:
                    query = (from Flight in filteredList
                             join date in DB.FlightsBackDep on new
                             {
                                 Flight.Id,
                                 TrueData = true
                             }
                             equals new
                             {
                                 Id = date.FlightOfferNavigationId,
                                 TrueData = (12 < int.Parse(date.Dt.ToString("H")) && int.Parse(date.Dt.ToString("H")) < 18) ? true : false
                             }
                             select Flight).ToList();
                    return query;
                case 3:
                    query = (from Flight in filteredList
                             join date in DB.FlightsBackDep on new
                             {
                                 Flight.Id,
                                 TrueData = true
                             }
                             equals new
                             {
                                 Id = date.FlightOfferNavigationId,
                                 TrueData = (18 < int.Parse(date.Dt.ToString("H")) && int.Parse(date.Dt.ToString("H")) < 24) ? true : false
                             }
                             select Flight).ToList();
                    return query;
            }
            return default!;
        }

        public IEnumerable<FlightOffersDB> getNumberOfFlightsToArrInTime(int timeBegin, IEnumerable<FlightOffersDB> filteredList)
        {
            IEnumerable<FlightOffersDB> query = default!;
            timeBegin = timeBegin / 6;
            if (timeBegin == 4)
            {
                timeBegin = 0;
            }
            switch (timeBegin)
            {
                case 0:
                    query = (from Flight in filteredList
                             join date in DB.FlightsToArr on new
                             {
                                 Flight.Id,
                                 TrueData = true
                             }
                             equals new
                             {
                                 Id = date.FlightOfferNavigationId,
                                 TrueData = (0 < int.Parse(date.Dt.ToString("H")) && int.Parse(date.Dt.ToString("H")) < 6) ? true : false
                             }
                             select Flight).ToList();
                    return query;
                case 1:
                    query = (from Flight in filteredList
                             join date in DB.FlightsToArr on new
                             {
                                 Flight.Id,
                                 TrueData = true
                             }
                             equals new
                             {
                                 Id = date.FlightOfferNavigationId,
                                 TrueData = (6 < int.Parse(date.Dt.ToString("H")) && int.Parse(date.Dt.ToString("H")) < 12) ? true : false
                             }
                             select Flight).ToList();
                    return query;
                case 2:
                    query = (from Flight in filteredList
                             join date in DB.FlightsToArr on new
                             {
                                 Flight.Id,
                                 TrueData = true
                             }
                             equals new
                             {
                                 Id = date.FlightOfferNavigationId,
                                 TrueData = (12 < int.Parse(date.Dt.ToString("H")) && int.Parse(date.Dt.ToString("H")) < 18) ? true : false
                             }
                             select Flight).ToList();
                    return query;
                case 3:
                    query = (from Flight in filteredList
                             join date in DB.FlightsToArr on new
                             {
                                 Flight.Id,
                                 TrueData = true
                             }
                             equals new
                             {
                                 Id = date.FlightOfferNavigationId,
                                 TrueData = (18 < int.Parse(date.Dt.ToString("H")) && int.Parse(date.Dt.ToString("H")) < 24) ? true : false
                             }
                             select Flight).ToList();
                    return query;
            }
            return default!;
        }
    }
}
