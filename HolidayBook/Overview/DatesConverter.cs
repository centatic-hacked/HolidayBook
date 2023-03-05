using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using HolidayBook.ViewModels;

namespace HolidayBook.Overview
{
    public class DatesConverter : IValueConverter
    {
        public MainWindow mw { get; } = MainViewModel.MW;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            System.Windows.Controls.Calendar cal = default!;
            if (value is DateTime)
            {
                DateTime? dt = (DateTime)value;
                if (dt == mw.calendar.SelectedDate && dt != null)
                {
                    cal = mw.calendar;
                }
                else if (dt == mw.calendar2.SelectedDate && dt != null)
                {
                    cal = mw.calendar2;
                }
            }
            else if (value is ObservableCollection<DateTime>)
            {
                ObservableCollection<DateTime> dt = (ObservableCollection<DateTime>)value;
                if (dt == mw.calendar.SelectedDates)
                {
                    cal = mw.calendar;
                }
                else if (dt == mw.calendar2.SelectedDates)
                {
                    cal = mw.calendar2;
                }
            }
            string txt = default!;
            if (cal != null)
            {
                if (value is DateTime && cal.Name == "calendar")
                {

                }
                else if (value is ObservableCollection<DateTime>)
                {
                    mw.popupCalendar.IsOpen = false;
                    txt = (DateTime.Now.Year == mw.calendar.SelectedDates.First().Year) ? mw.calendar.SelectedDates.First().ToString("ddd, MMM dd") : mw.calendar.SelectedDates.First().ToString("ddd, MMM dd, yyyy");
                    txt += (DateTime.Now.Year == mw.calendar.SelectedDates.Last().Year) ? mw.calendar.SelectedDates.Last().ToString(" - ddd, MMM dd") : mw.calendar.SelectedDates.Last().ToString(" - ddd, MMM dd, yyyy");
                    mw.txtDateRange.Foreground = Brushes.Black;

                }
            }
            return txt;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
