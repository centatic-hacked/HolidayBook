using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolidayBook.Overview.Classes
{
    public class FlightBookData
    {
        public static string startAirport { get; set; } = string.Empty;

        public static string endAirport { get; set; } = string.Empty;

        public static string depDate { get; set; } = string.Empty;

        public static string? retDate { get; set; } = default;

        public static int adults { get; set; } = 1;

        public static int children { get; set; } = 0;

        public static int infants { get; set; } = 0;

        public static string? @class { get; set; } = default;

        public static string? includeAir { get; set; } = default;

        public static string? excludeAir { get; set; } = default;

        public static bool nonStop { get; set; } = false;

        public static string? currency { get; set; } = default;

        public static int? maxPrice { get;  set; } = default;

        public static IDictionary<string, string?> travellers { get; set; } = new Dictionary<string, string?>();
    }
}
