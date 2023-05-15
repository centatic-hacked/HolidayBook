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

namespace HolidayBook.ViewModels
{
    public class ReturnViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public static MainViewModel MVW { get; set; }

        public Return MW { get; }

        public ReturnViewModel(Return mw)
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
                                MVW.FOG.Afs.ActivatedTimesArrReturn.Add(0);
                                MVW.Flights = MVW.FOG.CreateNewList();
                                MainViewModel.PR.checkForAirline();
                                break;
                            case '2':
                                MVW.FOG.Afs.ActivatedTimesArrReturn.Add(6);
                                MVW.Flights = MVW.FOG.CreateNewList();
                                MainViewModel.PR.checkForAirline();
                                break;
                            case '3':
                                MVW.FOG.Afs.ActivatedTimesArrReturn.Add(12);
                                MVW.Flights = MVW.FOG.CreateNewList();
                                MainViewModel.PR.checkForAirline();
                                break;
                            case '4':
                                MVW.FOG.Afs.ActivatedTimesArrReturn.Add(18);
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
                                MVW.FOG.Afs.ActivatedTimesDepReturn.Add(0);
                                MVW.Flights = MVW.FOG.CreateNewList();
                                MainViewModel.PR.checkForAirline();
                                break;
                            case '2':
                                MVW.FOG.Afs.ActivatedTimesDepReturn.Add(6);
                                MVW.Flights = MVW.FOG.CreateNewList();
                                MainViewModel.PR.checkForAirline();
                                break;
                            case '3':
                                MVW.FOG.Afs.ActivatedTimesDepReturn.Add(12);
                                MVW.Flights = MVW.FOG.CreateNewList();
                                MainViewModel.PR.checkForAirline();
                                break;
                            case '4':
                                MVW.FOG.Afs.ActivatedTimesDepReturn.Add(18);
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
                                MVW.FOG.Afs.ActivatedTimesArrReturn.Remove(0);
                                MVW.Flights = MVW.FOG.CreateNewList();
                                MainViewModel.PR.checkForAirline();
                                break;
                            case '2':
                                MVW.FOG.Afs.ActivatedTimesArrReturn.Remove(6);
                                MVW.Flights = MVW.FOG.CreateNewList();
                                MainViewModel.PR.checkForAirline();
                                break;
                            case '3':
                                MVW.FOG.Afs.ActivatedTimesArrReturn.Remove(12);
                                MVW.Flights = MVW.FOG.CreateNewList();
                                MainViewModel.PR.checkForAirline();
                                break;
                            case '4':
                                MVW.FOG.Afs.ActivatedTimesArrReturn.Remove(18);
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
                                MVW.FOG.Afs.ActivatedTimesDepReturn.Remove(0);
                                MVW.Flights = MVW.FOG.CreateNewList();
                                MainViewModel.PR.checkForAirline();
                                break;
                            case '2':
                                MVW.FOG.Afs.ActivatedTimesDepReturn.Remove(6);
                                MVW.Flights = MVW.FOG.CreateNewList();
                                MainViewModel.PR.checkForAirline();
                                break;
                            case '3':
                                MVW.FOG.Afs.ActivatedTimesDepReturn.Remove(12);
                                MVW.Flights = MVW.FOG.CreateNewList();
                                MainViewModel.PR.checkForAirline();
                                break;
                            case '4':
                                MVW.FOG.Afs.ActivatedTimesDepReturn.Remove(18);
                                MVW.Flights = MVW.FOG.CreateNewList();
                                MainViewModel.PR.checkForAirline();
                                break;

                        }
                    }
                }
                MVW.FOG.ChangeNumbers(MVW.Flights, ck);
            });
        }

        private string returnDepTxt = "Departs from *";

        public string ReturnDepTxt
        {
            get { return returnDepTxt; }
            set
            {
                returnDepTxt = $"Departs from {value}";
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ReturnDepTxt)));
            }
        }

        private string returnArrTxt = "Arrives at *";

        public string ReturnArrTxt
        {
            get { return returnArrTxt; }
            set
            {
                returnArrTxt = $"Arrives at {value}";
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ReturnArrTxt)));
            }
        }

        private int returnDepNr1 = 0;

        public int ReturnDepNr1
        {
            get { return returnDepNr1; }
            set
            {
                returnDepNr1 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ReturnDepNr1)));
            }
        }

        private int returnDepNr2 = 0;

        public int ReturnDepNr2
        {
            get { return returnDepNr2; }
            set
            {
                returnDepNr2 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ReturnDepNr2)));
            }
        }

        private int returnDepNr3 = 0;

        public int ReturnDepNr3
        {
            get { return returnDepNr3; }
            set
            {
                returnDepNr3 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ReturnDepNr3)));
            }
        }

        private int returnDepNr4 = 0;

        public int ReturnDepNr4
        {
            get { return returnDepNr4; }
            set
            {
                returnDepNr4 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ReturnDepNr4)));
            }
        }

        private int returnArrNr1 = 0;

        public int ReturnArrNr1
        {
            get { return returnArrNr1; }
            set
            {
                returnArrNr1 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ReturnArrNr1)));
            }
        }

        private int returnArrNr2 = 0;

        public int ReturnArrNr2
        {
            get { return returnArrNr2; }
            set
            {
                returnArrNr2 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ReturnArrNr2)));
            }
        }

        private int returnArrNr3 = 0;

        public int ReturnArrNr3
        {
            get { return returnArrNr3; }
            set
            {
                returnArrNr3 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ReturnArrNr3)));
            }
        }

        private int returnArrNr4 = 0;

        public int ReturnArrNr4
        {
            get { return returnArrNr4; }
            set
            {
                returnArrNr4 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ReturnArrNr4)));
            }
        }

        public ICommand CheckboxDesign { get; }
    }
}
