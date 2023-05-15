using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolidayBook.Overview.Classes
{
    public class ActivatedFilters
    {
        public bool OneStop { get; set; }

        public List<string> ExcludedAirlines { get; set; } = default!;

        public List<int> ActivatedTimesDepOutbound { get; set; } = default!;

        public List<int> ActivatedTimesArrOutbound { get; set; } = default!;

        public List<int> ActivatedTimesDepReturn { get; set; } = default!;

        public List<int> ActivatedTimesArrReturn { get; set; } = default!;

        public int MaxTravelTime { get; set; }

        public ActivatedFilters(bool oneStop, List<string> excludedAirlines, List<int> activatedTimesDepOutbound, List<int> activatedTimesArrOutbound, List<int> activatedTimesDepReturn, List<int> activatedTimesArrReturn, int maxTravelTime)
        {
            OneStop = oneStop;
            ExcludedAirlines = excludedAirlines;
            ActivatedTimesDepOutbound = activatedTimesDepOutbound;
            ActivatedTimesArrOutbound = activatedTimesArrOutbound;
            ActivatedTimesDepReturn = activatedTimesDepReturn;
            ActivatedTimesArrReturn = activatedTimesArrReturn;
            MaxTravelTime = maxTravelTime;
        }
    }
}
