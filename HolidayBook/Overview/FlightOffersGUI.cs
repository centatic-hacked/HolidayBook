using Bogus;
using HolidayBook.Components;
using HolidayBook.Overview.Classes;
using HolidayBook.ViewModels;
using Model.Infrastructure;
using Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Shell;

namespace HolidayBook.Overview
{
    public class FlightOffersGUI
    {
        public MainWindow MW { get; } = default!;

        public HolidayBookContext Database { get; } = default!;

        public FlightOffersGUI(MainWindow mW, HolidayBookContext database)
        {
            MW = mW;
            Database = database;
        }

        public ActivatedFilters Afs { get; } = new(false, new List<string>(), new List<int>(), new List<int>(), new List<int>(), new List<int>(), default!);

        public void ChangeNumbers(IEnumerable<FlightOffersDB> offers, object sender)
        {
            string name = "";
            MainViewModel mvw = (MainViewModel)MW.DataContext;
            if (sender is not RadioButton)
            {
                mvw.ShowResultsCount = offers.Count();
            }
            mvw.ShowResults = $"{offers.Count()}";
            mvw.ShowResultsOfOneStopCount = offers.Where(flights => flights.Arr_Stops <= 1 && flights.Dep_Stops <= 1).Count();
            OutboundViewModel outboundVM = (OutboundViewModel)mvw.Outbound.DataContext;
            outboundVM.OutboundDepTxt = MW.txtDepartureAirport.Content.ToString().Substring(4);
            outboundVM.OutboundArrTxt = MW.txtArrivalAirport.Content.ToString().Substring(4);
            ReturnViewModel returnVM = (ReturnViewModel)mvw.Return.DataContext;
            returnVM.ReturnDepTxt = MW.txtArrivalAirport.Content.ToString().Substring(4);
            returnVM.ReturnArrTxt = MW.txtDepartureAirport.Content.ToString().Substring(4);
            if (sender is CheckBox)
            {
                CheckBox chb = (CheckBox)sender;
                name = chb.Name;
            }
            if (!name.Contains("Return") && !name.Contains("Outbound"))
            {
                outboundVM.OutboundDepNr1 = MainViewModel.PR.getNumberOfFlightsToDepInTime(0, offers.ToList()).Count();
                outboundVM.OutboundDepNr2 = MainViewModel.PR.getNumberOfFlightsToDepInTime(6, offers.ToList()).Count();
                outboundVM.OutboundDepNr3 = MainViewModel.PR.getNumberOfFlightsToDepInTime(12, offers.ToList()).Count();
                outboundVM.OutboundDepNr4 = MainViewModel.PR.getNumberOfFlightsToDepInTime(18, offers.ToList()).Count();
                outboundVM.OutboundArrNr1 = MainViewModel.PR.getNumberOfFlightsToArrInTime(0, offers.ToList()).Count();
                outboundVM.OutboundArrNr2 = MainViewModel.PR.getNumberOfFlightsToArrInTime(6, offers.ToList()).Count();
                outboundVM.OutboundArrNr3 = MainViewModel.PR.getNumberOfFlightsToArrInTime(12, offers.ToList()).Count();
                outboundVM.OutboundArrNr4 = MainViewModel.PR.getNumberOfFlightsToArrInTime(18, offers.ToList()).Count();
                returnVM.ReturnDepNr1 = MainViewModel.PR.getNumberOfFlightsFromDepInTime(0, offers.ToList()).Count();
                returnVM.ReturnDepNr2 = MainViewModel.PR.getNumberOfFlightsFromDepInTime(6, offers.ToList()).Count();
                returnVM.ReturnDepNr3 = MainViewModel.PR.getNumberOfFlightsFromDepInTime(12, offers.ToList()).Count();
                returnVM.ReturnDepNr4 = MainViewModel.PR.getNumberOfFlightsFromDepInTime(18, offers.ToList()).Count();
                returnVM.ReturnArrNr1 = MainViewModel.PR.getNumberOfFlightsFromArrInTime(0, offers.ToList()).Count();
                returnVM.ReturnArrNr2 = MainViewModel.PR.getNumberOfFlightsFromArrInTime(6, offers.ToList()).Count();
                returnVM.ReturnArrNr3 = MainViewModel.PR.getNumberOfFlightsFromArrInTime(12, offers.ToList()).Count();
                returnVM.ReturnArrNr4 = MainViewModel.PR.getNumberOfFlightsFromArrInTime(18, offers.ToList()).Count();
            }
            if (sender is not Slider)
            {
                mvw.MinimalTime = MainViewModel.PR.MinimalTime / 60;
                mvw.MaximalTime = MainViewModel.PR.MaximalTime / 60;
                mvw.SliderVal = MainViewModel.PR.MaximalTime / 60;
            }
            showListOnRuntime();
        }

