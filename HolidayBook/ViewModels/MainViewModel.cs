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
            set { 
                dateend = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Dateend)));
            }
        }

        private DateTime datestart2 = DateCalculator.calcFirstDayNextMonth();

        public DateTime Datestart2
        {
            get { return datestart2; }
            set { 
                datestart2 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Datestart2)));
            }
        }

        public DateTime Dateend2 { get; } = DateCalculator.calcEndDate2(DateTime.Now.AddMonths(13));

        private DateTime dpDate;

        public DateTime DpDate
        {
            get { return dpDate; }
            set {
                if(dpDate < value)
                {

                } else 
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
                if(dpDate2 > value)
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

        public HolidayBookContext Database { get; private set; } = default!;

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

            Database = db;
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
            removeChildren = new RelayCommand(() => Children--);
            addChildren = new RelayCommand(() => Children++);
            checkTravellers = new RelayCommand(() =>
            {
                WPFRuntimeElements wpf = new WPFRuntimeElements(MW, Database);
                wpf.checkFinishTraveller();
            });
            restartApp = new RelayCommand(() =>
            {
                Process.Start(Process.GetCurrentProcess().MainModule.FileName);
                Application.Current.Shutdown();
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
        }

        public static MainWindow MW { get; private set; } = default!;

        public ICommand addAdult { get; }

        public ICommand removeAdult { get; }

        public ICommand showPopUp { get; }

        public ICommand removeChildren { get; }

        public ICommand addChildren { get; }

        public ICommand checkTravellers { get; }

        public ICommand restartApp { get; }

        public ICommand showAirportTxt { get; }

        public ICommand RadioButtons { get; }

    }
}
