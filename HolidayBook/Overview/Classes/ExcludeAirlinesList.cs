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

namespace HolidayBook.Overview.Classes
{
    public class ExcludeAirlinesList
    {
        public string Name { get; set; } = string.Empty;

        public int OffersOfAirliens { get; set; }

        public ICommand AirlineCheck { get; }

        public ExcludeAirlinesList(string name, int offersOfAirliens)
        {
            Name = name;
            OffersOfAirliens = offersOfAirliens;
            AirlineCheck = new RelayCommand((object sender) => {
                CheckBox ck = (CheckBox)sender;
                if (ck.IsChecked == true)
                {
                    ck.Background = (Brush)new BrushConverter().ConvertFrom("#006ce4");
                    Border parentBorder = VisualTreeHelper.GetParent(ck) as Border;
                    parentBorder.Background = (Brush)new BrushConverter().ConvertFrom("#006ce4");
                    parentBorder.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#006ce4");
                }
                else
                {
                    ck.Background = System.Windows.Media.Brushes.White;
                    Border parentBorder = VisualTreeHelper.GetParent(ck) as Border;
                    parentBorder.Background = System.Windows.Media.Brushes.White;
                    parentBorder.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#8c8c8e");
                }
            });
        }
    }
}