        public IEnumerable<FlightOffersDB> CreateNewList()
        {
            IEnumerable<FlightOffersDB> list = Database.Flights;
            if (Afs.OneStop == true)
            {
                list = list.Where(flights => flights.Arr_Stops <= 1 && flights.Dep_Stops <= 1);
            }
            if (Afs.ExcludedAirlines.Count() > 0)
            {
                foreach (string airline in Afs.ExcludedAirlines)
                {
                    list = list.Where(flights => !flights.Arrives_airlines.Contains(airline) && !flights.Departs_airlines.Contains(airline));
                }
            }
            if (Afs.ActivatedTimesDepOutbound.Count() > 0)
            {
                List<FlightOffersDB> tempList = new();
                foreach (int times in Afs.ActivatedTimesDepOutbound)
                {
                    tempList.AddRange(MainViewModel.PR.getNumberOfFlightsToDepInTime(times, list));
                }
                list = tempList;
            }
            if (Afs.ActivatedTimesArrOutbound.Count() > 0)
            {
                List<FlightOffersDB> tempList = new();
                foreach (int times in Afs.ActivatedTimesArrOutbound)
                {
                    tempList.AddRange(MainViewModel.PR.getNumberOfFlightsToArrInTime(times, list));
                }
                list = tempList;
            }
            if (Afs.ActivatedTimesDepReturn.Count() > 0)
            {
                List<FlightOffersDB> tempList = new();
                foreach (int times in Afs.ActivatedTimesDepReturn)
                {
                    tempList.AddRange(MainViewModel.PR.getNumberOfFlightsFromDepInTime(times, list));
                }
                list = tempList;
            }
            if (Afs.ActivatedTimesArrReturn.Count() > 0)
            {
                List<FlightOffersDB> tempList = new();
                foreach (int times in Afs.ActivatedTimesArrReturn)
                {
                    tempList.AddRange(MainViewModel.PR.getNumberOfFlightsFromArrInTime(times, list));
                }
                list = tempList;
            }
            if (Afs.MaxTravelTime != default)
            {
                //foreach(FlightOffersDB flight in list) {
                //    int a = flight.Arrives_duration;
                //    int b = flight.Departs_duration;
                //}
                list = list.Where(flights => flights.Arrives_duration / 60 < Afs.MaxTravelTime && flights.Departs_duration / 60 < Afs.MaxTravelTime);
            }
            return list;
        }

