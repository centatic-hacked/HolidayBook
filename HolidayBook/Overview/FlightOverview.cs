using HolidayBook.JSON;
using Microsoft.EntityFrameworkCore;
using Model.Infrastructure;
using Nancy.Json;
using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Numerics;
using System.Resources;
using Model;
using System.Net.Http;
using HolidayBook.Overview.Classes;
using HolidayBook.ViewModels;
using System.Globalization;
using Model.Model;

namespace HolidayBook.Overview
{
    public class FlightOverview
    {
        private static async Task<string> Token()
        {
            var token = "";
            HttpClient client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://test.api.amadeus.com/v1/security/oauth2/token"),
                Content = new StringContent("grant_type=client_credentials&client_id=PSbGuoQo5UG55nqijuXUGo1ALzCGtyP7&client_secret=CCC57j68HIUSpdRF", Encoding.UTF8, "application/x-www-form-urlencoded"),
            };

            var body = "";
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                body = await response.Content.ReadAsStringAsync();
                var js = new JavaScriptSerializer();
                dynamic block = js.Deserialize<dynamic>(body);
                token = block["access_token"];
            }
            return token;
        }


        public static async Task<List<FlightOfferDetails>> FlightData(HolidayBookContext db, string startLoc, string destLoc, string depDate
            , string? retDate = null, int adults = 1, int children = 0, int infants = 0, string? klass = null, string? includeAir = null
            , string? excludeAir = null, bool nonStop = false, string? currencyCode = null, int? maxPrice = null)
        {
            string url = attributeCheck(retDate, adults, children, infants, klass, includeAir, excludeAir, nonStop, currencyCode, maxPrice);
            string test = $"https://test.api.amadeus.com/v2/shopping/flight-offers?originLocationCode={startLoc}&destinationLocationCode={destLoc}&departureDate={depDate}{url}&max=250";
            var flightdata = new List<FlightOfferDetails>();
            string token = await Token();
            HttpClient client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://test.api.amadeus.com/v2/shopping/flight-offers?originLocationCode={startLoc}&destinationLocationCode={destLoc}&departureDate={depDate}{url}&max=250"),
                Content = new StringContent("", Encoding.UTF8, "application/vnd.amadeus+json"),
            };

            request.Headers.Add("Authorization", $"Bearer {token}");
            request.Headers.Add("accept", "application/vnd.amadeus+json");

            string body = "";
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                body = await response.Content.ReadAsStringAsync();
                var js = new JavaScriptSerializer();
                Root block = js.Deserialize<Root>(body);
                dynamic changes = js.Deserialize<dynamic>(body);
                var dictionaries = changes["dictionaries"];
                foreach (Datum offers in block.data)
                {
                    string departs_airlines = "";
                    string arrives_airlines = "";
                    var airlines_full = dictionaries["carriers"];
                    int departs_first = 0;
                    int departs_breaks = 0;
                    List<DateTime> dep_Departures = new List<DateTime>();
                    List<DateTime> dep_Arrivals = new List<DateTime>();
                    for (int i = 0; i < offers.itineraries[0].segments.Count(); i++)
                    {
                        dep_Departures.Add(offers.itineraries[0].segments[i].departure.at);
                        dep_Arrivals.Add(offers.itineraries[0].segments[i].arrival.at);
                        if (!departs_airlines.Contains(airlines_full[offers.itineraries[0].segments[i].carrierCode]))
                        {
                            departs_airlines += (departs_first == 0) ? airlines_full[offers.itineraries[0].segments[i].carrierCode] : ", " + airlines_full[offers.itineraries[0].segments[i].carrierCode];
                            departs_first++;
                        }
                        departs_breaks++;
                    }
                    int arrives_first = 0;
                    int arrives_breaks = 0;
                    List<DateTime> arr_Departures = new List<DateTime>();
                    List<DateTime> arr_Arrivals = new List<DateTime>();
                    for (int i = 0; i < offers.itineraries[1].segments.Count(); i++)
                    {
                        arr_Departures.Add(offers.itineraries[1].segments[i].departure.at);
                        arr_Arrivals.Add(offers.itineraries[1].segments[i].arrival.at);
                        if (!arrives_airlines.Contains(airlines_full[offers.itineraries[1].segments[i].carrierCode]))
                        {
                            arrives_airlines += (arrives_first == 0) ? airlines_full[offers.itineraries[1].segments[i].carrierCode] : ", " + airlines_full[offers.itineraries[1].segments[i].carrierCode];
                            arrives_first++;
                        }
                        arrives_breaks++;
                    }
                    //string arrives_dep = offers.itineraries[1].segments[0].departure.at.ToString("dd MMM");

                    string dep_datetime = offers.itineraries[0].duration.Substring(offers.itineraries[0].duration.IndexOf("PT") + 2);
                    string dep_hour = (dep_datetime.Contains("H")) ? dep_datetime.Substring(0, dep_datetime.IndexOf("H")) : null;
                    string dep_min = (dep_datetime.Contains("M")) ? dep_datetime.Substring(dep_datetime.IndexOf("H") + 1, dep_datetime.IndexOf("M") - dep_datetime.IndexOf("H") - 1) : null;
                    int dep_duration = Int32.Parse(dep_hour ?? "0") * 60 + Int32.Parse(dep_min ?? "0");
                    string arr_datetime = offers.itineraries[1].duration.Substring(offers.itineraries[1].duration.IndexOf("PT") + 2);
                    string arr_hour = (arr_datetime.Contains("H")) ? arr_datetime.Substring(0, arr_datetime.IndexOf("H")) : null;
                    string arr_min = (arr_datetime.Contains("M")) ? arr_datetime.Substring(arr_datetime.IndexOf("H") + 1, arr_datetime.IndexOf("M") - arr_datetime.IndexOf("H") - 1) : null;
                    int arr_duration = Int32.Parse(arr_hour ?? "0") * 60 + Int32.Parse(arr_min ?? "0");
                    float tot_price = float.Parse(offers.price.grandTotal, CultureInfo.InvariantCulture.NumberFormat);
                    //char currency = Char.Parse(offers.price.currency);
                    string currency = db.Currencies.SingleOrDefault(c => c.Currency_Code == offers.price.currency).Currency_Symbol;

                    //.WriteLine($"Departs Airlines:{departs_airlines} Departs Duration:{dep_duration} Arrives Airlines:{arrives_airlines} Arrives_Duration:{arr_duration} Price:{offers.price.grandTotal}{currency}");
                    flightdata.Add(new FlightOfferDetails(departs_airlines, dep_duration, arrives_airlines, arr_duration, tot_price, currency, dep_Departures, dep_Arrivals, arr_Departures, arr_Arrivals, departs_breaks, arrives_breaks));
                    //Console.WriteLine(arrives_airlines);
                }
                return flightdata;
            }
        }
        public static HolidayBookContext createDB()
        {
            var options = new DbContextOptionsBuilder();

            options.UseSqlite("Data Source=HolidayBook.db");

            HolidayBookContext db = new HolidayBookContext(options.Options);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            return db;
        }

        private static string attributeCheck(string? retDate = null, int adults = 1, int children = 0, int infants = 0, string? klass = null, string? includeAir = null
            , string? excludeAir = null, bool nonStop = false, string? currencyCode = null, int? maxPrice = null)
        {
            string url = "";

            url += (retDate == null)? "" : $"&returnDate={retDate}";

            url += $"&adults={adults}";

            url += (children == 0)? "" : $"&children={children}";

            url += (infants == 0)? "" : $"&infants={infants}";

            url += (klass == null)? "" : $"&travelClass={klass}";

            url += (includeAir == null)? "" : $"&includedAirlineCodes={includeAir}";

            url += (excludeAir == null)? "" : $"&excludedAirlineCodes={excludeAir}";

            url += $"&nonStop={nonStop.ToString().ToLower()}";

            url += (currencyCode == null)? "" : $"&currencyCode={currencyCode}";

            url += (maxPrice == null)? "" : $"&maxPrice={maxPrice}";

            return url ;
        }

        public static List<FlightOffersDB> addToDb(List<FlightOfferDetails> flights, HolidayBookContext db) 
        {
            List<FlightOffersDB> flightsDB = new List<FlightOffersDB>();
            foreach(FlightOfferDetails flight in flights) {
                FlightOffersDB flightInDB = new(flight.Departs_airlines, flight.Departs_duration, flight.Arrives_airlines, flight.Price, flight.Currency, 
                    //flight.Dep_Departures, flight.Dep_Arrivals, flight.Arr_Departures, flight.Arr_Arrivals,
                    flight.Dep_Stops, flight.Arr_Stops);
                foreach(DateTime fl in flight.Dep_Departures)
                {
                    FlightToDep flDB = new FlightToDep(fl, flightInDB);
                    db.AddFlightDates(flDB);
                }
                foreach (DateTime fl in flight.Dep_Arrivals)
                {
                    FlightToArr flDB = new FlightToArr(fl, flightInDB);
                    db.AddFlightDates(flDB);
                }
                foreach (DateTime fl in flight.Arr_Departures)
                {
                    FlightBackDep flDB = new FlightBackDep(fl, flightInDB);
                    db.AddFlightDates(flDB);
                }
                foreach (DateTime fl in flight.Arr_Arrivals)
                {
                    FlightBackArr flDB = new FlightBackArr(fl, flightInDB);
                    db.AddFlightDates(flDB);
                }

                flightsDB.Add(flightInDB);
            }
            return flightsDB;
        }
    }
}