using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using HolidayBook.ViewModels;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace HolidayBook.Overview.Classes
{
    public class ExcludeAirlinesList
    {
        public string Name { get; set; } = string.Empty;

        public int OffersOfAirliens { get; set; }

        public MainViewModel MW { get; set; }

        public ICommand AirlineCheck { get; }

        public int Index { get; private set; }   

        public ExcludeAirlinesList(string name, int offersOfAirliens, MainViewModel mw)
        {
            Name = name;
            OffersOfAirliens = offersOfAirliens;
            MW = mw;
            AirlineCheck = new RelayCommand((object sender) => {
                CheckBox ck = (CheckBox)sender;
                ck.Name = ck.Name + MW.AirlinesList.SingleOrDefault(flight => flight.Name == ck.Content).Index;
                if (ck.IsChecked == true)
                {
                    ck.Background = (Brush)new BrushConverter().ConvertFrom("#006ce4");
                    Border parentBorder = VisualTreeHelper.GetParent(ck) as Border;
                    parentBorder.Background = (Brush)new BrushConverter().ConvertFrom("#006ce4");
                    parentBorder.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#006ce4");
                    MW.FOG.Afs.ExcludedAirlines.Remove(name);
                    MW.Flights = MW.FOG.CreateNewList();
                }
                else
                {
                    ck.Background = System.Windows.Media.Brushes.White;
                    Border parentBorder = VisualTreeHelper.GetParent(ck) as Border;
                    parentBorder.Background = System.Windows.Media.Brushes.White;
                    parentBorder.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#8c8c8e");
                    MW.FOG.Afs.ExcludedAirlines.Add(name);
                    MW.Flights = MW.FOG.CreateNewList();  
                }
                MW.FOG.ChangeNumbers(MW.Flights, ck);
            });
        }
    }
}