        public void showListOnRuntime()
        {
            MainViewModel mvw = (MainViewModel)MW.DataContext;
            //foreach(RowDefinition ui in MW.ShownFlightOffers.RowDefinitions)
            //{
            //    MW.ShownFlightOffers.RowDefinitions.Remove(ui);
            //}
            int i = 0;
            foreach (var flight in mvw.Flights)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new System.Windows.GridLength(200);
                MW.ShownFlightOffers.RowDefinitions.Add(row);
                Border br = new Border();
                br.Width = 800;
                br.CornerRadius = new CornerRadius(10);
                br.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#a2a2a2");
                br.BorderThickness = new Thickness(0.4);
                br.Margin = new Thickness(0, 0, 0, 20);
                StackPanel flightOfferDetails = new StackPanel();
                flightOfferDetails.Orientation = Orientation.Horizontal;
                br.Child = flightOfferDetails;
                StackPanel flightSpecificDetails = new StackPanel();
                flightSpecificDetails.Margin = new Thickness(20,0,0,0);
                StackPanel outboundFlight = new StackPanel();
                outboundFlight.Width = Double.NaN;
                outboundFlight.Orientation = Orientation.Horizontal;
                outboundFlight.Margin = new Thickness(0, 20, 0, 5);
                outboundFlight.Children.Add(createDepInfo(MainViewModel.PR.GetAirportAndDateOutboundDep(flight, mvw.Flights)));
                outboundFlight.Children.Add(createDuration(flight.Departs_duration / 60, flight.Departs_duration % 60, flight.Dep_Stops));
                outboundFlight.Children.Add(createArrInfo(MainViewModel.PR.GetAirportAndDateOutboundArr(flight, mvw.Flights)));
                Label outboundNames = new Label();
                outboundNames.FontSize = 11;
                outboundNames.Margin = new Thickness(0, 0, 0, 8);
                outboundNames.Content = flight.Departs_airlines;
                StackPanel returnFlight = new StackPanel();
                returnFlight.Width = Double.NaN;
                returnFlight.Orientation = Orientation.Horizontal;
                returnFlight.Margin = new Thickness(0, 0, 0, 5);
                returnFlight.Children.Add(createDepInfo(MainViewModel.PR.GetAirportAndDateReturnDep(flight, mvw.Flights)));
                returnFlight.Children.Add(createDuration(flight.Departs_duration / 60, flight.Departs_duration % 60, flight.Dep_Stops));
                returnFlight.Children.Add(createArrInfo(MainViewModel.PR.GetAirportAndDateReturnArr(flight, mvw.Flights)));
                Label returnNames = new Label();
                returnNames.FontSize = 11;
                returnNames.Content = flight.Arrives_airlines;
                flightSpecificDetails.Children.Add(outboundFlight);
                flightSpecificDetails.Children.Add(outboundNames);
                flightSpecificDetails.Children.Add(returnFlight);
                flightSpecificDetails.Children.Add(returnNames);
                flightOfferDetails.Children.Add(flightSpecificDetails);
                flightOfferDetails.Children.Add(generateSeperator());
                flightOfferDetails.Children.Add(generatePriceIndicator(flight.Price, flight.Currency));
                MW.ShownFlightOffers.Children.Add(br);
                br.SetValue(Grid.RowProperty, i);
                i++;
            }
        }
        
        public StackPanel generateSeperator()
        {
            StackPanel stackPanel= new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;
            stackPanel.Margin = new Thickness(15,0,0,0);
            Rectangle sp = new Rectangle();
            sp.Width = 0.5;
            sp.VerticalAlignment = VerticalAlignment.Stretch;
            sp.Fill = (Brush)new BrushConverter().ConvertFrom("#a2a2a2");
            sp.Stroke = (Brush)new BrushConverter().ConvertFrom("#a2a2a2");
            stackPanel.Children.Add(sp);
            return stackPanel;
        }

