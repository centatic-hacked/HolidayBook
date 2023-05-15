using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Threading.Tasks;
using Bogus;
using System.Windows.Controls;
using System.Windows;
using HolidayBook.Overview;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Model.Infrastructure;
using System.Collections;
using System.Windows.Media.Media3D;
using System.Globalization;
using System.Collections.ObjectModel;
using HolidayBook.Overview.Classes;
using Bogus.Bson;
using System.Runtime.ConstrainedExecution;
using System.Windows.Media;
using HolidayBook.Components;
using static System.Net.Mime.MediaTypeNames;
using Model.Model;

namespace HolidayBook.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int adults = 1;

        public int Adults
        {
            get { return adults; }
            set
            {
                MW.popupTraveller.IsOpen = true;
                MW.adultIncrease.IsEnabled = true;
                MW.travellersIncrease.IsEnabled = true;
                if (value > 1 && children == 0)
                {
                    adults = value;
                    Travellerscontent = $"{adults} Adults";
                    AdultCount = true;
                }
                else if (value == 1 && children == 0)
                {
                    adults = value;
                    Travellerscontent = $"{adults} Adult";
                    AdultCount = false;
                }
                else
                {
                    if (value == 1)
                    {
                        AdultCount = false;
                    }
                    else
                    {
                        AdultCount = true;
                    }
                    adults = value;
                    Travellerscontent = $"{adults + children} Travellers";
                }
                if (adults + children == 9)
                {
                    MW.adultIncrease.IsEnabled = false;
                    MW.travellersIncrease.IsEnabled = false;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Adults)));
            }
        }

        private int children = 0;

        public int Children
        {
            get { return children; }
            set
            {
                MW.adultIncrease.IsEnabled = true;
                MW.popupTraveller.IsOpen = true;
                MW.travellersIncrease.IsEnabled = true;
                WPFRuntimeElements wpf = new WPFRuntimeElements(MW, Database);
                if (value == 0)
                {
                    ChildrenCount = false;
                }
                if (value == 0 && adults > 1)
                {
                    wpf.childrenItemCreator(value, children);
                    children = value;
                    Travellerscontent = $"{adults} Adults";
                    ChildrenCount = true;
                }
                else if (value == 0 && adults == 1)
                {
                    wpf.childrenItemCreator(value, children);
                    children = value;
                    Travellerscontent = $"{adults} Adult";
                    ChildrenCount = false;
                }
                else
                {
                    wpf.childrenItemCreator(value, children);
                    children = value;
                    ChildrenCount = true;
                    Travellerscontent = $"{adults + children} Travellers";
                }
                if (adults + children == 9)
                {
                    MW.adultIncrease.IsEnabled = false;
                    MW.travellersIncrease.IsEnabled = false;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Children)));
            }
        }

        private bool adultCount = false;

        public bool AdultCount
        {
            get { return adultCount; }
            set
            {
                adultCount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AdultCount)));
            }
        }

        private bool childrenCount = false;

        public bool ChildrenCount
        {
            get { return childrenCount; }
            set
            {
                childrenCount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ChildrenCount)));
            }
        }


        private string travellerscontent = "1 adult";

        public string Travellerscontent
        {
            get { return travellerscontent; }
            set
            {
                travellerscontent = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Travellerscontent)));
            }
        }

        private string depAirportFinder = "Airport or City";

        public string DepAirportFinder
        {
            get { return depAirportFinder; }
            set
            {
                depAirportFinder = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DepAirportFinder)));
                if (value.Count() > 1)
                {
                    MW.depAirportPanel.Children.RemoveAt(1);
                    WPFRuntimeElements wpf = new WPFRuntimeElements(MW, Database);
                    wpf.airportLookUp(value, "depLookUp");
                }
                else
                {
                    if (MW.depAirportPanel.Children[1] is TextBlock)
                    {
                        TextBlock = MW.compareFlights;
                    }
                    else
                    {
                        MW.depAirportPanel.Children.RemoveAt(1);
                        MW.depAirportPanel.Children.Add(TextBlock);
                        if (value.Count() == 0)
                        {
                            depAirportFinder = "Airport or City";
                        }
                    }
                }
            }
        }

        public TextBlock TextBlock { get; set; }

        private string arrAirportFinder = "Airport or City";

        public string ArrAirportFinder
        {
            get { return arrAirportFinder; }
            set
            {
                arrAirportFinder = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ArrAirportFinder)));
                if (value.Count() > 1)
                {
                    MW.arrAirportPanel.Children.RemoveAt(1);
                    WPFRuntimeElements wpf = new WPFRuntimeElements(MW, Database);
                    wpf.airportLookUp(value, "arrLookUp");
                }
                else
                {
                    if (MW.arrAirportPanel.Children[1] is TextBlock)
                    {
                        TextBlock = MW.compareFlights;
                    }
                    else
                    {
                        MW.arrAirportPanel.Children.RemoveAt(1);
                        MW.arrAirportPanel.Children.Add(TextBlock);
                        if (value.Count() == 0)
                        {
                            arrAirportFinder = "Airport or City";
                        }
                    }
                }
            }
        }

        public DateTime Datestart { get; } = DateCalculator.calcStartDate();


        private DateTime dateend = DateCalculator.calcEndDate();

        public DateTime Dateend
        {
            get { return dateend; }
            set
            {
                dateend = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Dateend)));
            }
        }

        private DateTime datestart2 = DateCalculator.calcFirstDayNextMonth();

        public DateTime Datestart2
        {
            get { return datestart2; }
            set
            {
                datestart2 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Datestart2)));
            }
        }

        public DateTime Dateend2 { get; } = DateCalculator.calcEndDate2(DateTime.Now.AddMonths(13));

        private DateTime dpDate;

        public DateTime DpDate
        {
            get { return dpDate; }
            set
            {
                if (dpDate < value)
                {

                }
                else
                {
                    DateTime start = DateCalculator.newDisplayDate(DpDate2);
                    Dateend = Dateend.AddMonths(-1);
                    Datestart2 = start.AddMonths(-1);
                    DpDate2 = start.AddMonths(-1);
                }
                dpDate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DpDate)));
            }
        }

        private DateTime dpDate2;

        public DateTime DpDate2
        {
            get { return dpDate2; }
            set
            {
                if (dpDate2 > value)
                {

                }
                else if (value.Month + value.Year * 12 <= Dateend2.Month + Dateend2.Year * 12)
                {
                    DateTime start = DateCalculator.newDisplayDate(value);
                    Dateend = Dateend.AddMonths(1);
                    Datestart2 = start;
                    DpDate = start.AddMonths(-1);
                }
                dpDate2 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DpDate2)));
            }
        }

        private string @class;

        public string Class
        {
            get
            {
                return @class;
            }
            set
            {
                @class = value;
                FlightBookData.@class = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Class)));
            }
        }

        private string showResults = "0 Ergebnisse werden angezeigt";

        public string ShowResults
        {
            get { return showResults; }
            set { 
                showResults = $"{value} Ergebnisse werden angezeigt";
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShowResults)));
            }
        }

        private string minimalPriceAll = "From €0";

        public string MinimalPriceAll
        {
            get { return minimalPriceAll; }
            set { 
                minimalPriceAll = $"From €{PR.LowestPrice}";
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MinimalPriceAll)));
            }
        }

        private string minimalPriceMax1 = "From €0";

        public  string MinimalPriceMax1
        {
            get { return minimalPriceMax1; }
            set { 
                minimalPriceMax1 = $"From €{PR.OneLowestPrice}";
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MinimalPriceMax1)));
            }
        }


        public ObservableCollection<string> Classes { get; private set; } = new ObservableCollection<string> { "Economy", "Premium economy", "Business", "First class" };

        public static bool OneWay { get; private set; }


        public HolidayBookContext Database { get; private set; } = default!;

        public Outbound Outbound { get; private set; }

        public Return Return { get; private set; } = new();


        private IEnumerable<FlightOffersDB> flights;

        public IEnumerable<FlightOffersDB> Flights
        {
            get { return flights; }
            set {
                flights = value;
                PR.Flights = value.ToList();
            }
        }


        public MainViewModel(MainWindow mw)
        {
            MW = mw;
            var options = new DbContextOptionsBuilder();

            options.UseSqlite("Data Source=HolidayBook.db");

            HolidayBookContext db = new HolidayBookContext(options.Options);
            if (db.Database.EnsureCreated() == false)
            {
                FlightOverview.createDB();
                db.Seed();
            }
            FOG = new FlightOffersGUI(MW, db);
            Class = "Economy";
            Database = db;
            Outbound = (Outbound) MW.Outbound_Return.Content;
            OutboundViewModel.MVW = this;
            ReturnViewModel.MVW = this;
            addAdult = new RelayCommand(() => Adults++);
            removeAdult = new RelayCommand(() => Adults--);
            showPopUp = new RelayCommand((object sender) =>
            {
                Button bx = sender as Button;
                switch (bx.Name)
                {
                    case "txtDateRange":
                        MW.popupCalendar.IsOpen = true;
                        break;
                    case "txtArrivalAirport":
                        MW.popupArrPort.IsOpen = true;
                        break;
                    case "txtDepartureAirport":
                        MW.popupDepPort.IsOpen = true;
                        break;
                    case "Travellers":
                        MW.popupTraveller.IsOpen = true;
                        break;
                }
            });
            removeChildren = new RelayCommand(() =>
            {
                Children--;
                FlightBookData.adults--;
            });
            addChildren = new RelayCommand(() =>
            {
                Children++;
                FlightBookData.adults++;
            });
            checkTravellers = new RelayCommand(() =>
            {
                WPFRuntimeElements wpf = new WPFRuntimeElements(MW, Database);
                wpf.checkFinishTraveller();
            });
            restartApp = new RelayCommand(() =>
            {
                Process.Start(Process.GetCurrentProcess().MainModule.FileName);
                System.Windows.Application.Current.Shutdown();
            });
            showAirportTxt = new RelayCommand((object sender) =>
            {
                TextBox txt = sender as TextBox;
                txt.Text = string.Empty;
                txt.Foreground = System.Windows.Media.Brushes.Black;
                txt.Focus();
            });
            RadioButtons = new RelayCommand((object sender) =>
            {
                MW.Flights.BorderThickness = new Thickness(0);
                MW.Stays.BorderThickness = new Thickness(0);
                MW.FlightsHotel.BorderThickness = new Thickness(0);
                Button btn = (Button)sender;
                btn.BorderThickness = new Thickness(1.5);
            });
            FlightBack = new RelayCommand((object sender) =>
            {
                RadioButton rb = (RadioButton)sender;
                if (rb.Name == "AlsoBack")
                {
                    OneWay = false;
                    MW.calendar.SelectionMode = CalendarSelectionMode.MultipleRange;
                    MW.calendar2.SelectionMode = CalendarSelectionMode.MultipleRange;
                    MW.calendar2.SelectedDates.Clear();
                    MW.calendar.SelectedDates.Clear();
                }
                else
                {
                    OneWay = true;
                    MW.calendar.SelectionMode = CalendarSelectionMode.SingleDate;
                    MW.calendar2.SelectionMode = CalendarSelectionMode.SingleDate;
                    MW.calendar2.SelectedDates.Clear();
                    MW.calendar.SelectedDates.Clear();
                }
            });
            DirectFlight = new RelayCommand(() =>
            {
                FlightBookData.nonStop = MW.DirectFlights.IsChecked.Value;
            });
            ButtonChecker = new RelayCommand(async () =>
            {
                bool execute = true;
                foreach (string keys in FlightBookData.travellers.Keys)
                {
                    string? value = FlightBookData.travellers[keys];
                    if (value == null || FlightBookData.startAirport == default || FlightBookData.endAirport == default || FlightBookData.depDate == default)
                    {
                        execute = false;
                    }
                    else if (int.Parse(value) <= 2)
                    {
                        FlightBookData.infants++;
                    }
                    else if (int.Parse(value) < 12)
                    {
                        FlightBookData.children++;
                    }
                }
                switch(FlightBookData.@class)
                {
                    case "Economy":
                        FlightBookData.@class = "ECONOMY";
                        break;
                    case "Premium economy":
                        FlightBookData.@class = "PREMIUM_ECONOMY";
                        break;
                    case "Business":
                        FlightBookData.@class = FlightBookData.@class.ToUpper();
                        break;
                    case "First class":
                        FlightBookData.@class = FlightBookData.@class.Substring(0, FlightBookData.@class.IndexOf(" ")).ToUpper();
                        break;
                }
                if (execute == true)
                {
                   List<FlightOfferDetails> ls = await FlightOverview.FlightData(Database, FlightBookData.startAirport, FlightBookData.endAirport, FlightBookData.depDate, FlightBookData.retDate,
                        FlightBookData.adults, FlightBookData.children, FlightBookData.infants, FlightBookData.@class, FlightBookData.includeAir, FlightBookData.excludeAir,
                        FlightBookData.nonStop, FlightBookData.currency, FlightBookData.maxPrice);
                    Database.AddFlights(await FlightOverview.addToDb(ls, Database));
                    PR = new PreparedDataFilter(Database, this);
                    Flights = Database.Flights;
                    FOG.ChangeNumbers(Flights, "");
                    PR.checkForAirline();
                }
            });
            ArrOrDep = new RelayCommand((object sender) =>
            {
                Button btn = (Button)sender;
                if(btn.Name == "outbound")
                {
                    btn.Foreground = (Brush)new BrushConverter().ConvertFrom("#006ce4");
                    MW.arrBor.Background = (Brush)new BrushConverter().ConvertFrom("#006ce4");
                    MW.arrBor.Height = 2;
                    MW.@return.Foreground = System.Windows.Media.Brushes.Black;
                    MW.depBor.Height = 1;
                    MW.depBor.Background = (Brush)new BrushConverter().ConvertFrom("#e7e7e7");
                    MW.Outbound_Return.Content = Outbound;
                } else
                {
                    btn.Foreground = (Brush)new BrushConverter().ConvertFrom("#006ce4");
                    MW.depBor.Height = 2;
                    MW.depBor.Background = (Brush)new BrushConverter().ConvertFrom("#006ce4");
                    MW.outbound.Foreground = System.Windows.Media.Brushes.Black;
                    MW.arrBor.Background = (Brush)new BrushConverter().ConvertFrom("#e7e7e7");
                    MW.arrBor.Height = 1;
                    MW.Outbound_Return.Content = Return;
                }
            });
            OrderButtons = new RelayCommand((object sender) =>
            {
                MW.Bests.BorderBrush = System.Windows.Media.Brushes.Transparent;
                MW.Cheapests.BorderBrush = System.Windows.Media.Brushes.Transparent;
                MW.Fastests.BorderBrush = System.Windows.Media.Brushes.Transparent;
                MW.Best.Foreground = System.Windows.Media.Brushes.Black;
                MW.Cheapest.Foreground = System.Windows.Media.Brushes.Black;
                MW.Fastest.Foreground = System.Windows.Media.Brushes.Black;
                Button btn = (Button) sender;
                btn.Foreground = (Brush)new BrushConverter().ConvertFrom("#006ce4");
                
                switch (btn.Name)
                {
                    case "Fastest":
                        MW.Fastests.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#006ce4");
                        break;
                    case "Cheapest":
                        MW.Cheapests.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#006ce4");
                        break;
                    case "Best":
                        MW.Bests.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#006ce4");
                        break;
                }
            });
            AnyOrMaxOne = new RelayCommand((object sender) =>
            {
                RadioButton btn = (RadioButton) sender;
                switch(btn.Name)
                {
                    case "Any":
                        FOG.Afs.OneStop = false;
                        Flights = FOG.CreateNewList();
                        PR.checkForAirline();
                        break;
                    case "MaxOne":
                        FOG.Afs.OneStop = true;
                        Flights = FOG.CreateNewList();
                        PR.checkForAirline();
                        break;
                }
                FOG.ChangeNumbers(Flights, btn);
            });
        }

        private int sliderVal;

        public int SliderVal
        {
            get { return sliderVal; }
            set { 
                sliderVal = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SliderVal)));
                MW.HourShow.Text = $"{value} hours";
                if(value != MW.HoursRequired.Maximum)
                {
                    FOG.Afs.MaxTravelTime = value;
                    Flights = FOG.CreateNewList();
                    PR.checkForAirline();
                    FOG.ChangeNumbers(Flights, MW.HoursRequired);
                    FOG.Afs.MaxTravelTime = default!;
                }
            }
        }


        private int minimalTime;

        public int MinimalTime
        {
            get { return minimalTime; }
            set { 
                minimalTime = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MinimalTime)));
            }
        }

        private int maximalTime;

        public int MaximalTime
        {
            get { return maximalTime; }
            set
            {
                maximalTime = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MaximalTime)));
            }
        }

        private int showResultsCount = 0;

        public int ShowResultsCount
        {
            get { return showResultsCount; }
            set { 
                showResultsCount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShowResultsCount)));
            }
        }
        private int showResultsOfOneStopCount = 0;

        public int ShowResultsOfOneStopCount
        {
            get { return showResultsOfOneStopCount; }
            set { 
                showResultsOfOneStopCount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShowResultsOfOneStopCount)));
            }
        }
        private List<ExcludeAirlinesList> airlinesList = default!;

        public List<ExcludeAirlinesList> AirlinesList
        {
            get { return airlinesList; }
            set { 
                airlinesList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AirlinesList)));
            }
        }

        public static PreparedDataFilter PR { get; set; } = default!;


        public static MainWindow MW { get; private set; } = default!;

        public FlightOffersGUI FOG { get; }

        public ICommand addAdult { get; }

        public ICommand removeAdult { get; }

        public ICommand showPopUp { get; }

        public ICommand removeChildren { get; }

        public ICommand addChildren { get; }

        public ICommand checkTravellers { get; }

        public ICommand restartApp { get; }

        public ICommand showAirportTxt { get; }

        public ICommand RadioButtons { get; }

        public ICommand FlightBack { get; }

        public ICommand DirectFlight { get; }

        public ICommand ButtonChecker { get; }

        public ICommand ArrOrDep { get; }

        public ICommand OrderButtons { get; }

        public ICommand AnyOrMaxOne { get; }
    }
}
