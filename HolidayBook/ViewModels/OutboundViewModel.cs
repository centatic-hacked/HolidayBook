using HolidayBook.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace HolidayBook.ViewModels
{
    public class OutboundViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Outbound MW { get; }

        public static MainViewModel MVW { get; set; }

        public OutboundViewModel(Outbound mw)
        {
            MW = mw;
            CheckboxDesign = new RelayCommand((object sender) =>
            {
                CheckBox ck = (CheckBox)sender;
                if (ck.IsChecked == true)
                {
                    ck.Background = (Brush)new BrushConverter().ConvertFrom("#006ce4");
                    Border parentBorder = VisualTreeHelper.GetParent(ck) as Border;
                    parentBorder.Background = (Brush)new BrushConverter().ConvertFrom("#006ce4");
                    parentBorder.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#006ce4");
                    if (ck.Name.Contains("Arr"))
                    {

                        switch (ck.Name.Last())
                        {
                            case '1':
                                MVW.FOG.Afs.ActivatedTimesArrOutbound.Add(0);
                                MVW.Flights = MVW.FOG.CreateNewList();
                                MainViewModel.PR.checkForAirline();
                                break;
                            case '2':
                                MVW.FOG.Afs.ActivatedTimesArrOutbound.Add(6);
                                MVW.Flights = MVW.FOG.CreateNewList();
                                MainViewModel.PR.checkForAirline();
                                break;
                            case '3':
                                MVW.FOG.Afs.ActivatedTimesArrOutbound.Add(12);
                                MVW.Flights = MVW.FOG.CreateNewList();
                                MainViewModel.PR.checkForAirline();
                                break;
                            case '4':
                                MVW.FOG.Afs.ActivatedTimesArrOutbound.Add(18);
                                MVW.Flights = MVW.FOG.CreateNewList();
                                MainViewModel.PR.checkForAirline();
                                break;

                        }
                    }
                    else
                    {
                        switch (ck.Name.Last())
                        {
                            case '1':
                                MVW.FOG.Afs.ActivatedTimesDepOutbound.Add(0);
                                MVW.Flights = MVW.FOG.CreateNewList();
                                MainViewModel.PR.checkForAirline();
                                break;
                            case '2':
                                MVW.FOG.Afs.ActivatedTimesDepOutbound.Add(6);
                                MVW.Flights = MVW.FOG.CreateNewList();
                                MainViewModel.PR.checkForAirline();
                                break;
                            case '3':
                                MVW.FOG.Afs.ActivatedTimesDepOutbound.Add(12);
                                MVW.Flights = MVW.FOG.CreateNewList();
                                MainViewModel.PR.checkForAirline();
                                break;
                            case '4':
                                MVW.FOG.Afs.ActivatedTimesDepOutbound.Add(18);
                                MVW.Flights = MVW.FOG.CreateNewList();
                                MainViewModel.PR.checkForAirline();
                                break;

                        }
                    }
                }
                else
                {
                    ck.Background = System.Windows.Media.Brushes.White;
                    Border parentBorder = VisualTreeHelper.GetParent(ck) as Border;
                    parentBorder.Background = System.Windows.Media.Brushes.White;
                    parentBorder.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#8c8c8e");
                    if (ck.Name.Contains("Arr"))
                    {

                        switch (ck.Name.Last())
                        {
                            case '1':
                                MVW.FOG.Afs.ActivatedTimesArrOutbound.Remove(0);
                                MVW.Flights = MVW.FOG.CreateNewList();
                                MainViewModel.PR.checkForAirline();
                                break;
                            case '2':
                                MVW.FOG.Afs.ActivatedTimesArrOutbound.Remove(6);
                                MVW.Flights = MVW.FOG.CreateNewList();
                                MainViewModel.PR.checkForAirline();
                                break;
                            case '3':
                                MVW.FOG.Afs.ActivatedTimesArrOutbound.Remove(12);
                                MVW.Flights = MVW.FOG.CreateNewList();
                                MainViewModel.PR.checkForAirline();
                                break;
                            case '4':
                                MVW.FOG.Afs.ActivatedTimesArrOutbound.Remove(18);
                                MVW.Flights = MVW.FOG.CreateNewList();
                                MainViewModel.PR.checkForAirline();
                                break;

                        }
                    }
                    else
                    {
                        switch (ck.Name.Last())
                        {
                            case '1':
                                MVW.FOG.Afs.ActivatedTimesDepOutbound.Remove(0);
                                MVW.Flights = MVW.FOG.CreateNewList();
                                MainViewModel.PR.checkForAirline();
                                break;
                            case '2':
                                MVW.FOG.Afs.ActivatedTimesDepOutbound.Remove(6);
                                MVW.Flights = MVW.FOG.CreateNewList();
                                MainViewModel.PR.checkForAirline();
                                break;
                            case '3':
                                MVW.FOG.Afs.ActivatedTimesDepOutbound.Remove(12);
                                MVW.Flights = MVW.FOG.CreateNewList();
                                MainViewModel.PR.checkForAirline();
                                break;
                            case '4':
                                MVW.FOG.Afs.ActivatedTimesDepOutbound.Remove(18);
                                MVW.Flights = MVW.FOG.CreateNewList();
                                MainViewModel.PR.checkForAirline();
                                break;

                        }
                    }
                }
                MVW.FOG.ChangeNumbers(MVW.Flights, ck);
            });
        }

        private string outboundDepTxt = "Departs from *";

        public string OutboundDepTxt
        {
            get { return outboundDepTxt; }
            set
            {
                outboundDepTxt = $"Departs from {value}";
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OutboundDepTxt)));
            }
        }

        private string outboundArrTxt = "Arrives at *";

        public string OutboundArrTxt
        {
            get { return outboundArrTxt; }
            set
            {
                outboundArrTxt = $"Arrives at {value}";
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OutboundArrTxt)));
            }
        }

        private int outboundDepNr1 = 0;

        public int OutboundDepNr1
        {
            get { return outboundDepNr1; }
            set
            {
                outboundDepNr1 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OutboundDepNr1)));
            }
        }

        private int outboundDepNr2 = 0;

        public int OutboundDepNr2
        {
            get { return outboundDepNr2; }
            set
            {
                outboundDepNr2 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OutboundDepNr2)));
            }
        }

        private int outboundDepNr3 = 0;

        public int OutboundDepNr3
        {
            get { return outboundDepNr3; }
            set
            {
                outboundDepNr3 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OutboundDepNr3)));
            }
        }

        private int outboundDepNr4 = 0;

        public int OutboundDepNr4
        {
            get { return outboundDepNr4; }
            set
            {
                outboundDepNr4 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OutboundDepNr4)));
            }
        }

        private int outboundArrNr1 = 0;

        public int OutboundArrNr1
        {
            get { return outboundArrNr1; }
            set
            {
                outboundArrNr1 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OutboundArrNr1)));
            }
        }

        private int outboundArrNr2 = 0;

        public int OutboundArrNr2
        {
            get { return outboundArrNr2; }
            set
            {
                outboundArrNr2 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OutboundArrNr2)));
            }
        }

        private int outboundArrNr3 = 0;

        public int OutboundArrNr3
        {
            get { return outboundArrNr3; }
            set
            {
                outboundArrNr3 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OutboundArrNr3)));
            }
        }

        private int outboundArrNr4 = 0;

        public int OutboundArrNr4
        {
            get { return outboundArrNr4; }
            set
            {
                outboundArrNr4 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OutboundArrNr4)));
            }
        }

        public ICommand CheckboxDesign { get; }
    }
}