        public StackPanel generatePriceIndicator(float price, string currency)
        {
            StackPanel stackPanel= new StackPanel();
            stackPanel.HorizontalAlignment = HorizontalAlignment.Right;
            stackPanel.Orientation = Orientation.Vertical;
            stackPanel.Margin = new Thickness(15,0,0,0);
            Label priceLabel = new Label();
            priceLabel.Content = $"{currency}{price}";
            priceLabel.FontSize = 24;
            priceLabel.Margin = new Thickness(0,50,0,0);
            Label shText = new Label();
            shText.FontSize = 11;
            shText.Content = "Total price for all travellers";
            shText.Margin = new Thickness(0,5,0,0);
            Button button= new Button();
            button.Margin = new Thickness(0,15,0,0);
            button.Background = Brushes.White;
            button.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#006ce4");
            button.Foreground = (Brush)new BrushConverter().ConvertFrom("#006ce4");
            button.Width = 150;
            button.Height = 30;
            button.Content = "See flight";
            stackPanel.Children.Add(priceLabel);
            stackPanel.Children.Add(shText);
            stackPanel.Children.Add(button);
            return stackPanel;
        } 
        public StackPanel createDuration(int hours, int minutes, int stops)
        {
            StackPanel outboundTime = new StackPanel();
            outboundTime.Orientation = Orientation.Vertical;
            outboundTime.HorizontalAlignment = HorizontalAlignment.Center;
            outboundTime.Width = 150;
            TextBlock outboundDuration = new TextBlock();
            outboundDuration.FontSize = 11;
            outboundDuration.HorizontalAlignment = HorizontalAlignment.Center;
            outboundDuration.Margin = new Thickness(0, 5, 0, 0);
            outboundDuration.Text = $"{hours}h {minutes}m";
            Border outboundLine = new Border();
            outboundLine.Width = 140;
            outboundLine.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#a2a2a2");
            outboundLine.BorderThickness = new Thickness(0.7);
            outboundLine.Margin = new Thickness(0, 5, 0, 0);
            TextBlock outboundConnectionMethod = new TextBlock();
            outboundConnectionMethod.HorizontalAlignment = HorizontalAlignment.Center;
            outboundConnectionMethod.FontSize = 11;
            outboundConnectionMethod.Text = (stops == 0) ? "Direct" : (stops == 1)? $"{stops} stop" : $"{stops} stops";
            outboundTime.Children.Add(outboundDuration);
            outboundTime.Children.Add(outboundLine);
            outboundTime.Children.Add(outboundConnectionMethod);
            return outboundTime;
        }
        public StackPanel createDepInfo(string input)
        {
            StackPanel outboundDepInfo = new StackPanel();
            outboundDepInfo.Orientation = Orientation.Vertical;
            outboundDepInfo.Width = 100;
            outboundDepInfo.Margin = new Thickness(15, 0, 0, 0);
            TextBlock outboundDepartureTime = new TextBlock();
            outboundDepartureTime.FontSize = 13;
            outboundDepartureTime.FontWeight = FontWeights.Bold;
            outboundDepartureTime.Text = input.Substring(0,5);
            TextBlock outboundDepartureDatePort = new TextBlock();
            outboundDepartureDatePort.FontSize = 11;
            outboundDepartureDatePort.Text = input.Substring(6);
            outboundDepInfo.Children.Add(outboundDepartureTime);
            outboundDepInfo.Children.Add(outboundDepartureDatePort);
            return outboundDepInfo;
        }

        public StackPanel createArrInfo(string input)
        {
            StackPanel outboundDepInfo = new StackPanel();
            outboundDepInfo.Orientation = Orientation.Vertical;
            outboundDepInfo.HorizontalAlignment = HorizontalAlignment.Right;
            outboundDepInfo.Width = 100;
            outboundDepInfo.Margin = new Thickness(15, 0, 0, 0);
            TextBlock outboundDepartureTime = new TextBlock();
            outboundDepartureTime.FontSize = 13;
            outboundDepartureTime.FontWeight = FontWeights.Bold;
            outboundDepartureTime.Text = input.Substring(0, 5);
            TextBlock outboundDepartureDatePort = new TextBlock();
            outboundDepartureDatePort.FontSize = 11;
            outboundDepartureDatePort.Text = input.Substring(6);
            outboundDepInfo.Children.Add(outboundDepartureTime);
            outboundDepInfo.Children.Add(outboundDepartureDatePort);
            return outboundDepInfo;
        }
    }
}
