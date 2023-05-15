using Bogus.DataSets;
using Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using HolidayBook.ViewModels;

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

        public void checkForAirline()
        {
            List<ExcludeAirlinesList> ls = new List<ExcludeAirlinesList>();
            foreach (string airline in Airlines.Keys)
            {
                ls.Add(new ExcludeAirlinesList(airline, Airlines[airline], this.MW));
            }
            MW.AirlinesList = ls;
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
            return Math.Min(Flights.Min(flightDat => flightDat.Departs_duration), Flights.Min(flightDat => flightDat.Arrives_duration));
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
                                 TrueData = (0 < int.Parse(date.Dt.ToString("HH")) && int.Parse(date.Dt.ToString("HH")) < 6) ? true : false
                             }
                             where date.Airport == MainViewModel.MW.txtDepartureAirport.Content.ToString().Substring(0, 3)
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
                                 TrueData = (6 < int.Parse(date.Dt.ToString("HH")) && int.Parse(date.Dt.ToString("HH")) < 12) ? true : false
                             }
                             where date.Airport == MainViewModel.MW.txtDepartureAirport.Content.ToString().Substring(0, 3)
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
                                 TrueData = (12 < int.Parse(date.Dt.ToString("HH")) && int.Parse(date.Dt.ToString("HH")) < 18) ? true : false
                             }
                             where date.Airport == MainViewModel.MW.txtDepartureAirport.Content.ToString().Substring(0, 3)
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
                                 TrueData = (18 < int.Parse(date.Dt.ToString("HH")) && int.Parse(date.Dt.ToString("HH")) < 24) ? true : false
                             }
                             where date.Airport == MainViewModel.MW.txtDepartureAirport.Content.ToString().Substring(0, 3)
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
                                 TrueData = (0 < int.Parse(date.Dt.ToString("HH")) && int.Parse(date.Dt.ToString("HH")) < 6) ? true : false
                             }
                             where date.Airport == MainViewModel.MW.txtDepartureAirport.Content.ToString().Substring(0,3)
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
                                 TrueData = (6 < int.Parse(date.Dt.ToString("HH")) && int.Parse(date.Dt.ToString("HH")) < 12) ? true : false
                             }
                             where date.Airport == MainViewModel.MW.txtDepartureAirport.Content.ToString().Substring(0, 3)
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
                                 TrueData = (12 < int.Parse(date.Dt.ToString("HH")) && int.Parse(date.Dt.ToString("HH")) < 18) ? true : false
                             }
                             where date.Airport == MainViewModel.MW.txtDepartureAirport.Content.ToString().Substring(0, 3)
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
                                 TrueData = (18 < int.Parse(date.Dt.ToString("HH")) && int.Parse(date.Dt.ToString("HH")) < 24) ? true : false
                             }
                             where date.Airport == MainViewModel.MW.txtDepartureAirport.Content.ToString().Substring(0, 3)
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
                                 TrueData = (0 < int.Parse(date.Dt.ToString("HH")) && int.Parse(date.Dt.ToString("HH")) < 6) ? true : false
                             }
                             where date.Airport == MainViewModel.MW.txtArrivalAirport.Content.ToString().Substring(0, 3)
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
                                 TrueData = (6 < int.Parse(date.Dt.ToString("HH")) && int.Parse(date.Dt.ToString("HH")) < 12) ? true : false
                             }
                             where date.Airport == MainViewModel.MW.txtArrivalAirport.Content.ToString().Substring(0, 3)
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
                                 TrueData = (12 < int.Parse(date.Dt.ToString("HH")) && int.Parse(date.Dt.ToString("HH")) < 18) ? true : false
                             }
                             where date.Airport == MainViewModel.MW.txtArrivalAirport.Content.ToString().Substring(0, 3)
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
                                 TrueData = (18 < int.Parse(date.Dt.ToString("HH")) && int.Parse(date.Dt.ToString("HH")) < 24) ? true : false
                             }
                             where date.Airport == MainViewModel.MW.txtArrivalAirport.Content.ToString().Substring(0, 3)
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
                                 TrueData = (0 < int.Parse(date.Dt.ToString("HH")) && int.Parse(date.Dt.ToString("HH")) < 6) ? true : false
                             }
                             where date.Airport == MainViewModel.MW.txtArrivalAirport.Content.ToString().Substring(0, 3)
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
                                 TrueData = (6 < int.Parse(date.Dt.ToString("HH")) && int.Parse(date.Dt.ToString("HH")) < 12) ? true : false
                             }
                             where date.Airport == MainViewModel.MW.txtArrivalAirport.Content.ToString().Substring(0, 3)
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
                                 TrueData = (12 < int.Parse(date.Dt.ToString("HH")) && int.Parse(date.Dt.ToString("HH")) < 18) ? true : false
                             }
                             where date.Airport == MainViewModel.MW.txtArrivalAirport.Content.ToString().Substring(0, 3)
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
                                 TrueData = (18 < int.Parse(date.Dt.ToString("HH")) && int.Parse(date.Dt.ToString("HH")) < 24) ? true : false
                             }
                             where date.Airport == MainViewModel.MW.txtArrivalAirport.Content.ToString().Substring(0, 3)
                             select Flight).ToList();
                    return query;
            }
            return default!;
        }

        public string GetAirportAndDateOutboundDep(FlightOffersDB flight, IEnumerable<FlightOffersDB> fls)
        {
            string format = "";
            var data = (from Flight in fls
                        join date in DB.FlightsToDep on new
                        {
                            flight.Id
                        }
                        equals new
                        {
                            Id = date.FlightOfferNavigationId
                        }
                        where date.Airport == MainViewModel.MW.txtDepartureAirport.Content.ToString().Substring(0, 3)
                        select new {date.Dt, date.Airport}).ToList();
            foreach(var item in data)
            {
                format = $"{item.Dt.ToString("HH:mm")} {item.Airport}•{item.Dt.ToString("dd MMM")}";
            }
            return format;
        }

        public string GetAirportAndDateOutboundArr(FlightOffersDB flight, IEnumerable<FlightOffersDB> fls)
        {
            string format = "";
            var data = (from Flight in fls
                        join date in DB.FlightsToArr on new
                        {
                            flight.Id
                        }
                        equals new
                        {
                            Id = date.FlightOfferNavigationId
                        }
                        where date.Airport == MainViewModel.MW.txtArrivalAirport.Content.ToString().Substring(0, 3)
                        select new { date.Dt, date.Airport }).ToList();
            foreach (var item in data)
            {
                format = $"{item.Dt.ToString("HH:mm")} {item.Airport}•{item.Dt.ToString("dd MMM")}";
            }
            return format;
        }

        public string GetAirportAndDateReturnDep(FlightOffersDB flight, IEnumerable<FlightOffersDB> fls)
        {
            string format = "";
            var data = (from Flight in fls
                        join date in DB.FlightsBackDep on new
                        {
                            flight.Id
                        }
                        equals new
                        {
                            Id = date.FlightOfferNavigationId
                        }
                        where date.Airport == MainViewModel.MW.txtArrivalAirport.Content.ToString().Substring(0, 3)
                        select new { date.Dt, date.Airport }).ToList();
            foreach (var item in data)
            {
                format = $"{item.Dt.ToString("HH:mm")} {item.Airport}•{item.Dt.ToString("dd MMM")}";
            }
            return format;
        }

        public string GetAirportAndDateReturnArr(FlightOffersDB flight, IEnumerable<FlightOffersDB> fls)
        {
            string format = "";
            var data = (from Flight in fls
                        join date in DB.FlightsBackArr on new
                        {
                            flight.Id
                        }
                        equals new
                        {
                            Id = date.FlightOfferNavigationId
                        }
                        where date.Airport == MainViewModel.MW.txtDepartureAirport.Content.ToString().Substring(0, 3)
                        select new { date.Dt, date.Airport }).ToList();
            foreach (var item in data)
            {
                format = $"{item.Dt.ToString("HH:mm")} {item.Airport}•{item.Dt.ToString("dd MMM")}";
            }
            return format;
        }

    }
}
