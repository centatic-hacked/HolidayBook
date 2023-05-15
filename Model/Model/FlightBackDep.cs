using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Model
{
    public class FlightBackDep
    {
        protected FlightBackDep() { }

        public FlightBackDep(DateTime dt, FlightOffersDB flb, string airport)
        {
            Dt = dt;
            FlightOfferNavigation = flb;
            FlightOfferNavigationId = flb.Id;
            Airport = airport;
        }
        public int Id { get; private set; }
        public DateTime Dt { get; private set; }


        public FlightOffersDB FlightOfferNavigation { get; private set; } = default!;

        public int FlightOfferNavigationId { get; private set; }

        public string Airport { get; private set; }
    }
}
