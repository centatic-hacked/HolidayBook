using Bogus;
using Microsoft.EntityFrameworkCore;
using Model.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HolidayBook.Overview;
using System.Diagnostics;
using System.Windows.Media.Media3D;
using System.DirectoryServices;
using System.Xml.Linq;
using Nancy.Extensions;
using System.Windows.Automation.Provider;
using System.Globalization;
using HolidayBook.ViewModels;
using HolidayBook.Overview.Classes;

namespace HolidayBook
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel(this);
            calendar.BlackoutDates.AddDatesInPast();
            calendar.DisplayDate = DateTime.Now;
            calendar.SelectedDatesChanged += (s, e) => multipleDates(s, e, calendar);
            calendar2.SelectedDatesChanged += (s, e) => multipleDates(s, e, calendar2);
        }

        private void multipleDates(object sender, RoutedEventArgs e, System.Windows.Controls.Calendar cal)
        {
            List<DateTime> ls = cal.SelectedDates.ToList();
            if (calendar.SelectedDates.Count() == calendar2.SelectedDates.Count() && ls.Count != 1 || ls.Count() == 0)
            {
                return;
            }
            if (MainViewModel.OneWay == false)
            {
                if (ls.Count > 1)
                {
                    popupCalendar.IsOpen = false;
                    txtDateRange.Content = (DateTime.Now.Year == cal.SelectedDates.First().Year) ? cal.SelectedDates.First().ToString("ddd, MMM dd") : cal.SelectedDates.First().ToString("ddd, MMM dd, yyyy");
                    txtDateRange.Content += (DateTime.Now.Year == cal.SelectedDates.Last().Year) ? cal.SelectedDates.Last().ToString(" - ddd, MMM dd") : cal.SelectedDates.Last().ToString(" - ddd, MMM dd, yyyy");
                    txtDateRange.Foreground = Brushes.Black;
                    if (cal.Name == "calendar")
                    {
                        calendar2.SelectedDates.Clear();
                        selDates = calendar.SelectedDates;
                        FlightBookData.depDate = calendar.SelectedDates.First().ToString("yyyy-MM-dd");
                        FlightBookData.retDate = calendar.SelectedDates.Last().ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        calendar.SelectedDates.Clear();
                        selDates = calendar2.SelectedDates;
                        FlightBookData.depDate = calendar2.SelectedDates.First().ToString("yyyy-MM-dd");
                        FlightBookData.retDate = calendar2.SelectedDates.Last().ToString("yyyy-MM-dd");
                    }
                }
                else if (ls.Count == 1)
                {
                    if (cal.Name == "calendar")
                    {
                        if (calendar2.SelectedDates.Count() == 1)
                        {
                            string fullTxt = (string)txtDateRange.Content;
                            string restTxt = fullTxt.Substring(fullTxt.IndexOf(" - "));
                            fullTxt = ((DateTime.Now.Year == cal.SelectedDates.First().Year) ? cal.SelectedDates.First().ToString("ddd, MMM dd") : cal.SelectedDates.First().ToString("ddd, MMM dd, yyyy")) + restTxt;
                            txtDateRange.Content = fullTxt;
                            FlightBookData.depDate = calendar.SelectedDates.First().ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            txtDateRange.Content = (DateTime.Now.Year == cal.SelectedDates.First().Year) ? cal.SelectedDates.First().ToString("ddd, MMM dd") : cal.SelectedDates.First().ToString("ddd, MMM dd, yyyy");
                            txtDateRange.Content += " - Rückflug";
                            FlightBookData.depDate = calendar.SelectedDates.First().ToString("yyyy-MM-dd");
                        }
                    }
                    else
                    {
                        if (calendar.SelectedDates.Count() == 1)
                        {
                            string fullTxt = (string)txtDateRange.Content;
                            string firstTxt = fullTxt.Substring(0, fullTxt.IndexOf(" -") + 3);
                            fullTxt = firstTxt + ((DateTime.Now.Year == cal.SelectedDates.First().Year) ? cal.SelectedDates.First().ToString("ddd, MMM dd") : cal.SelectedDates.First().ToString("ddd, MMM dd, yyyy"));
                            txtDateRange.Content = fullTxt;
                            FlightBookData.retDate = calendar2.SelectedDates.Last().ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            txtDateRange.Content = "Abflug - ";
                            txtDateRange.Content += (DateTime.Now.Year == cal.SelectedDates.First().Year) ? cal.SelectedDates.First().ToString("ddd, MMM dd") : cal.SelectedDates.First().ToString("ddd, MMM dd, yyyy");
                            FlightBookData.retDate = calendar2.SelectedDates.Last().ToString("yyyy-MM-dd");
                        }
                    }
                }
            }
            else
            {
                if (cal.Name == "calendar")
                {
                    txtDateRange.Content = (DateTime.Now.Year == cal.SelectedDates.First().Year) ? cal.SelectedDates.First().ToString("ddd, MMM dd") : cal.SelectedDates.First().ToString("ddd, MMM dd, yyyy");
                    calendar2.SelectedDates.Clear();
                }
                else if (cal.Name == "calendar2")
                {
                    txtDateRange.Content = (DateTime.Now.Year == cal.SelectedDates.First().Year) ? cal.SelectedDates.First().ToString("ddd, MMM dd") : cal.SelectedDates.First().ToString("ddd, MMM dd, yyyy");
                    calendar.SelectedDates.Clear();
                }
            }
            txtDateRange.Foreground = Brushes.Black;
        }

        public SelectedDatesCollection selDates { get; private set; }
    }
}
