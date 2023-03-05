using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Model
{
    public class Airport
    {
        protected Airport()
        {}
        public Airport(string iATA, string name, string iCAO, string country, string city)
        {
            IATA = iATA;
            Name = name;
            ICAO = iCAO;
            Country = country;
            City = city;
        }

        public string IATA { get; private set; } = string.Empty;

        public string Name { get; private set; } = string.Empty;

        public string ICAO { get; private set; } = string.Empty;

        public string Country { get; private set; } = string.Empty;

        public string City { get; private set; } = string.Empty;
    }
}
