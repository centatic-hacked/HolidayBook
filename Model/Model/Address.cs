using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Model
{
    public class Address
    {
        public string Street { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string PostalCode { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;

        public string HouseNr { get; set; } = string.Empty;

        public Address(string street, string city, string postalCode, string country, string houseNr)
        {
            Street = street;
            City = city;
            PostalCode = postalCode;
            Country = country;
            HouseNr = houseNr;
        }
    }
}
