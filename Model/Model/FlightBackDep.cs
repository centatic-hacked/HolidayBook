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

        public FlightBackDep(DateTime dt, FlightOffersDB flb)
        {
            Dt = dt;
            FlightOfferNavigation = flb;
            FlightOfferNavigationId = flb.Id;
        }
        public int Id { get; private set; }
        public DateTime Dt { get; private set; }


        public FlightOffersDB FlightOfferNavigation { get; private set; } = default!;

        public int FlightOfferNavigationId { get; private set; }
    }
}
